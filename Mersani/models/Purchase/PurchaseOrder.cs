using System;
using System.Collections.Generic;

namespace Mersani.models.Purchase
{
    public class PurchaseOrderMaster
    {
        public int? IPOH_SYS_ID { set; get; }

        public string IPOH_CODE { set; get; }
        public string IPOH_DESC { set; get; }

        public string RQST_NAME_AR { set; get; }
        public string RQST_NAME_EN { set; get; }

        public int? IPOH_SUPP_SYS_ID { set; get; }
        public int? IPOH_IPRH_SYS_ID { set; get; }

        public decimal? IPOH_DISCOUNT_PERC { set; get; }
        public decimal? IPOH_DISCOUNT_AMNT { set; get; }

        public int? IPOH_CURR_SYS_ID { set; get; }
        public decimal? IPOH_CURR_EX_RATE { set; get; }

        public DateTime? IPOH_DATE { set; get; }

        public char? IPOH_APPRVD_Y_N { set; get; }
        public int? IPOH_APPRVD_BY { set; get; }
        public DateTime? IPOH_APPRVD_DATE { set; get; }
        public char? IPOH_FROM_RQST_Y_N { set; get; }

        public string IPOH_V_CODE { set; get; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
    public class PurchaseOrderDetails
    {
        public int? IPOD_SYS_ID { set; get; }
        public int? IPOD_IPOH_SYS_ID { set; get; }

        public int? IPOD_ITEM_SYS_ID { set; get; }
        public int? IPOD_UOM_SYS_ID { set; get; }
        public int? IPOD_QTY { set; get; }

        public decimal? IPOD_PRCH_PRICE { set; get; }
        public decimal? IPOD_DISCOUNT_PERC { set; get; }
        public decimal? IPOD_DISCOUNT_AMNT { set; get; }

        public int? IPOD_IPRD_SYS_ID { set; get; }

        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
    public class PurchaseOrder
    {
        public PurchaseOrderMaster MASTER { set; get; }
        public List<PurchaseOrderDetails> DETAILS { set; get; }
    }
}
