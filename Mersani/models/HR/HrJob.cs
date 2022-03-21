using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.HR
{
    public class HrJob
    {

        public int? HRJ_SYS_ID { get; set; }
        public string HRJ_CODE { get; set; }
        public string HRJ_NAME_AR { get; set; }
        public string HRJ_NAME_EN { get; set; }
        public string HRJ_NOTES { get; set; }
        public string HRJ_IS_MED_Y_N { get; set; }
        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }
    }
}
