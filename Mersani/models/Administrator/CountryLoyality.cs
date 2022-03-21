using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Administrator
{
    public class CountryLoyalitySetup
    {
        public int? GCLS_SYS_ID { get; set; }
        public int? GCLS_C_SYS_ID { get; set; }
        public decimal? GCLS_FROM_AMOUNT { get; set; }
        public decimal? GCLS_TO_AMOUNT { get; set; }
        public decimal? GCLS_PCT { get; set; }
        public string GCLS_NOTES { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }
}
