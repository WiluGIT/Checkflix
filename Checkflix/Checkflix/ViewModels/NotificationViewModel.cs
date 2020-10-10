using System;
using System.ComponentModel.DataAnnotations;

namespace Checkflix.ViewModels
{
    public class NotificationViewModel
    {
        public int NotificationId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Content { get; set; }
    }
}