using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLocal.Controllers {
    public class RenderController : PublicBaseController {
        //
        // GET: /Render/

        public ActionResult Index( string page = "") {
            try {
                if (page.Length == 0) { throw new Exception(""); }

                eLocalDataContext db = new eLocalDataContext();
                SiteContent content = db.SiteContents.Where(x => x.page_title == page).FirstOrDefault<SiteContent>();
                if (content.content_text.Length == 0) {
                    throw new Exception("");
                }

                ViewBag.content = content;
                return View();
            } catch (Exception e) {
                return Redirect("/");
            }
        }

    }
}
