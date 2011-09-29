using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using eLocal.Models;

namespace eLocal.Controllers
{
    public class LocatorController : PublicBaseController
    {
        //
        // GET: /Locator/

        public ActionResult Index(){
            eLocalDataContext db = new eLocalDataContext();
            JavaScriptSerializer js = new JavaScriptSerializer();

            List<LocationWithServices> locations = (from l in db.Locations
                                                 select new LocationWithServices { 
                                                    locationID = l.locationID,
                                                    name = l.name,
                                                    phone = l.phone,
                                                    fax = l.fax,
                                                    email = l.email,
                                                    address = l.address,
                                                    city = l.city,
                                                    state = db.States.Where(x => x.stateID == l.stateID).Select(x => x.abbr).FirstOrDefault<string>(),
                                                    zip = Convert.ToInt32(l.zip),
                                                    isPrimary = l.isPrimary,
                                                    latitude = l.latitude,
                                                    longitude = l.longitude,
                                                    places_status = l.places_status,
                                                    places_reference = l.places_reference,
                                                    places_id = l.places_id,
                                                    services = (from s in db.Services
                                                                join ls in db.Location_Services on s.serviceID equals ls.serviceID
                                                                where ls.locationID.Equals(l.locationID)
                                                                select s).ToList<Service>()
                                                 }).ToList<LocationWithServices>();
            ViewBag.locations = locations;
            ViewBag.locations_json = js.Serialize(locations);
            ViewBag.states = APIModels.GetStates();

            return View();
        }

    }
}
