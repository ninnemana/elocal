using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml;

namespace eLocal.Models {
    public class BlogCategory {

        public static List<eLocal.Category> GetCategories() {
            try {
                eLocalDataContext db = new eLocalDataContext();
                List<eLocal.Category> categories = new List<eLocal.Category>();

                categories = db.Categories.Where(x => x.active == true).OrderBy(x => x.name).ToList<eLocal.Category>();

                return categories;
            } catch {
                return new List<eLocal.Category>();
            }
        }

        public static eLocal.Category GetCategory(int id = 0) {
            try {
                eLocalDataContext db = new eLocalDataContext();
                eLocal.Category category = new eLocal.Category();

                category = db.Categories.Where(x => x.CategoryID == id).FirstOrDefault<eLocal.Category>();

                return category;
            } catch {
                return new eLocal.Category();
            }
        }

        public static eLocal.Category GetCategoryByName(string name = "") {
            try {
                eLocalDataContext db = new eLocalDataContext();
                eLocal.Category category = new eLocal.Category();

                category = db.Categories.Where(x => x.name == name).FirstOrDefault<eLocal.Category>();

                return category;
            } catch {
                return new eLocal.Category();
            }
        }

    }
}