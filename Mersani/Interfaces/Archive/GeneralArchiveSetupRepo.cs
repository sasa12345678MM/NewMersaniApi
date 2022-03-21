using Mersani.models.Archive;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Archive
{
    public interface GeneralArchiveSetupRepo
    {
        // head
        Task<DataSet> GetGeneralArchiveSetupHeaders(LArchiveHead header, string authParms);
        Task<DataSet> BulkGeneralArchiveSetupHeaders(List<LArchiveHead> headers, string authParms);
        Task<DataSet> DeleteGeneralArchiveSetupHeader(LArchiveHead header, string authParms);


        // details
        Task<DataSet> GetGeneralArchiveSetupDetails(LArchiveDetail detail, string authParms);
        Task<DataSet> BulkGeneralArchiveSetupDetails(List<LArchiveDetail> details, string authParms);
        Task<DataSet> DeleteGeneralArchiveSetupDetail(LArchiveDetail detail, string authParms);


    }
}
