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
    public class GeneralArchiveRepository : GeneralArchiveRepo
    {
        // headers
        public async Task<DataSet> GetGeneralArchiveHeaders(ArchiveHead header, string authParms)
        {
            var query = $"SELECT AH.*, LAH.AH_NAME_AR, LAH.AH_NAME_EN " +
                $"  FROM ARCHIVE_HEAD AH" +
                $"       LEFT OUTER JOIN l_ARCHIVE_HEAD LAH" +
                $"          ON LAH.AH_SYS_ID = AH.AH_AH_CODE" +
                $" WHERE(AH.AH_SYS_ID = :AH_SYS_ID OR: AH_SYS_ID = 0) " +
                $"  and (AH_V_CODE='"+ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH +"')";
            var parms = new List<OracleParameter>() { new OracleParameter("AH_SYS_ID", header.AH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        // details
        public async Task<DataSet> GetGeneralArchiveDetails(ArchiveDetail detail, string authParms)
        {
            var query = $"select AD.*,lAD.AD_NAME_AR,lAD.AD_NAME_EN from ARCHIVE_DETAIL AD" +
                $"  LEFT OUTER JOIN l_ARCHIVE_DETAIL lAD" +
                $"          ON lAD.AD_CODE = AD.AD_AD_CODE" +
                $"          where(AD_AH_SYS_ID =:AD_AH_SYS_ID or: AD_AH_SYS_ID = 0)";
            var parms = new List<OracleParameter>() { new OracleParameter("AD_AH_SYS_ID", detail.AD_AH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> BulkGeneralArchiveDetails(List<ArchiveDetail> details, string authParms)
        {
            foreach (var entity in details)
            {
                if (entity.AD_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            }

            return await OracleDQ.ExcuteXmlProcAsync("PRC_ARCHIVE_DETAIL_XML", details.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> DeleteGeneralArchiveDetail(ArchiveDetail detail, string authParms)
        {
            detail.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_ARCHIVE_DETAIL_XML", new List<dynamic>() { detail }, authParms);
        }

        public async Task<DataSet> GetLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX (TO_NUMBER (AH_CODE)), 0) + 1 AS Code FROM ARCHIVE_HEAD where AH_V_CODE='"+ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH + "'";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> saveGeneralArchive(GeneralArchives entities, string authParms)
        {
            //MSTR
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            //entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entities.ARCHIVEHEAD.CURR_USER = authP.UserCode.Value;
            entities.ARCHIVEHEAD.AH_V_CODE = authP.User_Act_PH;

            if (entities.ARCHIVEHEAD.AH_SYS_ID > 0)
                entities.ARCHIVEHEAD.STATE = (int)OperationType.Update;
            else entities.ARCHIVEHEAD.STATE = (int)OperationType.Add;
            // DTL
            for (int i = 0; i < entities.ARCHIVEDETAIL.Count; i++)
            {
                entities.ARCHIVEDETAIL[i].CURR_USER = authP.UserCode;
                if (entities.ARCHIVEDETAIL[i].AD_SYS_ID > 0)
                    if (entities.ARCHIVEDETAIL[i].STATE == 3)
                    {
                        entities.ARCHIVEDETAIL[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.ARCHIVEDETAIL[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.ARCHIVEDETAIL[i].STATE = (int)OperationType.Add;
            }


            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.ARCHIVEHEAD });
            parameters.Add("xml_document_d", entities.ARCHIVEDETAIL.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_ARCHIVE_HEAD_XML", parameters, authParms);
        }
        public async Task<DataSet> DeleteGeneralArchiveHeader(ArchiveHead header, string authParms)
        {
            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            header.STATE = (int)OperationType.Delete;
            parameters.Add("xml_document_h", new List<dynamic>() { header });
            parameters.Add("xml_document_d", new List<dynamic>() { });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_ARCHIVE_HEAD_XML", parameters, authParms);
        }

    }
}
