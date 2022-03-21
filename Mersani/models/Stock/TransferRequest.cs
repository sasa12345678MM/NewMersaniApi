using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Stock
{
    public class TransferRequestMaster
    {
        public int? ITRH_SYS_ID { set; get; }
        public string ITRH_CODE { set; get; }
        public string ITRH_V_CODE { set; get; }
        public DateTime? ITRH_DATE { set; get; }
        public int? ITRH_RQSTR_INV_SYS_ID { set; get; }
        public int? ITRH_RQSTD_INV_SYS_ID { set; get; }
        public string ITRH_DESC { set; get; }
        public int? ITRH_OWNER_SYS_ID { set; get; }

        public char? ITRH_APPROVED_Y_N { set; get; }
        public int? ITRH_APPROVED_BY { set; get; }
        public DateTime? ITRH_APPROVED_DATE { set; get; }

        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class TransferRequestDetails
    {
        public int? ITRD_SYS_ID { set; get; }
        public int? ITRD_ITRH_SYS_ID { set; get; }
        public int? ITRD_BTCH_SYS_ID { set; get; }
        public int? ITRD_ITEM_SYS_ID { set; get; }
        public int? ITRD_UOM_SYS_ID { set; get; }
        public int? ITRD_RQSTD_QTY { set; get; }
        public int? ITRD_APPRVD_QTY { set; get; }

        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class TransferRequest
    {
        public TransferRequestMaster MASTER { set; get; }
        public List<TransferRequestDetails> DETAILS { set; get; }
    }
}
