using Mersani.models.Stock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Stock
{
    public interface InvAssmblyItemRepo
    {
        Task<DataSet> GetInvAssmblyItemHdr(InvAssmblyItemHdr entity, string authParms);
        Task<DataSet> GetinvInvAssmblyItemDtls(invAssmblyItemHDtl entity, string authParms);
        Task<DataSet> PostInvAssmblyItemMasterDetails(InvAssmblyItmData entities, string authParms);
        Task<DataSet> DeleteInvAssmblyItemMasterDetails(InvAssmblyItemHdr entity, string authParms);
        Task<DataSet> GetLastCode(string type,string authParms);

    }
}
