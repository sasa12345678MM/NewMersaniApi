using System;
using System.Collections.Generic;

namespace Mersani.models.Sales
{
    public class S_PaymentMaster
    {
        public string S_PAY_NOTE { set; get; }
        public string S_PAY_CODE { set; get; }
        public string S_PAY_V_CODE { set; get; }
        public char? S_PAY_POSTED_Y_N { set; get; }

        public int? S_PAY_SYS_ID { set; get; }
        public int? S_PAY_CUST_SYS_ID { set; get; }
        public int? S_PAY_ACC_CODE { set; get; }
        public int? S_PAY_CHQ_NO { set; get; }
        public int? S_PAY_CHQ_BANK_SYS_ID { set; get; }
        public int? S_PAY_POSTED_S_NO { set; get; }
        public decimal? S_PAY_AMOUNT { set; get; }

        public DateTime? S_PAY_DATE { set; get; }
        public DateTime? S_PAY_CHQ_DATE { set; get; }
        public DateTime? S_PAY_POSTED_DATE { set; get; }


        public int? S_PAY_DOC_NO { set; get; }
        public int? S_PAY_DOC_TYPE { set; get; }
        public int? S_PAY_CR_ACC_CODE { set; get; }
        public int? S_PAY_DR_ACC_CODE { set; get; }
        public DateTime? S_PAY_DOC_DATE { set; get; }


        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
    public class S_PaymentDetails
    {
        public int? S_PAY_DTLS_SYS_ID { set; get; }
        public int? S_PAY_MST_SYS_ID { set; get; }
        public int? S_PAY_DTLS_INV_SYS_ID { set; get; }
        

        public decimal? S_PAY_DTLS_PAY { set; get; }
        public decimal? S_PAY_DTLS_AMT { set; get; }
        public decimal? S_PAY_DTLS_REM { set; get; }

        public char? S_PAY_DTLS_PAY_Y_N_P { set; get; }
        public DateTime? S_PAY_DTLS_INV_DATE { set; get; }


        public char? SELECTED_Y_N { set; get; }
        public string S_PAY_INV_NO { set; get; }

        public string S_PAY_V_CODE { set; get; }
        public char? S_PAY_POSTED_Y_N { set; get; }
        public int? S_PAY_DR_ACC_CODE { set; get; }
        public int? S_PAY_CR_ACC_CODE { set; get; }
        public string S_PAY_NOTE { set; get; }

        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
}
    public class SalesPayment
    {
        public S_PaymentMaster PAYMENT_HDR { set; get; }
        public List<S_PaymentDetails> PAYMENT_DTL { set; get; }
    }
}
