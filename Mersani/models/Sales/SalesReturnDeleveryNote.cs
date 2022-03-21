using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Sales
{
    public class InvSalesRtrnDnHdr
    {
        public int? ISRDH_SYS_ID { get; set; }
        public string ISRDH_CODE { get; set; }
        public string ISRDH_V_CODE { get; set; }
        public int? ISRDH_SROH_SYS_ID { get; set; }
        public string ISRDH_DN_ATTACHED { get; set; }
        public DateTime? ISRDH_DATE { get; set; }
        public string ISRDH_NOTE { get; set; }
        public int? ISRDH_DR_ACC_CODE { get; set; }
        public int? ISRDH_CR_ACC_CODE { get; set; }
        public char? ISRDH_APPROVED_Y_N { get; set; }
        public int? ISRDH_APPROVED_BY { get; set; }
        public DateTime? ISRDH_APPROVED_DATE { get; set; }
        public int? ISRDH_INV_SYS_ID { get; set; }
        public string ISRDH_DESC { get; set; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
    public class InvSalesRtrnDnDtl
    {
        public int? ISRDD_SYS_ID { get; set; }
        public int? ISRDD_ISRDH_SYS_ID { get; set; }
        public int? ISRDD_ITEM_SYS_ID { get; set; }
        public int? ISRDD_ITEM_QTY { get; set; }
        public int? ISRDD_ITEM_UOM_SYS_ID { get; set; }
        public decimal? ISRDD_AMOUNT { get; set; }
        public int? ISRDD_BATCH_SYS_ID { get; set; }
        public char? ISRDD_REQ_INSP_Y_N { get; set; }
        public char? ISRDD_INSP_GOOD_BAD_G_B { get; set; }
        public int? ISRDD_INSP_BY { get; set; }
        public DateTime? ISRDD_INSP_DATE { get; set; }
        public int? ISRDD_INSP_INV_SYS_ID { get; set; }
        public string ISRDD_INSP_NOTES { get; set; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }

    }
    public class InvSalesReturnDeleveryNote
    {
       public  InvSalesRtrnDnHdr INVSALESRTRNDNHDR { get; set; }
       public List<InvSalesRtrnDnDtl> INVSALESRTRNDNDTL { get; set; }

    }
}
