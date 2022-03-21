using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;
using System.Threading.Tasks;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : GeneralBaseController
    {
        protected readonly IRegionRepo _regionRepo;
        public RegionController(IRegionRepo regionRepo)
        {
            _regionRepo = regionRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetRegions([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _regionRepo.GetRegions(id, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostRegion([FromBody] Region region)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _regionRepo.PostRegion(region, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCountry([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _regionRepo.DeleteRegion(new Region() { R_SYS_ID = id }, authParms));
        }


        [HttpGet("GetLastCode/{id}")]
        public async Task<ActionResult> GetLastCode([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _regionRepo.GetLastCode(id, authParms));
        }

    }
}
