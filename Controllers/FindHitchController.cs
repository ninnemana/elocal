using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using eLocal.Models;

namespace eLocal.Controllers
{
    public class FindHitchController : PublicBaseController
    {
        //
        // GET: /FindHitch/
        [ValidateInput(false)]
        public ActionResult Index(string year = "", string make = "", string model = "", string style = "", string sort = "")
        {
            int acctID = 0;
            try {
                acctID = Convert.ToInt32(Session["cust_id"]);
            } catch (Exception) {}


            // Assign GET data
            year = (Request.QueryString["year"] != null) ? HttpUtility.UrlDecode(Request.QueryString["year"]) : year;
            make = (Request.QueryString["make"] != null) ? HttpUtility.UrlDecode(Request.QueryString["make"]) : make;
            model = (Request.QueryString["model"] != null) ? HttpUtility.UrlDecode(Request.QueryString["model"]) : model;
            style = (Request.QueryString["style"] != null) ? HttpUtility.UrlDecode(Request.QueryString["style"]) : style;
            sort = (Request.QueryString["sort"] != null) ? HttpUtility.UrlDecode(Request.QueryString["sort"]) : sort;

            // Assign POST data if available
            year = (Request.Form["select_year"] != null) ? Request.Form["select_year"] : year;
            make = (Request.Form["select_make"] != null) ? Request.Form["select_make"] : make;
            model = (Request.Form["select_model"] != null) ? Request.Form["select_model"] : model;
            style = (Request.Form["select_style"] != null) ? Request.Form["select_style"] : style;
            sort = (Request.Form["sort"] != null) ? Request.Form["sort"] : sort;

            // Store our vehicle information into ViewBag
            ViewBag.year = year;
            ViewBag.make = make;
            ViewBag.model = model;
            ViewBag.style = style;

            // Store the vehicle fields in cookies
            HttpCookie year_cookie = new HttpCookie("vehicle_year");
            year_cookie.Value = year;
            year_cookie.Expires = DateTime.Now.AddDays(14);
            Response.Cookies.Add(year_cookie);
            ViewBag.vehicleYear = year;

            HttpCookie make_cookie = new HttpCookie("vehicle_make");
            make_cookie.Value = make;
            make_cookie.Expires = DateTime.Now.AddDays(14);
            Response.Cookies.Add(make_cookie);
            ViewBag.vehicleMake = make;

            HttpCookie model_cookie = new HttpCookie("vehicle_model");
            model_cookie.Value = model;
            model_cookie.Expires = DateTime.Now.AddDays(14);
            Response.Cookies.Add(model_cookie);
            ViewBag.vehicleModel = model;

            HttpCookie style_cookie = new HttpCookie("vehicle_style");
            style_cookie.Value = style;
            style_cookie.Expires = DateTime.Now.AddDays(14);
            Response.Cookies.Add(style_cookie);
            ViewBag.vehicleStyle = style;

            if (year.Length == 0 || make.Length == 0 || model.Length == 0 || style.Length == 0) {
                ViewBag.error += " Invalid vehicle information";
                ViewBag.other_parts = new List<APIPart>();
                ViewBag.ordered_parts = new List<Dictionary<string,List<APIPart>>>();
                ViewBag.classes = new List<string>();
            } else {
                // Get the parts for this vehicle
                List<APIPart> parts = Part.GetVehicleParts(year, make, model, style, sort, acctID);

                // Get the unique product classes
                List<string> classes = (from p in parts
                                        where p.pClass.Length > 0
                                        select p.pClass).Distinct().OrderBy(x => x).ToList<string>();
                ViewBag.classes = classes;
                
                // We're going to organize the products into our classes (this will make our view a lot simpler)
                List<Dictionary<string, List<APIPart>>> ordered_parts = new List<Dictionary<string,List<APIPart>>>();
                foreach(string pClass in classes){

                    List<APIPart> sorted_products = (from p in parts
                                                        where p.pClass.Equals(pClass)
                                                        select p).ToList<APIPart>();
                    Dictionary<string, List<APIPart>> sorted_dictionary = new Dictionary<string, List<APIPart>>();
                    sorted_dictionary.Add(pClass, sorted_products);
                    ordered_parts.Add(sorted_dictionary);
                }

                // We need to make sure that we grab all the parts that have blank classes. We'll display those in an 'Other' class.
                List<APIPart> other_parts = (from p in parts
                                             where p.pClass.Length == 0
                                             select p).ToList<APIPart>();
                if (other_parts.Count > 0) {
                    Dictionary<string, List<APIPart>> other_dictionary = new Dictionary<string, List<APIPart>>();
                    other_dictionary.Add("Other", other_parts);
                    ordered_parts.Add(other_dictionary);

                    classes.Add("Other");
                }
                ViewBag.ordered_parts = ordered_parts;
            }

            return View();
        }

    }
}
