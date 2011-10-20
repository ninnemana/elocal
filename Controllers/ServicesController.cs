using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLocal.Models;

namespace eLocal.Controllers {
    public class ServicesController : PublicBaseController {
        //
        // GET: /Services/

        public ActionResult Index() {
            // Get the services
            List<Service> services = CompanyModel.GetServices();
            ViewBag.services = services;

            return View();
        }

    }
}
