using System;
using System.Collections.Generic;

namespace Mersani.models.Stock
{
    public class TransferMaster
    {
        public string ITM_CODE { get; set; }
        public string ITM_DESC { get; set; }
        public string ITM_TRNS_REASON { get; set; }

        public int? ITM_SYS_ID { get; set; }
        public int? ITM_FRM_INV_SYS_ID { get; set; }
        public int? ITM_TO_INV_SYS_ID { get; set; }

        public int? ITM_RELEASED_BY { get; set; } // 1
        public int? ITM_RELATED_SYS_ID { get; set; } // 2
        public int? ITM_OWNER_SYS_ID { get; set; } // 3
        public int? ITM_APPROVED_BY { get; set; } // 4
        public DateTime? ITM_APPROVED_DATE { get; set; } // 5
        public string ITM_TYPE_LTO_LTI { get; set; } // 6
        public char? ITM_APPROVED_Y_N { get; set; } // 7
        public DateTime? ITM_DATE { get; set; }

        public string ITM_V_CODE { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

        public string ITM_OTHER_CHARGES_DESC { get; set; }
        public int? ITM_REQUEST_SYS_ID { get; set; }
        public int? ITM_DR_ACC_CODE { get; set; }
        public int? ITM_CR_ACC_CODE { get; set; }
        public decimal? ITM_ITEMS_TOT_COST { get; set; }
        public decimal? ITM_OTHER_CHARGES_AMT { get; set; }
    }

    public class TransferDetails
    {
        public int? ITD_SYS_ID { get; set; }
        public int? ITD_ITM_SYS_ID { get; set; }
        public int? ITD_ITEM_SYS_ID { get; set; }
        public int? ITD_BTCH_SYS_ID { get; set; }
        public int? ITD_ITEM_UOM_SYS_ID { get; set; }
        public int? ITD_QTY { get; set; }
        public decimal? ITD_ITEM_COST { get; set; }

        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class InventoryTransfer
    {
        public TransferMaster MASTER { get; set; }
        public List<TransferDetails> DETAILS { get; set; }
    }
}
