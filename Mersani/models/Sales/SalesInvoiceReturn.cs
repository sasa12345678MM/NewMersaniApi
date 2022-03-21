using System;
using System.Collections.Generic;

namespace Mersani.models.Sales
{
    public class SalesInvoicesReturnHead
    {
        public int? RIH_SYS_ID { get; set; }
        public string RIH_CODE { get; set; }
        public string RIH_V_CODE { get; set; }
        public int? RIH_CUST_SYS_ID { get; set; }
        public DateTime? RIH_DATE { get; set; }
        public string RIH_NOTES { get; set; }
        public int? RIH_CR_ACC_CODE { get; set; }
        public string RIH_PULLED_DT_RO { get; set; }
        public string RIH_RO_SYS_ID { get; set; }
        public decimal? RIH_DISCOUNT_PCT { get; set; }
        public decimal? RIH_DISCOUNT_AMT { get; set; }
        public string RIH_POSTED_Y_N { get; set; }
        public DateTime? RIH_POSTED_DATE { get; set; }
        public decimal? RIH_VAT_PCT { get; set; }
        public decimal? RIH_ADDED_AMOUNT { get; set; }
        public string RIH_ADDED_AMOUNT_DESC { get; set; }
        public decimal? RIH_ITEMS_TOTAL { get; set; }
        public decimal? RIH_GRAND_TOTAL { get; set; }
        public int? RIH_DB_ACC_CODE { get; set; }
        public int? RIH_CURR_SYS_ID { get; set; }
        public decimal? RIH_CURR_RATE { get; set; }
        public int? RIH_POSTED_BY { get; set; }
        public decimal? RIH_VAT_AMT { get; set; }
        public int? RIH_VAT_ACC_CODE { get; set; }
        public int? RIH_ADDED_AMOUNT_ACC_CODE { get; set; }
        public int? RIH_DISCOUNT_ACC_CODE { get; set; }
        public int? RIH_PERIOD_SYS_ID { get; set; }
        public int? RIH_YEAR { get; set; }
        public string RIH_CUST_INV_INFO { get; set; }
        public int? RIH_SINVH_SYS_ID { get; set; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
        public int? RIH_DR_ACC_CODE { set; get; }

    }
    public class SalesInvoicesReturnItem
    {
        public int? RII_SYS_ID { set; get; }
        public int? RII_RIH_SYS_ID { set; get; }
        public int? RII_ITEM_SYS_ID { set; get; }
        public int? RII_ITEM_QTY { set; get; }
        public int? RII_ITEM_UOM_SYS_ID { set; get; }
        public decimal? RII_ITEM_UNIT_PRICE { set; get; }
        public decimal? RII_ITEM_DISCOUNT_AMT { set; get; }
        public decimal? RII_ITEM_DISCOUNT_PCT { set; get; }
        public int? RII_ITEM_BATCH_SYS_ID { set; get; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
    public class SalesInvoiceReturnData
    {
        public SalesInvoicesReturnHead SALESINVOICERETURNHEAD { get; set; }
        public List<SalesInvoicesReturnItem> SALESINVOICERETURNITEM { get; set; }
    }
}
