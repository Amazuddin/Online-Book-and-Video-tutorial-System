using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onlineBookAndVideoTutorial.Context;
using onlineBookAndVideoTutorial.Models;

namespace onlineBookAndVideoTutorial.Controllers
{
    public class AuthenticationController : Controller
    {

        private tutorialContext db = new tutorialContext();
        //
        // GET: /Authentication/
        public ActionResult Register()
        {
            ViewBag.Department = db.Departments.ToList();
            ViewBag.semester = db.Semesters.ToList();
            ViewBag.Register = "active";
            if (Session["StudentId"] != null)
            {
                return RedirectToAction("HomePage", "Primary");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Register(Registration registration, HttpPostedFileBase Image)
        {
            ViewBag.Department = db.Departments.ToList();
            ViewBag.semester = db.Semesters.ToList();
            ViewBag.Register = "active";
            if (Image != null && Image.ContentLength > 0)
            {
                try
                {
                    string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(Image.FileName);
                    string uploadUrl = Server.MapPath("~/picture");
                    Image.SaveAs(Path.Combine(uploadUrl, fileName));
                    registration.Image = "picture/" + fileName;
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "ERROR:" + ex.Message.ToString();
                }
            }
            int k = db.Registrations.Count(r => r.Email == registration.Email);
            if (k == 0)
            {
                db.Registrations.Add(registration);
                db.SaveChanges();
            }
            else
            {
                ViewBag.Error = "Another Student is Registered with this Email";
                return View();
            }

            return RedirectToAction("Login");
        }




        public ActionResult Login(string id)
        {
            ViewBag.Login = "active";
            if (Session["StudentId"] != null)
            {
                return RedirectToAction("HomePage", "Primary");
            }
            ViewBag.Error = id;
            return View();
        }

        public ActionResult LoginUser(string username, string password)
        {
            Registration hs = db.Registrations.FirstOrDefault(r => r.Email == username && r.Password == password);
            if (hs != null)
            {
                Session["StudentId"] = hs.Id;
                Session["FarmerEmail"] = hs.Email;
                return RedirectToAction("HomePage", "Primary");
            }
            else
                return RedirectToAction("Login", "Authentication", new { id = "Error" });

        }
        public ActionResult Logout()
        {
            Session["StudentId"] = null;
            return RedirectToAction("Login");
        }



        //******************** Admin************************

        public ActionResult LoginAdmin(string id)
        {
            ViewBag.LoginAdmin = "active";
            if (Session["AdminId"] != null)
            {
                return RedirectToAction("StudentIndex", "StudentAdmin");
            }
            ViewBag.Error = id;
            return View();
        }
        [HttpPost]
        public ActionResult LoginAdmin(string username, string password)
        {
            Admin hs = db.Admins.FirstOrDefault(r => r.Email == username && r.Password == password);
            if (hs != null)
            {
                Session["AdminId"] = hs.Id;

                return RedirectToAction("StudentIndex", "StudentAdmin");
            }
            else
                return RedirectToAction("LoginAdmin", "Authentication", new { id = "Error" });
        }

        public ActionResult LogoutAdmin()
        {

            Session["AdminId"] = null;
            return RedirectToAction("LoginAdmin");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

	}
}