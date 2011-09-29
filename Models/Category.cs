using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml;

namespace eLocal.Models {
    public class Category {
        private List<Category> _subs = new List<Category>();

        public int catID { get; set; }
        public int parentID { get; set; }
        public DateTime dateAdded { get; set; }
        public string catTitle { get; set; }
        public string shortDesc { get; set; }
        public string longDesc { get; set; }
        public string image { get; set; }
        public int isLifestyle { get; set; }
        public List<Category> sub_categories {
            get {
                return this._subs;
            }
            set {
                this._subs = value;
            }
        }

        public static List<Category> GetCategories(int catID = 0) {
            List<Category> cats = new List<Category>();
            if (catID != 0) {
                
                string url = "http://docs.curthitch.biz/API/GetCategories?parentID=" + catID;
                XDocument xml = XDocument.Load(url);
                cats = (from c in xml.Descendants("Category")
                        select new Category {
                            catID = Convert.ToInt32(c.Attribute("CatID").Value),
                            dateAdded = Convert.ToDateTime(c.Attribute("DateAdded").Value),
                            parentID = Convert.ToInt32(c.Attribute("ParentID").Value),
                            catTitle = c.Attribute("CategoryName").Value,
                            shortDesc = c.Attribute("ShortDesc").Value,
                            longDesc = c.Attribute("LongDesc").Value,
                            image = c.Attribute("image").Value,
                            isLifestyle = Convert.ToInt32(c.Attribute("isLifestyle").Value)
                        }).ToList<Category>();
            }
            return cats;
        }

        public static List<Category> GetParentCategories() {
            try {
                // Get all the categories
                XDocument xml = XDocument.Load("http://docs.curthitch.biz/API/GetParentCategories");
                List<Category> parent_cats = (from c in xml.Descendants("Category")
                                              select new Category {
                                                  catID = Convert.ToInt32(c.Attribute("CatID").Value),
                                                  dateAdded = Convert.ToDateTime(c.Attribute("DateAdded").Value),
                                                  parentID = Convert.ToInt32(c.Attribute("ParentID").Value),
                                                  catTitle = c.Attribute("CategoryName").Value,
                                                  shortDesc = c.Attribute("ShortDesc").Value,
                                                  longDesc = c.Attribute("LongDesc").Value,
                                                  image = c.Attribute("image").Value,
                                                  isLifestyle = Convert.ToInt32(c.Attribute("isLifestyle").Value),
                                                  sub_categories = Category.GetCategories(Convert.ToInt32(c.Attribute("CatID").Value))
                                              }).ToList<Category>();
                return parent_cats;
                } catch (Exception e) {
                return new List<Category>();
            }
            
        }

        public List<Category> GetCategories() {
            List<Category> cats = new List<Category>();
            if (this.catID != 0) {

                string url = "http://docs.curthitch.biz/API/GetCategories?parentID=" + this.catID;
                XDocument xml = XDocument.Load(url);
                cats = (from c in xml.Descendants("Category")
                        select new Category {
                            catID = Convert.ToInt32(c.Attribute("CatID").Value),
                            dateAdded = Convert.ToDateTime(c.Attribute("DateAdded").Value),
                            parentID = Convert.ToInt32(c.Attribute("ParentID").Value),
                            catTitle = c.Attribute("CategoryName").Value,
                            shortDesc = c.Attribute("ShortDesc").Value,
                            longDesc = c.Attribute("LongDesc").Value,
                            image = c.Attribute("image").Value,
                            isLifestyle = Convert.ToInt32(c.Attribute("isLifestyle").Value)
                        }).ToList<Category>();
            }
            return cats;
        }

        public Category GetByName() {
            Category cat = new Category();
            string url = "http://docs.curthitch.biz/API/GetCategoryByName?catName=" + this.catTitle;
            try {
                
                XDocument cat_xml = XDocument.Load(url);
                cat = (from c in cat_xml.Descendants("Category")
                       select new Category {
                           catID = Convert.ToInt32(c.Attribute("CatID").Value),
                           parentID = Convert.ToInt32(c.Attribute("ParentID").Value),
                           dateAdded = Convert.ToDateTime(c.Attribute("DateAdded").Value),
                           catTitle = c.Attribute("CategoryName").Value,
                           shortDesc = c.Attribute("ShortDesc").Value,
                           longDesc = c.Attribute("LongDesc").Value,
                           image = c.Attribute("image").Value,
                           isLifestyle = Convert.ToInt32(c.Attribute("isLifestyle").Value),
                           sub_categories = (c.Descendants("SubCategory") != null) ?
                                                (from c2 in c.Descendants("SubCategory")
                                                 select new Category {
                                                     catID = Convert.ToInt32(c.Attribute("CatID").Value),
                                                     parentID = Convert.ToInt32(c.Attribute("ParentID").Value),
                                                     dateAdded = Convert.ToDateTime(c.Attribute("DateAdded").Value),
                                                     catTitle = c.Attribute("CategoryName").Value,
                                                     shortDesc = c.Attribute("ShortDesc").Value,
                                                     longDesc = c.Attribute("LongDesc").Value,
                                                     image = c.Attribute("image").Value,
                                                     isLifestyle = Convert.ToInt32(c.Attribute("isLifestyle").Value)
                                                 }).ToList<Category>() : new List<Category>()
                       }).FirstOrDefault<Category>();

            } catch (Exception e) {
                cat.catTitle = e.Message;
            }
            return cat;
        }

        public Category Get() {

            Category cat = new Category();
            try {
                string url = "http://docs.curthitch.biz/API/GetCategory?catID=" + this.catID;
                XDocument xml = XDocument.Load(url);
                cat = (from c in xml.Descendants("Category")
                       select new Category {
                           catID = Convert.ToInt32(c.Attribute("CatID").Value),
                           dateAdded = Convert.ToDateTime(c.Attribute("DateAdded").Value),
                           parentID = Convert.ToInt32(c.Attribute("ParentID").Value),
                           catTitle = c.Attribute("CategoryName").Value,
                           shortDesc = c.Attribute("ShortDesc").Value,
                           longDesc = c.Attribute("LongDesc").Value,
                           image = c.Attribute("image").Value,
                           isLifestyle = Convert.ToInt32(c.Attribute("isLifestyle").Value),
                           sub_categories = (c.Descendants("SubCategory") != null) ?
                                                (from c2 in c.Descendants("SubCategory")
                                                 select new Category {
                                                     catID = Convert.ToInt32(c2.Attribute("CatID").Value),
                                                     parentID = Convert.ToInt32(c2.Attribute("ParentID").Value),
                                                     dateAdded = Convert.ToDateTime(c2.Attribute("DateAdded").Value),
                                                     catTitle = c2.Attribute("CategoryName").Value,
                                                     shortDesc = c2.Attribute("ShortDesc").Value,
                                                     longDesc = c2.Attribute("LongDesc").Value,
                                                     image = c2.Attribute("image").Value,
                                                     isLifestyle = Convert.ToInt32(c2.Attribute("isLifestyle").Value)
                                                 }).ToList<Category>() : new List<Category>()
                       }).FirstOrDefault<Category>();
                
            } catch (Exception e) { }
            return cat;
        }

        public List<APIPart> GetParts(int acctID = 0) {
            List<APIPart> parts = new List<APIPart>();
            string url = "http://docs.curthitch.biz/API/GetCategoryParts?catID=" + this.catID + "&cust_id=" + acctID;
            try {
                
                XDocument xml = XDocument.Load(url);
                parts = (from p in xml.Descendants("Part")
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
                             content = (from c in p.Descendants("Content")
                                        select new APIAttribute {
                                            key = c.Name.ToString(),
                                            value = c.Value
                                        }).ToList<APIAttribute>(),
                             attributes = (from pa in p.Descendants("Attributes").Descendants()
                                           select new APIAttribute {
                                               key = XmlConvert.DecodeName(pa.Name.ToString()),
                                               value = pa.Value
                                           }).ToList<APIAttribute>()
                         }).ToList<APIPart>();
            } catch (Exception e) {
                string err = e.Message;
            }
            return parts;
        }

    }
}
