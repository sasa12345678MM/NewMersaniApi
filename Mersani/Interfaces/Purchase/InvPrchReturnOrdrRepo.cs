using Mersani.models.Purchase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Purchase
{
    public interface InvPrchReturnOrdrRepo
    {
        Task<DataSet> GetInvPrchReturnHdr(InvPrchReturnOrdrHdr entity, string PostedType, string authParms);
        Task<DataSet> GetInvPrchReturnOrdrDtl(InvPrchReturnOrdrDtl entity, string authParms);
        Task<DataSet> GetInvPrchLastCode(string authParms);
        Task<DataSet> InvPrchReturnPosting(List<InvPrchReturnOrdrHdr> entities, string authParms);
        Task<DataSet> GetDefaultAccountsForPurchase(InvPrchReturnOrdrHdr entity, string authParms);
        Task<DataSet> SaveInvPrchHdrandItem(PurchaseReturnOrderData entities, string authParms);
        Task<DataSet> DeleteInvPrchReturn(InvPrchReturnOrdrHdr entities, string authParms);
    }
}
