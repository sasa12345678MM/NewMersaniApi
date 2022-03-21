using System.Data;
using Mersani.Oracle;
using System.Threading.Tasks;
using Mersani.models.FinancialSetup;
using System.Collections.Generic;
using Mersani.Interfaces.FinancialSetup;
using Oracle.ManagedDataAccess.Client;
using System.Linq;

namespace Mersani.Repositories.FinancialSetup
{
    public class SupplierClassRepository : ISupplierClassRepo
    {
        public async Task<DataSet> BulkInsertUpdateSupplierData(List<SupplierClass> entities, string authParms)
        {
            foreach (SupplierClass entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.FSUC_SYS_ID > 0)
                {
                    entity.STATE = (int)OperationType.Update;
                }
                else
                {
                    entity.STATE = (int)OperationType.Add; 
                }
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_SUPP_CLASS_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> GetSupplierClassDataList(SupplierClass supplierClass, string authParms)
        {
            var query = $"SELECT * FROM FINS_SUPP_CLASS WHERE FSUC_SYS_ID = :pFSUC_SYS_ID OR :pFSUC_SYS_ID = 0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pFSUC_SYS_ID", supplierClass.FSUC_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> DeleteSupplierData(SupplierClass supplierClass, string authParms)
        {
            supplierClass.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_SUPP_CLASS_XML", new List<dynamic>() { supplierClass }, authParms);
        }
    }
}
