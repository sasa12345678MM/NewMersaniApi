using System;

namespace Mersani.models.FinancialSetup
{
    public class Supplier
    {
        public int? SUPP_SYS_ID { get; set; }
        public int? SUPP_CLASS_SYS_ID { get; set; }
        public string SUPP_COMMERCIAL_REG_NO { get; set; }
        public decimal? SUPP_CREDIT_LIMIT { get; set; }
        public int? SUPP_TIME_LIMIT { get; set; }
        public string SUPP_V_CODE { get; set; }
        public int? SUPP_ACC_CODE { get; set; }
        public string SUPP_VAT_NO { get; set; }
        public string SUPP_FRZ_Y_N { get; set; }
        public string SUPP_NOTE { get; set; }
        public string SUPP_CODE { get; set; }
        public string SUPP_PO_BOX { get; set; }
        public string SUPP_ADDRESS { get; set; }
        public string SUPP_ATT_NAME { get; set; }
        public string SUPP_TEL_1 { get; set; }
        public string SUPP_TEL_2 { get; set; }
        public string SUPP_FAX { get; set; }
        public string SUPP_ATT_MOBILE { get; set; }
        public string SUPP_ATT_EMAIL { get; set; }
        public string SUPP_NAME_AR { get; set; }
        public string SUPP_NAME_EN { get; set; }

        // DATA FOR SAVING
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
        
        //////////////////////////////////////////////////
        
        public char? SUPP_SERVICES_ITEMS_I_S { get; set; }
        public string SUPP_USERNAME { get; set; }
        public string SUPP_PASSWORD { get; set; }
        public char? SUPP_EMAIL_Y_N { get; set; }
        public char? SUPP_SMS_Y_N { get; set; }
        public char? SUPP_ADDED_SYSTEM_PORTAL_S_P { get; set; }
    }
}
