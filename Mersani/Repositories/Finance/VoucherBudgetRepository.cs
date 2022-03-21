using Mersani.Interfaces.Finance;
using Mersani.models.Finance;
using Mersani.Oracle;
using Mersani.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Finance
{
    public class VoucherBudgetRepository : IVoucherBudgetRepo
    {

        public async Task<DataSet> GetVoucherBudget(int id, string authParms)
        {
            var query = $"SELECT FINS_VOUCHER_HDR_BDGT.*, VCHR_POSTED_Y_N AS vchr_posted FROM FINS_VOUCHER_HDR_BDGT " +
                $" WHERE (VCHR_SYS_ID = :pCode OR :pCode = 0)" +
                 //$" and VCHR_POSTED_Y_N in('D','R') " +
                 //$" and VCHR_TXN_TYPE not in ('RCT','PAY') " +
                 $" and VCHR_V_CODE = FUN_GET_PARENT_V_CODE('{ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", id)
            }, authParms, CommandType.Text);
        }



        public async Task<DataSet> GetALLVoucherBudget(int id, string authParms)
        {
            var query = $"SELECT FINS_VOUCHER_HDR_BDGT.*, VCHR_POSTED_Y_N AS vchr_posted FROM FINS_VOUCHER_HDR_BDGT  " +
                $"WHERE (VCHR_SYS_ID = :pCode OR :pCode = 0) " +
                 $" and VCHR_PARENT_V_CODE = FUN_GET_PARENT_V_CODE('{ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", id)
            }, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetPostedVoucherBudget(int id, string authParms)
        {
            var query = $"SELECT FINS_VOUCHER_HDR_BDGT.*, VCHR_POSTED_Y_N AS vchr_posted FROM FINS_VOUCHER_HDR_BDGT  " +
                $" WHERE (VCHR_SYS_ID = :pCode OR :pCode = 0) and VCHR_POSTED_Y_N ='Y' " +
                  $" and VCHR_PARENT_V_CODE = FUN_GET_PARENT_V_CODE('{ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", id)
            }, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetUnPostedVoucherBudget(int id, char postedType, string authParms)
        {
            var query = $"SELECT FINS_VOUCHER_HDR_BDGT.*, VCHR_POSTED_Y_N AS vchr_posted  " +
                $"FROM FINS_VOUCHER_HDR_BDGT  WHERE (VCHR_SYS_ID = :pCode OR :pCode = 0) " +
                $"and VCHR_POSTED_Y_N ='" + postedType + "' ";
            query += $" and VCHR_PARENT_V_CODE = FUN_GET_PARENT_V_CODE('{ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", id)
            }, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetVoucherBudgetTrans(int VoucherBudgetMaster, string authParms)
        {
            var query = $"SELECT * FROM FINS_VOUCHER_DET_BDGT  WHERE VCHR_HDR_SYS_ID  = :pCode";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", VoucherBudgetMaster)
            }, authParms, CommandType.Text);
        }


        public async Task<DataSet> PostVoucherBudget(VoucherBudget entities, string authParms)
        {
            //header
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entities.VoucherBudgetHDR.V_CODE = authP.User_Act_PH;
            entities.VoucherBudgetHDR.VCHR_PARENT_V_CODE = authP.User_Parent_V_Code;
            entities.VoucherBudgetHDR.INS_USER = authP.UserCode.Value;
            entities.VoucherBudgetHDR.VCHR_POSTED_Y_N = entities.VoucherBudgetHDR.VCHR_POSTED;

            if (entities.VoucherBudgetHDR.VCHR_SYS_ID > 0)
                entities.VoucherBudgetHDR.STATE = (int)OperationType.Update;
            else entities.VoucherBudgetHDR.STATE = (int)OperationType.Add;

            // details 
            for (int i = 0; i < entities.VoucherBudgetDET.Count; i++)
            {
                entities.VoucherBudgetDET[i].VCHR_HDR_SYS_ID = entities.VoucherBudgetHDR.VCHR_SYS_ID;
                entities.VoucherBudgetDET[i].INS_USER = authP.UserCode;
                if (entities.VoucherBudgetDET[i].VCHR_DET_SYS_ID > 0)
                    if (entities.VoucherBudgetDET[i].STATE == 3)
                    {
                        entities.VoucherBudgetDET[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.VoucherBudgetDET[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.VoucherBudgetDET[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.VoucherBudgetHDR });
            parameters.Add("xml_document_d", entities.VoucherBudgetDET.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_FINS_VoucherBudget_XML", parameters, authParms);
        }
        public async Task<DataSet> PostedVoucherBudgetHdr(List<VoucherBudgetHdr> entities, string authParms)
        {
            foreach (VoucherBudgetHdr entity in entities)
            {
                var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
                entity.INS_USER = authP.UserCode;
                entity.V_CODE = authP.User_Act_PH;
                entity.VCHR_PARENT_V_CODE = authP.User_Parent_V_Code;
                entity.VCHR_POSTED_Y_N = entity.VCHR_POSTED;
                entity.STATE = 2;

            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_POST_VOUCHER_Budget_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> deleteVoucherBudget(VoucherBudget entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entities.VoucherBudgetHDR.STATE = 3;
            entities.VoucherBudgetHDR.V_CODE = authP.User_Act_PH;
            entities.VoucherBudgetHDR.VCHR_PARENT_V_CODE = authP.User_Parent_V_Code;
            entities.VoucherBudgetHDR.INS_USER = authP.UserCode;

            for (int i = 0; i < entities.VoucherBudgetDET.Count; i++)
            {
                entities.VoucherBudgetDET[i].INS_USER = authP.UserCode;
                entities.VoucherBudgetDET[i].STATE = 3;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_POST_VOUCHER_Budget_XML", new List<dynamic>() { entities }, authParms);
        }
        public async Task<DataSet> GetLastCode(string authParms)
        {
            var query = $"  SELECT  NVL (MAX (TO_NUMBER (VCHR_CODE)), 0) + 1 AS Code " +
                $"  FROM FINS_VOUCHER_HDR_BDGT where FINS_VOUCHER_HDR_BDGT.VCHR_PARENT_V_CODE = FUN_GET_PARENT_V_CODE('" + OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH + "')";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
    }
}