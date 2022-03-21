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
    public class VoucherRepository : IVoucherRepo
    {
        public async Task<DataSet> GetVoucher(int id, string authParms)
        {
            var query = $"SELECT FINS_VOUCHER_HDR.*, VCHR_POSTED_Y_N AS vchr_posted FROM FINS_VOUCHER_HDR " +
                $" WHERE (VCHR_SYS_ID = :pCode OR :pCode = 0)" +
                $" and VCHR_POSTED_Y_N in('D','R') " +
                $" and VCHR_TXN_TYPE not in ('RCT','PAY') " +
                $" and VCHR_PARENT_V_CODE = FUN_GET_PARENT_V_CODE('{ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", id)
            }, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetVoucherIn(int id, string authParms)
        {
            var query = $"SELECT FINS_VOUCHER_HDR.*, VCHR_POSTED_Y_N AS vchr_posted FROM FINS_VOUCHER_HDR  " +
                $"WHERE (VCHR_SYS_ID = :pCode OR :pCode = 0) " +
                $"and VCHR_POSTED_Y_N in('D','R') " +
                $"and VCHR_TXN_TYPE  in ('RCT') " +
                 $" and VCHR_PARENT_V_CODE = FUN_GET_PARENT_V_CODE('{ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", id)
            }, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetVoucherOut(int id, string authParms)
        {
            var query = $"SELECT FINS_VOUCHER_HDR.*, VCHR_POSTED_Y_N AS vchr_posted FROM FINS_VOUCHER_HDR  " +
                $"WHERE (VCHR_SYS_ID = :pCode OR :pCode = 0) " +
                $"and VCHR_POSTED_Y_N in('D','R') " +
                $"and VCHR_TXN_TYPE  in ('PAY') " +
                 $" and VCHR_PARENT_V_CODE = FUN_GET_PARENT_V_CODE('{ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", id)
            }, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetALLVoucher(int id, string authParms)
        {
            var query = $"SELECT FINS_VOUCHER_HDR.*, VCHR_POSTED_Y_N AS vchr_posted FROM FINS_VOUCHER_HDR  " +
                $"WHERE (VCHR_SYS_ID = :pCode OR :pCode = 0) " +
                  $" and VCHR_PARENT_V_CODE = FUN_GET_PARENT_V_CODE('{ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", id)
            }, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetPostedVoucher(int id, string authParms)
        {
            var query = $"SELECT FINS_VOUCHER_HDR.*, VCHR_POSTED_Y_N AS vchr_posted FROM FINS_VOUCHER_HDR  " +
                $" WHERE (VCHR_SYS_ID = :pCode OR :pCode = 0) and VCHR_POSTED_Y_N ='Y' " +
                $" and VCHR_PARENT_V_CODE = FUN_GET_PARENT_V_CODE('{ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", id)
            }, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetUnPostedVoucher(int id, char postedType, string authParms)
        {
            var query = $"SELECT FINS_VOUCHER_HDR.*, VCHR_POSTED_Y_N AS vchr_posted ,CHK_VCHR_BALANCE(FINS_VOUCHER_HDR.VCHR_SYS_ID) AS PALANCED " +
                $"FROM FINS_VOUCHER_HDR  WHERE (VCHR_SYS_ID = :pCode OR :pCode = 0) " +
                $"and VCHR_POSTED_Y_N ='" + postedType + "' ";
            if (postedType == 'Y' || postedType == 'N')
            {
                query += " and CHK_VCHR_BALANCE(FINS_VOUCHER_HDR.VCHR_SYS_ID) =1 ";
            }
            query += $" and VCHR_PARENT_V_CODE = FUN_GET_PARENT_V_CODE('{ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", id)
            }, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetVoucherTrans(int voucherMaster, string authParms)
        {
            var query = $"SELECT * FROM FINS_VOUCHER_DET  WHERE VCHR_HDR_SYS_ID  = :pCode";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", voucherMaster)
            }, authParms, CommandType.Text);
        }


        public async Task<DataSet> PostVoucher(Voucher entities, string authParms)
        {
            //header
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entities.VOUCHERHDR.INS_USER = authP.UserCode.Value;
            entities.VOUCHERHDR.VCHR_POSTED_Y_N = entities.VOUCHERHDR.VCHR_POSTED;

            if (entities.VOUCHERHDR.VCHR_SYS_ID > 0)
                entities.VOUCHERHDR.STATE = (int)OperationType.Update;
            else entities.VOUCHERHDR.STATE = (int)OperationType.Add;

            // details 
            for (int i = 0; i < entities.VOUCHERDET.Count; i++)
            {
                entities.VOUCHERDET[i].VCHR_HDR_SYS_ID = entities.VOUCHERHDR.VCHR_SYS_ID;
                entities.VOUCHERDET[i].INS_USER = authP.UserCode;
                if (entities.VOUCHERDET[i].VCHR_DET_SYS_ID > 0)
                    if (entities.VOUCHERDET[i].STATE == 3)
                    {
                        entities.VOUCHERDET[i].STATE = (int)OperationType.Delete;
                    }
                    else
                    {
                        entities.VOUCHERDET[i].STATE = (int)OperationType.Update;
                    }
                else
                    entities.VOUCHERDET[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entities.VOUCHERHDR });
            parameters.Add("xml_document_d", entities.VOUCHERDET.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_FINS_VOUCHER_XML", parameters, authParms);
        }
        public async Task<DataSet> PostedVoucherHdr(List<VoucherHdr> entities, string authParms)
        {
            foreach (VoucherHdr entity in entities)
            {
                var authP = OracleDQ.GetAuthenticatedUserObject(authParms);

                entity.INS_USER = authP.UserCode;
                entity.V_CODE = authP.User_Act_PH;
                entity.VCHR_PARENT_V_CODE = authP.User_Parent_V_Code;
                entity.VCHR_POSTED_Y_N = entity.VCHR_POSTED;
                entity.STATE = 2;

            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_POST_DEL_VOUCHER_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeletVoucher(Voucher entities, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entities.VOUCHERHDR.STATE = 3;
            entities.VOUCHERHDR.V_CODE = authP.User_Act_PH;
            entities.VOUCHERHDR.INS_USER = authP.UserCode;
            entities.VOUCHERHDR.VCHR_PARENT_V_CODE = authP.User_Parent_V_Code;

            for (int i = 0; i < entities.VOUCHERDET.Count; i++)
            {
                entities.VOUCHERDET[i].INS_USER = authP.UserCode;
                entities.VOUCHERDET[i].STATE = 3;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_POST_DEL_VOUCHER_XML", new List<dynamic>() { entities }, authParms);
        }

        public Task<DataSet> DeletVoucher(List<Voucher> entities, string authParms)
        {
            throw new NotImplementedException();
        }

        //private OracleDynamicParameters GetDynamicParameters(Voucher entities, string authParms, OperationType operationType)
        //{
        //    var dyParam = new OracleDynamicParameters();
        //    dyParam.Add("xml_document_h", OracleDbType.XmlType, ParameterDirection.Input, SerializeEntity.Encode(new List<dynamic>() { entities.VOUCHERHDR }));
        //    dyParam.Add("xml_document_d", OracleDbType.XmlType, ParameterDirection.Input, SerializeEntity.Encode(entities.VOUCHERDET.ToList<dynamic>()));
        //    dyParam.Add("VERRORCODE", OracleDbType.Varchar2, ParameterDirection.Output);
        //    dyParam.Add("VERRORMSG", OracleDbType.Varchar2, ParameterDirection.Output);
        //    return dyParam;
        //}
    }
}
