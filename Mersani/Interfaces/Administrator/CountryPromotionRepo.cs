using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mersani.models;

namespace Mersani.Interfaces.Administrator
{
    public interface CountryPromotionRepo
    {
        Task<DataSet> GetCountryPromotionHdr(COUNTRY_PROMOTION_HDR entity, string authParms);
        Task<DataSet> GetCountryPromotionDtl(COUNTRY_PROMOTION_DTL entity, string authParms);
        Task<DataSet> PostCountryPromotion(COUNTRY_PROMOTION entities, string authParms);
        Task<DataSet> DeleteCountryPromotionHdr(COUNTRY_PROMOTION_HDR entity, int v, string authParms);
        Task<DataSet> GetLastCode(string authParms);

    }
}
