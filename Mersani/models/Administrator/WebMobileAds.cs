using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Administrator
{
    public class WebMobileAds
    {
        public int? WMA_SYS_ID { get; set; }
        public string WMA_SHORT_NAME_AR { get; set; }
        public string WMA_SHORT_NAME_EN { get; set; }
        public string WMA_DESC_AR { get; set; }
        public string WMA_DESC_EN { get; set; }

        public string WMA_IMAGE { get; set; }
        public DateTime? WMA_START_DATE_TIME { get; set; }
        public DateTime? WMA_END_DATE_TIME { get; set; }
        public char? WMA_FRZ_Y_N { get; set; }
        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }



    }
}
