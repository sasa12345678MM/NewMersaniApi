using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : GeneralBaseController
    {
        protected readonly IActivityRepo _activityRepository;
        public ActivityController(IActivityRepo activityRepository)
        {
            _activityRepository = activityRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetActivities([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _activityRepository.GetActivityDataList(new Activity() { FAC_CODE = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostNewActivity([FromBody] List<Activity> activities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _activityRepository.BulkInsertUpdateActivity(activities, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteActivity([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _activityRepository.DeleteActivityData(new Activity() { FAC_CODE = id }, authParms));
        }
    }
}
