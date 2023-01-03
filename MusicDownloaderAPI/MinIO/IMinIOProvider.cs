namespace MusicDownloaderAPI.MinIO
{
    public interface IMinIOProvider
    {
        public void UploadFile(Stream stream, string fileName);

        public string GetDownloadLink(string fileName);
    }
}
