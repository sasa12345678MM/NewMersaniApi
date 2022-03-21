namespace Mersani.models.Administrator
{
    public class GeneralSetupMaster
    {
        public int? GSH_SYS_ID { get; set; }
        public string GSH_NAME_AR { get; set; }
        public string GSH_NAME_EN { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }
    public class GeneralSetupDetail
    {
        public string GSD_NAME_AR { get; set; }
        public string GSD_NAME_EN { get; set; }
        public int? GSD_SYS_ID { get; set; }
        public int? GSD_GSH_SYS_ID { get; set; }
        public string GSD_CODE { get; set; }
        public int? GSD_SORT { get; set; }
        public int? GSD_DEFAULT { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }
}

