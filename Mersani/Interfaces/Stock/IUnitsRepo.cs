using System.Data;
using Mersani.models.Stock;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mersani.Interfaces.Stock
{
    public interface IUnitsRepo
    {
        Task<DataSet> GetUnits(Units entity, string authParms);
        Task<DataSet> BulkUnits(List<Units> entities, string authParms);
        Task<DataSet> DeleteUnit(Units entity, string authParms);
    }
}
