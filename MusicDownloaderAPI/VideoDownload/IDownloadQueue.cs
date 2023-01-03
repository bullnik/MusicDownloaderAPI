namespace MusicDownloaderAPI.VideoDownload
{
    public interface IDownloadQueue
    {
        public void StartDownload(string link);
        public DownloadStatusInfo GetDownloadStatusInfo(string link);
    }
}
