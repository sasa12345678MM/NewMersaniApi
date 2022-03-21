using Mersani.Interfaces.Archive;
using Mersani.Interfaces.Notifications;
using Mersani.models.Archive;
using Mersani.models.Hubs;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Archive
{
    public class GeneralArchiveSetupRepository : GeneralArchiveSetupRepo
    {
        // headers
        public async Task<DataSet> GetGeneralArchiveSetupHeaders(LArchiveHead header, string authParms)
        {
            var query = $"select * from L_ARCHIVE_HEAD where (AH_SYS_ID=:AH_SYS_ID or :AH_SYS_ID=0)";
            var parms = new List<OracleParameter>() { new OracleParameter("AH_SYS_ID", header.AH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkGeneralArchiveSetupHeaders(List<LArchiveHead> headers, string authParms)
        {
            foreach (var entity in headers)
            {
                if (entity.AH_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_L_ARCHIVE_HEAD_XML", headers.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteGeneralArchiveSetupHeader(LArchiveHead header, string authParms)
        {
            header.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_L_ARCHIVE_HEAD_XML", new List<dynamic>() { header }, authParms);
        }


        // details
        public async Task<DataSet> GetGeneralArchiveSetupDetails(LArchiveDetail detail, string authParms)
        {
            var query = $"select * from L_ARCHIVE_DETAIL where (AD_AH_CODE=:AD_AH_CODE or :AD_AH_CODE=0)";
            var parms = new List<OracleParameter>() { new OracleParameter("AD_AH_CODE", detail.AD_AH_CODE) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkGeneralArchiveSetupDetails(List<LArchiveDetail> details, string authParms)
        {
            foreach (var entity in details)
            {
                if (entity.AD_CODE > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_L_ARCHIVE_DETAIL_XML", details.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteGeneralArchiveSetupDetail(LArchiveDetail detail, string authParms)
        {
            //var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            //detail.CURR_USER = authP.UserCode.Value;
            //detail.STATE = (int)OperationType.Delete;

            //Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            //parameters.Add("xml_document", new List<dynamic>() { detail });
            //return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_L_ARCHIVE_DETAIL_XML", parameters, authParms);
            detail.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_L_ARCHIVE_DETAIL_XML", new List<dynamic>() { detail }, authParms);
        }
    }
}
