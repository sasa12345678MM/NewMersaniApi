using Mersani.models.Administrator;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{ 
    public interface IOwnerSetupRepo
    {
        Task<List<OwnerSetup>> GetOwnerSetup(int id, string authParms);
        Task<DataSet> PostOwnerSetup(List<OwnerSetup> entities, string authParms);
        Task<DataSet> DeletOwnerSetup(List<OwnerSetup> entities, string authParms);

        Task<DataSet> GetGasCInsCompany(gasOwnerInsCo entities, string authParms);
        Task<DataSet> PostGasCInsCompany(List<gasOwnerInsCo> entities, string authParms);

        Task<DataSet> getOwnerByMobile(string mobile, string authParms);
    }
}
