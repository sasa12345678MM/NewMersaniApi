using System;
using System.Collections.Generic;

namespace Mersani.models.Archive
{

    public class ArchiveHead
    {
        public int? AH_SYS_ID { get; set; }
        public int? AH_CODE { get; set; }
        public int? AH_AH_CODE { get; set; }
        public string AH_DESC_AR { get; set; }
        public string AH_DESC_EN { get; set; }
        public string AH_NOTES { get; set; }
        public DateTime? AH_DATE { get; set; }
        public string AH_V_CODE { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class ArchiveDetail
    {
        public int? AD_SYS_ID { get; set; }
        public int? AD_AH_SYS_ID { get; set; }
        public int? AD_AD_CODE { get; set; }
        public string AD_DESC_AR { get; set; }
        public string AD_DESC_EN { get; set; }
        public string AD_NOTES { get; set; }
        public string AD_FILE_NAME { get; set; }
        public int? AD_FIFE_SIZE { get; set; }
        public DateTime? AD_END_DATE { get; set; }
        public char? AD_REMIND_Y_N { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }

    public class GeneralArchives
    {
        public ArchiveHead ARCHIVEHEAD { get; set; }
        public List<ArchiveDetail> ARCHIVEDETAIL { get; set; }
    }
}
