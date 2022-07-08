using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using onlineBookAndVideoTutorial.Models;
using onlineBookAndVideoTutorial.Context;

namespace onlineBookAndVideoTutorial.Controllers
{
    public class BookAdminController : Controller
    {

        public BookAdminController()
        {
            
            if (System.Web.HttpContext.Current.Session["AdminId"] == null)
            {
                RedirectToAction("LoginAdmin", "Authentication");
            }
           
        }
        private tutorialContext db = new tutorialContext();

        // GET: /BookAdmin/
        public ActionResult BookIndex()
        {
            ViewBag.BookIndex = "active";
            return View(db.Books.ToList());
        }

        // GET: /BookAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: /BookAdmin/Create
        public ActionResult Create()
        {
            ViewBag.Department = db.Departments.ToList();
            ViewBag.semester = db.Semesters.ToList();
            return View();
        }

        // POST: /BookAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BookName,AuthorName,DepartmentId,SemesterId,Pdf")] Book book, HttpPostedFileBase Pdf)
        {
            if (ModelState.IsValid)
            {
                if (Pdf != null && Pdf.ContentLength > 0)
                {

                    try
                    {
                        string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(Pdf.FileName);
                        string uploadUrl = Server.MapPath("~/Bookpdf");
                        Pdf.SaveAs(Path.Combine(uploadUrl, fileName));
                        book.Pdf = "Bookpdf/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }

                book.BookName = book.BookName;
                book.AuthorName = book.AuthorName;
                book.DepartmentId = book.DepartmentId;
                book.SemesterId = book.SemesterId;
                db.Books.Add(book);
                db.SaveChanges();
            }

            return RedirectToAction("Create", new { message = "Book added Successfully" });
        }

        // GET: /BookAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Department = db.Departments.ToList();
            ViewBag.semester = db.Semesters.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: /BookAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BookName,AuthorName,DepartmentId,SemesterId,Pdf")] Book book, HttpPostedFileBase Pdf, string pastPdf)
        {

            book.Pdf = pastPdf;
            if (Pdf != null && Pdf.ContentLength > 0)
            {
                string fullPath = Request.MapPath("~/" + pastPdf);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                try
                {
                    string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(Pdf.FileName);
                    string uploadUrl = Server.MapPath("~/Bookpdf");
                    Pdf.SaveAs(Path.Combine(uploadUrl, fileName));
                    book.Pdf = "Bookpdf/" + fileName;
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "ERROR:" + ex.Message.ToString();
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("BookIndex");
            }
            return View(book);
        }

        // GET: /BookAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: /BookAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("BookIndex");
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
