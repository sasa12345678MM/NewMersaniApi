using System.Data;
using System.Threading.Tasks;
using Mersani.models.FinancialSetup;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface ICustomerRepo
    {
        Task<DataSet> GetCustomerDataList(Customer customer, string authParms);
        Task<DataSet> BulkInsertUpdateCustomer(Customer customer, string authParms);
        Task<DataSet> DeleteCustomer(Customer customer, string authParms);

        Task<DataSet> SavePOSCustomer(Customer customer, string authParms);
        ///////////////////////////////////
        Task<DataSet> getFinsCustomerAddresses(FinsCustomerAddresses CustomerAddresses, int parentId, string authParms);
        Task<DataSet> PostFinsCustomerAddresses(FinsCustomerAddresses CustomerAddresses, string authParms);
        Task<DataSet> DeleteFinsCustomerAddresses(FinsCustomerAddresses CustomerAddresses, string authParms);
        Task<DataSet> getFinsCustomerRelatives(FinsCustomerRelatives CustomerRelatives, int parentId, string authParms);
        Task<DataSet> PostFinsCustomerRelatives(FinsCustomerRelatives CustomerRelatives, string authParms);
        Task<DataSet> DeleteFinsCustomerRelatives(FinsCustomerRelatives CustomerRelatives, string authParms);

        ///////////////////////////////////
        Task<DataSet> GetLastCode(string authParms); 
        Task<DataSet> GetPOSLastCode(string authParms); 
         //////////////////////////////////////////

         Task<DataSet> getCustomerByMobile(string mobile, string authParms);
    }
}
