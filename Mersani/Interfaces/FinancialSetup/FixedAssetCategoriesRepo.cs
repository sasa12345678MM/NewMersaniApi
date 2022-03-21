using Mersani.models.FinancialSetup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface IFixedAssetCategoriesRepo
    {
        Task<DataSet> GetFixedAssetCategoriesList(FixedAssetCategories FixedAssetCategories, string authParms);
        Task<DataSet> BulkInsertUpdateFixedAssetCategoriesData(List<FixedAssetCategories> FixedAssetCategories, string authParms);
        Task<DataSet> DeleteFixedAssetCategoriesData(FixedAssetCategories FixedAssetCategories, string authParms);



    }
}
