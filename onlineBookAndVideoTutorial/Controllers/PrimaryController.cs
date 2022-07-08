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
    public class PrimaryController : Controller
    {
        public PrimaryController()
        {
            if (System.Web.HttpContext.Current.Session["StudentId"] == null)
                RedirectToAction("Login", "Authentication");
        }

        //
        // GET: /Primary/
        public ActionResult HomePage()
        {
            ViewBag.HomePage = "active";
            return View();
        }

        public ActionResult Videotutorial()
        {
            if (Session["StudentId"] == null)
                return RedirectToAction("LogIn", "Authentication");
            ViewBag.Videotutorial = "active";
            List<Department> departments;
            List<Semester> semesters;

            using (var db = new tutorialContext())
            {
                departments = db.Departments.ToList();
                semesters = db.Semesters.ToList();
            }

            ViewBag.department = departments;
            ViewBag.semester = semesters;

            return View();
        }

        public JsonResult GetAllVideobyId(int deptid, int semid)
        {
            var videos = new List<Video>();

            using (var db = new tutorialContext())
            {
                var a = db.Videos.Where(s => s.DepartmentId == deptid && s.SemesterId == semid);
                foreach (var k in a)
                {
                    var video = new Video();
                    video.VideoName = k.VideoName;
                    video.Link = k.Link;
                    videos.Add(video);
                }
            }

            return Json(videos);
        }

        public ActionResult OnlineBook()
        {
            if (Session["StudentId"] == null)
                return RedirectToAction("LogIn", "Authentication");
            ViewBag.OnlineBook = "active";
            List<Department> departments;
            List<Semester> semesters;

            using (var db = new tutorialContext())
            {
                departments = db.Departments.ToList();
                semesters = db.Semesters.ToList();
            }

            ViewBag.department = departments;
            ViewBag.semester = semesters;

            return View();
        }

        public JsonResult GetAllBookbyId(int deptid, int semid)
        {
            var books = new List<Book>();

            using (var db = new tutorialContext())
            {
                var a = db.Books.Where(s => s.DepartmentId == deptid && s.SemesterId == semid);
                foreach (var k in a)
                {
                    var book = new Book();
                    book.BookName = k.BookName;
                    book.AuthorName = k.AuthorName;
                    book.Pdf = k.Pdf;
                    books.Add(book);
                }
            }

            return Json(books);
        }

        public ActionResult Profile()
        {
            if (Session["StudentId"] == null)
                return RedirectToAction("LogIn", "Authentication");
            ViewBag.Profile = "active";
            var studentid = Convert.ToInt32(Session["StudentId"]);

            var stu = new Registration();
            using (var db = new tutorialContext())
            {
                var am = db.Registrations.Where(s => s.Id == studentid);
                foreach (var k in am)
                {
                    stu.StudentName = k.StudentName;
                    stu.Email = k.Email;
                    stu.Mobile = k.Mobile;
                    stu.RegistrationNo = k.RegistrationNo;
                    stu.Image = k.Image;
                }

                ViewBag.Allinfo = stu;
            }

            return View();
        }

        public ActionResult ProfileUpdate()
        {
            if (Session["StudentId"] == null)
                return RedirectToAction("LogIn", "Authentication");
            ViewBag.ProfileUpdate = "active";
            var stu = new Registration();
            var studentId = Convert.ToInt32(Session["StudentId"]);
            using (var db = new tutorialContext())
            {
                var u = db.Registrations.Where(k => k.Id == studentId).Select(c =>
                    new {c.StudentName, c.Email, c.Mobile, c.RegistrationNo, c.Image});
                foreach (var k in u)
                {
                    stu.StudentName = k.StudentName;
                    stu.Email = k.Email;
                    stu.Mobile = k.Mobile;
                    stu.RegistrationNo = k.RegistrationNo;
                    stu.Image = k.Image;
                }

                ViewBag.Student = stu;
                return View();
            }
        }

        public ActionResult Update(Registration student, HttpPostedFileBase Image, string pastImage)
        {
            if (Session["StudentId"] == null)
                return RedirectToAction("LogIn", "Authentication");
            var studentId = Convert.ToInt32(Session["StudentId"]);

            using (var db = new tutorialContext())
            {
                student.Image = pastImage;
                if (Image != null && Image.ContentLength > 0)
                {
                    var fullPath = Request.MapPath("~/" + pastImage);
                    if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
                    try
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                        var uploadUrl = Server.MapPath("~/picture");
                        Image.SaveAs(Path.Combine(uploadUrl, fileName));
                        student.Image = "picture/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = "ERROR:" + ex.Message;
                    }
                }

                var pa = db.Registrations.Single(e => e.Id == studentId);
                if (pa.Id == studentId)
                {
                    pa.StudentName = student.StudentName;
                    pa.Email = student.Email;
                    pa.Mobile = student.Mobile;
                    pa.RegistrationNo = student.RegistrationNo;
                    pa.Image = student.Image;
                    db.SaveChanges();
                    return RedirectToAction("Profile", "Primary");
                }

                return RedirectToAction("Profile", "Primary");
            }
        }
    }
}