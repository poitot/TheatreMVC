using LocalTheatre.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace LocalTheatre.Web.Models
{
    public class AnnouncementViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDateTime { get; set; }

        public TimeSpan? Duration { get; set; }

        public string Author { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string AuthorId { get; set; }

        public bool CanEdit { get; set; }

        public static Expression<Func<Announcement, AnnouncementViewModel>> ViewModel
        {
            get
            {
                return a => new AnnouncementViewModel()
                {
                    Id = a.Id,
                    Description = a.Description,
                    Title = a.Title,
                    StartDateTime = a.StartDateTime,
                    Duration = a.Duration,
                    Author = a.Author.FullName,
                    Location = a.Location,
                    AuthorId = a.AuthorId
                };
            }
        }
    }

    public class AnnouncementsViewModel
    {
        public IEnumerable<AnnouncementViewModel> Announcements { get; set; }

        
    }
}