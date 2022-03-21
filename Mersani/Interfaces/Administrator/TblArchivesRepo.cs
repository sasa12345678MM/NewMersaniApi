using Mersani.models.Administrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
    public interface TblArchivesRepo
    {
        Task<DataSet> GetTblArchives(TblArchives entity, string authParms);
        Task<DataSet> PostTblArchives(List<TblArchives> entities, string authParms);
        Task<DataSet> DeleteTblArchives(TblArchives entities, string authParms);
        //Task<DataSet> getscanArchives(int  id, string authParms);
    }
}
