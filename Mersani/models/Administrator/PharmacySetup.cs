using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Administrator
{
    public class PharmacySetup 
    {
        public int PHARM_SYS_ID { get; set; }
        public string PHARM_CODE { get; set; }
        public int OWNER_SYS_ID { get; set; }
        public string PHARM_NAME_AR { get; set; }
        public string PHARM_NAME_EN { get; set; }
        public string PHARM_LOCATION { get; set; }
        public string PHARM_PHONE { get; set; }
        public string PHARM_EMAIL { get; set; }
        public string PHARM_WEBSITE { get; set; }
        public string PHARM_FRZ_Y_N { get; set; }
        public int GAS_CITY_ID { get; set; }
        public string GOOGLE_MAP_DIR { get; set; }
        public char? PHARM_REMINDER_Y_N { get; set; }
        public char? PHARM_EMAIL_Y_N { get; set; }
        public char? PHARM_SMS_Y_N { get; set; }
        public string PHARM_MOB { get; set; }
        public int PHARM_LAT { get; set; }
        public int PHARM_LONG { get; set; }
        public char? PHARM_SHOW_WEB_MOB_Y_N { get; set; }
        public int? INS_USER { get; set; }
        public int? UP_USER { get; set; }
        public int? STATE { get; set; }

    }
}

