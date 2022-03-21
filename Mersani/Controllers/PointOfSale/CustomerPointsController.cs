using Mersani.Interfaces.PointOfSale;
using Mersani.models.PointOfSale;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.PointOfSale
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerPointsController : GeneralBaseController
    {
        protected readonly ICustomerPointsRepo _customerPointsRepo;
        public CustomerPointsController(ICustomerPointsRepo customerPointsRepo)
        {
            _customerPointsRepo = customerPointsRepo; 

        }

        [HttpGet("{id}")]
        public async Task<ActionResult> getCustomerPoints([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerPointsRepo.getCustomerPoints(new CustomerPoints() { FCP_SYS_ID = id }, authParms));
        }

     
        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteCustomerPoints([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerPointsRepo.deleteCustomerPoints(new CustomerPoints() { FCP_SYS_ID = id }, authParms));
        }


        [HttpPost]
        public async Task<ActionResult> postCustomerPoints([FromBody] CustomerPoints entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerPointsRepo.postCustomerPoints(entity, authParms));
        }


        [HttpGet("point/{customer}/{point}")]
        public async Task<ActionResult> getCustomerPoints([FromRoute] int customer,int point)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerPointsRepo.getCustomerReplecPoint(customer, point, authParms));
        }

    }
}
