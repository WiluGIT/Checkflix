using Checkflix.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkflix.ViewModels
{
    public class ResponseViewModel
    {
        public List<string> Messages { get; set; }
        public ResponseStatus Status { get; set; }

        public object Data { get; set; }
    }
}
