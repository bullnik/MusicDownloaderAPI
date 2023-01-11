namespace MusicDownloaderAPI.VideoDownload
{
    public interface IDownloader
    {
        public Task<string> DownloadAsync(string link);
    }
}
