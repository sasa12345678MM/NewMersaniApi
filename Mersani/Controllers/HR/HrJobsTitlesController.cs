using Mersani.Interfaces.HR;
using Mersani.models.HR;
using Mersani.models.Hubs;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.HR
{
    [Route("api/[controller]")]
    [ApiController]

    public class HrJobsTitlesController : GeneralBaseController
    {
       protected  readonly HrJobTitlesRepo _hrJobTitlesRepo;
        public HrJobsTitlesController(HrJobTitlesRepo HrJobTitlesRepo)
        {
            _hrJobTitlesRepo = HrJobTitlesRepo;
           
        }

        [HttpGet("{id}")]

        public async Task<ActionResult> GetHrJobTitles([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrJobTitlesRepo.GetHrJobTitlesData(id, authParms));

        }


        [HttpPost("bulk")]
        public async Task<ActionResult> PostHrJobTitles([FromBody] List<HrJobTitles> hrJobTitles)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrJobTitlesRepo.PostHrJobTitlesData(hrJobTitles, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHrJobTitles([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrJobTitlesRepo.DeleteHrJobTitlesData(new HrJobTitles() { HRJT_SYS_ID = id }, authParms));
        }
    }

}
