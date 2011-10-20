using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eLocal.Models;
using System.Xml.Linq;
using System.Net;

namespace eLocal.Models {
    public class CompanyModel {
        public static Company Get() {
            try {
                eLocalDataContext db = new eLocalDataContext();

                Company comp = (from c in db.Companies
                                select c).FirstOrDefault<Company>();
                return comp;
            } catch (Exception e) {
                return new Company();
            }
        }


        public static Location GetPrimaryLocation() {
            try {
                eLocalDataContext db = new eLocalDataContext();
                Location location = new Location();

                location = (from l in db.Locations
                            where l.isPrimary.Equals(1)
                            select l).FirstOrDefault<Location>();
                if (location.locationID == null || location.locationID == 0) {
                    location = db.Locations.OrderBy(x => x.locationID).FirstOrDefault<Location>();
                    if (location.locationID == null || location.locationID == 0) {
                        return new Location();
                    }
                }
                return location;
            } catch (Exception e) {
                return new Location();
            }
        }

        /// <summary>
        /// Get the location record for the given location ID
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <returns>Location</returns>
        public static Location GetLocation(int id = 0) {
            try {
                eLocalDataContext db = new eLocalDataContext();
                Location loc = new Location();

                loc = db.Locations.Where(x => x.locationID == id).FirstOrDefault<Location>();
                return loc;
            } catch (Exception e) {
                return new Location();
            }
        }

        public static bool GetPage(string page_title = "") {
            try {
                eLocalDataContext db = new eLocalDataContext();
                SiteContent sc = new SiteContent();

                sc = db.SiteContents.Where(x => x.page_title == page_title).FirstOrDefault<SiteContent>();
                if (sc != null && sc.content_text.Length > 0) {
                    return true;
                }
                return false;
            } catch (Exception) {
                return false;
            }
        }

        public static List<Service> GetServices() {
            try {
                eLocalDataContext db = new eLocalDataContext();

                return db.Services.OrderBy(x => x.service_title).ToList<Service>();
            } catch (Exception) {
                return new List<Service>();
            }
        }

        public static List<string> GetPrimaryLinks() {
            try {
                eLocalDataContext db = new eLocalDataContext();
                List<string> links = new List<string>();

                links = db.SiteContents.Where(x => x.isPrimary == 1).OrderBy(x => x.page_title).Select(x => x.page_title).ToList<string>();
                return links;
            } catch (Exception) {
                return new List<string>();
            }
        }

        public static List<Banner> GetRandomBanners(int count = 0, bool random = false) {
            try {
                eLocalDataContext db = new eLocalDataContext();
                List<Banner> banners = new List<Banner>();

                if (count == 0) { // Get all the banners
                    banners = (from b in db.Banners
                               where b.starts <= DateTime.Now && b.ends >= DateTime.Now
                               select b).OrderBy(x => x.bannerID).ToList<Banner>();
                } else { // Get a specific number of banners
                    banners = (from b in db.Banners
                               where b.starts <= DateTime.Now && b.ends >= DateTime.Now
                               select b).OrderBy(x => x.bannerID).Take(count).ToList<Banner>();
                }
                if (random) {
                    banners = APIModels.Shuffle<Banner>(banners);
                }
                return banners;
            } catch (Exception e) {
                return new List<Banner>();
            }
        }

        public static SiteContent GetHomeContent() {
            try {
                eLocalDataContext db = new eLocalDataContext();
                SiteContent home = db.SiteContents.Where(x => x.page_title == "Home").FirstOrDefault<SiteContent>();
                if (home == null) {
                    home = new SiteContent();
                }
                return home;
            } catch (Exception) {
                return new SiteContent();
            }
        }

        public static int GetCustomerID(string email = "") {
            try {
                if(email.Length == 0){ throw new Exception(); }

                string url = "http://docs.curthitch.biz/API/GetCustomerID?email=" + email;
                WebClient client = new WebClient();
                string resp = client.DownloadString(url);

                return Convert.ToInt32(resp);
            } catch (Exception) {
                return 0;
            }
        }
    }
}