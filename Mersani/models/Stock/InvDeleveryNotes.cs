using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Stock
{
    public class invDeleveryNoteHdr
    {
        public int? IDNH_SYS_ID { get; set; }
        public string IDNH_CODE { get; set; }
        public string IDNH_V_CODE { get; set; }
        public DateTime? IDNH_DATE { get; set; }
        public string IDNH_DESC { get; set; }
        public int? IDNH_PO_SYS_ID { get; set; }
        public char? IDNH_APPRVD_Y_N { get; set; }
        public int? IDNH_APPRVD_BY { get; set; }
        public DateTime? IDNH_APPRVD_DATE { get; set; }
        public int? INS_USER { get; set; }
        public int? STATE { get; set; }
        ////////
        public int? IDNH_INV_SYS_ID { get; set; }
        public int? INVH_DR_ACCOUNT_CODE { get; set; }
        public int? INVH_CR_ACCOUNT_CODE { get; set; }
        public string INVH_TYPE_DNI_DNX_DNS { get; set; }
    }
    public class invDeleveryNoteDtl
    {
        public int? IDND_SYS_ID { get; set; }
        public int? IDND_IDNH_SYS_ID { get; set; }
        public int? IDND_ITEM_SYS_ID { get; set; }
        public int? IDND_UOM_SYS_ID { get; set; }
        public int? IDND_QTY { get; set; }
        ////////////////
        public string IDND_BATCH_NO { get; set; }
        public DateTime? IDND_BATCH_PROD_DATE { get; set; }
        public DateTime? IDND_BATCH_EXP_DATE { get; set; }
        public int? STATE { get; set; }
        public int? INS_USER { get; set; }
        public int? IDND_BATCH_SYS_ID { get; set; }
        public decimal? IDND_AMOUNT { get; set; }
        public string IDND_BARCODE { get; set; }
    }
    public class DeleveryNotesData
    {
        public invDeleveryNoteHdr INVDELEVERYNOTEHDR { get; set; }
        public List<invDeleveryNoteDtl> INVDELEVERYNOTEDTL { get; set; }
    }
}
