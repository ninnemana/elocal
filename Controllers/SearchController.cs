using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLocal.Models;
using System.Diagnostics;

namespace eLocal.Controllers {
    public class SearchController : PublicBaseController {
        //
        // GET: /Search/

        public ActionResult Index(string search = "", int page = 1) {
            List<SearchResult> results = new List<SearchResult>();
            if (search.Length > 0) {
                results = SearchModel.Search(search);
            } else {
                try {
                    List<SearchResult> cached_results = (List<SearchResult>)TempData["full_results"];
                    results = (cached_results == null) ? new List<SearchResult>() : cached_results;
                } catch (Exception) {
                    results = new List<SearchResult>();
                }
            }
            int skip_count = 0;
            if (results.Count > 25) {
                skip_count = (page * 25) - 25;
                ViewBag.results = results.Skip(skip_count).Take(25).ToList<SearchResult>();
            } else {
                ViewBag.results = results;
            }
            ViewBag.term = search;
            ViewBag.current_page = page;
            ViewBag.skip_count = skip_count;
            ViewBag.result_count = results.Count;
            TempData["full_results"] = results;
            return View();
        }

    }
}
