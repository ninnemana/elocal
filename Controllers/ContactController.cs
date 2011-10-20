using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLocal;
using eLocal.Models;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.Net;

namespace eLocal.Controllers
{
    public class ContactController : PublicBaseController
    {
        //
        // GET: /Contact/

        public ActionResult Index()
        {

            // get the different locations for this company

            eLocalDataContext db = new eLocalDataContext();
            List<LocationWithState> locations = new List<LocationWithState>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            try {
                locations = (from l in db.Locations
                             where l.email.Length > 0
                             select new LocationWithState {
                                 locationID = l.locationID,
                                 name = l.name,
                                 email = l.email,
                                 phone = l.phone,
                                 fax = l.fax,
                                 address = l.address,
                                 city = l.city,
                                 stateID = l.stateID,
                                 state = (from s in db.States
                                          where s.stateID.Equals(l.stateID)
                                          select s.abbr).FirstOrDefault<string>(),
                                 zip = (int)l.zip,
                                 latitude = l.latitude,
                                 longitude = l.longitude,
                                 isPrimary = l.isPrimary,
                                 places_id = l.places_id,
                                 places_reference = l.places_reference,
                                 places_status = l.places_status
                             }).Distinct().OrderBy(x => x.state).ToList<LocationWithState>();
                ViewBag.locations_json = js.Serialize(locations);
            } catch (Exception e) {
                ViewBag.error = e.Message;
            }
            ViewBag.locations = locations;
            ViewBag.hide_sidebar = true;
            return View();
        }

        public ActionResult Thank(string name = "", string contact = "", string message = "", string subject = "") {
            ViewBag.name = name;
            ViewBag.contact = contact;
            ViewBag.message = message;
            ViewBag.subject = subject;

            return View();
        }

        public string GetLocation(int id = 0) {
            eLocalDataContext db = new eLocalDataContext();
            LocationWithServices loc = new LocationWithServices();
            JavaScriptSerializer js = new JavaScriptSerializer();

            loc = (from l in db.Locations
                   where l.locationID.Equals(id)
                   select new LocationWithServices {
                       locationID = l.locationID,
                       name = l.name,
                       phone = l.phone,
                       fax = l.fax,
                       email = l.email,
                       address = l.address,
                       city = l.city,
                       state = (from st in db.States where st.stateID.Equals(l.stateID) select st.abbr).FirstOrDefault<string>(),
                       stateID = l.stateID,
                       zip = Convert.ToInt32(l.zip),
                       isPrimary = l.isPrimary,
                       latitude = l.latitude,
                       longitude = l.longitude,
                       places_status = l.places_status,
                       places_reference = l.places_reference,
                       places_id = l.places_id,
                       services = (from s in db.Services join ls in db.Location_Services on s.serviceID equals ls.serviceID where ls.locationID.Equals(id) select s).ToList<Service>()
                   }).FirstOrDefault<LocationWithServices>();
            return js.Serialize(loc);
        }

        public dynamic Send(string name = "", string email = "", string phone = "", int location = 0, string subject = "", string contact_type = "", string message = "") {
            try {
                Location loc = CompanyModel.GetLocation(location);


                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.curtmfg.com");

                mail.From = new MailAddress("no-reply@curthitch.biz");
                mail.To.Add(loc.email);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                string htmlBody = "";
                string contact = "";

                htmlBody += "<div style='max-width:400px'><h3>Contact from your website</h3>";
                htmlBody += "<h4>" + name + " is requesting contact via " + contact_type + ".</h4>";
                if (contact_type.ToUpper() == "PHONE") {
                    contact = phone;
                    htmlBody += "<p>" + name + "'s phone number is <strong>" + phone + "</strong></p>";
                    htmlBody += "<p>In case the phone number is invalid, we also collected the e-mail address: <strong>" + email + "</strong></p>";
                } else {
                    contact = email;
                    htmlBody += "<p>" + name + "'s e-mail is <strong>" + email + "</strong></p>";
                    htmlBody += "<p>In case the e-mail is invalid, we also collected the phone number: <strong>" + phone + "</strong></p>";
                }
                htmlBody += "<p>Below you will find the contents of the message.</p>";

                htmlBody += "<div style='border:2px solid #ccc;display:inline-block;padding:10px;padding-top:0px'>";
                htmlBody += "<p style='border-bottom:1px solid #ccc'><strong>" + subject + "</strong></p>";
                htmlBody += "<p>" + message + "</p></div></div>";
                mail.Body = htmlBody;

                SmtpServer.Port = 25;
                SmtpServer.Send(mail);
                return RedirectToAction("Thank", "Contact", new { name = name, contact = contact, message = message, subject = subject });
            } catch (Exception e) {
                return RedirectToAction("Index", "Contact");
            }
        }

    }

    public class LocationWithServices {
        public int locationID { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public int stateID { get; set; }
        public int zip { get; set; }
        public int isPrimary { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public string places_status { get; set; }
        public string places_reference { get; set; }
        public string places_id { get; set; }
        public List<Service> services { get; set; }
    }
}
