using System;
using System.Collections.Generic;

namespace Mersani.models.Sales
{
    public class SalesOrderReturnMaster
    {
        public int? SROH_SYS_ID { get; set; }
        public string SROH_CODE { get; set; }
        public string SROH_V_CODE { get; set; }
        public int? SROH_SOH_SYS_ID { get; set; }
        public DateTime? SROH_DATE { get; set; }
        public string SROH_NOTE { get; set; }
        public string SROH_RETURN_REASON { get; set; }
        public char? SROH_APPROVED_Y_N { get; set; }
        public int? SROH_APPROVED_BY { get; set; }
        public DateTime? SROH_APPROVED_DATE { get; set; }
        public decimal? SROH_DISCOUNT_PCT { get; set; }
        public decimal? SROH_DISCOUNT_AMT { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class SalesOrderReturnDetails
    {
        public int? SROD_SYS_ID { get; set; }
        public int? SROD_SROH_SYS_ID { get; set; }
        public int? SROD_ITEM_SYS_ID { get; set; }
        public double? SROD_ITEM_QTY { get; set; }
        public int? SROD_ITEM_UOM_SYS_ID { get; set; }
        public double? SROD_ITEM_UNIT_PRICE { get; set; }
        public double? BASIC_PRICE { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class SalesOrderReturn
    {
        public SalesOrderReturnMaster MASTER { get; set; }
        public List<SalesOrderReturnDetails> DETAILS { get; set; }
    }
}
