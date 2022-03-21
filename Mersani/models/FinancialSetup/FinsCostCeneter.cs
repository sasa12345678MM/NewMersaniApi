namespace Mersani.models.FinancialSetup
{
    public class FinsCostCeneter
    {
        public int? COST_CENTER_CODE { get; set; }
        public string COST_CENTER_NO { get; set; }
        public string COST_CENTER_NAME_AR { get; set; }
        public string COST_CENTER_NAME_EN { get; set; }
        public int? COST_CNT_COMP_SYS_ID { get; set; }
        public int? COST_CNT_BR_SYS_ID { get; set; }
        public int? ACTV_COMP_BR_SYS_ID { get; set; }
        public int? COST_CENTER_PARENT { get; set; }
        public string COST_CENTER_STATUS { get; set; }
        public int? COST_CENTER_LEVEL { get; set; }
        public int? COST_CENTER_DEPT { get; set; }

        public int? INS_USER { get; set; }
        public int? UP_USER { get; set; }
        public int? STATE { get; set; }
    }
}
