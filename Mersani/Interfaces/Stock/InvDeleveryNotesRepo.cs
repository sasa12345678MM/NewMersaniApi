using Mersani.models.Stock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Stock
{
    public interface InvDeleveryNotesRepo 
    {
        Task<DataSet> GetinvDeleveryNoteHdr(invDeleveryNoteHdr entity,string PostedType, string authParms);
        Task<DataSet> GetinvDeleveryNoteDtls(invDeleveryNoteDtl entity, string authParms);
        Task<DataSet> PostinvDeleveryNoteHdrDtl(DeleveryNotesData entities, string authParms);
        Task<DataSet> DeleteinvDeleveryNoteHdrDtl(invDeleveryNoteHdr entity, string authParms);
        Task<DataSet> approvalinvDeleveryNote(List<invDeleveryNoteHdr> entity, string authParms);
        Task<DataSet> GetLastCode(int inventory, string type,string authParms);


    }
}
