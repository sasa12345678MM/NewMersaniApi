using System;

namespace Mersani.models.FinancialSetup
{
    public class Customer
    {
        public int? CUST_SYS_ID { get; set; }
        public int? CUST_CLASS_SYS_ID { get; set; }
        public string CUST_COMMERCIAL_REG_NO { get; set; }
        public int? CUST_CREDIT_LIMIT { get; set; }
        public int? CUST_TIME_LIMIT { get; set; }
        public string CUST_V_CODE { get; set; }
        public int? CUST_ACC_CODE { get; set; }
        public string CUST_VAT_NO { get; set; }
        public char? CUST_FRZ_Y_N { get; set; }
        public char? CUST_LOYALTY_Y_N { get; set; }
        public string CUST_IS_COMP_Y_N { get; set; }
        public string CUST_NOTE { get; set; }
        public string CUST_CODE { get; set; }
        public string CUST_ADDRESS { get; set; }
        public string CUST_ATT_EMAIL { get; set; }
        public string CUST_TEL_1 { get; set; }
        public string CUST_TEL_2 { get; set; }
        public string CUST_FAX { get; set; }
        public string CUST_ATT_NAME { get; set; }
        public string CUST_ATT_MOBILE { get; set; }
        public string CUST_NAME_AR { get; set; }
        public string CUST_NAME_EN { get; set; }
        public string CUST_PO_BOX { get; set; }
        public int? CUST_COUNTRY_SYS_ID { get; set; }

        

        // DATA FOR SAVING
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

        public char? CUST_SERVICES_ITEMS_I_S { get; set; }
        public string CUST_USERNAME { get; set; }
        public string CUST_PASSWORD { get; set; }
        public char? CUST_EMAIL_Y_N { get; set; }
        public char? CUST_SMS_Y_N { get; set; }
        public char? CUST_ADDED_SYSTEM_PORTAL_S_P { get; set; }
        public int? CUST_OWNR_SYS_ID { get; set; }
        public string CUST_REG_METHOD { get; set; }
        public string CUST_FRZ_REASON { get; set; }
        public int? CUST_FRZ_BY { get; set; }
        public DateTime? CUST_FRZ_DATE { get; set; }
        public string CUST_MEDICAL_REPORT { get; set; }
        public string CUST_FILE_NAME { get; set; }
        /////////////////////////////////////
        public string R_NAME_AR { get; set; }
        public string R_NAME_EN { get; set; }
        public string C_NAME_AR { get; set; }
        public string C_NAME_EN { get; set; }
        public string CITY_NAME_AR { get; set; }
        public string CITY_NAME_EN { get; set; }

    }
    public class FinsCustomerAddresses
    {
        public int? FCA_SYS_ID { get; set; }
        public int? FCA_CUST_SYS_ID { get; set; }
        public string FCA_ADDRES_DESC { get; set; }
        public int? FCA_CONTERY_SYS_ID { get; set; }
        public int? FCA_REGION_SYS_ID { get; set; }
        public int? FCA_CITY_SYS_ID { get; set; }
        public string FCA_STREET { get; set; }
        public string FCA_BUILD_NO { get; set; }
        public string FCA_FLOOR_NO { get; set; }
        public string FCA_FLAT_NO { get; set; }
        public string FCA_OTHER_DETAILS { get; set; }
        public decimal? FCA_LOC_X { get; set; }
        public decimal? FCA_LOC_Y { get; set; }
        public int? FCA_NEAREST_PHARM_SYS_ID { get; set; }
        public char? FCA_ACTIVE_Y_N { get; set; }
        public char? FCA_DEFAULT_Y_N { get; set; }
       public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }
    public class FinsCustomerRelatives
    {
        public int? FCR_SYS_ID { get; set; }
        public int? FCR_CUST_SYS_ID { get; set; }
        public string FCR_TYPE_CODE { get; set; }
        public DateTime? FCR_BIRTH_DATE { get; set; }
        public string FCR_MEDICAL_REPORT { get; set; }
        public string FCR_FILE_NAME { get; set; }
        public char? FCA_ACTIVE_Y_N { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }

}
