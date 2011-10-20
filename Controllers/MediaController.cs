using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLocal.Models;
using Google.GData.Client;
using Google.GData.YouTube;
using Google.GData.Extensions.MediaRss;
using Google.YouTube;

namespace eLocal.Controllers {
    public class MediaController : PublicBaseController {
        //
        // GET: /Media/

        public ActionResult Index() {

            Feed<Google.YouTube.Video> curt_videos = Media.GetCurtVideos();
            ViewBag.curt_videos = curt_videos;

            return View();
        }

        public ActionResult OurVideos() {

            Dictionary<eLocal.Video, Google.YouTube.Video> videos = new Dictionary<Video, Google.YouTube.Video>();

            // Retrieve the videos
            List<eLocal.Video> records = Media.GetOurVideos();
            
            // Convert them to YouTube video objects
            foreach (eLocal.Video record in records) {
                Google.YouTube.Video vid = Media.GetYouTubeVideo(record.embed_link.Split('/').LastOrDefault<string>());
                if (vid != null && vid.VideoId.Length > 0) {
                    videos.Add(record, vid);
                }
            }
            ViewBag.videos = videos;

            return View();
        }

        public ActionResult PhotoGallery() {
            List<Gallery> images = Media.GetImages();
            ViewBag.images = images;
            return View();
        }

    }
}
