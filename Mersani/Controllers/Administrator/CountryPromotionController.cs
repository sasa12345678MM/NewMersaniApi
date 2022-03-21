using Mersani.Interfaces.Administrator;
using Mersani.models;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryPromotionController : GeneralBaseController
    {
        protected readonly CountryPromotionRepo _CountryPromotionRepo;
        protected readonly IWebMobileAdsRepo _webmobileRepo;
        public CountryPromotionController(CountryPromotionRepo CountryPromotionRepo, IWebMobileAdsRepo webmobileRepo)
        {
            _CountryPromotionRepo = CountryPromotionRepo;
            _webmobileRepo = webmobileRepo;
        }

        [HttpGet("hdr/{id}/{ParentId}")]
        public async Task<ActionResult> GetCountryPromotionHdr([FromRoute] int id,int ParentId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _CountryPromotionRepo.GetCountryPromotionHdr(new COUNTRY_PROMOTION_HDR() { GCPH_SYS_ID=id ,GCPH_C_SYS_ID = ParentId }, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostCountryPromotion([FromBody] COUNTRY_PROMOTION entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _CountryPromotionRepo.PostCountryPromotion(entity, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCountryPromotionHdr([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _CountryPromotionRepo.DeleteCountryPromotionHdr(new COUNTRY_PROMOTION_HDR() { GCPH_SYS_ID = id }, 1, authParms));
        }

        [HttpGet("Dtl/{id}")]
        public async Task<ActionResult> GetCountryPromotionDtl([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _CountryPromotionRepo.GetCountryPromotionDtl(new COUNTRY_PROMOTION_DTL() { GCPD_GCPH_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastSalesOrderCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _CountryPromotionRepo.GetLastCode(authParms));
        }
        ///////////////////////////////////////


    }
}