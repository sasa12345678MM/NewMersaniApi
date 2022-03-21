namespace Mersani.models.Administrator
{
    //[Table("FIN_ACTIVITY_COMP_BR")]
    public class BranchActivities
    {
        public int? FAC_SYS_ID { get; set; }
        public int? FAC_ACTIVITY_CODE { get; set; }
        public int? FAC_BR_SYS_ID { get; set; }

        public string ACTIVITY_NAME_AR { get; set; }
        public string ACTIVITY_NAME_EN { get; set; }
        public string BRANCH_NAME_AR { get; set; }
        public string BRANCH_NAME_EN { get; set; }

        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }
    }
}
