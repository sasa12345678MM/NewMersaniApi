using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.HR
{
    public class HrLoansType
    {

        public int? HRLT_SYS_ID { get; set; }
        public string HRLT_CODE { get; set; }
        public string HRLT_NAME_AR { get; set; }
        public string HRLT_NAME_EN { get; set; }

        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }


    }
}
