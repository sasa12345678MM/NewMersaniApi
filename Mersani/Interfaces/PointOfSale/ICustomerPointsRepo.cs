using Mersani.models.PointOfSale;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.PointOfSale
{
   public interface ICustomerPointsRepo 
    {
        Task<DataSet> getCustomerPoints(CustomerPoints entity, string authParms);
        Task<DataSet> postCustomerPoints(CustomerPoints entity, string authParms);
        Task<DataSet> deleteCustomerPoints(CustomerPoints entity, string authParms);
        Task<DataSet> getCustomerPaymentPoint(int point, string authParms);
        Task<DataSet> getCustomerReplecPoint(int CUST_SYS_ID, int points, string authParms);



    }
}
