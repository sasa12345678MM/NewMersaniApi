using System;
using System.Collections.Generic;

namespace Mersani.models.CostCenter
{
    public class TicketMaster
    {
        public int? TTM_SYS_ID { get; set; }
        public string TTM_CODE { get; set; }
        public DateTime? TTM_DATE_TIME { get; set; }
        public string TTM_STATUS { get; set; }
        public DateTime? TTM_STATUS_DATE { get; set; }
        public string TTM_STATUS_REASON { get; set; }
        public int? TTM_STATUS_USR_CODE { get; set; }
        public string TTM_REPORTER_TYPE { get; set; }
        public int? TTM_REPORTER_SYS_ID { get; set; }
        public int? TTM_IMP_CODE { get; set; }
        public string TTM_DESC { get; set; }
        public string TTM_CALL_MTHD_V_M_P_O { get; set; }
        public string TTM_REPORTED_TYPE { get; set; }
        public int? TTM_REPORTED_SYS_ID { get; set; }
        public string TTM_REPORTED_NOTES { get; set; }
        public string TTM_FILE_NAME { get; set; }
        public int? TTM_TYPE_CODE { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }

    }

    public class TktTicketDetail
    {
        public int? TTD_SYS_ID { get; set; }
        public int? TTD_TTM_SYS_ID { get; set; }
        public DateTime? TTD_DATE_TIME { get; set; }
        public string TTD_DESC { get; set; }
        public string TTD_FILE_NAME { get; set; }
        public string TTD_REPORTER_REPORTED_R_D { get; set; }
        public string TTD_APPRV_Y_N { get; set; }
        public int? CURR_USER { get; set; }
        public int? STATE { get; set; }
    }


    public class TktTicketData
    {
        public TicketMaster TKTTICKETMASTER { get; set; }
        public List<TktTicketDetail> TKTTICKETDETAIL { get; set; }
    }

    public class TicketMasterLog
    {
        public int? TTML_SYS_ID { get; set; }
        public int? TTML_TTM_SYS_ID { get; set; }
        public DateTime TTML_DATE { get; set; }
        public string TTML_STATUS { get; set; }
    }
}
