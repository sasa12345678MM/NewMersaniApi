using Mersani.Interfaces.HR;
using Mersani.models.HR;
using Mersani.Repositories.HR;
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
    public class HrJobController : GeneralBaseController
    {
        protected readonly HrJobRepo _HrJobRepo;
        public HrJobController(HrJobRepo HrJobRepo)
        {
            _HrJobRepo = HrJobRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetHrJob([FromRoute]  int id )
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _HrJobRepo.GetHrJobData(id, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostHrJob([FromBody] List< HrJob >hrjob)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _HrJobRepo.PostHrJobData(hrjob, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHrJob([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _HrJobRepo.DeleteHrJobData(new HrJob() { HRJ_SYS_ID = id }, authParms));
        }

    }
}
