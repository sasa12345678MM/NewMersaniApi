namespace Mersani.models.Administrator
{
    //[Table("GAS_COMPANY")]
    public class Company
    { 
        public int? COMP_SYS_ID { get; set; }
        public int? COMP_ID { get; set; }
        public int? COMP_GROUP_SYS_ID { get; set; }
        public string COMP_NAME_EN { get; set; }
        public string COMP_NAME_AR { get; set; }
        public int? COMP_COUNTRY_SYS_ID { get; set; }
        public string COMP_TXN_CODE { get; set; }

        public string C_NAME_EN { get; set; }
        public string C_NAME_AR { get; set; }
        public string GROUP_NAME_EN { get; set; }
        public string GROUP_NAME_AR { get; set; }
        public char? COMP_REMINDER_Y_N { get; set; }
        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }

    }
}
