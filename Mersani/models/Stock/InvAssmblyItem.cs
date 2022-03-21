using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Stock
{
    public class InvAssmblyItemHdr
    {
        public int? IAIH_SYS_ID { get; set; }
        //public int? IAIH_ITEM_SYS_ID { get; set; }
        public string IAIH_V_CODE { get; set; }
        public DateTime? IAIH_DATE { get; set; }
        public int? INS_USER { get; set; }
        public int? STATE { get; set; }
        ///////////////////////////////
        public char? IAIH_APPROVED_Y_N { get; set; }
        public int? IAIH_APPROVED_BY { get; set; }
        public DateTime? IAIH_APPROVED_DATE { get; set; }
        public char? IAIH_TYPE_ASM_DASM_A_D { get; set; }
        public int? IAIH_OWNER_SYS_ID { get; set; }
        public int? IAIH_INV_SYS_ID { get; set; }
        public string IAIH_NO { get; set; }


    }
    public class invAssmblyItemHDtl
    {
        public int? IAID_SYS_ID { get; set; }
        public int? IAID_HDR_SYS_ID { get; set; }
        public int? IAID_ITEM_SYS_ID { get; set; }
        public int? IAID_QTY { get; set; }
        public int? IAID_NOTES { get; set; }

        public int? INS_USER { get; set; }
        public int? STATE { get; set; }
    }
    public class InvAssmblyItmData
    {
        public InvAssmblyItemHdr INVASSMBLYITMHDR { get; set; }
        public List<invAssmblyItemHDtl> INVASSMBLYITMDTL { get; set; }

    }
}
