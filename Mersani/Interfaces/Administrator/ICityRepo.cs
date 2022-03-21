using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mersani.models.Administrator;

namespace Mersani.Interfaces.Administrator
{
    public interface ICityRepo
    {
        Task<DataSet> GetCity(int id, string authParms);
        Task<DataSet> PostCity(City City, string authParms);
        Task<DataSet> DeleteCity(City entity, string authParms);

        Task<DataSet> GetLastCode(int id, string authParms);

     

        





    }
}
