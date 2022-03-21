namespace Mersani.models.FinancialSetup
{
    public class TXNSetup
    {
        public int FTS_SYS_ID { get; set; }
        public string FTS_TXN_CODE { get; set; }
        public string FTS_RELATED_TXN { get; set; }
        public string FTS_NAME_AR { get; set; }
        public string FTS_NAME_EN { get; set; }
        public string FTS_TXN_TYPE { get; set; }
        public string FTS_YEAR_RESET_Y_N { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }
}
