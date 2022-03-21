using System;
using System.Collections.Generic;

namespace Mersani.models.Purchase
{
    public class P_PaymentMaster
    {
        public string P_PAY_NOTE { set; get; }
        public string P_PAY_CODE { set; get; }
        public string P_PAY_V_CODE { set; get; }
        public char? P_PAY_POSTED_Y_N { set; get; }

        public int? P_PAY_SYS_ID { set; get; }
        public int? P_PAY_SUPP_SYS_ID { set; get; }
        public int? P_PAY_CR_ACC_CODE { set; get; }
        public int? P_PAY_DR_ACC_CODE { set; get; }
        public int? P_PAY_CHQ_NO { set; get; }
        public int? P_PAY_DOC_NO { set; get; }
        public int? P_PAY_DOC_TYPE { set; get; }
        public int? P_PAY_CHQ_BANK_SYS_ID { set; get; }
        public int? P_PAY_POSTED_JRN_NO { set; get; }
        public decimal? P_PAY_AMOUNT { set; get; }

        public DateTime? P_PAY_DATE { set; get; }
        public DateTime? P_PAY_CHQ_DATE { set; get; }
        public DateTime? P_PAY_DOC_DATE { set; get; }
        public DateTime? P_PAY_POSTED_DATE { set; get; }

        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }

    public class P_PaymentDetails
    {
        public int? P_PAY_DTLS_SYS_ID { set; get; }
        public int? P_PAY_MST_SYS_ID { set; get; }
        public int? P_PAY_DTLS_INV_SYS_ID { set; get; }

        public decimal? P_PAY_DTLS_PAY { set; get; }
        public decimal? P_PAY_DTLS_AMT { set; get; }
        public decimal? P_PAY_DTLS_REM { set; get; }

        public string P_PAY_DTLS_PUR_SYS_ID { set; get; }
        public char? P_PAY_DTLS_PAY_Y_N_P { set; get; }

        public DateTime? P_PAY_DTLS_INV_DATE { set; get; }

        public char? SELECTED_Y_N { set; get; }
        public string P_PAY_INV_NO { set; get; }


        public string P_PAY_V_CODE { set; get; }
        public char? P_PAY_POSTED_Y_N { set; get; }
        public int? P_PAY_DR_ACC_CODE { set; get; }
        public int? P_PAY_CR_ACC_CODE { set; get; }
        public string P_PAY_NOTE { set; get; }

        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }

    }

    public class PurchasePayment
    {
        public P_PaymentMaster PAYMENT_HDR { set; get; }
        public List<P_PaymentDetails> PAYMENT_DTL { set; get; }
    }
}
