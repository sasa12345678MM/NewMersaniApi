using Mersani.models.Administrator;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{ 
    public interface IPharmacySetupRepo
    {
        Task<List<PharmacySetup>> GetPharmacySetup(int id, string authParms);
        Task<List<PharmacySetup>> GetOwnerPharmacySetup(int id, string authParms);
        Task<DataSet> PostPharmacySetup(List<PharmacySetup> entity, string authParms);
        Task<DataSet> DeletPharmacySetup(List<PharmacySetup> entity, string authParms);
        Task<DataSet> GetLastCode(int ownersysID,string authParms);

    }
}
