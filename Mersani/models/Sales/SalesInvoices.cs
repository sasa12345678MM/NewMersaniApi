using System;
using System.Collections.Generic;

namespace Mersani.models.Sales
{
    public class SalesInvoices
    {
        public string INVSH_NOTES { set; get; }
        public string INVSH_CODE { set; get; }
        public string INVSH_SO_SYS_ID { set; get; }

        public int? INVSH_SYS_ID { set; get; }
        public int? INVSH_CUST_SYS_ID { set; get; }
        public int? INVSH_TYPE_ID { set; get; }

        public int? INVSH_CR_ACC_CODE { set; get; }
        public int? INVSH_DR_ACC_CODE { set; get; }
        public int? INVSH_ADDED_AMOUNT_ACC_CODE { set; get; }
        public int? INVSH_VAT_ACC_CODE { set; get; }
        public int? INVSH_DISCOUNT_ACC_CODE { set; get; }


        public decimal? INVSH_DISCOUNT_PCT { set; get; }
        public decimal? INVSH_DISCOUNT_AMT { set; get; }

        public int? INVSH_POSTED_JRN_NO { set; get; }
        public decimal? INVSH_PAYMENT_AMOUNT { set; get; }
        public int? INVSH_PAYMENT_NO { set; get; }
        public int? INVSH_RTN_POSTED_JRN_NO { set; get; }
        public int? INVSH_RETURN_DISCOUNT { set; get; }

        public decimal? INVSH_VAT_PCT { set; get; }
        public decimal? INVSH_VAT_AMT { set; get; }
        public decimal? INVSH_CUR_RATE { set; get; }

        public int? INVSH_CURR_SYS_ID { set; get; }
        public int? INVH_POSTED_BY { set; get; }

        public DateTime? INVSH_DATE { set; get; }
        public DateTime? INVSH_POSTED_DATE { set; get; }
        public DateTime? INVSH_RETURNED_DATE { set; get; }

        public string INVSH_PULLED_DT_SO { set; get; }
        public string INVSH_POSTED_Y_N { set; get; }
        public string INVSH_PAYMENT_Y_N_P { set; get; }
        public string INVSH_RETURN_POSTED_Y_N { set; get; }
        public string INVSH_RETURNED_Y_N { set; get; }


        public string INVSH_ADDED_AMOUNT_DESC { set; get; }
        public decimal? INVSH_ADDED_AMOUNT { set; get; }

        public decimal? INVSH_ITEMS_TOTAL { set; get; }
        public decimal? INVSH_GRAND_TOTAL { set; get; }

        public string INVSH_V_CODE { set; get; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }

    public class SalesInvoicesData
    {
        public SalesInvoices INVOICES_HDR { set; get; }
        public List<SalesInvoiceItems> INVOICES_DTL { set; get; }
    }
}
