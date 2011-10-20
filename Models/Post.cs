using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLocal.Models
{
    public class PostModel
    {
        public static List<Post> GetAll()
        {
            try
            {
                eLocalDataContext db = new eLocalDataContext();
                List<Post> posts = new List<Post>();

                posts = db.Posts.Where(x => x.active == true).OrderBy(x => x.publishedDate).OrderBy(x => x.createdDate).ToList<Post>();

                return posts;
            }
            catch (Exception e)
            {
                return new List<Post>();
            }
        }

        public static List<PostWithCategories> GetAllPublished(int page = 1, int pageSize = 5)
        {
            try
            {
                eLocalDataContext db = new eLocalDataContext();
                List<PostWithCategories> posts = new List<PostWithCategories>();

                posts = (from p in db.Posts
                         where p.publishedDate.Value <= DateTime.Now && p.active.Equals(true)
                         orderby p.publishedDate descending
                         select new PostWithCategories
                         {
                             postID = p.postID,
                             SiteContent = (from sc in db.SiteContents where sc.contentID.Equals(p.siteContentID) select sc).First<SiteContent>(),
                             publishedDate = p.publishedDate,
                             createdDate = p.createdDate,
                             lastModified = p.lastModified,
                             meta_title = p.meta_title,
                             meta_description = p.meta_description,
                             active = p.active,
                             author = (from a in db.Authors where a.authorID.Equals(p.authorID) select a).First<Author>(),
                             categories = (from c in db.Categories join pc in db.Post_Categories on c.CategoryID equals pc.CategoryID where pc.postID.Equals(p.postID) select c).ToList<eLocal.Category>(),
                             comments = (from cm in db.Comments where cm.postID.Equals(p.postID) && cm.active.Equals(true) && cm.approved.Equals(true) select cm).ToList<Comment>()
                         }).Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return posts;
            }
            catch (Exception e)
            {
                return new List<PostWithCategories>();
            }
        }

        public static List<PostWithCategories> GetAllPublishedByCategory(int id = 0, int page = 1, int pageSize = 5) {
            try {
                eLocalDataContext db = new eLocalDataContext();
                List<PostWithCategories> posts = new List<PostWithCategories>();

                posts = (from p in db.Posts
                         join pca in db.Post_Categories on p.postID equals pca.postID
                         where p.publishedDate.Value <= DateTime.Now && p.active.Equals(true) && pca.CategoryID.Equals(id)
                         orderby p.publishedDate descending
                         select new PostWithCategories {
                             postID = p.postID,
                             SiteContent = (from sc in db.SiteContents where sc.contentID.Equals(p.siteContentID) select sc).First<SiteContent>(),
                             publishedDate = p.publishedDate,
                             createdDate = p.createdDate,
                             lastModified = p.lastModified,
                             meta_title = p.meta_title,
                             meta_description = p.meta_description,
                             active = p.active,
                             author = (from a in db.Authors where a.authorID.Equals(p.authorID) select a).First<Author>(),
                             categories = (from c in db.Categories join pc in db.Post_Categories on c.CategoryID equals pc.CategoryID where pc.postID.Equals(p.postID) select c).ToList<eLocal.Category>(),
                             comments = (from cm in db.Comments where cm.postID.Equals(p.postID) && cm.active.Equals(true) && cm.approved.Equals(true) select cm).ToList<Comment>()
                         }).Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return posts;
            } catch (Exception e) {
                return new List<PostWithCategories>();
            }
        }

        public static List<PostWithCategories> GetAllPublishedByDate(string month = "", string year = "", int page = 1, int pageSize = 5) {
            try {
                eLocalDataContext db = new eLocalDataContext();
                List<PostWithCategories> posts = new List<PostWithCategories>();
                DateTime startDate = Convert.ToDateTime(month + " 1, " + year);
                DateTime endDate = startDate.AddMonths(1);

                posts = (from p in db.Posts
                         where p.publishedDate.Value <= endDate && p.publishedDate.Value >= startDate && p.active.Equals(true)
                         orderby p.publishedDate descending
                         select new PostWithCategories {
                             postID = p.postID,
                             SiteContent = (from sc in db.SiteContents where sc.contentID.Equals(p.siteContentID) select sc).First<SiteContent>(),
                             publishedDate = p.publishedDate,
                             createdDate = p.createdDate,
                             lastModified = p.lastModified,
                             meta_title = p.meta_title,
                             meta_description = p.meta_description,
                             active = p.active,
                             author = (from a in db.Authors where a.authorID.Equals(p.authorID) select a).First<Author>(),
                             categories = (from c in db.Categories join pc in db.Post_Categories on c.CategoryID equals pc.CategoryID where pc.postID.Equals(p.postID) select c).ToList<eLocal.Category>(),
                             comments = (from cm in db.Comments where cm.postID.Equals(p.postID) && cm.active.Equals(true) && cm.approved.Equals(true) select cm).ToList<Comment>()
                         }).Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return posts;
            } catch (Exception e) {
                return new List<PostWithCategories>();
            }
        }

        public static int CountAllPublished() {
            try {
                eLocalDataContext db = new eLocalDataContext();
                int count = 0;
                count = (from p in db.Posts
                         where p.publishedDate.Value <= DateTime.Now && p.active.Equals(true)
                         select p.postID
                         ).Count();
                return count;
            } catch (Exception e) {
                return 0;
            }
        }

        public static int CountAllPublishedByDate(string month = "", string year = "") {
            try {
                eLocalDataContext db = new eLocalDataContext();
                DateTime startDate = Convert.ToDateTime(month + " 1, " + year);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                int count = 0;
                count = (from p in db.Posts
                         where p.publishedDate.Value <= endDate && p.publishedDate.Value >= startDate && p.active.Equals(true)
                         select p.postID
                         ).Count();
                return count;
            } catch (Exception e) {
                return 0;
            }
        }

        public static int CountAllPublishedByCategory(int id = 0) {
            try {
                eLocalDataContext db = new eLocalDataContext();
                int count = 0;
                count = (from p in db.Posts
                         join pca in db.Post_Categories on p.postID equals pca.postID
                         where p.publishedDate.Value <= DateTime.Now && p.active.Equals(true) && pca.CategoryID.Equals(id)
                         select p.postID
                         ).Count();
                return count;
            } catch (Exception e) {
                return 0;
            }
        }

        public static PostWithCategories Get(string date = "", string title = "")
        {
            try {
                DateTime post_date = Convert.ToDateTime(date);
                eLocalDataContext db = new eLocalDataContext();
                PostWithCategories post = new PostWithCategories();
                post = (from p in db.Posts
                         join sco in db.SiteContents on p.siteContentID equals sco.contentID
                         where sco.page_title.Equals(title) && Convert.ToDateTime(p.publishedDate).Day.Equals(post_date.Day)
                         && Convert.ToDateTime(p.publishedDate).Year.Equals(post_date.Year) && Convert.ToDateTime(p.publishedDate).Month.Equals(post_date.Month)
                         select new PostWithCategories
                         {
                             postID = p.postID,
                             SiteContent = (from sc in db.SiteContents where sc.contentID.Equals(p.siteContentID) select sc).First<SiteContent>(),
                             publishedDate = p.publishedDate,
                             createdDate = p.createdDate,
                             lastModified = p.lastModified,
                             meta_title = p.meta_title,
                             meta_description = p.meta_description,
                             active = p.active,
                             author = (from a in db.Authors where a.authorID.Equals(p.authorID) select a).First<Author>(),
                             categories = (from c in db.Categories join pc in db.Post_Categories on c.CategoryID equals pc.CategoryID where pc.postID.Equals(p.postID) select c).ToList<eLocal.Category>(),
                             comments = (from cm in db.Comments where cm.postID.Equals(p.postID) && cm.active.Equals(true) && cm.approved.Equals(true) select cm).ToList<Comment>()
                         }).First<PostWithCategories>();

                return post;
            } catch (Exception e) {
                return new PostWithCategories();
            }
        }

        public static Post GetById(int id = 0)
        {
            try {
                eLocalDataContext db = new eLocalDataContext();
                Post post = new Post();
                post = (from p in db.Posts
                        join sco in db.SiteContents on p.siteContentID equals sco.contentID
                        where p.postID.Equals(id)
                        select p).First<Post>();

                return post;
            } catch {
                return new Post();
            }
        }

        public static void Delete(int id = 0)
        {
            try
            {
                eLocalDataContext db = new eLocalDataContext();
                Post p = db.Posts.Where(x => x.postID == id).FirstOrDefault<Post>();
                p.active = false;
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
    
    public class PostWithCategories
    {
        public int postID { get; set; }
        public SiteContent SiteContent { get; set; }
        public DateTime? publishedDate { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime lastModified { get; set; }
        public Author author { get; set; }
        public string meta_title { get; set; }
        public string meta_description { get; set; }
        public bool active { get; set; }
        public List<eLocal.Category> categories { get; set; }
        public List<Comment> comments { get; set; }
    }

}