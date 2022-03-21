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
    public class HrBonusesController : GeneralBaseController
    {
        protected readonly HrBonusesRepo _hrBonusesRepo;

        public HrBonusesController(HrBonusesRepo hrBonusesRepo)
        {
            _hrBonusesRepo = hrBonusesRepo;
        }

        [HttpGet("{id}")]

        public async Task<ActionResult>GetHrBonuses([FromRoute ]int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrBonusesRepo.GetHrBonusesData(id, authParms));

        }


        [HttpPost("bulk")]
        public async Task<ActionResult> PostHrBonuses([FromBody] List<HrBonuses> hrBonuses)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrBonusesRepo.PostHrBonusesData( hrBonuses, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHrBanks([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrBonusesRepo.DeleteHrBonusesData(new HrBonuses() { HRB_SYS_ID = id }, authParms));
        }

    }
}
