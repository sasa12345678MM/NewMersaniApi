using System;

namespace Mersani.models.PointOfSale
{
    public class InsuranceContract
    {
        public int? PICNT_SYS_ID { get; set; }
        public int? PICNT_PIC_SYS_ID { get; set; }
        public int? PICNT_CUST_SYS_ID { get; set; }
        public double? PICNT_DISCOUNT_PCT { get; set; }
        public double? PICNT_DEDUCT_PCT { get; set; }
        public double? PICNT_MAX_DEDUCT_VAL { get; set; }
        public double? PICNT_UNAPPROV_LIMIT_VAL { get; set; }

        public DateTime? PICNT_START_DATE { get; set; }
        public DateTime? PICNT_END_DATE { get; set; }

        public string PICNT_CONTRACT_NO { get; set; }

        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class InsuranceContractClass
    {
        public int? PICNTC_SYS_ID { get; set; }
        public int? PICNTC_PICNT_SYS_ID { get; set; }
        public int? PICNTC_CLASS_CODE { get; set; }
        public double? PICNTC_DISCOUNT_PCT { get; set; }
        public double? PICNTC_DEDUCT_RATIO { get; set; }
        public double? PICNTC_MAX_DEDUCT { get; set; }
        public double? PICNTC_UNAPPROV_LIMIT { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }
}
