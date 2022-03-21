using Mersani.models.HR;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.HR
{
   public interface HrJobTitlesRepo
    {
        Task<DataSet> GetHrJobTitlesData(int hrJobTitles, string authParms);
        Task<DataSet> PostHrJobTitlesData(List<HrJobTitles> hrJobTitles, string authParms);
        Task<DataSet> DeleteHrJobTitlesData(HrJobTitles hrJobTitles, string authParms);
    }
}
