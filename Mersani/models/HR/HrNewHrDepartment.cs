using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.HR
{
    public class HrNewHrDepartment
    {
        public int? HRD_SYS_ID { get; set; }
        public string HRD_CODE { get; set; }
        public string HRD_NAME_AR { get; set; }
        public string HRD_NAME_EN { get; set; }
        public string HRD_FRZ_Y_N { get; set; }
        public int? HRD_WORK_HRS { get; set; }
        public int? HRD_DEPT_MANAGER_SYS_ID { get; set; }
        public string HRD_PRODUCTIOM_Y_N { get; set; }

        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }
    }
}
