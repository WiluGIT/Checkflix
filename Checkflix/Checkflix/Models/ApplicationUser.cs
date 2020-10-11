using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ApplicationUserVod> ApplicationUserVods { get; set; }
        public ICollection<ApplicationUserProduction> ApplicationUserProductions { get; set; }
        public ICollection<ApplicationUserCategory> ApplicationUserCategories { get; set; }
        public ICollection<ApplicationUserNotification> ApplicationUserNotifications { get; set; }
        public ICollection<Following> Followers { get; set; }
        public ICollection<Following> Followees { get; set; }
        public ApplicationUser()
        {
            ApplicationUserVods = new Collection<ApplicationUserVod>();
            ApplicationUserProductions = new Collection<ApplicationUserProduction>();
            ApplicationUserCategories = new Collection<ApplicationUserCategory>();
            ApplicationUserNotifications = new Collection<ApplicationUserNotification>();
            Followers = new Collection<Following>();
            Followees = new Collection<Following>();
        }
    }
}
