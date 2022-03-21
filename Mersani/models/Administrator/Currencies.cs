using System;

namespace Mersani.models.Administrator
{
    public class Currencies
    {
        public int? CURR_SYS_ID { get; set; }
        public string CURR_ID { get; set; }
        public string CURR_NAME_AR { get; set; }
        public string CURR_NAME_EN { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
        public string CURR_FRZ_Y_N { get; set; }
        public int? CURR_UNIT_POINTS { get; set; }
        public int? CURR_100_UNTS_AMNT { get; set; }

    }

    public class CurrencyRate
    {
        public int? CURRR_SYS_ID { get; set; }
        public int? CURRR_MAIN_CURR_SYS_ID { get; set; }
        public int? CURRR_DET_CURR_SYS_ID { get; set; }
        public decimal? CURRR_RATE { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
        public DateTime? CURRR_DATE { get; set; }

        public string CURR_NAME_AR { get; set; }
        public string CURR_NAME_EN { get; set; }
    }
}

   