using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.FinancialSetup
{
    public class BankSetup
    {
        public int FB_BANK_CODE { get; set; }
        public string FB_BANK_NAME_AR { get; set; }
        public string FB_BANK_NAME_EN { get; set; }
        public string FB_BANK_IBAN { get; set; }
        public string FB_BANK_TEL { get; set; }
        public string FB_BANK_FAX { get; set; }
        public string FB_CNTC_PERSION { get; set; }
        public string FB_CNTC_INFO { get; set; }
        public int? INS_USER { get; set; }
        public DateTime? INS_DATE { get; set; }
        public int? UP_USER { get; set; }
        public DateTime? UP_DATE { get; set; }

    }
}
