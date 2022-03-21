using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchActivitiesController : GeneralBaseController
    {
        protected readonly IBranchActivitiesRepo _branchActivitiesRepo;
        public BranchActivitiesController(IBranchActivitiesRepo branchActivitiesRepo)
        {
            _branchActivitiesRepo = branchActivitiesRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBranchActivities([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _branchActivitiesRepo.GetBranchActivities(new BranchActivities() { FAC_SYS_ID = id }, authParms));
        }

        [HttpGet("getByBranch/{id}")]
        public async Task<ActionResult> GetActivitiesByBranch([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _branchActivitiesRepo.GetActivityByBranch(new BranchActivities() { FAC_BR_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> BulkBranchActivities([FromBody] List<BranchActivities> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _branchActivitiesRepo.BulkBranchActivities(entities, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBranchActivities([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _branchActivitiesRepo.DeleteBranchActivity(new BranchActivities() { FAC_SYS_ID = id }, authParms));
        }
    }
}
