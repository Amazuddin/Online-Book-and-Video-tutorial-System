using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using onlineBookAndVideoTutorial.Models;
using onlineBookAndVideoTutorial.Context;

namespace onlineBookAndVideoTutorial.Controllers
{
    public class VideoAdminController : Controller
    {
        private tutorialContext db = new tutorialContext();

        public VideoAdminController()
        {
            
            if (System.Web.HttpContext.Current.Session["AdminId"] == null)
            {
                RedirectToAction("LoginAdmin", "Authentication");
            }
           
        }

        // GET: /VideoAdmin/
        public ActionResult VideoIndex()
        {
            ViewBag.VideoIndex = "active";
            return View(db.Videos.ToList());
        }

        // GET: /VideoAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // GET: /VideoAdmin/Create
        public ActionResult Create()
        {
            ViewBag.Department = db.Departments.ToList();
            ViewBag.semester = db.Semesters.ToList();
            return View();
        }

        // POST: /VideoAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,VideoName,DepartmentId,SemesterId,Link")] Video video)
        {
            if (ModelState.IsValid)
            {
                db.Videos.Add(video);
                db.SaveChanges();
                return RedirectToAction("VideoIndex");
            }

            return View(video);
        }

        // GET: /VideoAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Department = db.Departments.ToList();
            ViewBag.semester = db.Semesters.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: /VideoAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,VideoName,DepartmentId,SemesterId,Link")] Video video)
        {
            if (ModelState.IsValid)
            {
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("VideoIndex");
            }
            return View(video);
        }

        // GET: /VideoAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: /VideoAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Video video = db.Videos.Find(id);
            db.Videos.Remove(video);
            db.SaveChanges();
            return RedirectToAction("VideoIndex");
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
