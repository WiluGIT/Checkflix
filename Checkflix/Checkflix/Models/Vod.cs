using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.Models
{
    public class Vod
    {
        public int VodId { get; set; }
        public string PlatformName { get; set; }

        public ICollection<ApplicationUserVod> ApplicationUserVods { get; set; }
        public ICollection<VodProduction> VodProductions { get; set; }

        public Vod()
        {
            ApplicationUserVods = new Collection<ApplicationUserVod>();
            VodProductions = new Collection<VodProduction>();
        }
    }
}
