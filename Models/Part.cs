using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Web;
using System.Xml;

namespace eLocal.Models {
    class Part {

        /// <summary>
        /// Make call to the API and return a APIPart object
        /// </summary>
        /// <param name="partID">ID of the part to retrieve</param>
        /// <returns>APIPart</returns>
        public static APIPart GetPart(int partID, int acctID = 0) {
            try {
                XDocument part_xml = XDocument.Load("http://docs.curthitch.biz/API/GetPart?partID=" + partID + "&cust_id=" + acctID);
                APIPart part = new APIPart();
                part = (from p in part_xml.Descendants("Part")
                        select new APIPart {
                            partID = Convert.ToInt32(p.Attribute("partID").Value),
                            status = Convert.ToInt32(p.Attribute("status").Value),
                            dateModified = p.Attribute("dateModified").Value,
                            dateAdded = p.Attribute("dateAdded").Value,
                            shortDesc = p.Attribute("shortDesc").Value,
                            oldPartNumber = p.Attribute("oldPartNumber").Value,
                            listPrice = p.Attribute("listPrice").Value,
                            pClass = p.Attribute("pClass").Value,
                            relatedCount = Convert.ToInt32(p.Attribute("relatedCount").Value),
                            attributes = (from a in p.Descendants("Attributes").Descendants()
                                          select new APIAttribute {
                                              key = a.Name.ToString(),
                                              value = a.Value
                                          }).ToList<APIAttribute>(),
                            content = (from c in p.Descendants("Content").Descendants()
                                       select new APIAttribute {
                                           key = c.Name.ToString(),
                                           value = c.Value
                                       }).ToList<APIAttribute>()
                        }).FirstOrDefault<APIPart>();
                return part;
            } catch (Exception) {
                return new APIPart();
            }
        }

        public static List<APIPart> GetRelated(int partID, string sorting = "", int accID = 0) {
            XDocument related_xml = XDocument.Load("http://docs.curthitch.biz/API/GetRelatedParts?partID=" + partID + "&cust_id=" + accID);
            List<APIPart> parts = new List<APIPart>();
            if (sorting == "") {
                parts = (from p in related_xml.Descendants("Part")
                         select new APIPart {
                             partID = Convert.ToInt32(p.Attribute("partID").Value),
                             status = Convert.ToInt32(p.Attribute("status").Value),
                             dateModified = p.Attribute("dateModified").Value,
                             dateAdded = p.Attribute("dateAdded").Value,
                             shortDesc = p.Attribute("shortDesc").Value,
                             oldPartNumber = p.Attribute("oldPartNumber").Value,
                             listPrice = p.Attribute("listPrice").Value,
                             pClass = p.Attribute("pClass").Value,
                             relatedCount = Convert.ToInt32(p.Attribute("relatedCount").Value),
                             attributes = (from a in p.Descendants("Attributes").Descendants()
                                           select new APIAttribute {
                                               key = XmlConvert.DecodeName(a.Name.ToString()),
                                               value = a.Value
                                           }).ToList<APIAttribute>(),
                             content = (from c in p.Descendants("Content").Descendants()
                                        select new APIAttribute {
                                            key = XmlConvert.DecodeName(c.Name.ToString()),
                                            value = c.Value
                                        }).ToList<APIAttribute>()
                         }).OrderBy(x => x.pClass).OrderBy(x => x.partID).ToList<APIPart>();
            } else {
                #region Sorting Options
                switch (sorting) {
                    case "low_price":
                        parts = (from p in related_xml.Descendants("Part")
                                 select new APIPart {
                                     partID = Convert.ToInt32(p.Attribute("partID").Value),
                                     status = Convert.ToInt32(p.Attribute("status").Value),
                                     dateModified = p.Attribute("dateModified").Value,
                                     dateAdded = p.Attribute("dateAdded").Value,
                                     shortDesc = p.Attribute("shortDesc").Value,
                                     oldPartNumber = p.Attribute("oldPartNumber").Value,
                                     listPrice = p.Attribute("listPrice").Value,
                                     pClass = p.Attribute("pClass").Value,
                                     attributes = (from a in p.Descendants("Attributes").Descendants()
                                                   select new APIAttribute {
                                                       key = a.Name.ToString(),
                                                       value = a.Value
                                                   }).ToList<APIAttribute>(),
                                     content = (from c in p.Descendants("Content").Descendants()
                                                select new APIAttribute {
                                                    key = c.Name.ToString(),
                                                    value = c.Value
                                                }).ToList<APIAttribute>()
                                 }).OrderBy(x => x.pClass).OrderBy(x => Convert.ToDouble(x.listPrice.Replace("$", ""))).ToList<APIPart>();
                        break;

                    case "high_price":
                        parts = (from p in related_xml.Descendants("Part")
                                 select new APIPart {
                                     partID = Convert.ToInt32(p.Attribute("partID").Value),
                                     status = Convert.ToInt32(p.Attribute("status").Value),
                                     dateModified = p.Attribute("dateModified").Value,
                                     dateAdded = p.Attribute("dateAdded").Value,
                                     shortDesc = p.Attribute("shortDesc").Value,
                                     oldPartNumber = p.Attribute("oldPartNumber").Value,
                                     listPrice = p.Attribute("listPrice").Value,
                                     pClass = p.Attribute("pClass").Value,
                                     attributes = (from a in p.Descendants("Attributes").Descendants()
                                                   select new APIAttribute {
                                                       key = a.Name.ToString(),
                                                       value = a.Value
                                                   }).ToList<APIAttribute>(),
                                     content = (from c in p.Descendants("Content").Descendants()
                                                select new APIAttribute {
                                                    key = c.Name.ToString(),
                                                    value = c.Value
                                                }).ToList<APIAttribute>()
                                 }).OrderBy(x => x.pClass).OrderByDescending(x => Convert.ToDouble(x.listPrice.Replace("$", ""))).ToList<APIPart>();
                        break;

                    default:
                        parts = (from p in related_xml.Descendants("Part")
                                 select new APIPart {
                                     partID = Convert.ToInt32(p.Attribute("partID").Value),
                                     status = Convert.ToInt32(p.Attribute("status").Value),
                                     dateModified = p.Attribute("dateModified").Value,
                                     dateAdded = p.Attribute("dateAdded").Value,
                                     shortDesc = p.Attribute("shortDesc").Value,
                                     oldPartNumber = p.Attribute("oldPartNumber").Value,
                                     listPrice = p.Attribute("listPrice").Value,
                                     pClass = p.Attribute("pClass").Value,
                                     attributes = (from a in p.Descendants("Attributes").Descendants()
                                                   select new APIAttribute {
                                                       key = a.Name.ToString(),
                                                       value = a.Value
                                                   }).ToList<APIAttribute>(),
                                     content = (from c in p.Descendants("Content").Descendants()
                                                select new APIAttribute {
                                                    key = c.Name.ToString(),
                                                    value = c.Value
                                                }).ToList<APIAttribute>()
                                 }).OrderBy(x => x.pClass).OrderBy(x => x.partID).ToList<APIPart>();
                        break;

                }
                #endregion
            }
            return parts;
        }

        public static List<APIPart> GetVehicleParts(string year, string make, string model, string style, string sort = "", int acctID = 0) {
            string url_params = "year=" + HttpUtility.UrlEncode(year) + "&make=" + HttpUtility.UrlEncode(make) + "&model=" + HttpUtility.UrlEncode(model) + "&style=" + HttpUtility.UrlEncode(style) + "&cust_id=" + acctID;
            string url = "http://docs.curthitch.biz/API/GetParts?" + url_params;
            XDocument hitch_xml = XDocument.Load(url);
            List<APIPart> parts = new List<APIPart>();
            if (sort == "") {
                parts = (from p in hitch_xml.Descendants("Part")
                         select new APIPart {
                             partID = Convert.ToInt32(p.Attribute("partID").Value),
                             status = Convert.ToInt32(p.Attribute("status").Value),
                             dateModified = p.Attribute("dateModified").Value,
                             dateAdded = p.Attribute("dateAdded").Value,
                             shortDesc = p.Attribute("shortDesc").Value,
                             oldPartNumber = p.Attribute("oldPartNumber").Value,
                             listPrice = p.Attribute("listPrice").Value,
                             pClass = p.Attribute("pClass").Value,
                             relatedCount = Convert.ToInt32(p.Attribute("relatedCount").Value),
                             attributes = (from a in p.Descendants("Attributes").Descendants()
                                           select new APIAttribute {
                                               key = a.Name.ToString(),
                                               value = a.Value
                                           }).ToList<APIAttribute>(),
                             content = (from c in p.Descendants("Content").Descendants()
                                        select new APIAttribute {
                                            key = c.Name.ToString(),
                                            value = c.Value
                                        }).ToList<APIAttribute>(),
                             installTime = Convert.ToInt32(p.Attribute("installTime").Value),
                             drilling = p.Attribute("drilling").Value,
                             exposed = p.Attribute("exposed").Value
                         }).OrderBy(x => x.pClass).OrderBy(x => x.partID).ToList<APIPart>();
            } else {
                #region Sorting Options
                switch (sort) {
                    case "low_price":
                        parts = (from p in hitch_xml.Descendants("Part")
                                 select new APIPart {
                                     partID = Convert.ToInt32(p.Attribute("partID").Value),
                                     status = Convert.ToInt32(p.Attribute("status").Value),
                                     dateModified = p.Attribute("dateModified").Value,
                                     dateAdded = p.Attribute("dateAdded").Value,
                                     shortDesc = p.Attribute("shortDesc").Value,
                                     oldPartNumber = p.Attribute("oldPartNumber").Value,
                                     listPrice = p.Attribute("listPrice").Value,
                                     pClass = p.Attribute("pClass").Value,
                                     relatedCount = Convert.ToInt32(p.Attribute("relatedCount").Value),
                                     attributes = (from a in p.Descendants("Attributes").Descendants()
                                                   select new APIAttribute {
                                                       key = a.Name.ToString(),
                                                       value = a.Value
                                                   }).ToList<APIAttribute>(),
                                     content = (from c in p.Descendants("Content").Descendants()
                                                select new APIAttribute {
                                                    key = c.Name.ToString(),
                                                    value = c.Value
                                                }).ToList<APIAttribute>()
                                 }).OrderBy(x => x.pClass).OrderBy(x => Convert.ToDouble(x.listPrice.Replace("$", ""))).ToList<APIPart>();
                        break;

                    case "high_price":
                        parts = (from p in hitch_xml.Descendants("Part")
                                 select new APIPart {
                                     partID = Convert.ToInt32(p.Attribute("partID").Value),
                                     status = Convert.ToInt32(p.Attribute("status").Value),
                                     dateModified = p.Attribute("dateModified").Value,
                                     dateAdded = p.Attribute("dateAdded").Value,
                                     shortDesc = p.Attribute("shortDesc").Value,
                                     oldPartNumber = p.Attribute("oldPartNumber").Value,
                                     listPrice = p.Attribute("listPrice").Value,
                                     pClass = p.Attribute("pClass").Value,
                                     relatedCount = Convert.ToInt32(p.Attribute("relatedCount").Value),
                                     attributes = (from a in p.Descendants("Attributes").Descendants()
                                                   select new APIAttribute {
                                                       key = a.Name.ToString(),
                                                       value = a.Value
                                                   }).ToList<APIAttribute>(),
                                     content = (from c in p.Descendants("Content").Descendants()
                                                select new APIAttribute {
                                                    key = c.Name.ToString(),
                                                    value = c.Value
                                                }).ToList<APIAttribute>()
                                 }).OrderBy(x => x.pClass).OrderByDescending(x => Convert.ToDouble(x.listPrice.Replace("$", ""))).ToList<APIPart>();
                        break;

                    default:
                        parts = (from p in hitch_xml.Descendants("Part")
                                 select new APIPart {
                                     partID = Convert.ToInt32(p.Attribute("partID").Value),
                                     status = Convert.ToInt32(p.Attribute("status").Value),
                                     dateModified = p.Attribute("dateModified").Value,
                                     dateAdded = p.Attribute("dateAdded").Value,
                                     shortDesc = p.Attribute("shortDesc").Value,
                                     oldPartNumber = p.Attribute("oldPartNumber").Value,
                                     listPrice = p.Attribute("listPrice").Value,
                                     pClass = p.Attribute("pClass").Value,
                                     relatedCount = Convert.ToInt32(p.Attribute("relatedCount").Value),
                                     attributes = (from a in p.Descendants("Attributes").Descendants()
                                                   select new APIAttribute {
                                                       key = a.Name.ToString(),
                                                       value = a.Value
                                                   }).ToList<APIAttribute>(),
                                     content = (from c in p.Descendants("Content").Descendants()
                                                select new APIAttribute {
                                                    key = c.Name.ToString(),
                                                    value = c.Value
                                                }).ToList<APIAttribute>()
                                 }).OrderBy(x => x.pClass).OrderBy(x => x.partID).ToList<APIPart>();
                        break;

                }
                #endregion
            }
            return parts;
        }

        public static List<FullVehicle> GetPartVehicles(int partID) {
            try {
                List<FullVehicle> vehicles = new List<FullVehicle>();
                string url = "http://docs.curthitch.biz/API/GetPartVehicles?partID=" + partID;
                XDocument xml = XDocument.Load(url);
                vehicles = (from v in xml.Descendants("Vehicle")
                            select new FullVehicle {
                                vehicleID = Convert.ToInt32(v.Attribute("vehicleID").Value),
                                yearID = Convert.ToInt32(v.Attribute("vehicleID").Value),
                                makeID = Convert.ToInt32(v.Attribute("makeID").Value),
                                modelID = Convert.ToInt32(v.Attribute("modelID").Value),
                                styleID = Convert.ToInt32(v.Attribute("styleID").Value),
                                aaiaID = Convert.ToInt32(v.Attribute("aaiaID").Value),
                                year = Convert.ToDouble(v.Attribute("year").Value),
                                make = v.Attribute("make").Value,
                                model = v.Attribute("model").Value,
                                style = v.Attribute("style").Value
                            }).ToList<FullVehicle>();
                return vehicles;
            } catch (Exception e) {
                return new List<FullVehicle>();
            }

        }
    }
}
