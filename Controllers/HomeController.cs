using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LocalTheatre.Data;
using LocalTheatre.Web.Models;
using Microsoft.AspNet.Identity;
using LocalTheatre.Web.Extensions;

namespace LocalTheatre.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            try
            {
                var announcements = db.Announcements.OrderBy(a => a.StartDateTime).Where(a => a.IsPublic).Select(AnnouncementViewModel.ViewModel);
                var userId = User.Identity.GetUserId();

                List<AnnouncementViewModel> an = new List<AnnouncementViewModel>();
                foreach (AnnouncementViewModel announcement in announcements)
                {
                    var announceUserId = announcement.AuthorId;

                    if (userId == announceUserId || User.IsInRole("Admin"))
                    {
                        announcement.CanEdit = true;
                    }

                    else
                    {
                        announcement.CanEdit = true;
                    }

                    an.Add(announcement);
                }

                return View(new AnnouncementsViewModel()
                {
                    Announcements = an
                });
            }
            catch
            {
                this.AddNotification("Failed to Load Announcements.", NotificationType.INFO);
                return View();
            }

        }

        public ActionResult AnouncementCommentsById(int id)
        {
            var comments = db.Announcements.Where(a => a.IsPublic).Where(a => a.Id == id).Select(CommentsViewModel.ViewModel).FirstOrDefault();
            var userId = User.Identity.GetUserId();

            foreach (var comment in comments.Comments)
            {
                var commentUserId = comment.AuthorId;
                if (userId == commentUserId || User.IsInRole("Admin"))
                {
                    comment.CanEdit = true;
                }

                else
                {
                    comment.CanEdit = false;
                }
            }

            return PartialView("_Comments", new CommentsViewModel()
            {
                Comments = comments.Comments as IEnumerable<CommentViewModel>
            });

        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}