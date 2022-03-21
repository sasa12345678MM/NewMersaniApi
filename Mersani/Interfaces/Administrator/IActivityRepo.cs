using Mersani.models.Administrator;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
    public interface IActivityRepo
    {
        Task<DataSet> GetActivityDataList(Activity entity, string authParms);
        Task<DataSet> BulkInsertUpdateActivity(List<Activity> entities, string authParms);
        Task<DataSet> DeleteActivityData(Activity entity, string authParms);
    }
}
