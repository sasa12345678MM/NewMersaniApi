using Mersani.models.Auth;
using Mersani.models.FinancialSetup;
using Mersani.models.website;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Website.CusAuth
{
   public interface IWebAuthRepo
    {

        Task<DataSet> Login(WebLoginModel user);


        Task<DataSet> Register(Customer customer, string authParms);
        Task<DataSet> UpdateCustomer(Customer customer, string authParms);

    }
}
