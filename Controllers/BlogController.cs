using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLocal.Models;
using Microsoft.Web.Helpers;
using System.Text.RegularExpressions;

namespace eLocal.Controllers {
    public class BlogController : PublicBaseController {
        //

        public ActionResult Index(int page = 1, int pageSize = 5) {

            List<PostWithCategories> posts = PostModel.GetAllPublished(page,pageSize);
            ViewBag.posts = posts;

            int postcount = PostModel.CountAllPublished();
            ViewBag.total = postcount;

            decimal pagecount = Math.Ceiling(Convert.ToDecimal(postcount) / Convert.ToDecimal(5));
            ViewBag.pagecount = Convert.ToInt32(pagecount);

            List<eLocal.Category> categories = eLocal.Models.BlogCategory.GetCategories();
            ViewBag.categories = categories;

            List<Archive> months = Archive.GetMonths();
            ViewBag.months = months;

            ViewBag.page = page;

            return View();
        }

        public ActionResult ViewPost(string date = "", string title = "")
        {

            PostWithCategories post = PostModel.Get(date,Server.UrlDecode(title));
            ViewBag.post = post;

            List<Archive> months = Archive.GetMonths();
            ViewBag.months = months;

            List<eLocal.Category> categories = eLocal.Models.BlogCategory.GetCategories();
            ViewBag.categories = categories;

            return View();
        }

        public ActionResult ViewCategory(string name = "", int page = 1, int pageSize = 5) {

            eLocal.Category category = eLocal.Models.BlogCategory.GetCategoryByName(name);
            ViewBag.category = category;

            List<PostWithCategories> posts = PostModel.GetAllPublishedByCategory(category.CategoryID,page,pageSize);
            ViewBag.posts = posts;

            List<Archive> months = Archive.GetMonths();
            ViewBag.months = months;
            
            List<eLocal.Category> categories = eLocal.Models.BlogCategory.GetCategories();
            ViewBag.categories = categories;

            int postcount = PostModel.CountAllPublishedByCategory(category.CategoryID);
            ViewBag.total = postcount;

            decimal pagecount = Math.Ceiling(Convert.ToDecimal(postcount) / Convert.ToDecimal(5));
            ViewBag.pagecount = Convert.ToInt32(pagecount);

            ViewBag.page = page;

            return View();
        }

        public ActionResult ViewArchive(string month = "", string year = "", int page = 1, int pageSize = 5) {

            List<PostWithCategories> posts = PostModel.GetAllPublishedByDate(month, year, page, pageSize);
            ViewBag.posts = posts;

            List<eLocal.Category> categories = eLocal.Models.BlogCategory.GetCategories();
            ViewBag.categories = categories;
            
            List<Archive> months = Archive.GetMonths();
            ViewBag.months = months;

            int postcount = PostModel.CountAllPublishedByDate(month, year);
            ViewBag.total = postcount;

            decimal pagecount = Math.Ceiling(Convert.ToDecimal(postcount) / Convert.ToDecimal(pageSize));
            ViewBag.pagecount = Convert.ToInt32(pagecount);
            
            ViewBag.month = month;
            ViewBag.year = year;
            ViewBag.page = page;

            return View();
        }
        
        [ValidateInput(false)]
        public ActionResult Comment(int id = 0, string name = "", string email = "", string comment_text = "")
        {
            Post post = PostModel.GetById(id);
            string postdate = String.Format("{0:M-d-yyyy}", post.publishedDate);
            try
            {
                if(!(ReCaptcha.Validate(privateKey: "6Levd8cSAAAAAO_tjAPFuXbfzj6l5viTEaz5YjVv")))
                {
                    throw new Exception("Recaptcha Validation Failed.");
                }
                if (id == 0) { throw new Exception("You must be on a blog post to add a comment"); }
                if (name == "") { throw new Exception("You must enter your name"); }
                if (email != "" & (!IsValidEmail(email))) { throw new Exception("Your email address is not a valid format."); }
                if (comment_text.Trim() == "") { throw new Exception("You must enter a comment"); }
                
                eLocalDataContext db = new eLocalDataContext();
                bool moderate = CompanyModel.Get().moderate_blog;
                eLocal.Comment comment = new eLocal.Comment
                {
                    postID = id,
                    name = name,
                    email = email,
                    comment_text = Regex.Replace(comment_text.Trim(),"<.*?>",string.Empty),
                    createdDate = DateTime.Now,
                    active = true,
                    approved = (moderate) ? false : true
                };
                db.Comments.InsertOnSubmit(comment);
                db.SubmitChanges();
                string message = "";
                if(moderate)
                    message = "Your comment has been submitted for approval.";
                return RedirectToRoute("BlogPost", new { message = message, title = post.SiteContent.page_title, date = postdate });
            } catch (Exception e) {
                return RedirectToRoute("BlogPost", new { err = e.Message, title = post.SiteContent.page_title, date = postdate, email = email, name = name, comment_text = comment_text });
            }
        }

        public static bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }
    }
}
