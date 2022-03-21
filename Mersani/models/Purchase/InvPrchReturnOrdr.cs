using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Purchase
{
    public class InvPrchReturnOrdrHdr
    {
        public int? IPROH_SYS_ID { get; set; }
        public string IPROH_CODE { get; set; }
        public string IPROH_V_CODE { get; set; }
        public DateTime? IPROH_DATE { get; set; }
        public string IPROH_DESC { get; set; }
        public int? IPROH_PINVH_SYS_ID { get; set; }
        public char? IPROH_APPRVD_Y_N { get; set; }
        public int? IPROH_CURR_SYS_ID { get; set; }
        public decimal? IPROH_CURR_EX_RATE { get; set; }
        public int? IPROH_APPRVD_BY { get; set; }
        public DateTime? IPROH_APPRVD_DATE { get; set; }

        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }


    public class InvPrchReturnOrdrDtl
    {
        public int? IPROD_SYS_ID { get; set; }
        public int? IPROD_IPROH_SYS_ID { get; set; }
        public int? IPROD_ITEM_SYS_ID { get; set; }
        public int? IPROD_UOM_SYS_ID { get; set; }
        public decimal? IPROD_QTY { get; set; }
        public decimal? IPROD_PRCH_PRICE { get; set; }
        public decimal? IPROD_DISCOUNT_PERC { get; set; }
        public decimal? IPROD_DISCOUNT_AMNT { get; set; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
    public class PurchaseReturnOrderData
    {
        public InvPrchReturnOrdrHdr PURCHASERETURNORDERHDR { get; set; }
        public List<InvPrchReturnOrdrDtl> PURCHASERETURNORDERDTL { get; set; }
    }
}