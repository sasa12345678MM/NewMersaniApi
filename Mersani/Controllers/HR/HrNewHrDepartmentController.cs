using Mersani.Interfaces.HR;
using Mersani.models.HR;
using Mersani.Repositories.HR;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrNewHrDepartmentController : GeneralBaseController
    {
        HrNewHrDepartmentRepo _HrNewHrDepartmentRepo;

        public HrNewHrDepartmentController(HrNewHrDepartmentRepo HrNewHrDepartmentRepo)
        {
            _HrNewHrDepartmentRepo = HrNewHrDepartmentRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetNewHrDepartment([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _HrNewHrDepartmentRepo.GetHrNewHrDepartmentData(id, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostNewHrDepartment([FromBody] List<HrNewHrDepartment> hrNewHrDepartment)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _HrNewHrDepartmentRepo.PostHrNewHrDepartmentData(hrNewHrDepartment, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLoanType([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _HrNewHrDepartmentRepo.DeleteNewHrDepartmentData(new HrNewHrDepartment() { HRD_SYS_ID = id }, authParms));
        }
    }
}
