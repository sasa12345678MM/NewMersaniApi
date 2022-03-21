namespace Mersani.models.Sales
{
    public class SalesInvoiceItems
    {
        public int? INVSI_SYS_ID { set; get; }
        public int? INVSI_INVSH_SYS_ID { set; get; }
        public int? INVSI_ITEM_SYS_ID { set; get; }
        public int? INVSI_ITEM_QTY { set; get; }
        public int? INVSI_ITEM_UOM_SYS_ID { set; get; }
        public decimal? INVSI_ITEM_UNIT_PRICE { set; get; }
        public int? INVSI_ITEM_RET_QTY { set; get; }
        public decimal? INVSI_ITEM_UNIT_RET_PRICE { set; get; }
        public int? INVSI_ORDER_QTY { set; get; }
        public decimal? INVSI_ITEM_DISCOUNT_AMT { set; get; }
        public decimal? INVSI_ITEM_DISCOUNT_PCT { set; get; }

        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
}
