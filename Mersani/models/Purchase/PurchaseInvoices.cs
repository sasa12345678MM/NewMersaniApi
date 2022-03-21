using System;
using System.Collections.Generic;

namespace Mersani.models.Purchase
{
    public class PurchaseInvoices
    {
        public string INVH_NOTES { set; get; }
        public string INVH_CODE { set; get; }
        public int? INVH_PO_SYS_ID { set; get; }

        public int? INVH_SYS_ID { set; get; }
        public int? INVH_SUPP_SYS_ID { set; get; }
        public int? INVH_TYPE_ID { set; get; }

        public int? INVH_CR_ACC_CODE { set; get; }
        public int? INVH_DR_ACC_CODE { set; get; }
        public int? INVH_ADDED_AMOUNT_ACC_CODE { set; get; }
        public int? INVH_VAT_ACC_CODE { set; get; }
        public int? INVH_DISCOUNT_ACC_CODE { set; get; }

        public decimal? INVH_DISCOUNT_PCT { set; get; }
        public decimal? INVH_DISCOUNT_AMT { set; get; }

        public int? INVH_POSTED_JRN_NO { set; get; }
        public decimal? INVH_PAYMENT_AMOUNT { set; get; }
        public int? INVH_PAYMENT_NO { set; get; }

        public int? INVH_RTN_POSTED_JRN_NO { set; get; }
        public int? INVH_RETURN_DISCOUNT { set; get; }

        public decimal? INVH_VAT_PCT { set; get; }
        public decimal? INVH_VAT_AMT { set; get; }
        public decimal? INVH_CUR_RATE { set; get; }

        public int? INVH_CURR_SYS_ID { set; get; }
        public int? INVH_POSTED_BY { set; get; }

        public DateTime? INVH_DATE { set; get; }
        public DateTime? INVH_POSTED_DATE { set; get; }
        public DateTime? INVH_RETURNED_DATE { set; get; }

        public string INVH_PULLED_DT_PO { set; get; }
        public string INVH_POSTED_Y_N { set; get; }
        public string INVH_PAYMENT_Y_N_P { set; get; }
        public string INVH_RETURN_POSTED_Y_N { set; get; }
        public string INVH_RETURNED_Y_N { set; get; }
        public string INVH_SUPP_INV_INFO { set; get; }
        
        public string INVH_ADDED_AMOUNT_DESC { set; get; }
        public decimal? INVH_ADDED_AMOUNT { set; get; }

        public decimal? INVH_ITEMS_TOTAL { set; get; }
        public decimal? INV_GRAND_TOTAL { set; get; }

        public string INVH_V_CODE { set; get; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }

    public class PurchaseInvoicesData
    {
        public PurchaseInvoices INVOICES_HDR { set; get; }
        public List<PurchaseInvoiceItems> INVOICES_DTL { set; get; }
    }
}
