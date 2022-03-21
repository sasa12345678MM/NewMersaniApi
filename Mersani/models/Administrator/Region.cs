using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mersani.models.Administrator
{
    //[Table("GAS_REGION")]
    public class Region
    {
        public int  R_SYS_ID { get; set; }
        public int R_COUNTRY_SYS_ID { get; set; }
        public int R_REGION_ID { get; set; }
        public string R_NAME_EN { get; set; }
        public string R_NAME_AR { get; set; }
        public int? STATE { get; set; }
        public int? CURR_USER { get; set; }
    }
}
