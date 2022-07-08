using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using onlineBookAndVideoTutorial.Context;
using onlineBookAndVideoTutorial.Models;

namespace onlineBookAndVideoTutorial.Controllers
{
    public class StudentAdminController : Controller
    {
        private readonly tutorialContext db = new tutorialContext();

        public StudentAdminController()
        {
            
            if (System.Web.HttpContext.Current.Session["AdminId"] == null)
            {
                RedirectToAction("LoginAdmin", "Authentication");
            }
           
        }

        // GET: /StudentAdmin/
        public ActionResult StudentIndex()
        {
            ViewBag.StudentIndex = "active";
            return View(db.Registrations.ToList());
        }

        // GET: /StudentAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // GET: /StudentAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /StudentAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "Id,StudentName,DepartmentId,SemesterId,RegistrationNo,Email,Password,Image,Mobile")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Registrations.Add(registration);
                db.SaveChanges();
                return RedirectToAction("StudentIndex");
            }

            return View(registration);
        }

        // GET: /StudentAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Department = db.Departments.ToList();
            ViewBag.semester = db.Semesters.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: /StudentAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "Id,StudentName,DepartmentId,SemesterId,RegistrationNo,Email,Password,Image,Mobile")] Registration registration, HttpPostedFileBase Image, string pastImage)
        {
            registration.Image = pastImage;
            if (Image != null && Image.ContentLength > 0)
            {
                var fullPath = Request.MapPath("~/" + pastImage);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                try
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                    var uploadUrl = Server.MapPath("~/picture");
                    Image.SaveAs(Path.Combine(uploadUrl, fileName));
                    registration.Image = "picture/" + fileName;
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "ERROR:" + ex.Message;
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(registration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("StudentIndex");
            }
            return View(registration);
        }

        // GET: /StudentAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: /StudentAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var registration = db.Registrations.Find(id);
            db.Registrations.Remove(registration);
            db.SaveChanges();
            return RedirectToAction("StudentIndex");
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
