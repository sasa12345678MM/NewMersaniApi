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
    public class HrExpensestypesController : GeneralBaseController
    {
        protected readonly HrExpensestypesRepo _hrExpensestypesRepo;

        public HrExpensestypesController(HrExpensestypesRepo hrExpensestypesRepo)
        {
            _hrExpensestypesRepo = hrExpensestypesRepo;
        }

        [HttpGet("{id}")]

        public async Task<ActionResult> GetHrExpensestypes([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrExpensestypesRepo.GetHrExpensestypesData(id, authParms));

        }


        [HttpPost("bulk")]
        public async Task<ActionResult> PostHrExpensestypes([FromBody] List<HrExpensestypes> hrExpensestypes)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrExpensestypesRepo.PostHrExpensestypesData(hrExpensestypes, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHrExpensestypes([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrExpensestypesRepo.DeleteHrExpensestypesData(new HrExpensestypes() { HRET_SYS_ID = id }, authParms));
        }
    }
}
