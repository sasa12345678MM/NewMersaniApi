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
    public class HrIncrementsTypesController : GeneralBaseController 
    {
        protected readonly HrIncrementsTypesRepo _incrementsTypesRepo;

      public HrIncrementsTypesController (HrIncrementsTypesRepo hrIncrementsTypes)

        {
            _incrementsTypesRepo = hrIncrementsTypes;
        }



        [HttpGet("{id}")]

        public async Task <ActionResult> GetHrIncrementsTypes ([FromRoute] int id )
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _incrementsTypesRepo.GetHrIncrementsTypesData(id, authParms));

        }

        [HttpPost("bulk")]

        public async Task<ActionResult> PostHrIncrementsTypes([FromBody] List<HrIncrementsTypes> hrIncrementsTypesjob)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _incrementsTypesRepo.PostHrIncrementsTypesData(hrIncrementsTypesjob, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHrJob([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _incrementsTypesRepo.DeleteHrIncrementsTypesData(new HrIncrementsTypes() { HRIT_SYS_ID = id }, authParms));
        }

    }
}
