using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mersani.models.Administrator;

namespace Mersani.Interfaces.Administrator
{
    public interface IGroupRepo
    {
        Task<DataSet> GetGroup(Group entity, string authParms);
        Task<DataSet> BulkGroups(List<Group> entities, string authParms);
        Task<DataSet> DeleteGroup(Group entity, string authParms);

        Task<DataSet> GetLastCode(string authParms);
    }
}
