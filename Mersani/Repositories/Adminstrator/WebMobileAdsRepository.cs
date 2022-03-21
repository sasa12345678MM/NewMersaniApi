using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class WebMobileAdsRepository : IWebMobileAdsRepo
    {
        public async Task<DataSet> DeleteWebMobileAds(WebMobileAds WebMobile, string authParms)
        {
            WebMobile.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_WEBMOBILE_XML", new List<dynamic>() { WebMobile }, authParms);
        }

        public async Task<DataSet> GetWebMobileAds(int id, string authParms="")
        {
            var query = $"SELECT * FROM WEB_MOBILE_ADS WHERE WMA_SYS_ID = :pCode OR :pCode = 0";
            var parms = new List<OracleParameter>() { new OracleParameter("pWEB_MOBILE_ADS", id) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text,_public:true);
        }

        public async Task<DataSet> PostWebMobileAds(WebMobileAds WebMobile, string authParms)
        {
            if (WebMobile.WMA_SYS_ID > 0) WebMobile.STATE = (int)OperationType.Update;
            else WebMobile.STATE = (int)OperationType.Add;
            WebMobile.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_WEBMOBILE_XML", new List<dynamic>() { WebMobile }, authParms);
        }

        public async Task<DataSet> GetSliderImages()
        {
            var query = $"SELECT * FROM WEB_MOBILE_ADS WHERE WMA_FRZ_Y_N = 'N' AND SYSDATE >= WMA_START_DATE_TIME AND SYSDATE <= WMA_END_DATE_TIME";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, "", CommandType.Text, _public: true);
        }


    }
}

