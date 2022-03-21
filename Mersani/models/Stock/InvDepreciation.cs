using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Stock
{
    public class invAdjstDepMstr
    {
        public int? IADM_SYS_ID { get; set; }
        public string IADM_CODE { get; set; }
        public DateTime? IADM_DATE { get; set; }
        public string IADM_DESC { get; set; }
        public string IADM_TYPE_INV_DEP_ADJ { get; set; }
        public int? IADM_INV_SYS_ID { get; set; }                     //NOT NULL;
        public int? IADM_CMTE_MNGR_USR_CODE { get; set; }             //NOT NULL;
        public int? IADM_CMTE_MBR1_USR_CODE { get; set; }             //NOT NULL;
        public int? IADM_CMTE_MBR2_USR_CODE { get; set; }             //NOT NULL;
        public int? IADM_CMTE_MBR3_USR_CODE { get; set; }             //NOT NULL;
        public string IADM_ATTACHMENT_PATH { get; set; }
        public int? IADM_DR_ACCOUNT_CODE { get; set; }                //NOT NULL;
        public int? IADM_CR_ACCOUNT_CODE { get; set; }                 // NOT NULL;
        public string IADM_POSTED_D_Y_N { get; set; }                  //NOT NULL
        public string IADM_V_CODE { get; set; }
        public int? INS_USER { set; get; }
        public int? STATE { set; get; }

    }
    public class invAdjstDepDtls
    {
        public int? IADD_SYS_ID { get; set; }
        public int? IADD_IADM_SYS_ID { get; set; }                //NOT NULL,
        public int? IADD_ITEM_SYS_ID { get; set; }                //NOT NULL,
        public int? IADD_ITEM_UOM { get; set; }                //NOT NULL,
        public int? IADD_ITEM_QTY { get; set; }                //NOT NULL,
        public string IADD_DESC { get; set; }
        public int? IADD_AMOUNT { get; set; }
        public int? IADD_ITEM_BATCH_SYS_ID { get; set; }
        public int? INS_USER { set; get; }
        public int? STATE { set; get; }

    }
    public class AdjstDepData
    {
        public invAdjstDepMstr INVADJSTDEPMSTR { get; set; }
        public List<invAdjstDepDtls> INVADJSTDEPDTLS { get; set; }
    }

}

