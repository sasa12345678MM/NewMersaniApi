using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Administrator
{

    [Route("api/[controller]")]
    [ApiController]
    public class GeneralReportParmsController : GeneralBaseController
    {
        protected readonly IGeneralReportParmsRepo _generalReportParmsRepo;
        public GeneralReportParmsController(IGeneralReportParmsRepo GeneralReportParmsRepo)
        {
            _generalReportParmsRepo = GeneralReportParmsRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetGeneralReportParms([FromRoute] int reportId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _generalReportParmsRepo.GetGeneralReportParms(new GeneralReportParms() { GRP_SYS_ID=reportId }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteGeneralReportParms([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _generalReportParmsRepo.deleteGeneralReportParms(new GeneralReportParms() { GRP_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> bulkGeneralReportParms([FromBody] List<GeneralReportParms> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _generalReportParmsRepo.bulkGeneralReportParms(entities, authParms));
        }
    }

}

  