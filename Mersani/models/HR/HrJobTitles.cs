using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.HR
{
    public class HrJobTitles
    {
        public int? HRJT_SYS_ID { get; set; }
        public string HRJT_CODE { get; set; }
        public int? HRJT_HRJG_SYS_ID { get; set; }
        public string HRJT_NAME_AR { get; set; }
        public string HRJT_NAME_EN { get; set; }
        public string HRJT_REMARKS { get; set; }
        public int? HRJT_UPPER_JOB { get; set; }
        public int? HRJT_PARENT_SYS_ID { get; set; }
        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }

    }
}


