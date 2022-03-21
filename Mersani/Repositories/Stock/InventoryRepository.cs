using System.Collections.Generic;
using System.Threading.Tasks;
using Mersani.models.Stock;
using Mersani.Interfaces.Stock;
using System.Data;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;

namespace Mersani.Repositories.Stock
{
    public class InventoryRepository : IInventoryRepo
    {
        public async Task<DataSet> GetInventoryData(Inventory entity, string authParms)
        {
            //int userCode = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode.Value;
            string vCode = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;
            //var query = $"SELECT * FROM INV_INVENTORY_MASTER WHERE IIM_SYS_ID = :pSYS_ID OR :pSYS_ID = 0";
            var query = $"SELECT DISTINCT IIM_SYS_ID,IIM_CODE, IIM_NAME_AR,IIM_NAME_EN,IIM_OWNER_SYS_ID,IIM_CITY_SYS_ID,IIM_AREA,IIM_LENGTH,IIM_WIDTH,IIM_NO_OF_SHELVES, " +
                $" INV_FRZ_Y_N,IIM_MGR_USR_CODE,IIM_INV_TYPE_I_S,IIM_INV_S_PHARM_SYS_ID,IIM_V_CODE,IIM_DR_ACCOUNT_CODE,IIM_CR_ACCOUNT_CODE,IIM_OP_APPROVED_Y_N, IIM_ALLOW_NGTV_STK_Y_N " +
                $" FROM USR_INV_VIEW WHERE V_CODE = '{vCode}'";
            //var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", userCode) };
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostInventoryData(Inventory entity, string authParms)
        {
            entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            entity.IIM_V_CODE = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;

            if (entity.IIM_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
            else entity.STATE = (int)OperationType.Add;

            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_INVENTORY_DATA_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> DeleteInventoryData(Inventory entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_INV_INVENTORY_DATA_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetInventoryDataById(int invId, string authParms)
        {
            var query = $"SELECT  * FROM INV_INVENTORY_MASTER WHERE IIM_SYS_ID = :pSYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pSYS_ID", invId) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (IIM_CODE, '^[0-9]+') THEN IIM_CODE ELSE '0' END)), 0) + 1 AS Code FROM INV_INVENTORY_MASTER " +
                $" WHERE FUN_GET_PARENT_V_CODE (IIM_V_CODE) = FUN_GET_PARENT_V_CODE('{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}')";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }


    }
}
