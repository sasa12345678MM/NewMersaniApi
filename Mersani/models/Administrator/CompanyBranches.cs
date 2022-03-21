using System;

namespace Mersani.models.Administrator
{
    //[Table("GAS_COMPANY_BRANCHES")]
    public class CompanyBranches
    {
        public int? CB_SYS_ID { get; set; }
        public int? CB_ID { get; set; }
        public int? CB_COMPANY_SYS_ID { get; set; }
        public string CB_NAME_EN { get; set; } 
        public string CB_NAME_AR { get; set; }
        public int? CB_CITY_SYS_ID { get; set; }
        public string CB_TXN_CODE { get; set; }

        public string CITY_NAME_AR { get; set; }
        public string CITY_NAME_EN { get; set; }

        public string COMP_NAME_AR { get; set; }
        public string COMP_NAME_EN { get; set; }
        public char? CB_REMINDER_Y_N { get; set; }
        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }

    }
}