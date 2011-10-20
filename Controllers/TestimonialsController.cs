using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLocal.Models;

namespace eLocal.Controllers {
    public class TestimonialsController : PublicBaseController {
        //
        // GET: /Testimonials/

        public ActionResult Index() {

            // Get the five newest testimonials
            List<Testimonial> tests = TestimonialModel.Get(5);
            ViewBag.tests = tests;

            return View();
        }

        public ActionResult Add(string reviewer = "", string review = "", string msg = "") {
            ViewBag.reviewer = reviewer;
            ViewBag.review = review;
            ViewBag.msg = msg;

            return View();
        }

        public dynamic Save(string reviewer = "", string review = "") {
            try {
                Company comp = ViewBag.company;

                TestimonialModel.Add(reviewer, review, comp.testimonial_submission);

                return RedirectToAction("Index", "Testimonials");
            } catch (Exception e) {
                return RedirectToAction("Add", "Testimonials", new { msg = e.Message });
            }
        }

    }
}
