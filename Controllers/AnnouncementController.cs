using LocalTheatre.Data;
using LocalTheatre.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LocalTheatre.Web.Extensions;
using System.Net;
using System.Data.Entity;

namespace LocalTheatre.Web.Controllers
{
    [Authorize]
    public class AnnouncementController : BaseController
    {
        // GET: Announcement
        public ActionResult Index()
        {
            return View();
        }

        // GET: Create Annoucncement page
        public ActionResult Create()
        {
            return View();
        }

        // Saves the Announcement to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AnnouncementInputModel model)
        {
            if (ModelState.IsValid)
            {
                var a = new Announcement()
                {
                    AuthorId = User.Identity.GetUserId(),
                    Description = model.Description,
                    StartDateTime = model.StartDateTime,
                    Title = model.Title,
                    Location = model.Location,
                    IsPublic = true,
                    Duration = model.Duration
                };
                db.Announcements.Add(a);
                db.SaveChanges();
                // Displays a message to the user telling them if the announcement was created successfull or not
                this.AddNotification("Announcement Created.", NotificationType.INFO);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);

            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Announcement announcement = db.Announcements.Find(id);
            db.Announcements.Remove(announcement);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // displays a message to the user when an announcement is created
        public ActionResult AnnouncementCreateMsg()
        {
            return View();
        }

        // GET: Announcement/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement a = db.Announcements.Find(id);
            if (a == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FullName", a.AuthorId);
            return View(a);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Announcement model)
        {
            if (ModelState.IsValid)
            {
                Announcement edited = db.Announcements.Find(model.Id);
                edited.Description = model.Description;
                edited.Duration = model.Duration;
                edited.Location = model.Location;
                edited.StartDateTime = model.StartDateTime;
                edited.Title = model.Title;
                db.Entry(edited).State = EntityState.Modified;
                db.SaveChanges();
                this.AddNotification("Announcement " + edited.Title + " Updated Successfully", NotificationType.SUCCESS);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FullName", User.Identity.GetUserId().ToString());
            this.AddNotification("Announcement " + model.Title + " unable to update", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }
    }
}