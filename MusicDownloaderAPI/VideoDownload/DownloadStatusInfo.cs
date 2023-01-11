using Newtonsoft.Json;

namespace MusicDownloaderAPI.VideoDownload
{
    public class DownloadStatusInfo
    {
        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("miniolink")]
        public string MinioLink { get; set; }

        [JsonProperty("downloaded")]
        public bool Downloaded { get; set; }

        [JsonProperty("error")]
        public bool Error { get; set; }

        public DownloadStatusInfo(string link, string minioLink = "")
        {
            Link = link;
            MinioLink = minioLink;
            Downloaded = MinioLink != "";
            Error = !Downloaded;
        }
    }
}
