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
    public class FixedAssetTypeRepository : IFixedAssetTypeRepo
    {
        public async Task<DataSet> BulkInsertUpdateFixedAssetTypeData(List<FixedAssetType> entities, string authParms)
        {
            foreach (FixedAssetType entity in entities)
            {
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                if (entity.ASSET_TYPE_SYS_ID > 0)
                {
                    entity.STATE = (int)OperationType.Update;
                }
                else
                {
                    entity.STATE = (int)OperationType.Add;
                }
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_FIXED_ASSET_TYPES_XML", entities.ToList<dynamic>(), authParms);

        }

        public async Task<DataSet> GetFixedAssetTypeList(FixedAssetType FixedAssetType, string authParms)
        {
            {
                var query = $"SELECT * FROM FINS_FIXED_ASSET_TYPES WHERE ASSET_TYPE_SYS_ID = :pASSET_TYPE_SYS_ID OR :pASSET_TYPE_SYS_ID = 0";
                var parms = new List<OracleParameter>() {
                new OracleParameter("pASSET_TYPE_SYS_ID", FixedAssetType.ASSET_TYPE_SYS_ID)
            };
                return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
            }
        }


        public async Task<DataSet> DeleteFixedAssetTypeData(FixedAssetType FixedAssetType, string authParms)
        {
            FixedAssetType.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_FINS_FIXED_ASSET_TYPES_XML", new List<dynamic>() { FixedAssetType }, authParms);
        }


    }
}

