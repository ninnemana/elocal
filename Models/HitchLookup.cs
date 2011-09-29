using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace eLocal.Models {
    public class HitchLookup {

        public static List<string> GetYears() {
            XDocument xml = XDocument.Load("http://docs.curthitch.biz/API/GetYear");
            List<string> years = (from y in xml.Descendants("Year")
                                  select y.Value).ToList<string>();
            return years;
        }

        public static List<string> GetMakes(double year) {
            XDocument xml = XDocument.Load("http://docs.curthitch.biz/API/GetMake?year=" + year);
            List<string> makes = (from m in xml.Descendants("Make")
                                  select m.Value).ToList<string>();
            return makes;
        }

        public static List<string> GetModels(double year, string make) {
            XDocument xml = XDocument.Load("http://docs.curthitch.biz/API/GetModel?year=" + year + "&make=" + make);
            List<string> models = (from m in xml.Descendants("Model")
                                   select m.Value).ToList<string>();
            return models;
        }

        public static List<string> GetStyles(double year, string make, string model) {
            XDocument xml = XDocument.Load("http://docs.curthitch.biz/API/GetStyle?year=" + year + "&make=" + make + "&model=" + model);
            List<string> styles = (from s in xml.Descendants("Style")
                                   select s.Value).ToList<string>();
            return styles;
        }

    }
}