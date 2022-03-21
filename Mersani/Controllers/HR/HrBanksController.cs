using Mersani.Interfaces.HR;
using Mersani.models.HR;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.HR
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrBanksController : GeneralBaseController
    {
        protected readonly HrBanksRepo _HrBanksRepo;
        public HrBanksController(HrBanksRepo hrBanksRepo)
        {
            _HrBanksRepo = hrBanksRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetHrBanks([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _HrBanksRepo.GetHrBanksData(id, authParms));
        }

        
        [HttpPost("bulk")]
        public async Task<ActionResult> PostHrBanks([FromBody] List<HrBanks> hrbanks)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _HrBanksRepo.PostHrBanksData(hrbanks, authParms));
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHrBanks([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _HrBanksRepo.DeleteHrBanksData(new HrBanks() { HREB_SYS_ID = id }, authParms));
        }

    }
}
