using System;

namespace Mersani.models.FinancialSetup
{
    public class FinsAccount
    {
        public int? ACC_CODE { get; set; }
        public string ACC_NO { set; get; }
        public int? ACC_LEVEL_CODE { set; get; }
        public string ACC_NAME_AR { set; get; }
        public string ACC_NAME_EN { set; get; }
        public int? PARENT_ACC_CODE { set; get; }
        public string PARENT_ACC_NO { set; get; }
        public string ACC_STATUS { set; get; }
        public int? ACC_ITMNODR { set; get; }
        public int? ACC_ITMNOCR { set; get; }
        public int? ACC_CLASS_CODE { set; get; }
        public string ACC_TYPICAL_BALANCE { set; get; }
        public string ACC_POSTING_TYPE { set; get; }
        public string ACC_OLD_NO { set; get; }
        public string ACC_V_CODE { set; get; }
        public int? ACC_COST_CENTER { set; get; }
        public int? ACC_OPEN_DR { set; get; }
        public int? ACC_OPEN_CR { set; get; }
        public int? ACC_TRIAL_BALANCE { set; get; }
        public int? ACC_BR_ACTV_SYS_ID { set; get; }
        public int? ACC_PHARM_SYS_ID { set; get; }
        public int? INS_USER { get; set; }
        public int? STATE { get; set; }

    }
}

