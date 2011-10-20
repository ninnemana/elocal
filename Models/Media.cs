using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.YouTube;
using Google.GData.Extensions.MediaRss;
using Google.YouTube;

namespace eLocal.Models {
    public class Media {

        public static Feed<Google.YouTube.Video> GetCurtVideos() {
            try {
                YouTubeRequestSettings settings = new YouTubeRequestSettings("eLocal", "AI39si6iCFZ_NutrvZe04i9_m7gFhgmPK1e7LF6-yHMAwB-GDO3vC3eD0R-5lberMQLdglNjH3IWUMe3tJXe9qrFe44n2jAUyg");
                YouTubeRequest req = new YouTubeRequest(settings);

                YouTubeQuery query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);
                query.Author = "curtmfg";
                query.Formats.Add(YouTubeQuery.VideoFormat.Embeddable);
                query.OrderBy = "viewCount";

                // We need to load the feed data for the CURTMfg Youtube Channel
                Feed<Google.YouTube.Video> video_feed = req.Get<Google.YouTube.Video>(query);
                return video_feed;
            } catch (Exception) {
                return null;
            }
        }

        public static AtomFeed GetFeed(string url = "", int start = 0, int num_results = 0) {
            FeedQuery query = new FeedQuery("");
            Google.GData.Client.Service service = new Google.GData.Client.Service();
            query.Uri = new Uri(url);
            if (start > 0) {
                query.StartIndex = start;
            }
            if (num_results > 0) {
                query.NumberToRetrieve = num_results;
            }

            AtomFeed feed = service.Query(query);
            return feed;
        }

        public static List<Video> GetOurVideos() {
            try {
                eLocalDataContext db = new eLocalDataContext();
                return db.Videos.OrderBy(x => x.dateAdded).ToList<Video>();
            } catch (Exception) {
                return new List<Video>();
            }
        }

        public static Google.YouTube.Video GetYouTubeVideo(string id = "") {
            try {
                // Initiate video object
                Google.YouTube.Video video = new Google.YouTube.Video();

                // Initiate YouTube request object
                YouTubeRequestSettings settings = new YouTubeRequestSettings("eLocal", "AI39si6iCFZ_NutrvZe04i9_m7gFhgmPK1e7LF6-yHMAwB-GDO3vC3eD0R-5lberMQLdglNjH3IWUMe3tJXe9qrFe44n2jAUyg");
                YouTubeRequest req = new YouTubeRequest(settings);

                // Create the URI and make request to YouTube
                Uri video_url = new Uri("http://gdata.youtube.com/feeds/api/videos/" + id);
                video = req.Retrieve<Google.YouTube.Video>(video_url);

                return video;
            } catch (Exception) {
                return new Google.YouTube.Video();
            }
        }

        public static List<Gallery> GetImages() {
            try {
                eLocalDataContext db = new eLocalDataContext();

                return db.Galleries.OrderBy(x => x.sort_order).ToList<Gallery>();
            } catch (Exception) {
                return new List<Gallery>();
            }
        }
    }
}
