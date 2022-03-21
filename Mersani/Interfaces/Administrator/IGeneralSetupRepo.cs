using Mersani.models.Administrator;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
    public interface IGeneralSetupRepo
    { 
        // master
        Task<DataSet> GetMasterData(GeneralSetupMaster master, string authParms);
        Task<DataSet> PostMasterData(GeneralSetupMaster master, string authParms);
        Task<DataSet> DeleteMasterData(GeneralSetupMaster master, string authParms);
        // details
        Task<DataSet> GetDetailsData(GeneralSetupDetail detail, string authParms);
        Task<DataSet> BulkDetailsData(List<GeneralSetupDetail> details, string authParms);
        Task<DataSet> DeleteDetailsData(GeneralSetupDetail detail, string authParms);
    }
}
