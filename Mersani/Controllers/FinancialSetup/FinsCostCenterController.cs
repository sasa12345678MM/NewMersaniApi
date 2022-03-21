using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mersani.Interfaces.FinancialSetup;
using Mersani.models.Administrator;
using Mersani.models.FinancialSetup;
using Mersani.Repositories.FinancialSetup;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mersani.Controllers.FinancialSetup
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinsCostCenterController : GeneralBaseController
    {
            IFinsCostCeneterRepository FINS_COST_CENTERRepo;

        public FinsCostCenterController(IFinsCostCeneterRepository _finCostCenterRepository)
            {
            FINS_COST_CENTERRepo = _finCostCenterRepository;
            }
      
        [HttpGet("{id}")]
        public async Task<ActionResult> GetFinisCostCenter([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await FINS_COST_CENTERRepo.GetFinCostCenterDetails(new FinsCostCeneter() { COST_CENTER_CODE = id }, authParms));

        }

        [HttpPost]
        public async Task<ActionResult> PostNewfinsCostCenter([FromBody] List<FinsCostCeneter> FinisCostCenter)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await FINS_COST_CENTERRepo.PostFinCostCenter( FinisCostCenter , authParms));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletefinCostCenterAsync([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await FINS_COST_CENTERRepo.DeleteFinCostCenter(new FinsCostCeneter() { COST_CENTER_CODE = id }, authParms));
        }
    }
}


