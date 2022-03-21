using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Administrator
{
    public class OwnerSetup
    {
        public int OWNER_SYS_ID { get; set; }
        public string OWNER_CODE { get; set; }
        public string OWNER_NAME_AR { get; set; }
        public string OWNER_NAME_EN { get; set; }
        public string OWNER_MOB { get; set; }
        public string OWNER_EMAIL_ID { get; set; }
        public string OWNER_TEL { get; set; }
        public string OWNER_FRZ_Y_N { get; set; }
        public int OWNER_COUNTRY_SYS_ID { get; set; }
        public char? OWNER_REMINDER_Y_N { get; set; }
        public int OWNER_USR_CODE { get; set; }

        public int? INS_USER { get; set; }
        public int? UP_USER { get; set; }
        public int? STATE { get; set; }
    }

    public class gasOwnerInsCo
    {
        public int? GOIC_SYS_ID { get; set; }
        public int? GOIC_OWNER_SYS_ID { get; set; }
        public int? GOIC_INS_CO_SYS_ID { get; set; }
        public int? GOIC_ACC_CODE { get; set; }
        public string GOIC_NOTES { get; set; }
        public char? GOIC_FRZ_Y_N { get; set; }
        public string GOIC_FRZ_REASON { get; set; }
        

        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }
}
