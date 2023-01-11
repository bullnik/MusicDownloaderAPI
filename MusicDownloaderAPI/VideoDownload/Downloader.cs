using MusicDownloaderAPI.MinIO;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace MusicDownloaderAPI.VideoDownload
{
    public class Downloader : IDownloader
    {
        private readonly IMinIOProvider _minIOProvider;

        public Downloader(IMinIOProvider minIOProvider)
        {
            _minIOProvider = minIOProvider;
        }

        public Task<string> DownloadAsync(string link)
        {
            Task<string> task = new(() =>
            {
                try
                {
                    return Download(link);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }
            });
            task.Start();
            return task;
        }

        private string Download(string link)
        {
            var youtube = new YoutubeClient();
            var task = youtube.Videos.Streams.GetManifestAsync(link);
            while (!task.IsCompleted)
            {
                Thread.Sleep(1);
            }
            var video = task.Result;
            var streamInfo = video.GetAudioOnlyStreams().GetWithHighestBitrate();
            var stream = youtube.Videos.Streams.GetAsync(streamInfo);
            while (!stream.IsCompleted)
            {
                Thread.Sleep(1);
            }
            link += ".mp3";
            _minIOProvider.UploadFile(stream.Result, link);
            return _minIOProvider.GetDownloadLink(link);
        }
    }
}
