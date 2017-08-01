using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalTheatre.Data
{
    public class Announcement
    {
        public Announcement()
        {
            IsPublic = true;
            StartDateTime = DateTime.Now;
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength (250)]
        public string Title { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        public TimeSpan? Duration { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public bool IsPublic { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
