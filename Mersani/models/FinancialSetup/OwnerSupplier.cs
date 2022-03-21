using System;

namespace Mersani.models.FinancialSetup
{
    public class OwnerSupplier
    {


        public int? FOS_SYS_ID { get; set; }
        public int? FOS_CLASS_SYS_ID { get; set; }
        public string FOS_COMMERCIAL_REG_NO { get; set; }
        public decimal? FOS_CREDIT_LIMIT { get; set; }
        public int? FOS_TIME_LIMIT { get; set; }
        public string FOS_V_CODE { get; set; }
        public int FOS_ACC_CODE { get; set; }
        public string FOS_VAT_NO { get; set; }
        public string FOS_FRZ_Y_N { get; set; }
        public string FOS_NOTE { get; set; }
        public string FOS_CODE { get; set; }
        public string FOS_PO_BOX { get; set; }
        public string FOS_ADDRESS { get; set; }
        public string FOS_ATT_NAME { get; set; }
        public string FOS_TEL_1 { get; set; }
        public string FOS_TEL_2 { get; set; }
        public string FOS_FAX { get; set; }
        public string FOS_ATT_MOBILE { get; set; }
        public string FOS_ATT_EMAIL { get; set; }
        public string FOS_NAME_AR { get; set; }
        public string FOS_NAME_EN { get; set; }

        // DATA FOR SAVING
        
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
        //////////////////////////////////////
        //public int? FOS_OWNER_SYS_ID { get; set; }
        public int? FOS_OWNER_COMP_SYS_ID { get; set; }
        public string FOS_WHATSAPP { get; set; }
        public int? FOS_LNKED_SUPP_SYS_ID { get; set; }
        public string FOS_OWN_CMP { get; set; }


        public char? FOS_SERVICES_ITEMS_I_S { get; set; }
        public string FOS_USERNAME { get; set; }
        public string FOS_PASSWORD { get; set; }
        public char? FOS_EMAIL_Y_N { get; set; }
        public char? FOS_SMS_Y_N { get; set; }
        public char?FOS_ADDED_SYSTEM_PORTAL_S_P { get; set; }

    }
}
