using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Finance
{
    public class AccountStatement
    {
        public int? ACCOUNTLEVEL { get; set; }
        public int? ACCOUNTNUMBER { get; set; }
        public DateTime DATEFROM { get; set; }
        public DateTime DATETO { get; set; }
        public string V_CODE { get; set; }
        public int INS_USER { get; set; }


    }
}
