using Microsoft.AspNetCore.Mvc;
using MusicDownloaderAPI.Models;
using MusicDownloaderAPI.VideoDownload;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MusicDownloaderAPI.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IDownloadQueue _downloadQueue;

        public HomeController()//IDownloadQueue downloadQueue)
        {
            //_downloadQueue = downloadQueue;
            //_downloadQueue.StartHandle();
        }

        //[HttpGet]
        //public string Download(string link)
        //{
        //    DownloadStatusInfo statusError = new(link, true);
        //    string errorMessage = JsonConvert.SerializeObject(statusError);
        //    if (link is null || link.Split("v=").Length < 2)
        //    {
        //        return errorMessage;
        //    }
        //    link = link.Split("v=")[1].Split('&')[0]; 
            
        //    _downloadQueue.StartDownload(link);
        //    var downloadStatusInfo = _downloadQueue.GetDownloadStatusInfo(link);
        //    string json = JsonConvert.SerializeObject(downloadStatusInfo);
        //    return json;
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}