using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.HR
{
    public class HrBanks
    {

      public int? HREB_SYS_ID  {get; set;}
      public string HREB_CODE {get; set;}
      public string HREB_NAME_AR { get; set; }
      public string HREB_NAME_EN { get; set; }
      public string HREB_SWIFT_CODE { get; set; }
      public int? STATE { get; set; }
      public int? CURR_USER { get; set; }
    }
}
