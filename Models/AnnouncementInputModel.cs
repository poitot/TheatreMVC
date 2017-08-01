using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LocalTheatre.Web.Models
{
    public class AnnouncementInputModel
    {
        [Required(ErrorMessage = "An announcement title is required")]
        [StringLength(200, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date & Time")]
        public DateTime StartDateTime { get; set; }

        [Required]
        public TimeSpan? Duration { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(200)]
        public string Location { get; set; }
        
    }
}