using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.HR
{
    public class HrJobGroups
    {
        public int? HRJG_SYS_ID { get; set; }
        public string HRJG_CODE { get; set; }
        public string HRJG_NAME_AR { get; set; }
        public string HRJG_NAME_EN { get; set; }

        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }
    }
}
