using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.PointOfSale
{
    public class CustomerPoints
    {
        public int? FCP_SYS_ID { get; set; }
        public int? FCP_CUST_SYS_ID { get; set; }
        public DateTime FCP_DATE { get; set; }
        public int? FCP_CURR_POINTS { get; set; }
        public int? FCP_REPLACED_POINTS { get; set; }
        public int? FCP_REPLACED_POINTS_AMOUNT { get; set; }
        public int? FCP_REM_POINTS { get; set; }
        public int? FCP_TAKEN_POINTS_INV_SYS_ID { get; set; }
        public string FCP_DESC { get; set; }
        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }

    }

}
