using AngleSharp.Io;
using MusicDownloadASPNET.Rabbit;
using MusicDownloaderAPI.MinIO;
using Newtonsoft.Json;

namespace MusicDownloaderAPI.VideoDownload
{
    public class DownloadQueue : IHostedService
    {
        private readonly IRabbitMqService _rabbit;
        private readonly IDownloader _downloader;
        private readonly Queue<string> _downloadRequests;
        private readonly IDictionary<string, Task<string>> _downloading;
        private readonly IDictionary<string, string> _downloaded;
        private readonly IList<string> _resultsToUpload;
        private readonly string _requestsQueueName;
        private readonly string _resultsQueueName;
        private readonly int _maxConcurrentDownloads;
        private bool _cancellationRequested = false;
        private bool _stopped = false;

        public DownloadQueue(IDownloader downloader, IRabbitMqService rabbit) 
        {
            string? maxConcurrentDownloads = Environment.GetEnvironmentVariable("MAX_CONC_DOWNLOADS");

            if (maxConcurrentDownloads is null)
            {
                _maxConcurrentDownloads = 5;
            }
            else
            {
                _ = int.TryParse(maxConcurrentDownloads, out _maxConcurrentDownloads);
            }
            
            _requestsQueueName = "DownloadRequests";
            _resultsQueueName = "DownloadResults";
            _rabbit = rabbit;
            _downloader = downloader;
            _downloadRequests = new Queue<string>();
            _downloading = new Dictionary<string, Task<string>>();
            _downloaded = new Dictionary<string, string>();
            _resultsToUpload = new List<string>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task task = Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested || _cancellationRequested)
                {
                    HandleNewMessage();
                    UpdateDownloads();
                    UploadResults();
                    Thread.Sleep(100);
                }
                _stopped = true;
            }, cancellationToken);
            return task;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => 
            { 
                _cancellationRequested = true; 
                while (!_stopped)
                {
                    Thread.Sleep(100);
                }
            }, cancellationToken);
        }

        private void HandleNewMessage()
        {
            if (!_rabbit.TryReceiveMessage(_requestsQueueName, out string link)
                || _downloading.ContainsKey(link)
                || _downloadRequests.Contains(link))
            {
                return;
            }

            if (_downloaded.ContainsKey(link))
            {
                DownloadStatusInfo info = new(link, _downloaded[link]);
                string message = JsonConvert.SerializeObject(info);
                _resultsToUpload.Add(message);
                return;
            }

            _downloadRequests.Enqueue(link);
        }

        private void UpdateDownloads()
        {
            foreach (string link in _downloading.Keys)
            {
                if (!_downloading[link].IsCompleted)
                {
                    continue;
                }

                _downloaded.Add(link, _downloading[link].Result);
                DownloadStatusInfo info = new(link, _downloaded[link]);
                string message = JsonConvert.SerializeObject(info);
                _resultsToUpload.Add(message);
                _downloading.Remove(link);
            }

            if (_downloading.Count < _maxConcurrentDownloads
                && _downloadRequests.Count > 0)
            {
                string link = _downloadRequests.Dequeue();
                Task<string> task = _downloader.DownloadAsync(link);
                _downloading.Add(link, task);
            }
        }

        private void UploadResults()
        {
            foreach (var item in _resultsToUpload)
            {
                _rabbit.SendMessage(_resultsQueueName, item);
            }
            _resultsToUpload.Clear();
        }
    }
}
