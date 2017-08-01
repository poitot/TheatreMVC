using LocalTheatre.Data;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace LocalTheatre.Web.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string AuthorId { get; set; }

        public string Author { get; set; }

        public bool CanEdit { get; set; }

        public int AnnouncementId { get; set; }

        public static Expression<Func<Comment, CommentViewModel>> ViewModel
        {
            get
            {
                return c => new CommentViewModel()
                {
                    Text = c.Text,
                    Author = c.Author.FullName,
                    Id = c.Id,
                    AuthorId = c.AuthorId
                    
                };
            }
        }
    }

    public class CommentsViewModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static Expression<Func<Announcement, CommentsViewModel>> ViewModel
        {
            get
            {
                return c => new CommentsViewModel()
                {
                    Comments = c.Comments.AsQueryable().Select(CommentViewModel.ViewModel)
                };
            }
        }
    }
}