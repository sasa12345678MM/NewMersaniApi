using Mersani.models.FinancialSetup;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface IFixedAssetTypeRepo
    {
        Task<DataSet> GetFixedAssetTypeList(FixedAssetType FixedAssetType, string authParms);
        Task<DataSet> BulkInsertUpdateFixedAssetTypeData(List<FixedAssetType> FixedAssetTaype, string authParms);
        Task<DataSet> DeleteFixedAssetTypeData(FixedAssetType FixedAssetType, string authParms);
    }
}
