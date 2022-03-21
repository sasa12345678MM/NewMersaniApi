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
    public class InsuranceContractRepository : IInsuranceContractRepo
    {
        // main data
        public async Task<DataSet> GetInsuranceContractDataList(InsuranceContract entity, string authParms)
        {
            var query = $"SELECT cnt.*, cmp.PIC_NAME_AR, cmp.PIC_NAME_EN, cust.CUST_NAME_AR, cust.CUST_NAME_EN FROM POS_INSURANCE_CONTRACT cnt " +
                $" LEFT JOIN POS_INSURANCE_CMP cmp ON cnt.PICNT_PIC_SYS_ID = cmp.PIC_SYS_ID " +
                $" LEFT JOIN FINS_CUSTOMER cust ON cust.CUST_SYS_ID = cnt.PICNT_CUST_SYS_ID " +
                $" WHERE (PICNT_SYS_ID = :pPICNT_SYS_ID OR :pPICNT_SYS_ID = 0) ";
            var parms = new List<OracleParameter>() { new OracleParameter("pPICNT_SYS_ID", entity.PICNT_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> InsertUpdateInsuranceContract(InsuranceContract entity, string authParms)
        {
            if (entity.PICNT_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
            else entity.STATE = (int)OperationType.Add;
            entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;

            return await OracleDQ.ExcuteXmlProcAsync("PRC_POS_INSURANCE_CONTRACT_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> DeleteInsuranceContract(InsuranceContract entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_POS_INSURANCE_CONTRACT_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetContractLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (PICNT_CONTRACT_NO, '^[0-9]+') THEN PICNT_CONTRACT_NO ELSE '0' END)), 0) + 1 AS Code FROM POS_INSURANCE_CONTRACT ";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        // classes
        public async Task<DataSet> GetInsuranceContractClassList(InsuranceContractClass entity, string authParms)
        {
            var query = $"SELECT POS_INSURANCE_CONTRACT_CLASS.*, GSD_NAME_AR AS CLASS_NAME_AR, GSD_NAME_EN AS CLASS_NAME_EN FROM POS_INSURANCE_CONTRACT_CLASS,GAS_GNRL_SET_DTL " +
                $" WHERE PICNTC_CLASS_CODE = GSD_CODE AND GSD_GSH_SYS_ID = 61 AND (PICNTC_PICNT_SYS_ID = :pPICNTC_PICNT_SYS_ID OR :pPICNTC_PICNT_SYS_ID = 0)";
            var parms = new List<OracleParameter>() { new OracleParameter("pPICNTC_PICNT_SYS_ID", entity.PICNTC_PICNT_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetInsuranceContractClassById(InsuranceContractClass entity, string authParms)
        {
            var query = $"SELECT * FROM POS_INSURANCE_CONTRACT_CLASS WHERE (PICNTC_SYS_ID = :pPICNTC_SYS_ID OR :pPICNTC_SYS_ID = 0)";
            var parms = new List<OracleParameter>() { new OracleParameter("pPICNTC_SYS_ID", entity.PICNTC_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostInsuranceContractClasses(InsuranceContractClass entity, string authParms)
        {
            if (entity.PICNTC_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
            else entity.STATE = (int)OperationType.Add;
            entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;

            return await OracleDQ.ExcuteXmlProcAsync("PRC_POS_INS_CONTRACT_CLASS_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> DeleteInsuranceContractClasses(InsuranceContractClass entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_POS_INS_CONTRACT_CLASS_XML", new List<dynamic>() { entity }, authParms);
        }
    }
}