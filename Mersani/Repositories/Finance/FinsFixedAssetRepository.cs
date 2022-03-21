using System.Data;
using System.Linq;
using Mersani.Oracle;
using System.Threading.Tasks;
using Mersani.models.Finance;
using Mersani.Interfaces.Finance;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

namespace Mersani.Repositories.Finance
{
    public class FinsFixedAssetRepository : IFinsFixedAssetRepo
    {
        public async Task<DataSet> GetDefaultFixedAssetAccount(string authParms)
        {
            var query = $"select * from FA_DEFAULT_ACC_VIEW " +
                $"where FUN_GET_PARENT_V_CODE(FA_DEFAULT_ACC_VIEW.V_CODE) =  FUN_GET_PARENT_V_CODE('{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() { }, authParms, CommandType.Text);
        }
        public async Task<DataSet> GetFinsFixedAssetTrans(int FinsFixedAsset, string PostType, string authParms)
        {
            var query = $"select * from FINS_FIXED_ASSET where FINS_FIXED_ASSET.ASSET_SYS_ID = :pCode OR :pCode = 0 " +
                $" and ASSET_PARENT_V_CODE= FUN_GET_PARENT_V_CODE('{ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}')";
            if (PostType != "")
                query += $" and ASSET_POSTED_Y_N='" + PostType + "'";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", FinsFixedAsset)
            }, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetSaleFinsFixedAssetTrans(int FinsFixedAsset, string authParms)
        {
            var query = $"select * from FINS_FIXED_ASSET " +
                $"where ASSET_SALE_Y_N='Y' " +
                $"and  (FINS_FIXED_ASSET.ASSET_SYS_ID = :pCode OR :pCode = 0) " +
                $"and ASSET_PARENT_V_CODE= FUN_GET_PARENT_V_CODE('{ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", FinsFixedAsset)
            }, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetUnSaleFinsFixedAssetTrans(int FinsFixedAsset, string authParms)
        {
            var query = $"select * from FINS_FIXED_ASSET " +
                 $"where ASSET_SALE_Y_N Not in('Y')" +
                 $" and  (FINS_FIXED_ASSET.ASSET_SYS_ID = :pCode OR :pCode = 0) " +
                $" and ASSET_PARENT_V_CODE= FUN_GET_PARENT_V_CODE('{ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            return await OracleDQ.ExcuteGetQueryAsync(query, new List<OracleParameter>() {
                new OracleParameter("pCode", FinsFixedAsset)
            }, authParms, CommandType.Text);
        }
        public async Task<DataSet> AddFinsFixedAsset(List<FinsFixedAsset> entities, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            foreach (FinsFixedAsset entity in entities)
            {
                entity.INS_USER = auth.UserCode;
                entity.ASSET_V_CODE = auth.User_Act_PH;
                entity.ASSET_PARENT_V_CODE = auth.User_Parent_V_Code;
                
                entity.STATE = 1;
                if (entity.ASSET_SYS_ID > 0)
                {
                    entity.STATE = 2;
                }
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_FIXED_ASSET_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> SaleFinsFixedAsset(List<FinsFixedAsset> entities, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            foreach (FinsFixedAsset entity in entities)
            {
                entity.INS_USER = auth.UserCode;
                entity.ASSET_V_CODE = auth.User_Act_PH;
                entity.ASSET_PARENT_V_CODE = auth.User_Parent_V_Code;

               // entity.ASSET_SALE_Y_N = "Y";
                entity.STATE = 2;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_SALE_FINS_FIXED_ASSET_XML", entities.ToList<dynamic>(), authParms);
        }
        public async Task<DataSet> DeletFinsFixedAsset(FinsFixedAsset entity, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);

            entity.INS_USER = auth.UserCode;
            entity.ASSET_V_CODE = auth.User_Act_PH;
            entity.ASSET_PARENT_V_CODE = auth.User_Parent_V_Code;

            entity.STATE = 3;

            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_FIXED_ASSET_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> PostFinsFixedAsset(List<FinsFixedAsset> entities, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            foreach (FinsFixedAsset entity in entities)
            {
                entity.INS_USER = auth.UserCode;
                entity.ASSET_V_CODE = auth.User_Act_PH;
                entity.ASSET_PARENT_V_CODE = auth.User_Parent_V_Code;
                entity.STATE = 1;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_POST_FINS_FIXED_ASSET_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> GetFinsFixedAssetDep(int FinsFixedAsset, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            var query = $" select* from FINS_FIXED_ASSET_DEPR where (FINS_FIXED_ASSET_DEPR.ASSETD_ASSET_SYS_ID=:PASSETD_SYS_ID or :PASSETD_SYS_ID=0)";
            var parms = new List<OracleParameter>() {
                new OracleParameter(":PASSETD_SYS_ID", FinsFixedAsset)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public bool ExecfixedAssetDepretiation(string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("P_V_CODE", OracleDbType.Varchar2, ParameterDirection.Input, auth.User_Act_PH);
            dyParam.Add("P_USR", OracleDbType.Int32, ParameterDirection.Input, auth.UserCode);
            return OracleDQ.PostData("PRC_FIXED_ASSET_DEP", authParms, dyParam, commandType: CommandType.StoredProcedure);
            
        }
        public async Task<DataSet> PostFinsFixedAssetDepr(List<FinsFixedAssetDepr> entities, string authParms)
        {
            var auth = OracleDQ.GetAuthenticatedUserObject(authParms);
            foreach (FinsFixedAssetDepr entity in entities)
            {
                entity.INS_USER = auth.UserCode;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_POST_FINS_ASSET_DEPR_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> GetLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX (TO_NUMBER (ASSET_CODE)), 0) + 1 AS Code FROM FINS_FIXED_ASSET where ASSET_PARENT_V_CODE= FUN_GET_PARENT_V_CODE('" + OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH + "')";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetfixedAssetDepretiation(int id, string authParms)
        {
            var query = $"select * from FINS_FIXED_ASSET " +
                   $"where ASSET_SALE_Y_N Not in('Y')" +
                   $"AND ASSET_SALVAGED_Y_N = 'N' " +
                   $"and ASSET_POSTED_Y_N = 'Y' " +
                   $"and  (FINS_FIXED_ASSET.ASSET_SYS_ID = :pCode OR :pCode = 0) " +
                   $"and ASSET_PARENT_V_CODE= FUN_GET_PARENT_V_CODE('{ OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}') ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pCode", id)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
    }
}
