using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebMobileAdsController : GeneralBaseController
    {
        protected readonly IWebMobileAdsRepo _webmobileRepo;
        public WebMobileAdsController(IWebMobileAdsRepo webmobileRepo)
        {
            _webmobileRepo = webmobileRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetWebMobile([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _webmobileRepo.GetWebMobileAds(id, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostWebMobile([FromBody] WebMobileAds WebMobile)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _webmobileRepo.PostWebMobileAds(WebMobile, authParms));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWebMobile([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _webmobileRepo.DeleteWebMobileAds(new WebMobileAds() { WMA_SYS_ID = id }, authParms));
        }

        [HttpGet("slides")]
        public async Task<ActionResult> GetSliderImages()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            return Ok(await _webmobileRepo.GetSliderImages());
        }
    }
}
