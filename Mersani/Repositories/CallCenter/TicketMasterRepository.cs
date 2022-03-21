using Mersani.Interfaces.CallCenter;
using Mersani.models.CostCenter;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.CallCenter
{
    public class TicketMasterRepository : ITicketMasterRepo
    {
        public async Task<DataSet> GetTicketMasterDataByCustomer(string authParms)
        {
            var query = $"SELECT TKT_TICKET_MASTER.*, " +
                $" CASE WHEN TKT_TICKET_MASTER.TTM_REPORTED_TYPE = 'PH' THEN GAS_PHARMACY.PHARM_NAME_en ELSE GAS_COMPANY.COMP_NAME_en END AS TTM_REPORTED_TYPE_name_En, " +
                $" CASE WHEN TKT_TICKET_MASTER.TTM_REPORTED_TYPE = 'PH' THEN GAS_PHARMACY.PHARM_NAME_ar ELSE GAS_COMPANY.COMP_NAME_ar END AS TTM_REPORTED_TYPE_name_ar  " +
                $" FROM TKT_TICKET_MASTER " +
                $" LEFT OUTER JOIN GAS_PHARMACY ON GAS_PHARMACY.PHARM_SYS_ID = TKT_TICKET_MASTER.TTM_REPORTED_SYS_ID " +
                $" LEFT OUTER JOIN GAS_COMPANY ON GAS_COMPANY.COMP_SYS_ID = TKT_TICKET_MASTER.TTM_REPORTED_SYS_ID " +
                $" WHERE TTM_REPORTER_SYS_ID = :pTTM_REPORTER_SYS_ID " +
                $" ORDER BY TTM_IMP_CODE ASC, TTM_CODE ASC ";

            var parms = new List<OracleParameter>() { new OracleParameter("pTTM_REPORTER_SYS_ID", OracleDQ.GetAuthenticatedUserObject(authParms).UserCode) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetTicketMasterData(TicketMaster entity, string authParms)
        {
            var query = $"SELECT TKT_TICKET_MASTER.*, " +
                $" CASE WHEN TKT_TICKET_MASTER.TTM_REPORTED_TYPE = 'PH' THEN GAS_PHARMACY.PHARM_NAME_en ELSE GAS_COMPANY.COMP_NAME_en END AS TTM_REPORTED_TYPE_name_En, " +
                $" CASE WHEN TKT_TICKET_MASTER.TTM_REPORTED_TYPE = 'PH' THEN GAS_PHARMACY.PHARM_NAME_ar ELSE GAS_COMPANY.COMP_NAME_ar END AS TTM_REPORTED_TYPE_name_ar  " +
                $" FROM TKT_TICKET_MASTER " +
                $" LEFT OUTER JOIN GAS_PHARMACY ON GAS_PHARMACY.PHARM_SYS_ID = TKT_TICKET_MASTER.TTM_REPORTED_SYS_ID " +
                $" LEFT OUTER JOIN GAS_COMPANY ON GAS_COMPANY.COMP_SYS_ID = TKT_TICKET_MASTER.TTM_REPORTED_SYS_ID " +
                $" WHERE (TTM_SYS_ID=:pTTM_SYS_ID OR :pTTM_SYS_ID=0) " +
                $" ORDER BY ttm_imp_code ASC, TTM_CODE ASC ";

            var parms = new List<OracleParameter>() { new OracleParameter("pTTM_SYS_ID", entity.TTM_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (TTM_CODE, '^[0-9]+') THEN TTM_CODE ELSE '0' END)), 0) + 1 AS Code FROM TKT_TICKET_MASTER ";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> UpdateTicketMaster(TicketMaster entity, string authParms)
        {
            var Auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            if (entity.TTM_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
            else entity.STATE = (int)OperationType.Add;
            entity.CURR_USER = Auth.UserCode;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_TICKET_MASTER_XML", new List<dynamic>() { entity }, authParms);
        }
        public async Task<DataSet> deleteTicketMaster(TicketMaster entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_TICKET_MASTER_XML", new List<dynamic>() { entity }, authParms); ;
        }

        public async Task<DataSet> GetTicketDetail(TktTicketDetail entity, string authParms)
        {
            var query = $"SELECT * FROM TKT_TICKET_DETAIL where TTD_TTM_SYS_ID =:PTTD_TTM_SYS_ID";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PTTD_TTM_SYS_ID", entity.TTD_TTM_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> SaveTicketDetail(List<TktTicketDetail> entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);

            for (int i = 0; i < entity.Count; i++)
            {
                entity[i].CURR_USER = authP.UserCode;
                if (entity[i].TTD_SYS_ID > 0)
                    if (entity[i].STATE == 3)
                    {
                        entity[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entity[i].STATE = (int)OperationType.Update;
                    }
                else
                    entity[i].STATE = (int)OperationType.Add;
            }


            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_d", entity.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_TICKET_DETAIL_XML", parameters, authParms);
        }

        public async Task<DataSet> getUnAnswerdTickedMaster(int id, string calltype, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $"SELECT TKT_TICKET_MASTER.*," +
                 $"       CASE" +
                $"          WHEN TKT_TICKET_MASTER.TTM_REPORTED_TYPE = 'PH'" +
                $"          THEN" +
                $"             GAS_PHARMACY.PHARM_NAME_en" +
                $"          ELSE" +
                $"             GAS_COMPANY.COMP_NAME_en" +
                $"       END" +
                $"          AS TTM_REPORTED_TYPE_name_En," +
                $"                CASE" +
                $"          WHEN TKT_TICKET_MASTER.TTM_REPORTED_TYPE = 'PH'" +
                $"          THEN" +
                $"             GAS_PHARMACY.PHARM_NAME_ar" +
                $"                ELSE" +
                $"             GAS_COMPANY.COMP_NAME_ar" +
                $"                END" +
                $"          AS TTM_REPORTED_TYPE_name_ar" +
                $"  FROM TKT_TICKET_MASTER" +
                $"       LEFT OUTER JOIN GAS_PHARMACY" +
                $"          ON GAS_PHARMACY.PHARM_SYS_ID =" +
                $"                TKT_TICKET_MASTER.TTM_REPORTED_SYS_ID" +
                $"       LEFT OUTER JOIN GAS_COMPANY" +
                $"          ON GAS_COMPANY.COMP_SYS_ID = TKT_TICKET_MASTER.TTM_REPORTED_SYS_ID " +
                $" WHERE     1=1 ";
            if (calltype == "CC") { query += $"  and TTM_STATUS  in ('TOC','WIC') AND (FUN_GET_PARENT_V_CODE(TTM_REPORTED_TYPE || TTM_REPORTED_SYS_ID) = FUN_GET_PARENT_V_CODE('{authP.User_Act_PH}') )"; }
            if (calltype == "ACT") { query += $" and TTM_STATUS  in ('WIP','TOP','OPN')  AND (FUN_GET_PARENT_V_CODE(TTM_REPORTED_TYPE || TTM_REPORTED_SYS_ID) = FUN_GET_PARENT_V_CODE('{authP.User_Act_PH}') )"; }
            query += $"order by ttm_imp_code  ,TTM_CODE ";

            var parms = new List<OracleParameter>() { new OracleParameter("TTM_SYS_ID", id) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> SaveTicketMasteDetail(TktTicketData entity, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            //entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entity.TKTTICKETMASTER.CURR_USER = authP.UserCode.Value;
            //entity.TKTTICKETMASTER.TTM_STATUS = authP.User_Act_PH;

            for (int i = 0; i < entity.TKTTICKETDETAIL.Count; i++)
            {
                entity.TKTTICKETDETAIL[i].CURR_USER = authP.UserCode;
                if (entity.TKTTICKETDETAIL[i].TTD_SYS_ID > 0)
                    if (entity.TKTTICKETDETAIL[i].STATE == 3)
                    {
                        entity.TKTTICKETDETAIL[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entity.TKTTICKETDETAIL[i].STATE = (int)OperationType.Update;
                    }
                else
                    entity.TKTTICKETDETAIL[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entity.TKTTICKETMASTER });
            parameters.Add("xml_document_d", entity.TKTTICKETDETAIL.ToList<dynamic>());
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_TICKET_Master_DETAIL_XML", parameters, authParms);
        }

        public async Task<DataSet> GetTicketMasterLogData(TicketMasterLog entity, string authParms)
        {
            var query = $"  select * from TKT_TICKET_MASTER_LOG where TKT_TICKET_MASTER_LOG.TTML_TTM_SYS_ID=:PTTML_TTM_SYS_ID ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("PTTML_TTM_SYS_ID", entity.TTML_TTM_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
    }
}
