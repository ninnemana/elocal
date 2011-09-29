using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;
using System.Web.UI;
using System.IO;

namespace eLocal.Models {
    public class ImageRouteHandler : IRouteHandler{
        public IHttpHandler GetHttpHandler(RequestContext requestContext) {
            string filename = requestContext.RouteData.Values["filename"] as string;

            if (string.IsNullOrEmpty(filename)) {
                return null;
            } else {
                requestContext.HttpContext.Response.Clear();
                requestContext.HttpContext.Response.ContentType = GetContentType(requestContext.HttpContext.Request.Url.ToString());

                string filepath = requestContext.HttpContext.Server.MapPath("~/Content/img/pdf_icon.png");
                requestContext.HttpContext.Response.WriteFile(filepath);
                requestContext.HttpContext.Response.End();
            }
            return null;
        }

        private static string GetContentType(String path) {
            switch (Path.GetExtension(path)) {
                case ".bmp": return "Image/bmp";
                case ".gif": return "Image/gif";
                case ".jpg": return "Image/jpeg";
                case ".png": return "Image/png";
                default: break;
            }
            return "";
        }

    }
}