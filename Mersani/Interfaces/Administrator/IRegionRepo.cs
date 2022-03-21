using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mersani.models.Administrator;

namespace Mersani.Interfaces.Administrator
{
    public interface IRegionRepo
    {
        Task<DataSet> GetRegions(int id, string authParms);
        Task<DataSet> PostRegion(Region region, string authParms);
        Task<DataSet> DeleteRegion(Region entity, string authParms);

        Task<DataSet> GetLastCode(int id, string authParms);
    }
}
