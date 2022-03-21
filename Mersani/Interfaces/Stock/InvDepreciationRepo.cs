using Mersani.models.Stock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Stock
{
    public interface InvDepreciationRepo
    {
        Task<DataSet> GetAdjstDepMstr(invAdjstDepMstr entity, string authParms);
        Task<DataSet> GetinvAdjstDepDtls(invAdjstDepDtls entity, string authParms);
        Task<DataSet> PostAdjstDepMasterDetails(AdjstDepData entities, string authParms);
        Task<DataSet> DeleteAdjstDepMasterDetails(invAdjstDepMstr entity, string authParms);
        Task<DataSet> approvalinvAdjstDepMstr(List<invAdjstDepMstr> entity, string authParms);
    }
}
