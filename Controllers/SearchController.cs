using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLocal.Controllers {
    public class SearchController : PublicBaseController {
        //
        // GET: /Search/

        public ActionResult Index(string term = "") {

            if (term.Length > 0) {

            }
            ViewBag.term = term;
            ViewBag.result_count = 0;

            return View();
        }

    }
}
