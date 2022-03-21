using Mersani.models.Archive;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Archive
{
    public interface GeneralArchiveRepo
    {
        Task<DataSet> saveGeneralArchive(GeneralArchives entities, string authParms);
        // head
        Task<DataSet> GetGeneralArchiveHeaders(ArchiveHead header, string authParms);
        Task<DataSet> DeleteGeneralArchiveHeader(ArchiveHead header, string authParms);


        // details
        Task<DataSet> GetGeneralArchiveDetails(ArchiveDetail detail, string authParms);
        Task<DataSet> BulkGeneralArchiveDetails(List<ArchiveDetail> details, string authParms);
        Task<DataSet> DeleteGeneralArchiveDetail(ArchiveDetail detail, string authParms);

        Task<DataSet> GetLastCode(string authParms);


    }
}
