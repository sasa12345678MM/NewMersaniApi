using System.Collections.Generic;

namespace Mersani.models.Archive
{

    public class LArchiveHead
    {
        public int? AH_CODE { get; set; }
        public string AH_NAME_AR { get; set; }
        public string AH_NAME_EN { get; set; }
        public string AH_NOTES { get; set; }
        public int? AH_SYS_ID { get; set; }
        public char? AH_FRZ_Y_N { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class LArchiveDetail
    {
        public int? AD_CODE { get; set; }
        public int? AD_AH_CODE { get; set; }
        public string AD_NAME_AR { get; set; }
        public string AD_NAME_EN { get; set; }
        public string AD_NOTES { get; set; }
        public char? AD_FRZ_Y_N { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }
 

}
