using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models
{
    public class COUNTRY_PROMOTION_HDR
    {
        public int? GCPH_SYS_ID { get; set; }
        public int? GCPH_C_SYS_ID { get; set; }
        public DateTime? GCPH_FROM_DATE { get; set; }
        public DateTime? GCPH_TO_DATE { get; set; }
        public char ? GCPH_CONF_Y_N { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }
    public class COUNTRY_PROMOTION_DTL
    {

        public int? GCPD_SYS_ID { get; set; }
        public int? GCPD_GCPH_SYS_ID { get; set; }
        public string GCPD_TYPE_M_G_I_V { get; set; }
        public int? GCPD_TYPE_SYS_ID { get; set; }
        public decimal? GCPD_FROM { get; set; }
        public decimal? GCPD_TO { get; set; }
        public string GCPD_ACTION_FP_DS_GF { get; set; }
        public int? GCPD_ACTION_VALUE { get; set; }
        public string GCPD_Q_V { get; set; }
        public string GCPD_ACTION_P_V { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }

    public class COUNTRY_PROMOTION
    {
        public COUNTRY_PROMOTION_HDR GASCOUNTRYPROMOTIONHDR { get; set; }
        public List<COUNTRY_PROMOTION_DTL> GASCOUNTRYPROMOTIONDTL { get; set; }


    }
}
