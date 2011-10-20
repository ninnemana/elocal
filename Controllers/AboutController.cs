using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLocal.Controllers {
    public class AboutController : PublicBaseController {
        //
        // GET: /About/

        public ActionResult Index() {

            // Get the About Us content entry for the database
            SiteContent about_content = new SiteContent();
            eLocalDataContext db = new eLocalDataContext();
            about_content = db.SiteContents.Where(x => x.page_title == "About").FirstOrDefault<SiteContent>();
            ViewBag.content = about_content;

            return View();
        }

    }
}
