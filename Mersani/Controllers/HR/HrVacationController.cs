using Mersani.Interfaces.HR;
using Mersani.models.HR;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace Mersani.Controllers.HR
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrVacationController : GeneralBaseController
    {
        protected readonly HrVacationRepo _HrVacationRepo;
        public HrVacationController(HrVacationRepo hrVacationRepo)
        {
            _HrVacationRepo = hrVacationRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetHrVacation([FromRoute] int id)
        {
           if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

           string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

           return Ok(await _HrVacationRepo.GetHrVacationsData(id, authParms));
            
        }

      

        [HttpPost("bulk")]
        public async Task<ActionResult> PostHrVacation([FromBody] List<HrVacations> hrvacation)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _HrVacationRepo.PostHrVacationsData(hrvacation, authParms));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHrVacation([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _HrVacationRepo.DeleteHrVacationsData(new HrVacations() { HRVT_SYS_ID = id }, authParms));
        }
    }
}
