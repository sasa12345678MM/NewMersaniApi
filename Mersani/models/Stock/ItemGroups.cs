namespace Mersani.models.Stock
{
    public class ItemGroups
    {
        public int? IIG_SYS_ID { set; get; }
        public string IIG_CODE { set; get; }
        public string IIG_NAME_AR { set; get; }
        public string IIG_NAME_EN { set; get; }
        public int? IIG_PARENT_SYS_ID { set; get; }
        public int? IIG_LEVEL { set; get; }
        public string IIG_STK_SRV_S_V { set; get; }

        public char? IIG_MDCHN_Y_N { set; get; }
        public char? IIG_NEED_MDCHL_DESC_Y_N { set; get; }
        public char? IIG_NEED_AUTH_Y_N { set; get; }
        public char? IIG_ASSMPLY_Y_N { set; get; }
        public string IIG_ICON { set; get; }
        public char? IIG_MOB_WEB_SHOW_Y_N { set; get; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
}
