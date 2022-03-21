using System;

namespace Mersani.models.FinancialSetup
{
    public class FinsAcountClass
    {
        public int ACC_CLASS_CODE { get; set; }
        public string ACC_CLASS_NAME_AR { get; set; }
        public string ACC_CLASS_NAME_EN { get; set; }
        public int INS_USER { get; set; }
        public DateTime INS_DATE { get; set; }
        public int UP_USER { get; set; }
        public DateTime UP_DATE { get; set; }
    }
}
