using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using eLocal.Models;
using System.Xml.Linq;
using System.Xml;
using System.Net;
using System.IO;

namespace eLocal.Controllers
{
    public class PublicBaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
            base.Initialize(requestContext);

            try {

                // Get the parent categories
                List<eLocal.Models.Category> parent_cats = eLocal.Models.Category.GetParentCategories();
                ViewBag.cats = parent_cats;

                // Get the company record for the database
                Company company = CompanyModel.Get();
                if (company == null) {
                    throw new Exception("The site settings have not been configured.");
                }
                ViewBag.company = company;

                // We need to make sure we have the customer ID in a session object in order to properly retrieve pricing
                try {
                    // Attempt conversion from session variable to int, this will throw an exception if the cust_id Session variable is not usable
                    int cust_id = Convert.ToInt32(Session["cust_id"]);
                    if (cust_id == 0) {
                        throw new Exception();
                    }
                } catch (Exception) {
                    // Retrieve the customer ID from the API
                    int cust_id = CompanyModel.GetCustomerID(company.contact_email);

                    // Store it in the session
                    Session["cust_id"] = cust_id;
                }

                // Get the primary location
                Location location = CompanyModel.GetPrimaryLocation();
                ViewBag.primary_location = location;

                // Get 5 random banners
                List<Banner> banners = new List<Banner>();
                List<Banner> left_banners = new List<Banner>();
                List<Banner> right_banners = new List<Banner>();
                banners = CompanyModel.GetRandomBanners(5, true);
                int i = 0;
                foreach(Banner banner in banners){
                    if (i < 3) {
                        left_banners.Add(banner);
                    } else {
                        right_banners.Add(banner);
                    }
                    i++;
                }
                if (left_banners == null) { left_banners = new List<Banner>(); }
                if (right_banners == null) { right_banners = new List<Banner>(); }
                ViewBag.left_banners = left_banners;
                ViewBag.right_banners = right_banners;

                if (company.homepage_lookup == 1) {
                    // Get the vehicle years
                    ViewBag.years = HitchLookup.GetYears();
                }

                // Initiate our ViewBag variables for the lookup
                ViewBag.currentYear = 0;

                if (Request.Form["getMake"] != null) {
                    string year = Request.Form["select_year"];
                    ViewBag.currentYear = Convert.ToDouble(year);
                    if (Convert.ToDouble(year) > 0) {
                        ViewBag.years = HitchLookup.GetYears();
                        ViewBag.makes = HitchLookup.GetMakes(Convert.ToDouble(year));
                        ViewBag.getModels = "1";
                    }
                }
                
                if (Request.Form["getModel"] != null) {
                    string year = Request.Form["select_year"];
                    string make = Request.Form["select_make"];
                    ViewBag.currentYear = Convert.ToDouble(year);
                    ViewBag.currentMake = make;
                    if (Convert.ToDouble(year) > 0 && make.Length > 0) {
                        ViewBag.years = HitchLookup.GetYears();
                        ViewBag.makes = HitchLookup.GetMakes(Convert.ToDouble(year));
                        ViewBag.models = HitchLookup.GetModels(Convert.ToDouble(year), make);
                        ViewBag.getStyles = "1";
                    }
                }


                if (Request.Form["getStyle"] != null) {
                    string year = Request.Form["select_year"];
                    string make = Request.Form["select_make"];
                    string model = Request.Form["select_model"];
                    ViewBag.currentYear = Convert.ToDouble(year);
                    ViewBag.currentMake = make;
                    ViewBag.currentModel = model;
                    if (Convert.ToDouble(year) > 0 && make.Length > 0) {
                        ViewBag.years = HitchLookup.GetYears();
                        ViewBag.makes = HitchLookup.GetMakes(Convert.ToDouble(year));
                        ViewBag.models = HitchLookup.GetModels(Convert.ToDouble(year), make);
                        ViewBag.styles = HitchLookup.GetStyles(Convert.ToDouble(year), make, model);
                    }
                }

                // Get all the cookies
                HttpCookie vehicleYear = Request.Cookies.Get("vehicle_year");
                ViewBag.vehicleYear = (vehicleYear != null && vehicleYear.Value != null)? vehicleYear.Value.ToString() : "";

                HttpCookie vehicleMake = Request.Cookies.Get("vehicle_make");
                ViewBag.vehicleMake = (vehicleMake != null && vehicleMake.Value != null) ? vehicleMake.Value.ToString() : "";

                HttpCookie vehicleModel = Request.Cookies.Get("vehicle_model");
                ViewBag.vehicleModel = (vehicleModel != null && vehicleModel.Value != null) ? vehicleModel.Value.ToString() : "";

                HttpCookie vehicleStyle = Request.Cookies.Get("vehicle_style");
                ViewBag.vehicleStyle = (vehicleStyle != null && vehicleStyle.Value != null) ? vehicleStyle.Value.ToString() : "";

                // Check to see if we have any recently viewed parts
                JavaScriptSerializer js = new JavaScriptSerializer();
                HttpCookie recent_parts_cookie = Request.Cookies.Get("recent_parts");
                List<APIPart> parts = new List<APIPart>();
                if (recent_parts_cookie != null && recent_parts_cookie.Value != null && recent_parts_cookie.Value.Length > 0) {
                    List<int> recent_ids = new List<int>();
                    recent_ids = js.Deserialize<List<int>>(recent_parts_cookie.Value);
                    foreach (int id in recent_ids) {
                        APIPart recent_part = Part.GetPart(id);
                        parts.Add(recent_part);
                    }
                }
                ViewBag.recent_parts = parts;

                // Get the primary links
                List<string> links = CompanyModel.GetPrimaryLinks();
                ViewBag.links = links;

                // Get the number of videos added to the Admin
                int video_count = Media.GetOurVideos().Count;
                ViewBag.video_count = video_count;

                // Get the number of testimonials add to the site
                int test_count = TestimonialModel.Get().Count;
                ViewBag.test_count = test_count;

                // Get the number of photo gallery images
                int gallery_count = Media.GetImages().Count;
                ViewBag.gallery_count = gallery_count;

            } catch (Exception e) {
                ViewBag.cats = new List<eLocal.Models.Category>();
                ViewBag.error = e.Message;
            }
        }

    }
}
