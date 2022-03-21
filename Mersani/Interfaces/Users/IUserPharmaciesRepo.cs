using Mersani.models.Users;

using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Users
{
    public interface IUserPharmaciesRepo
    {
        Task<DataSet> GetUserPharmciesByUserId(int id, string authParms);
        Task<DataSet> BulkUserPharmcies(List<UserPharmacies> pharmacies, string authParms);
        Task<DataSet> DeleteUserPharmcy(int id, string authParms);
    }
}
