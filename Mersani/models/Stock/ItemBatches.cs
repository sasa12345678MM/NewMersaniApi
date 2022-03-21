using System;

namespace Mersani.models.Stock
{
    public class ItemBatches
    {
        public int? IIB_SYS_ID { get; set; }
        public int? IIB_III_SYS_ID { get; set; }
        public int? IIB_BATCH_SYS_ID { get; set; }
        public int? IIB_OP_QTY { get; set; }
        public decimal? IIB_OP_UNIT_AMOUNT { get; set; }
        public int? IIB_UOM_SYS_ID { get; set; }
        public int? IIB_CURR_STK { get; set; }

        public int? CURRENT_QTY { get; set; }
        public int? BASIC_UNIT_ID { get; set; }
        public string BASIC_NAME_AR { get; set; }
        public string BASIC_NAME_EN { get; set; }

        public char? IIB_ADDED_M_A { get; set; }

        public string BATCH_NAME_AR { get; set; }
        public string BATCH_NAME_EN { get; set; }

        public string IIB_BATCH_NO { get; set; }
        public string IIB_BARCODE { get; set; }
        public DateTime? IIB_BATCH_PROD_DATE { get; set; }
        public DateTime? IIB_BATCH_EXP_DATE { get; set; }


        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }
}