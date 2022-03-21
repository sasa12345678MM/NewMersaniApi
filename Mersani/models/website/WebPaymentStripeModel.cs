using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.website
{
    public class WebPaymentStripeModel
    {
        public string stripeToken { get; set; }
        public string stripeEmail { get; set; }
        public decimal amount { get; set; }
    }

    public class StripeSettings
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
    public class WebPaymentStripeReturnModel
    {

        public int? TSOP_SYS_ID { get; set; }
        public string TSOP_TKT_ID { get; set; }
        public decimal TSOP_TKT_AMOUNT { get; set; }
        public decimal TSOP_AMOUNT_CAPTURED { get; set; }
        public string TSOP_BALANCE_TRANSACTION { get; set; }
        public string TSOP_CAPTURED { get; set; }
        public string TSOP_CURRENCY { get; set; }
        public string TSOP_PAID { get; set; }
        public string TSOP_PAYMENT_METHOD { get; set; }
        public string TSOP_BRAND { get; set; }
        public string TSOP_FUNDING { get; set; }
        public string TSOP_STATUS { get; set; }
        public int? INS_USER { get; set; }
        public DateTime INS_DATE { get; set; }
        public int? UP_USER { get; set; }
        public int? STATE { get; set; }
        public string TSOP_TRANS_ID { get; set; }
    }

}
