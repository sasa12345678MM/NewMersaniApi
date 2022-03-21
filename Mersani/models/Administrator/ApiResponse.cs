using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.models.Administrator
{
    public class ApiResponse
    {
        public int VERRORCODE { set; get; }
        public String VERRORMSG { set; get; }
        public object Data { set; get; }
    }
}
