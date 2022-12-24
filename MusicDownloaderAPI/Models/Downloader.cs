using MediaToolkit;
using MediaToolkit.Model;
using System.IO;
using System.Runtime.InteropServices;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace MusicDownloaderAPI.Models
{
    public class Downloader
    {
        private string _downloadsFolder { get; set; } = "/app/music/";

        public Downloader()
        {

        }

        public void Download(string link)
        {
            var youtube = new YoutubeClient();
            string savedLink = link.Replace("/", "a");
            link = link.Split("v=")[1].Split('&')[0];
            var task = youtube.Videos.Streams.GetManifestAsync(link);
            while (!task.IsCompleted)
            {
                Thread.Sleep(100);
            }
            var video = task.Result;
            Console.WriteLine("TASK COMPLETED");
            var streamInfo = video.GetAudioOnlyStreams().GetWithHighestBitrate();
            Console.WriteLine("STREAM INFO CREATED");
            string path = _downloadsFolder + savedLink;
            Console.WriteLine("DOWNLOAD FILE TO " + path + ".mp3");
            var file = youtube.Videos.Streams.DownloadAsync(streamInfo, path + ".mp3");
            while (!file.IsCompleted)
            {
                Thread.Sleep(100);
            }
            string[] allfiles = Directory.GetFiles(_downloadsFolder);
            foreach (string filename in allfiles)
            {
                Console.WriteLine(filename);
            }
            Console.WriteLine("FILE DOWNLOADED");
            //File.Move(path, path + ".mp3");
        }

        //public void Download(string link)
        //{
        //    string path = _downloadsFolder + link;
        //    FileStream stream = File.Create(path);
        //    File.Move(path, path + ".txt");
        //    stream.Close();
        //}

        //public void Download(string link)
        //{
        //    var source = _downloadsFolder;
        //    var youtube = YouTube.Default;
        //    var vid = youtube.GetVideo(link);

        //    File.WriteAllBytes(source + vid.FullName, vid.GetBytes());

        //    var inputFile = new MediaFile { Filename = source + vid.FullName };
        //    var outputFile = new MediaFile { Filename = $"{_downloadsFolder + link}" };

        //    using (var engine = new Engine())
        //    {
        //        engine.GetMetadata(inputFile);

        //        engine.Convert(inputFile, outputFile);
        //    }

        //    string path = _downloadsFolder + link;
        //    File.Move(path, path + ".mp3");
        //}
    }
}
