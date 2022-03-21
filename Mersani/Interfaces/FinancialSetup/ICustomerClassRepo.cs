using Mersani.models.FinancialSetup;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface ICustomerClassRepo
    {
        Task<DataSet> GetCustomerClassDataList(CustomerClass customerClass, string authParms);
        Task<DataSet> BulkInsertUpdateCustomerData(List<CustomerClass> customerClasses, string authParms);
        Task<DataSet> DeleteCustomerData(CustomerClass customerClass, string authParms);
    }
}
