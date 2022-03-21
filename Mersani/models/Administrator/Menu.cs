namespace Mersani.models.Administrator
{
    // table name: GAS_MNU
    public class Menu
    {
        public int? MNU_CODE { get; set; }
        public string MNU_NAME { get; set; }
        public string MNU_PATH { get; set; }
        public string MNU_LABEL_AR { get; set; }
        public string MNU_LABEL_EN { get; set; }
        public int? MNU_PARENT { get; set; }
        public string MNU_TYPE { get; set; }
        public string MNU_PAGE_REPORT_P_R { get; set; }
        public int? MNU_ORD { get; set; }
        public string MNU_FORM { get; set; }

        public string MENU_NAME { get; set; }
    }
}
