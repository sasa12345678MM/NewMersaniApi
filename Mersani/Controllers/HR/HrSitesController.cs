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
    public class HrSitesController : GeneralBaseController
    {
        HrSitesRepo _hrSitesRepo;
        public HrSitesController(HrSitesRepo hrSitesRepo)
        {
            _hrSitesRepo = hrSitesRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetNewHrDepartment([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrSitesRepo.GetHrSitesData(id, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostNewHrDepartment([FromBody] List<HrSites> hrsites)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrSitesRepo.PostHrSitesData(hrsites, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLoanType([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _hrSitesRepo.DeleteHrSitesData(new HrSites() { HRS_SYS_ID = id }, authParms));
        }


    }
}
