using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Sales
{
    public class InvSalesDnHdr
    {
        public int? ISDH_SYS_ID { get; set; }
        public string ISDH_CODE { get; set; }
        public string ISDH_V_CODE { get; set; }
        public int? ISDH_SOH_SYS_ID { get; set; }
        public string ISDH_DN_ATTACHED { get; set; }
        public DateTime? ISDH_DATE { get; set; }
        public string ISDH_NOTE { get; set; }
        public int? ISDH_DR_ACC_CODE { get; set; }
        public int? ISDH_CR_ACC_CODE { get; set; }
        public char? ISDH_APPROVED_Y_N { get; set; }
        public int? ISDH_APPROVED_BY { get; set; }
        public DateTime? ISDH_APPROVED_DATE { get; set; }
        public char? ISDH_CUST_RCVD_Y_N { get; set; }
        public string ISDH_CUST_RCVD_ATTH { get; set; }

        public int? ISDH_INV_SYS_ID { get; set; }
        public string ISDH_DESC { get; set; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
    public class InvSalesDnDtl
    {
        public int? ISDD_SYS_ID { get; set; }
        public int? ISDD_ISDH_SYS_ID { get; set; }
        public int? ISDD_ITEM_SYS_ID { get; set; }
        public int? ISDD_ITEM_QTY { get; set; }
        public int? ISDD_ITEM_UOM_SYS_ID { get; set; }
        public int? ISDD_AMOUNT { get; set; }
        public int? ISDD_BATCH_SYS_ID { get; set; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
    public class InvSalesDeleveryNote
    {
        public InvSalesDnHdr INVSALESDNHDR { get; set; }
        public List<InvSalesDnDtl> INVSALESDNDTL { get; set; }
    }

}
