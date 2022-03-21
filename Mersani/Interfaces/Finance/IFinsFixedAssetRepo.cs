using Mersani.models.Finance;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Finance
{
    public interface IFinsFixedAssetRepo
    {
        Task<DataSet> GetFinsFixedAssetTrans(int FinsFixedAsset,string PostType, string authParms);
        Task<DataSet> GetSaleFinsFixedAssetTrans(int FinsFixedAsset, string authParms);
        Task<DataSet> GetUnSaleFinsFixedAssetTrans(int FinsFixedAsset, string authParms);
        Task<DataSet> GetDefaultFixedAssetAccount(string authParms);
        Task<DataSet> AddFinsFixedAsset(List<FinsFixedAsset> entities, string authParms);
        Task<DataSet> SaleFinsFixedAsset(List<FinsFixedAsset> entities, string authParms);
        Task<DataSet> PostFinsFixedAsset(List<FinsFixedAsset> entities, string authParms);
        Task<DataSet> DeletFinsFixedAsset(FinsFixedAsset entities, string authParms);
        Task<DataSet> GetFinsFixedAssetDep(int id, string authParms);
        bool ExecfixedAssetDepretiation(string authParms);
        Task<DataSet> PostFinsFixedAssetDepr(List<FinsFixedAssetDepr> entities, string authParms);
        Task<DataSet> GetfixedAssetDepretiation(int FinsFixedAsset, string authParms);
        
        Task<DataSet> GetLastCode(string authParms);
    }
}
