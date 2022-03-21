using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Stock
{
    public class InvRtrnDnHdr
    {
        public int? IRDNH_SYS_ID { get; set; }
        public string IRDNH_CODE { get; set; }
        public int? IRDNH_INV_SYS_ID { get; set; }
        public DateTime? IRDNH_DATE { get; set; }
        public string IRDNH_DESC { get; set; }
        public int? IRDNH_RPO_SYS_ID { get; set; }
        public string IRDNH_APPRVD_Y_N { get; set; }
        public int? IRDNH_APPRVD_BY { get; set; }
        public DateTime? IRDNH_APPRVD_DATE { get; set; }
        public int? IRDNH_DR_ACCOUNT_CODE { get; set; }
        public int? IRDNH_CR_ACCOUNT_CODE { get; set; }
        public string IRDNH_TYPE_DNI_DNX_DNS { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }
    public class InvRtrnDnDtl
    {
        public int? IRDND_SYS_ID { get; set; }
        public int? IRDND_IRDNH_SYS_ID { get; set; }
        public int? IRDND_ITEM_SYS_ID { get; set; }
        public int? IRDND_UOM_SYS_ID { get; set; }
        public int? IRDND_QTY { get; set; }
        public decimal? IRDND_AMOUNT { get; set; }
        public int? IRDND_BATCH_SYS_ID { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }
    public class InvRtrnDeleveryNotesData
    {
        public InvRtrnDnHdr INVRTRNDELEVERYNOTEHDR { get; set; }
        public List<InvRtrnDnDtl> INVRTRNDELEVERYNOTEDTL { get; set; }
    }
}
