using Mersani.Interfaces.HR;
using Mersani.models.HR;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mersani.Controllers.HR
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeductionTypeController : GeneralBaseController
    {
        protected readonly DeductionTypeRepo _deductionTypeRepo;
        public DeductionTypeController(DeductionTypeRepo deductionTypeRepo)
        {
            _deductionTypeRepo = deductionTypeRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetHrDeductionType([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _deductionTypeRepo.GetHrDeductionTypeData(id, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostHrDeductionType([FromBody] List<DeductionType> deductionType)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _deductionTypeRepo.PostHrDeductionTypeData(deductionType, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHrDeductionType([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _deductionTypeRepo.DeleteHrDeductionTypeData(new DeductionType() { HRD_SYS_ID = id }, authParms));
        }


    }
}
