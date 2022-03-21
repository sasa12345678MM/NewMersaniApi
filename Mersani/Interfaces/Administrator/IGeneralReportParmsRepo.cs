using Mersani.models.Administrator;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
   public interface IGeneralReportParmsRepo
    {
        Task<DataSet> GetGeneralReportParms(GeneralReportParms reportId, string authParms);
        Task<DataSet> bulkGeneralReportParms(List<GeneralReportParms> GeneralReportParms, string authParms);
        Task<DataSet> deleteGeneralReportParms(GeneralReportParms GeneralReportParms, string authParms);

    }
}
