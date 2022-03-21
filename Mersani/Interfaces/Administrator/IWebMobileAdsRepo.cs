using Mersani.models.Administrator;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
    public interface IWebMobileAdsRepo
    {
        Task<DataSet> GetWebMobileAds(int id, string authParms);
        Task<DataSet> PostWebMobileAds(WebMobileAds WebMobile, string authParms);
        Task<DataSet> DeleteWebMobileAds(WebMobileAds WebMobile, string authParms);

        Task<DataSet> GetSliderImages();
    }
}

   
