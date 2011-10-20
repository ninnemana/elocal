using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLocal.Models;
using System.Xml.Linq;


namespace eLocal.Controllers
{
    public class IndexController : PublicBaseController
    {
        //
        // GET: /Index/

        public ActionResult Index()
        {

            // Get the homepage content
            SiteContent home_content = new SiteContent();
            home_content = CompanyModel.GetHomeContent();
            ViewBag.home_content = home_content;
            
            return View();
        }

    }
}
