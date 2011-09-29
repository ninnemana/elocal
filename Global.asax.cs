using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eLocal
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // http://localhost/Category/Ball Mounts
            /*routes.MapRoute(
                "Category",
                "Category/{cat_name}",
                new { controller = "Category", action = "ByName", cat_name = "" }
            );*/

            //http://elocal.curtmfg.com/search/searching...
            routes.MapRoute("Search", "Search/{term}", new { controller = "Search", action = "Index", term = "" });

            routes.MapRoute(
                "Part",
                "Part/{id}",
                new { controller = "Parts", action = "Index", id = "" }
            );

            // http://localhost/Parts/11000
            routes.MapRoute(
                "Parts",
                "Parts/{id}",
                new { controller = "Parts", action = "Index", id = "" }
            );

            // http://localhost/Related/11000
            routes.MapRoute(
                "Related",
                "Related/{partID}",
                new { controller = "Parts", action = "Related", partID = "" }
            );

            // http://localhost/FindHitch/2010/Ford/Edge/All, Except Sport
            routes.MapRoute(
                "FindHitch",
                "FindHitch/{year}/{make}/{model}/{style}",
                new { controller = "FindHitch", action = "Index", year = "", make = "", model = "", style = "", sort = "" }
            );

            // http://localhost/Blog/Archive/January/2011/page/1, page is optional
            routes.MapRoute(
                "BlogArchive",
                "Blog/Archive/{month}/{year}/{pagetext}/{page}",
                new { controller = "Blog", action = "ViewArchive", month = "", year = "", pagetext = "page", page = UrlParameter.Optional }
            );

            // http://localhost/Blog/Category/Hitches/page/1, page is optional
            routes.MapRoute(
                "BlogCategory",
                "Blog/Category/{name}/{pagetext}/{page}",
                new { controller = "Blog", action = "ViewCategory", name = "", pagetext = "page", page = UrlParameter.Optional }
            );

            // http://localhost/Blog/Post/Comment/1, the number is the post id
            routes.MapRoute(
                "BlogPostComment",
                "Blog/Post/Comment/{id}",
                new { controller = "Blog", action = "Comment", id = "", message = UrlParameter.Optional }
            );

            // http://localhost/Blog/8-24-2011/This+is+a+blog+post+title
            routes.MapRoute(
                "BlogPost",
                "Blog/Post/{date}/{title}",
                new { controller = "Blog", action = "ViewPost", date = "", title = ""}
            );

            // http://localhost/Blog/8-24-2011/This+is+a+blog+post+title
            routes.MapRoute(
                "BlogPage",
                "Blog/page/{page}",
                new { controller = "Blog", action = "Index", page = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Index", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
