namespace Mersani.models.Purchase
{
    public class PurchaseInvoiceItems
    {
        public int? INVI_SYS_ID { set; get; }
        public int? INVI_INVH_SYS_ID { set; get; }
        public int? INVI_ITEM_SYS_ID { set; get; }
        public int? INVI_ITEM_QTY { set; get; }
        public int? INVI_ITEM_UOM_SYS_ID { set; get; }
        public decimal? INVI_ITEM_UNIT_PRICE { set; get; }
        public int? INVI_ITEM_RET_QTY { set; get; }
        public decimal? INVI_ITEM_UNIT_RET_PRICE { set; get; }
        public int? INVI_ORDER_QTY { set; get; }
        public decimal? INVI_ITEM_DISCOUNT_AMT { set; get; }
        public decimal? INVI_ITEM_DISCOUNT_PCT { set; get; }

        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
}
