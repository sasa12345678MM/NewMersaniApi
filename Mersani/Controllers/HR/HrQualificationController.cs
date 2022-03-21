using Mersani.Interfaces.HR;
using Mersani.models.HR;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Mersani.Controllers.HR
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrQualificationController : GeneralBaseController
    {
        protected readonly HrQualificationRepo _HrQualificationRepo;
        public HrQualificationController(HrQualificationRepo hrQualificationRepo)
        {
            _HrQualificationRepo = hrQualificationRepo;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetHrQualification([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _HrQualificationRepo.GetHrQualificationData(id, authParms));
        }


        [HttpPost("bulk")]
        public async Task<ActionResult> PostListHrQualification([FromBody] List<HrQualification> hrqualification)
         {
        if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

        string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

        return Ok(await _HrQualificationRepo.PostHrQualificationListData(hrqualification, authParms));

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHrQualification([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _HrQualificationRepo.DeleteHrQualificationData(new HrQualification() { HRQ_SYS_ID = id }, authParms));
        }

    }
}
