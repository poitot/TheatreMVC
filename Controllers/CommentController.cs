using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LocalTheatre.Data;
using LocalTheatre.Web.Models;
using Microsoft.AspNet.Identity;
using LocalTheatre.Web.Extensions;

namespace LocalTheatre.Web.Controllers
{
    public class CommentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comment/Create
        [Authorize]
        public ActionResult Create()
        {
            if (db.Users.Find(User.Identity.GetUserId()).IsSuspended)
            {
                this.AddNotification("You are unable to post a comment as you are suspended", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FullName");
            if (Response.Cookies.Get("cookie_comment_text") != null)
            {
                CommentViewModel model = new CommentViewModel()
                {
                    Text = Request.Cookies["cookie_comment_text"].Value
                };

                Response.Cookies.Get("cookie_comment_text").Expires = DateTime.Now.AddDays(-10);
                return View(model);
            }
            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(CommentViewModel model, int id)
        {
            var comment = new Comment();
            if (ModelState.IsValid)
            {
                comment.Text = model.Text;
                comment.Date = DateTime.Now;
                comment.AuthorId = User.Identity.GetUserId();
                comment.Author = db.Users.Where(u => u.Id == comment.AuthorId).First();
                comment.Announcement = db.Announcements.Where(a => a.Id == id).First();

                db.Comments.Add(comment);
                db.SaveChanges();
                this.AddNotification("Comment Posted On " + comment.Announcement.Title, NotificationType.INFO);
                return RedirectToAction("Index", "Home");
            }

            this.AddNotification("Failed To Post Comment", NotificationType.INFO);
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FullName", comment.AuthorId);
            return View(comment);
        }

        // GET: Comment/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FullName", comment.AuthorId);
            ViewBag.AnnouncementId = comment.Announcement.Id;
            return View(comment);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Edit(Comment comment)
        {
            try
            {
                Comment temp = db.Comments.Find(comment.Id);
                temp.Text = comment.Text;
                temp.Announcement = db.Announcements.Find(temp.Announcement.Id);
                db.Entry(temp).State = EntityState.Modified;
                await db.SaveChangesAsync();
                this.AddNotification("Comment Updated Successfully", NotificationType.SUCCESS);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                this.AddNotification("Comment Could Not Be Updated", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Comment/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        [Authorize]
        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            this.AddNotification("Comment Deleted Successfully", NotificationType.SUCCESS);
            return RedirectToAction("Index", "Home");
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
