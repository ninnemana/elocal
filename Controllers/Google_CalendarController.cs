using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;

namespace eLocal.Controllers
{
    public class Google_CalendarController : PublicBaseController
    {
        //
        // GET: /Google_Calendar/

        public ActionResult Index(string img)
        {
            if (img != null) {
                Response.Redirect("~/Content/pdf_icon.png");
            }
            /* This version of the Google Calendar Authentication will work easier if we want to database credentials */
            /*CalendarService cal_service = new CalendarService("curtmfg-eLocal-1.0");
            cal_service.setUserCredentials("ninnemana@gmail.com", "d3fj@m84");

            CalendarQuery cal_query = new CalendarQuery();
            cal_query.Uri = new Uri("https://www.google.com/calendar/feeds/default/allcalendars/full");
            CalendarFeed cal_feed = (CalendarFeed)cal_service.Query(cal_query);
            foreach (CalendarEntry entry in cal_feed.Entries) {
                Response.Write(entry.Title.Text + "\n");
            }*/


            ViewBag.googlecal_entry = null;
            try{
                /*string google_token = Session["google_token"].ToString();
                GAuthSubRequestFactory authFactory = new GAuthSubRequestFactory("cl", "eLocalCalender");
                authFactory.Token = google_token;
                Service service = new Service("cl", authFactory.ApplicationName);
                service.RequestFactory = authFactory;

                CalendarQuery query = new CalendarQuery();
                query.Uri = new Uri("https://www.google.com/calendar/feeds/default/allcalendars/full");
                //CalendarFeed feed = (CalendarFeed)service.Query(query);
                AtomFeed feed = (AtomFeed)service.Query(query);

                AtomEntry entry = (from cal in feed.Entries
                                   where cal.Title.Text.Equals("Outlook Calendar")
                                   select cal).FirstOrDefault<AtomEntry>();
                ViewBag.googlecal_entry = entry;*/
                ViewBag.hide_sidebar = true;
            } catch(Exception e) {
                Response.Redirect("~/Google_Calendar/Authenticate?error="+e.Message);
            }

            return View();
            
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public void RetrieveToken() {

            Session["google_token"] = AuthSubUtil.exchangeForSessionToken(Request.QueryString["token"], null);
            Response.Redirect("~/Google_Calendar");
        }

        public ActionResult Authenticate() {
            return View();
        }

    }
}
