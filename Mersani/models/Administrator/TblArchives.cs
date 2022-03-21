using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Administrator
{
    public class TblArchives
    {
        public int ARCH_SYS_ID { get; set; }
        public string ARCH_FILE_PATH { get; set; }
        public string ARCH_DESC_AR { get; set; }
        public string ARCH_DESC_EN { get; set; }
        public string ARCH_REMIND_Y_N { get; set; }
        public DateTime? ARCH_END_DATE { get; set; }
        public string ARCH_PARENT_TBL_NAME { get; set; }
        public int ARCH_PARENT_TBL_SYS_ID { get; set; }
        public int? INS_USER { get; set; }
        public int? STATE { get; set; }
        
    }
}
