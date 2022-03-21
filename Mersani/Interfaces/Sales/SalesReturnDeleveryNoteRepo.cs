using Mersani.models.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Sales
{
    public interface SalesReturnDeleveryNoteRepo
    {
        Task<DataSet> GetRtrnDeleveryNoteHdr(InvSalesRtrnDnHdr entity, string PostedType, string authParms);
        Task<DataSet> GetInvSalesRtrnDnDtl(InvSalesRtrnDnDtl entity, string authParms);
        Task<DataSet> GetRtrnDeleveryNoteLastCode(string authParms);
        Task<DataSet> RtrnDeleveryNotePosting(List<InvSalesRtrnDnHdr> entities, string authParms);
        Task<DataSet> SaveRtrnDeleveryNoteHdrandItem(InvSalesReturnDeleveryNote entities, string authParms);
        Task<DataSet> DeleteRtrnDeleveryNote(InvSalesRtrnDnHdr entities, string authParms);
    }
}
