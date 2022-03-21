using Mersani.models.website;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.CostCenter
{
    public class TktSalesOrderHdr
    {
        public int? TSOH_SYS_ID { get; set; }
        public int? TSOH_CODE { get; set; }
        public DateTime? TSOH_DATE { get; set; }
        public string TSOH_NOTE { get; set; }
        public int? TSOH_CUST_SYS_ID { get; set; }
        public decimal? TSOH_DISCOUNT_PCT { get; set; }
        public decimal? TSOH_DISCOUNT_AMT { get; set; }
        public int? TSOH_CUST_ADDRSS_SYS_ID { get; set; }
        public int? TSOH_PH_SYS_ID { get; set; }
        public string TSOH_CURRENT_STATUS { get; set; }
        public DateTime? TSOH_CURRENT_STATUS_DATE { get; set; }
        public int? TSOH_CURRENT_STATUS_USR { get; set; }
        public string TSOH_CURRENT_STATUS_REASON { get; set; }
        public string TSOH_FILE_NAME { get; set; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
        public char TSOH_PAYMENT_Y_N { get; set; }
        public decimal TSOH_GRAND_TOTAL { get; set; }


    }
    public class TktSalesOrderDtl
    {
        public int? TSOD_SYS_ID { get; set; }
        public int? TSOD_TSOH_SYS_ID { get; set; }
        public int? TSOD_ITEM_SYS_ID { get; set; }
        public string ITEM_NAME_EN { get; set; }
        public string ITEM_NAME_AR { get; set; }
        public decimal? TSOD_ITEM_QTY { get; set; }
        public int? TSOD_ITEM_UOM_SYS_ID { get; set; }
        public int? TSOD_ITEM_UNIT_PRICE { get; set; }
        public char? TSOD_AVALIBLE_Y_N { get; set; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }

    }

    public class TktSalesOrderDetail
    {
        public int? TSOL_SYS_ID { get; set; }
        public int? TSOL_SOH_SYS_ID { get; set; }
        public DateTime? TSOL_DATE_TIME { get; set; }
        public string TSOL_DESC { get; set; }
        public string TSOL_FILE_NAME { get; set; }
        public string TSOL_REPORTER_REPORTED_R_D { get; set; }
        public string TSOL_APPRV_Y_N { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }
    public class TktSalesOrderHdrLog
    {
        public int? TSOHL_SYS_ID { get; set; }
        public int? TSOHL_TSOH_SYS_ID { get; set; }
        public DateTime? TSOHL_DATE { get; set; }
        public int? TSOHL_STATUS { get; set; }
    }
    public class TktSalesOrder
    {
        public TktSalesOrderHdr TKTSALESORDERHDR { get; set; }
        public List<TktSalesOrderDtl> TKTSALESORDERDTL { get; set; }

        public   WebPaymentStripeReturnModel paymentDetails { get; set; }

    }
}
