using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Checkflix.ViewModels
{
    public class NotificationFormViewModel
    {
        public int NotificationId { get; set; }
        [Required]
        public string Content { get; set; }
        public bool ToAll { get; set; }
        public ICollection<VodViewModel> Vods { get; set; }
        public ICollection<CategoryViewModel> Categories { get; set; }
    }
}