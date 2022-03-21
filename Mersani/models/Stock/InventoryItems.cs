namespace Mersani.models.Stock
{
    public class InventoryItems
    {
        public int? III_SYS_ID { set; get; }
        public int? III_ITEM_SYS_ID { set; get; }
        public int? III_INV_SYS_ID { set; get; }
        public int? III_DELVRY_SHRM_MAXQTY { set; get; }
        public int? III_MIN_STK_QTY { set; get; }
        public int? III_MAX_STK_QTY { set; get; }
        public int? III_DELVRY_REQ_DAYS { set; get; }
        public int? III_MNUL_AVRG_SALES_QTY { set; get; }
        public char? III_DLVRY_TO_SHRM_Y_N { set; get; }
        public char? III_ADDED_M_A { set; get; }

        public int? III_CURR_QTY { set; get; }
        public int? BASIC_UNIT_ID { set; get; }
        public string BASIC_NAME_AR { set; get; }
        public string BASIC_NAME_EN { set; get; }

        public char? ITEM_BTCH_Y_N { set; get; }

        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
}
