using Mersani.models.Stock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Stock
{
    public interface  InvRtrnDeleveryNotesRepo
    {


        Task<DataSet> GetRtrnDeleveryNoteHdr(InvRtrnDnHdr entity, string PostedType, string authParms);
        Task<DataSet> GetInvRtrnDnDtl(InvRtrnDnDtl entity, string authParms);
        Task<DataSet> GetRtrnDeleveryNoteLastCode(int inventory,string authParms);
        Task<DataSet> RtrnDeleveryNotePosting(List<InvRtrnDnHdr> entities, string authParms);
        Task<DataSet> SaveRtrnDeleveryNoteHdrandItem(InvRtrnDeleveryNotesData entities, string authParms);
        Task<DataSet> DeleteRtrnDeleveryNote(InvRtrnDnHdr entities, string authParms);


    }
}
