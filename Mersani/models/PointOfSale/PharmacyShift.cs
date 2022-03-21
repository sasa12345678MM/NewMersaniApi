using System;
using System.Collections.Generic;

namespace Mersani.models.PointOfSale
{
    public class PharmacyShiftMaster
    {
        public int? PSH_SYS_ID { set; get; }
        public int? PSH_PHARM_SYS_ID { set; get; }
        public int? PSH_WORKING_HOURS { set; get; }
        public string PSH_START_TIME { set; get; }
        public string PSH_END_TIME { set; get; }
        public int? PSH_NO_OF_SHIFTS { set; get; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }

    public class PharmacyShiftDetail
    {
        public int? PSD_SYS_ID { set; get; }
        public int? PSD_PSH_SYS_ID { set; get; }
        public string PSD_SHIFT_START_TIME { set; get; }
        public string PSD_SHIFT_END_TIME { set; get; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
    public class PharmacyShiftData
    {
        public PharmacyShiftMaster MASTER { set; get; }
        public List<PharmacyShiftDetail> DETAILS { set; get; }
    }

    public class PharmacyShiftActivation
    {
        public int? PSA_SYS_ID { set; get; }
        public int? PSA_PSD_SYS_ID { set; get; }
        public DateTime? PSA_DATE { set; get; }
        public int? PSA_USR_CODE { set; get; }
        public decimal? PSA_PRE_CASH { set; get; }
        public decimal? PSA_TOTAL_SALES { set; get; }
        public decimal? PSA_TOTAL_CASH { set; get; }
        public decimal PSA_TOTAL_CREDIT { set; get; }
        public decimal? PSA_CASH_LEFT { set; get; }
        public decimal? PSA_TOTAL_EXPENSES { set; get; }
        public int? PSA_CASH_ACC_CODE { set; get; }
        public int? PSA_EXPENSES_ACC_CODE { set; get; }
        public char? PSA_ACTIVE_Y_N { set; get; }
        public int? PHARM_SYS_ID { set; get; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
}
