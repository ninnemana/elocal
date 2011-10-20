using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Xml.Linq;

namespace eLocal.Models {
    public class SearchModel {

        public static List<SearchResult> Search(string term = "") {
            try {
                List<SearchResult> results = new List<SearchResult>();

                SearchAsync searcher = new SearchAsync();
                eLocal.Models.SearchAsync.AsyncMethodCaller caller = new eLocal.Models.SearchAsync.AsyncMethodCaller(searcher.APISearch);

                IAsyncResult async_result = caller.BeginInvoke(term, out results, null, null);

                List<SearchResult> local_results = new List<SearchResult>();

                results = caller.EndInvoke(out results, async_result);

                results.AddRange(local_results);
                return results;
            } catch (Exception) {
                return new List<SearchResult>();
            }
        }
    }

    public class SearchAsync {
        public delegate List<SearchResult> AsyncMethodCaller(string term, out List<SearchResult> results);

        public List<SearchResult> APISearch(string term, out List<SearchResult> results) {
            try {
                List<SearchResult> result_list = new List<SearchResult>();
                XDocument result_xml = XDocument.Load("http://docs.curthitch.biz/api/Search_eLocal?search_term=" + term);

                result_list = (from r in result_xml.Descendants("Result")
                               select new SearchResult {
                                   link = r.Attribute("link").Value,
                                   short_desc = r.Attribute("short_desc").Value,
                                   descriptions = (from d in r.Descendants("description")
                                                   select d.Value).ToList<string>()
                               }).ToList<SearchResult>();

                results = result_list;
                return results;
            } catch (Exception) {
                results = new List<SearchResult>();
                return results;
            }
        }
    }
}