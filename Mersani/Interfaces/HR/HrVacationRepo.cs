using Mersani.models.HR;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
namespace Mersani.Interfaces.HR
{
    public interface HrVacationRepo
    {

        Task<DataSet> GetHrVacationsData(int HrVacations, string authParms);
        Task<DataSet> PostHrVacationsData(List<HrVacations> HrVacations, string authParms);
        Task<DataSet> DeleteHrVacationsData(HrVacations HrVacations, string authParms);
    }
}
