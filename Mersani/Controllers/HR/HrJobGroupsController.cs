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
    public class HrJobGroupsController : GeneralBaseController
    {
        protected readonly HrJobGroupsRepo _hrJobGroupsRepo;
        public HrJobGroupsController(HrJobGroupsRepo hrJobGroupsRepo)
        {
            _hrJobGroupsRepo = hrJobGroupsRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetHrJobGroups([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrJobGroupsRepo.GetHrJobGroupsData(id, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostHrJobGroups([FromBody] List<HrJobGroups> JobGroups)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrJobGroupsRepo.PostHrJobGroupsData(JobGroups, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHrJobGroups([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrJobGroupsRepo.DeleteHrJobGroupsData(new HrJobGroups() { HRJG_SYS_ID = id }, authParms));
        }

    }
}
