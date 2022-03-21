using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.HR
{
    public class HrDepartment
    {

        public int? HRD_SYS_ID { get; set; }
        public string HRD_CODE { get; set; }
        public string HRD_NAME_AR { get; set; }
        public string HRD_NAME_EN { get; set; }
        public string HRD_DESCRIPTION { get; set; }
        public int? HRD_PARENT_SYS_ID { get; set; }
        ///////////////////////
        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }

    }
}
