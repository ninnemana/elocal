using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLocal.Models;
using System.Web.Script.Serialization;

namespace eLocal.Controllers {
    public class PartsController : PublicBaseController {
        //
        // GET: /Parts/

        public ActionResult Index(int id = 0) {
            if (id == 0) {
                return Redirect("/");
            }


            // Get the acctID 
            int acctID = 0;
            try {
                acctID = Convert.ToInt32(Session["cust_id"]);
            } catch (Exception) { }

            // Get the part information
            APIPart part = Part.GetPart(id, acctID);
            ViewBag.part = part;

            // Check if we have vehicle information for this user
            APIPart vehicle_part = new APIPart();
            HttpCookie vehicleYear = Request.Cookies.Get("vehicle_year");
            string year = (vehicleYear != null && vehicleYear.Value != null) ? vehicleYear.Value.ToString() : "";

            HttpCookie vehicleMake = Request.Cookies.Get("vehicle_make");
            string make = (vehicleMake != null && vehicleMake.Value != null) ? vehicleMake.Value.ToString() : "";

            HttpCookie vehicleModel = Request.Cookies.Get("vehicle_model");
            string model = (vehicleModel != null && vehicleModel.Value != null) ? vehicleModel.Value.ToString() : "";

            HttpCookie vehicleStyle = Request.Cookies.Get("vehicle_style");
            string style = (vehicleStyle != null && vehicleStyle.Value != null) ? vehicleStyle.Value.ToString() : "";
            if (year.Length > 0 && make.Length > 0 && model.Length > 0 && style.Length > 0) {
                List<APIPart> vehicle_parts = Part.GetVehicleParts(year, make, model, style);
                vehicle_part = vehicle_parts.Where(x => x.partID == part.partID).FirstOrDefault<APIPart>();
            }
            ViewBag.vehicle_part = vehicle_part;

            // Get the related parts
            List<APIPart> related_parts = Part.GetRelated(id);
            ViewBag.related_parts = related_parts;

            // Get the vehicles for this part
            List<FullVehicle> vehicles = Part.GetPartVehicles(id);
            ViewBag.vehicles = vehicles;

            // Add this part to our related parts object
            HttpCookie recent_cookie = Request.Cookies.Get("recent_parts");
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<int> recent_parts = new List<int>();

            if (recent_cookie == null || recent_cookie.Value == null) { // Cookie is invalid
                // Create new cookie and set its expiration to 30 days from now
                recent_cookie = new HttpCookie("recent_parts");
                recent_cookie.Expires = DateTime.Now.AddDays(30);

                // Add this part to the recent parts
                recent_parts.Add(part.partID);

                // Serialize the recent part array and set the cookie's value
                recent_cookie.Value = js.Serialize(recent_parts);
                Response.Cookies.Add(recent_cookie);

            } else { // Cookie found, so we update

                Request.Cookies.Remove("recent_parts"); // Remove old cookie

                // Create new cookie and set it's expiration to 30 days from now
                HttpCookie new_cookie = new HttpCookie("recent_parts");
                new_cookie.Expires = DateTime.Now.AddDays(30);

                // Begin compiling new recent part array
                List<int> unique_ids = new List<int>();
                List<int> recent_partIds = new List<int>();
                recent_partIds = js.Deserialize<List<int>>(recent_cookie.Value);
                
                if (!recent_partIds.Contains(part.partID)) {
                    recent_partIds.Add(part.partID);
                }
                recent_partIds = recent_partIds.Take(5).ToList<int>();
                new_cookie.Value = js.Serialize(recent_partIds);
                Response.Cookies.Add(new_cookie);
            }


            return View();
        }

        public ActionResult Related(int partID = 0) {
            int part_id = partID;

            // Find out if we need to sort or not
            string sorting = "";
            sorting = (Request.QueryString["sorting"] != null) ? Request.QueryString["sorting"] : sorting;
            sorting = (Request.Form["sort"] != null) ? Request.Form["sort"] : sorting;

            // Get the part we're acting on
            APIPart part = Part.GetPart(partID);
            ViewBag.part = part;

            // Get the related parts
            List<APIPart> related_parts = Part.GetRelated(partID, sorting);
            ViewBag.related_parts = related_parts;

            return View();
        }

    }
}
