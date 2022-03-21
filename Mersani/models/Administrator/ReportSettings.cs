using Mersani.models.Administrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Administrator
{
    public class IMenuReports
    {
        public int? MNURPT_SYS_ID { get; set; }
        public int? MNURPT_MNU_CODE { get; set; }
        public string MNURPT_ASSEMPLY_NAME { get; set; }
        public string MNURPT_NAME_AR { get; set; }
        public string MNURPT_NAME_EN { get; set; }
        public string MNURPT_FRZ_Y_N { get; set; }
        public string MNURPT_CLASS_NAME { get; set; }
        public string MNURPT_ICON { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class IMenuReportParm
    {
        public int? RDTL_SYS_ID { get; set; }
        public int? RDTL_MNU_CODE { get; set; }
        public int? RDTL_GRP_SYS_ID { get; set; }
        public int? RDTL_ORDER { get; set; }
        public string RDTL_SHOW_Y_N { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class IMenuReportUsers
    {
        public int? GMRU_SYS_ID { get; set; }
        public int? GMRU_MNURPT_SYS_ID { get; set; }
        public int? GMRU_USR_CODE { get; set; }

        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

}