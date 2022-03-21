using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mersani.models.Finance
{
    public class VoucherBudgetHdr
    {
        [Key]
        public int? VCHR_SYS_ID { set; get; }
        public string VCHR_TXN_TYPE { set; get; }
        public int? VCHR_CODE { set; get; }
        public DateTime? VCHR_DATE { set; get; }
        public int? VCHR_PERIOD_SYS_ID { set; get; }
        public int? VCHR_YEAR { set; get; }
        public string VCHR_RLTD_TXN_TYPE { set; get; }
        //public string VCHR_RLTD_CODE { set; get; }
        public int? VCHR_DOC_NO { set; get; }
        public int? VCHR_DOC_TYPE { set; get; }
        public DateTime? VCHR_DOC_DATE { set; get; }
        public int? VCHR_DEBIT_ACC { set; get; }
        public int? VCHR_CREDIT_ACC { set; get; }
        public decimal? VCHR_AMOUNT { set; get; }
        public int? VCHR_CURR_SYS_ID { set; get; }
        public decimal? VCHR_CUR_RATE { set; get; }
        public string VCHR_DESC { set; get; }
        public string VCHR_POSTED_Y_N { set; get; }
        public string VCHR_POSTED { set; get; }
        public DateTime? VCHR_POSTED_DATE { set; get; }
        public int? VCHR_POSTED_BY { set; get; }
        public string VCHR_POSTING_NOTES { set; get; }
        public string VCHR_DELETED_Y_N { set; get; }
        public DateTime? VCHR_DELETED_DATE { set; get; }
        public int? VCHR_DELETED_BY { set; get; }
        public string V_CODE { set; get; }
        public string VCHR_PARENT_V_CODE { set; get; }
        public int? INS_USER { get; set; }
        public int? STATE { get; set; }
        public int? VCHR_RLTD_TXN_SYS_ID { get; set; }
        // vchr_pay_rec_type?: number
        // vchr_br_actv_sys_id?: number;
        // vchr_pharm_sys_id?: number;
    }
    public class VoucherBudgetDet
    {
        [Key]
        public int? VCHR_DET_SYS_ID { set; get; }
        public int? VCHR_HDR_SYS_ID { set; get; }
        public string VCHR_DET_CODE { set; get; }
        public int? VCHR_DET_ACC_CODE { set; get; }
        public decimal? VCHR_DET_DEBIT { set; get; }
        public decimal? VCHR_DET_CREDIT { set; get; }
        public decimal? VCHR_DET_LDEBIT { set; get; }
        public decimal? VCHR_DET_LCREDIT { set; get; }
        public string VCHR_DET_DESC { set; get; }
        public int? VCHR_DET_COST_CENTER { set; get; }
        public string VCHR_DET_REFERENCE { set; get; }
        public int? INS_USER { set; get; }
        public int? STATE { get; set; }
    }
    public class VoucherBudget
    {
        public VoucherBudgetHdr VoucherBudgetHDR { set; get; }
        public List<VoucherBudgetDet> VoucherBudgetDET { set; get; }
    }
}
