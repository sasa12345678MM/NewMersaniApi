using Mersani.Oracle;

namespace Mersani.models.FinancialSetup
{
    public class CustomerClass
    {
        public int? FCUC_SYS_ID { get; set; }
        public int? FCUC_CODE { get; set; }
        public string FCUC_NAME_AR { get; set; }
        public string FCUC_NAME_EN { get; set; }
        public int? FCUC_DISC_PERC { get; set; }

        public char? FCUC_CASH_Y_N { get; set; }
        public char? FCUC_DEFAULT_CASH_Y_N { get; set; }

        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }
}

