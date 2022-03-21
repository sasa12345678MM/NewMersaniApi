using Mersani.models.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Sales
{
    public interface SalesDeleveryNoteRepo
    {
        Task<DataSet> GetDeleveryNoteHdr(InvSalesDnHdr entity, string PostedType, string authParms);
        Task<DataSet> GetInvSalesDnDtl(InvSalesDnDtl entity, string authParms);
        Task<DataSet> GetDeleveryNoteLastCode(string authParms);
        Task<DataSet> DeleveryNotePosting(List<InvSalesDnHdr> entities, string authParms);
        Task<DataSet> SaveDeleveryNoteHdrandItem(InvSalesDeleveryNote entities, string authParms);
        Task<DataSet> DeleteDeleveryNote(InvSalesDnHdr entities, string authParms);
        Task<DataSet> getInvSalesOrderDtl( int id, string authParms);
         ///////////
        Task<DataSet> getInvItemcurrStk(int invSysId, int itemSysId, int batchSysId, int uomSysId, string authParms);
    }
}
