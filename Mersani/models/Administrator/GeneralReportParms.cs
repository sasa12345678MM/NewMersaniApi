using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Administrator
{
    public class GeneralReportParms
    {
        public int? GRP_SYS_ID { get; set; }
        public string GRP_NAME_AR { get; set; }

        public string GRP_NAME_EN { get; set; }

        public string GRP_CONTROLNAME { get; set; }

        public char? GRP_TYPE_N_T_D_L { get; set; }

        public int? GRP_APPCODE { get; set; }

        public int? GRP_NOTES { get; set; }

        public int? GRP_ORDER { get; set; }

        public char? GRP_DEFAULT_Y_N { get; set; }

        public int? CURR_USER { get; set; }
        public int? STATE { get; internal set; }
    }
}




