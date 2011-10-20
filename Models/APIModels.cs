using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Security.Cryptography;

namespace eLocal.Models {
    public class APIModels {

        public static string MakeJSONRequest(string url = "") {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream receiveStream = resp.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            return readStream.ReadToEnd();
        }

        public static XDocument MakeXMLRequest(string url = "") {
            XDocument doc = new XDocument(url);
            return doc;
        }

        public static Boolean FileExists(string path = "") {
            if (path == null || path.Trim().Length == 0) {
                return false;
            }
            WebRequest req = WebRequest.Create(path);
            req.Method = "HEAD";

            try {
                WebResponse resp = req.GetResponse();
                return true;
            } catch (Exception e) {
                return false;
            }
        }

        public static List<T> Shuffle<T>(IList<T> list) {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1) {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list.ToList();
        }

        public static List<State> GetStates() {
            try {
                eLocalDataContext db = new eLocalDataContext();
                List<State> states = new List<State>();
                states = db.States.OrderBy(x => x.abbr).ToList<State>();
                return states;
            } catch (Exception e) {
                return new List<State>();
            }
        }

    }

    public class APIPart {

        private List<APIAttribute> _content = new List<APIAttribute>();
        private List<APIAttribute> _attributes = new List<APIAttribute>();
        private int _partID = 0;
        private int _status = 0;
        private string _dateModified = "";
        private string _dateAdded = "";
        private string _shortDesc = "";
        private string _oldPartNumber = "";
        private string _listPrice = "";
        private string _pClass = "";
        private int _relatedCount = 0;

        public int partID {
            get {
                return this._partID;
            }
            set {
                if (value != null && value != this._partID) {
                    this._partID = value;
                }
            }
        }
        public int status {
            get {
                return this._status;
            }
            set {
                if (value != null && value != this._status) {
                    this._status = value;
                }
            }
        }
        public string dateModified {
            get {
                return this._dateModified;
            }
            set {
                if (value != null && value != this._dateModified) {
                    this._dateModified = value;
                }
            }
        }
        public string dateAdded {
            get {
                return this._dateAdded;
            }
            set {
                if (value != null && value != this._dateAdded) {
                    this._dateAdded = value;
                }
            }
        }
        public string shortDesc {
            get {
                return this._shortDesc;
            }
            set {
                if (value != null && value != this._shortDesc) {
                    this._shortDesc = value;
                }
            }
        }
        public string oldPartNumber {
            get {
                return this._oldPartNumber;
            }
            set {
                if (value != null && value != this._oldPartNumber) {
                    this._oldPartNumber = value;
                }
            }
        }
        public string listPrice {
            get {
                return this._listPrice;
            }
            set {
                if (value != null && value != this._listPrice) {
                    this._listPrice = value;
                }
            }
        }
        public List<APIAttribute> attributes {
            get {
                return this._attributes;
            }
            set {
                if (value != null && value != this._attributes) {
                    this._attributes = value;
                }
            }
        }
        public List<APIAttribute> content {
            get {
                return this._content;
            }
            set {
                if (value != null && value != this._content) {
                    this._content = value;
                }
            }
        }
        public string pClass {
            get {
                return this._pClass;
            }
            set {
                if (value != null && value != this._pClass) {
                    this._pClass = value;
                }
            }
        }

        public int relatedCount {
            get {
                return this._relatedCount;
            }
            set {
                if (value != null && value != this._relatedCount) {
                    this._relatedCount = value;
                }
            }
        }
        public int installTime { get; set; }
        public string drilling { get; set; }
        public string exposed { get; set; }
    }

    public class APIAttribute {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class CityState_Location {
        public int locationID { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }

    public class LocationWithState {
        public int locationID { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public int stateID { get; set; }
        public string state { get; set; }
        public int zip { get; set; }
        public int isPrimary { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public string places_id { get; set; }
        public string places_reference { get; set; }
        public string places_status { get; set; }
    }

    /// <summary>
    /// This object will hold strings containing the actual values of the year, make, model, and style for a vehicle.
    /// </summary>
    public class FullVehicle {
        public int vehicleID { get; set; }
        public int yearID { get; set; }
        public int makeID { get; set; }
        public int modelID { get; set; }
        public int styleID { get; set; }
        public int aaiaID { get; set; }
        public double year { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string style { get; set; }
    }

    public class SearchResult {
        public string link { get; set; }
        public string short_desc { get; set; }
        public List<string> descriptions { get; set; }
    }
}