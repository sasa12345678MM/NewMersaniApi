using Mersani.Interfaces.HR;
using Mersani.models.HR;
using Mersani.models.Hubs;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.HR
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrDepartmentController : GeneralBaseController
    {
        protected readonly HrDepartmentRepo _HrDepartmentRepo;

        public HrDepartmentController(HrDepartmentRepo HrDepartmentRepo, IHubContext<NotificationsHub> hubContext)
        {
            _HrDepartmentRepo = HrDepartmentRepo; 
            //_hubContext = hubContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> getHrDepartment([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _HrDepartmentRepo.GetHrDepartmentData(id, authParms));
        }

 
        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteCustomerPoints([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _HrDepartmentRepo.DeleteHrDepartmentData(new HrDepartment() { HRD_SYS_ID = id }, authParms));
        }


        [HttpPost("bulk")]
        public async Task<ActionResult> postCustomerPoints([FromBody] List<HrDepartment> entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _HrDepartmentRepo.PostHrDepartmentData(entity, authParms));
        }
    
    }
}
