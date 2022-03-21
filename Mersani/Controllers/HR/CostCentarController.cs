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
    public class CostCentarController : GeneralBaseController
    {
        protected readonly CostCenterRepo _costCenterRepo;

        public CostCentarController(CostCenterRepo costCenterRepo)
        {
            _costCenterRepo = costCenterRepo;
        }

        [HttpGet("{id}")]

        public async Task<ActionResult> GetHrCostCenter([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _costCenterRepo.GetHrCostCenterData(id, authParms));

        }


        [HttpPost("bulk")]
        public async Task<ActionResult> PostHrCostCenter([FromBody] List<CostCentar> costCentar)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _costCenterRepo.PostHrCostCenterData(costCentar, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHrCostCenter([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _costCenterRepo.DeleteHrCostCenterData(new CostCentar() { HRCC_SYS_ID = id }, authParms));
        }

    }
}
