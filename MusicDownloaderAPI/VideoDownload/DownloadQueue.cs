using MusicDownloaderAPI.MinIO;

namespace MusicDownloaderAPI.VideoDownload
{
    public class DownloadQueue : IDownloadQueue
    {
        private readonly IDownloader _downloader;
        private readonly IDictionary<string, Task<string>> _downloading;

        public DownloadQueue(IDownloader downloader) 
        {
            _downloader = downloader;
            _downloading = new Dictionary<string, Task<string>>();
        }

        public void StartDownload(string link)
        {
            if (_downloading.ContainsKey(link))
            {
                return;
            }

            Task<string> task = new(() => _downloader.Download(link));
            task.Start();
            _downloading.Add(link, task);
        }

        public DownloadStatusInfo GetDownloadStatusInfo(string link)
        {
            if (_downloading.ContainsKey(link))
            {
                Task<string> task = _downloading[link];
                if (task.IsCompleted)
                {
                    return new(true, 100, task.Result, false);
                }
                return new(false, 50, "", false);
            }

            return new(false, 0, "", true);
        }
    }
}
