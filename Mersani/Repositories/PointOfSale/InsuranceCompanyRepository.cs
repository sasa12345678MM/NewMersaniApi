using Mersani.Interfaces.PointOfSale;
using Mersani.models.PointOfSale;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Repositories.PointOfSale
{
    public class InsuranceCompanyRepository : IInsuranceCompanyRepo
    {

        public async Task<DataSet> GetInsuranceCompanyDataList(InsuranceCompany entity, string authParms)
        {
            var query = $"SELECT * FROM POS_INSURANCE_CMP WHERE (PIC_SYS_ID = :pPIC_SYS_ID OR :pPIC_SYS_ID = 0) ";
            var parms = new List<OracleParameter>() { new OracleParameter("pPIC_SYS_ID", entity.PIC_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> InsertUpdateInsuranceCompany(InsuranceCompany entity, string authParms)
        {
            if (entity.PIC_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
            else entity.STATE = (int)OperationType.Add;
            entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;

            return await OracleDQ.ExcuteXmlProcAsync("PRC_POS_INSURANCE_CMP_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> DeleteInsuranceCompany(InsuranceCompany entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_POS_INSURANCE_CMP_XML", new List<dynamic>() { entity }, authParms);
        }

        public async Task<DataSet> GetLastCode(string authParms)
        {
            var query = $"SELECT  NVL (MAX ( TO_NUMBER ( CASE WHEN REGEXP_LIKE (PIC_CODE, '^[0-9]+') THEN PIC_CODE ELSE '0' END)), 0) + 1 AS Code FROM POS_INSURANCE_CMP ";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
    }
}
