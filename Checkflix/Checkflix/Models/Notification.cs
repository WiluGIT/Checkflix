using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Checkflix.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public bool IsSeen { get; set; }
        public ICollection<ApplicationUserNotification> ApplicationUserNotifications { get; set; }
        public Notification()
        {
            ApplicationUserNotifications = new Collection<ApplicationUserNotification>();
        }
    }
}