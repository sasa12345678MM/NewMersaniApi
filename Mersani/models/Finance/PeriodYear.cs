using System;
using System.Collections.Generic;

namespace Mersani.models.Finance
{
    public class FinsPeriod
    {
        public int? PERIOD_SYS_ID { set; get; }
        public int? PERIOD_DAYS { set; get; }
        public int? PERIOD_MONTH { set; get; }
        public string PERIOD_Y_N { set; get; }
        public int? PERIOD_YEAR { set; get; }
        public DateTime? PERIOD_START_DT { set; get; }
        public DateTime? PERIOD_END_DT { set; get; }
        public string PERIOD_V_CODE { set; get; }
        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }
    public class FinsYear
    {
        public int? YEAR_SYS_ID { set; get; }
        public int? PERIOD_YEAR { set; get; }
        public string YEAR_V_CODE { set; get; }
        public char? YEAR_OPEN_Y_N { set; get; }
        public char? ADDED_OWN_CMP_Y_N { set; get; }
        public string PARENT_V_CODE { set; get; }

        public int? CURR_USER { set; get; }
        public int? STATE { set; get; }
    }

    public class FinancialYearPeriods
    {
        public FinsYear YEAR { set; get; }
        public List<FinsPeriod> PERIODS { set; get; }
    }
}
