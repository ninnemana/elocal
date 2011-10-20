using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLocal.Models {
    public class TestimonialModel {

        /// <summary>
        /// Return a List of Testimonials
        /// </summary>
        /// <param name="count">Number to return, returns all if not set</param>
        /// <returns>List of Testimonials</returns>
        public static List<Testimonial> Get(int count = 0) {
            try {
                eLocalDataContext db = new eLocalDataContext();
                if (count > 0) {
                    return db.Testimonials.Where(x => x.is_hidden == 0).OrderByDescending(x => x.date_added).Take(count).ToList<Testimonial>();
                } else {
                    return db.Testimonials.Where(x => x.is_hidden == 0).OrderByDescending(x => x.date_added).ToList<Testimonial>();
                }
            } catch (Exception) {
                return new List<Testimonial>();
            }
        }

        public static void Add(string reviewer = "", string review = "", string testimonial_submission = "") {
            if (reviewer.Length == 0) {
                reviewer = "Anonymous";
            }
            if (review.Length == 0) { throw new Exception("Testimonials cannot be blank."); }

            int is_hidden = 1;
            if (testimonial_submission == "PartiallyClosed") {
                is_hidden = 0;
            }

            eLocalDataContext db = new eLocalDataContext();
            Testimonial new_test = new Testimonial {
                reviewer = reviewer,
                review = review,
                date_added = DateTime.Now,
                is_new = 1,
                is_hidden = is_hidden
            };

            db.Testimonials.InsertOnSubmit(new_test);
            db.SubmitChanges();
        }
    }
}