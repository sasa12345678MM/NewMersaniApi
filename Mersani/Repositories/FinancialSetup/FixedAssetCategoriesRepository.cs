 using Mersani.Interfaces.FinancialSetup;
using Mersani.models.FinancialSetup;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.FinancialSetup
{
    public class FixedAssetCategoriesRepository : IFixedAssetCategoriesRepo
    {
        public async Task<DataSet> BulkInsertUpdateFixedAssetCategoriesData(List<FixedAssetCategories> entities, string authParms)
        {
            foreach (FixedAssetCategories entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.ASSET_CTGRY_SYS_ID > 0)
                {
                    entity.STATE = (int)OperationType.Update;
                }
                else
                {
                    entity.STATE = (int)OperationType.Add;
                }
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_FIXED_ASSET_CATEG_XML", entities.ToList<dynamic>(), authParms);
        }

        public async Task<DataSet> GetFixedAssetCategoriesList (FixedAssetCategories FixedAssetCategories, string authParms)
        {
            var query = $"SELECT * FROM FINS_FIXED_ASSET_CATEGORIES WHERE ASSET_CTGRY_SYS_ID = :pASSET_CTGRY_SYS_ID OR :pASSET_CTGRY_SYS_ID = 0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pASSET_CTGRY_SYS_ID", FixedAssetCategories.ASSET_CTGRY_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }


        public async Task<DataSet> DeleteFixedAssetCategoriesData(FixedAssetCategories FixedAssetCategories, string authParms)
        {
            FixedAssetCategories.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_FIXED_ASSET_CATEG_XML", new List<dynamic>() { FixedAssetCategories }, authParms);
        }
    }
}
