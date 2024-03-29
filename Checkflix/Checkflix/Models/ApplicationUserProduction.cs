﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.Models
{
    public class ApplicationUserProduction
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ProductionId { get; set; }
        public Production Production { get; set; }

        public bool? Favourites { get; set; }
        public bool? ToWatch { get; set; }
        public bool? Watched { get; set; }
    }
}
