using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Administrator
{
    public class UserRoles
    {
        public int? GUR_SYS_ID { get; set; }
        public string GUR_DESC_AR { get; set; }
        public string GUR_DESC_EN { get; set; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }

    }
}
