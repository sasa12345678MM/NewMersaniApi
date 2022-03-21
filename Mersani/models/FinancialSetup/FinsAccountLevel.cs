using System;

namespace Mersani.models.FinancialSetup
{
    public class FinsAccountLevel
    {
        public int? ACC_LEVEL_CODE { get; set; }
        public string ACC_LEVEL_NAME_AR { get; set; }
        public string ACC_LEVEL_NAME_EN { get; set; }
        public int ACC_LEVEL_DIGITS { get; set; }
        public int? INS_USER { get; set; }
        public DateTime? INS_DATE { get; set; }
        public int? UP_USER { get; set; }
        public DateTime? UP_DATE { get; set; }
    }
}
