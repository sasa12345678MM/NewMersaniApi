namespace Mersani.models.Notifications
{
    public class ReminderHeader
    {
        public int? RH_SYS_ID { get; set; }
        public string RH_CODE { get; set; }
        public string RH_DESC_AR { get; set; }
        public string RH_DESC_EN { get; set; }
        public char? RH_FRZ_Y_N { get; set; }
        public string RH_NOTES { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class ReminderDetail
    {
        public int? RD_SYS_ID { get; set; }
        public string RD_CODE { get; set; }
        public int? RD_RH_SYS_ID { get; set; }
        public string RD_DEC_AR { get; set; }
        public string RD_DEC_EN { get; set; }
        public int? RD_APP_CODE { get; set; }

        public int? RU_MNU_CODE { get; set; }
        public char? RD_FRZ_Y_N { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class ReminderUser
    {
        public int? RU_SYS_ID { get; set; }
        public int? RU_RD_SYS_ID { get; set; }
        public int? RU_USR_ROLE_SYS_ID { get; set; }
        public int? RU_BEF_DAYS { get; set; }
        public int? RU_AFT_DAYS { get; set; }
        public int? RU_LEVEL { get; set; }
        public int? CURR_USER { get; set; }
        public string CONNECTION_ID { get; set; }
        public int? STATE { get; set; }

        public int? RU_USR_CODE { get; set; }
        public string RU_V_CODE { get; set; }
    }
}
