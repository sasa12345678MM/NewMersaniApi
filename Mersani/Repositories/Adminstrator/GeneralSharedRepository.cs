using Mersani.Controllers.Administrator;
using Mersani.Interfaces.Administrator;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class GeneralSharedRepository : GeneralSharedRepo
    {
        public async Task<DataSet> getInvItemcurrStk(int invSysId, int itemSysId, dynamic batchSysId, dynamic uomSysId, string authParms)
        {
            batchSysId = batchSysId == 0 ? null : batchSysId;
            uomSysId = uomSysId == 0 ? null : uomSysId;
            var query = $"select round(NVL(fn__item_btch_curr_stk(:p_inv_sys_id ,:p_item_sys_id ,:p_batch_sys_id ,:p_uom_sys_id ), 0),8) as curr_stk_qty from dual ";
            var parms = new List<OracleParameter>() {
                new OracleParameter("p_inv_sys_id",invSysId),
                new OracleParameter("p_item_sys_id", itemSysId),
                new OracleParameter("p_batch_sys_id", batchSysId),
                new OracleParameter("p_uom_sys_id", uomSysId)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetLoggedInPharmacyData(string authParms)
        {
            var query = $"SELECT * FROM gas_pharmacy WHERE ('PH' || PHARM_SYS_ID) = '{OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH}'";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetNearbyPharmacies(NearbyPharmaciesPosition position)
        {
            return await OracleDQ.ExcuteSelectizeProcAsync("PRC_GET_NEARBY_PHARAMCIES", new List<dynamic>() { position }, "", true);
        }

        public async Task<DataSet> GetPharamciesWithNotAvaliableItems(int ticketId, string authParms)
        {

            return await OracleDQ.ExcuteSelectizeProcAsync(
                "PRC_GET_PHARAMCIES_ITEMS_NOT_INSTOCK", 
                new List<dynamic>() { new { TICKET_ID  = ticketId, CURRENCY_ID = OracleDQ.GetAuthenticatedUserObject(authParms).UserCurrency } }, 
                authParms);
        }
    }
}
