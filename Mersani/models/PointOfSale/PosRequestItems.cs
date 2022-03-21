using System;
using System.Collections.Generic;

namespace Mersani.models.PointOfSale
{
    public class PosRequestItemsMaster
    {
        public int? PRIH_SYS_ID { get; set; }
        public int? PRIH_CODE { get; set; }
        public string PRIH_V_CODE { get; set; }
        public DateTime? PRIH_DATE { get; set; }
        public int? PRIH_TSOH_SYS_ID { get; set; }
        public int? PRIH_RQSTR_PHRM_SYS_ID { get; set; }
        public int? PRIH_SNDR_PHRM_SYS_ID { get; set; }
        public char? PRIH_SNDR_APPRVD_Y_N { get; set; }
        public char? PRIH_RQSTR_CONFR_Y_N { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }
    public class PosRequestItemsDetails
    {
        public int? PRID_SYS_ID { get; set; }
        public int? PRID_PRIH_SYS_ID { get; set; }
        public int? PRID_ITEM_SYS_ID { get; set; }
        public int? PRID_UOM_SYS_ID { get; set; }
        public int? PRID_QTY { get; set; }
        public int? PRID_PRCH_PRICE { get; set; }
        public int? PRID_DISCOUNT_PERC { get; set; }
        public int? PRID_DISCOUNT_AMNT { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }
    public class PosRequestItems
    {
        public PosRequestItemsMaster MASTER { get; set; }
        public List<PosRequestItemsDetails> DETAILS { get; set; }
    }
}
