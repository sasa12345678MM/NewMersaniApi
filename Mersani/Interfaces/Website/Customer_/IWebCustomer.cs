using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Website.Customer_
{
  public  interface IWebCustomer
    {
        Task<DataSet> GetCustomerDetailedAdresses(int customerid, string authParms);



    }
}
