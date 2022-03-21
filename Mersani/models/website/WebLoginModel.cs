using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.website
{
    public class WebLoginModel
    {

        public string email { get; set; }
        public string Password { get; set; }
        public int? Language { get; set; }
    }
}
