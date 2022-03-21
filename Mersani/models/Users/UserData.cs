namespace Mersani.models.Users
{
    public class UserData
    {
        public int? USR_CODE { get; set; }
        public string USR_LOGIN { get; set; }
        public string USR_PW { get; set; }
        public string USR_FULL_NAME_AR { get; set; }
        public string USR_FULL_NAME_EN { get; set; }
        public int? USRGRP_CODE { get; set; }
        public string USR_FRZ_Y_N { get; set; }
        public string USR_FRZ_REASON { get; set; }
        public string USR_MOB { get; set; }
        public string USR_EMAIL_ID { get; set; }
        public string USR_TEL { get; set; }
        public string USR_DEF_LANG { get; set; }
        public string DEFAULT_V_CODE { get; set; }
        public string USR_TYPE { get; set; }
        public string PIC_PATH { get; set; }
        public int? USR_ROLE_SYS_ID { get; set; }
        public char? USR_STARTUP_Y_N { get; set; }
        public char? USR_EMAIL_Y_N { get; set; }
        public char? USR_SMS_Y_N { get; set; }
        public char? USR_CASHIER_DISCOUNT_Y_N { get; set; }
        public int? USR_CASHIER_DISCOUNT_PCT { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }


    public class UpladFile
     {
        public string  FILENAME { get; set; }
        public string  Extention { get; set; }
        public string  FilePath { get; set; }
    }
}
