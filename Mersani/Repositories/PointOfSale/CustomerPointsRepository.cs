using Mersani.Interfaces.PointOfSale;
using Mersani.models.PointOfSale;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.PointOfSale
{
    public class CustomerPointsRepository : ICustomerPointsRepo
    {
        public async  Task<DataSet> deleteCustomerPoints(CustomerPoints entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_CUSTOMER_POINTS_XML", new List<dynamic>() { entity }, authParms);
        }


        public async Task<DataSet> getCustomerPoints(CustomerPoints entity, string authParms)
        {
            var query = $"SELECT * FROM FINS_CUSTOMER_POINTS WHERE (FCP_SYS_ID = :pFCP_SYS_ID OR :pFCP_SYS_ID = 0) ";
            var parms = new List<OracleParameter>() { new OracleParameter("pFCP_SYS_ID", entity.FCP_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> postCustomerPoints(CustomerPoints entity, string authParms)
        {
            if (entity.FCP_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
            else entity.STATE = (int)OperationType.Add;
            entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;

            return await OracleDQ.ExcuteXmlProcAsync("PRC_CUSTOMER_POINTS_XML", new List<dynamic>() { entity }, authParms);
        }
        public async Task<DataSet> getCustomerPaymentPoint(int CUST_SYS_ID, string authParms)
        {
            var query = $"SELECT NVL(SUM(PCH_PONITS_PAYMENT),0)  FROM POS_CHASHER_HDR WHERE PCH_CUST_SYS_ID={CUST_SYS_ID}";

            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text); ;
        }
        public async Task<DataSet> getCustomerReplecPoint(int CUST_SYS_ID, int points, string authParms)
        {
            var query = $"select fn_get_POS_POINTS_AMOUNT({points},{CUST_SYS_ID}) as POINTS_AMOUNT from dual ";

            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text); ;
        }
    }
}
