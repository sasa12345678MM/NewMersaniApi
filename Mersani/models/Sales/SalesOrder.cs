using System;
using System.Collections.Generic;

namespace Mersani.models.Sales
{
    public class SalesOrderMaster
    {
        public int? SOH_SYS_ID { get; set; }
        public string SOH_CODE { get; set; }
        public string SOH_V_CODE { get; set; }
        public char? SOH_PULLED_FROM_D_Q { get; set; }
        public int? SOH_SQH_SYS_ID { get; set; }
        public string SOH_TYPE_SO_CN { get; set; }
        public string SOH_CONTRACT_NO { get; set; }
        public string SOH_CN_ATTACHED { get; set; }
        public string SOH_CUST_PO_NO { get; set; }
        public char? SOH_APPROVED_Y_N { get; set; }
        public int? SOH_APPROVED_BY { get; set; }
        public DateTime? SOH_APPROVED_DATE { get; set; }
        public DateTime? SOH_DATE { get; set; }
        public string SOH_NOTE { get; set; }
        public char? SOH_TO_OWNER_CUST_O_C { get; set; }
        public int? SOH_OWNER_CUST_SYS_ID { get; set; }
        public decimal? SOH_DISCOUNT_PCT { get; set; }
        public decimal? SOH_DISCOUNT_AMT { get; set; }
        public int? SOH_INV_SYS_ID { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class SalesOrderDetails
    {
        public int? SOD_SYS_ID { get; set; }
        public int? SOD_SOH_SYS_ID { get; set; }
        public int? SOD_ITEM_SYS_ID { get; set; }
        public double? SOD_ITEM_QTY { get; set; }
        public int? SOD_ITEM_UOM_SYS_ID { get; set; }
        public double? SOD_ITEM_UNIT_PRICE { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class SalesOrder
    {
        public SalesOrderMaster MASTER { get; set; }
        public List<SalesOrderDetails> DETAILS { get; set; }
    }
}
