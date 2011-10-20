using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLocal.Models;

namespace eLocal.Controllers
{
    public class CategoryController : PublicBaseController
    {
        //
        // GET: /Category/

        public ActionResult Index(int id = 0){

            // Get the category information

            eLocal.Models.Category category = new eLocal.Models.Category();
            category.catID = id;
            ViewBag.category = category.Get();

            return View();
        }

        public ActionResult ByName(string cat_name = "") {
            try {

                // Create Category object
                eLocal.Models.Category cat = new eLocal.Models.Category();
                cat.catTitle = cat_name;

                // Get the category information
                cat = cat.GetByName();
                ViewBag.category = cat;
                
                // Attempt to retrieve the customer ID from the Session variable
                int acctID = 0;
                try {
                    acctID = Convert.ToInt32(Session["cust_id"]);
                } catch (Exception) { }

                // Get the Parts for this category
                List<APIPart> parts = cat.GetParts(acctID);


                if (parts.Count == 0 && cat.sub_categories.Count == 0) {
                    return View("NoParts");
                } else if (parts.Count == 0 && cat.catID > 0) {
                    Response.Redirect("~/Category/Index/" + cat.catID);
                }

                // Get the unique product classes
                List<string> classes = (from p in parts
                                        where p.pClass.Length > 0
                                        select p.pClass).Distinct().OrderBy(x => x).ToList<string>();
                ViewBag.classes = classes;

                // We're going to organize the products into our classes (this will make our view a lot simpler)
                List<Dictionary<string, List<APIPart>>> ordered_parts = new List<Dictionary<string, List<APIPart>>>();
                foreach (string pClass in classes) {

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
                    classes.Add("Other");
                    Dictionary<string, List<APIPart>> other_dictionary = new Dictionary<string, List<APIPart>>();
                    other_dictionary.Add("Other", other_parts);
                    ordered_parts.Add(other_dictionary);
                }

                ViewBag.ordered_parts = ordered_parts;
            } catch (Exception e) {
                string err = e.Message;
            }
            return View();
        }

    }
}
