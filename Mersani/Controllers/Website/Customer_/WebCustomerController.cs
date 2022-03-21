using Mersani.Interfaces.Website.Customer_;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Website.Customer_
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebCustomerController : GeneralBaseController
    {

        protected readonly IWebCustomer _webcustomer;
      
        public WebCustomerController(IWebCustomer webcustomer)
        {
            _webcustomer = webcustomer;
        }

        [HttpGet("{customerid}")]
        public async Task<ActionResult> GetCustomerDetailedAdresses([FromRoute] int customerid)
        {

            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _webcustomer.GetCustomerDetailedAdresses(customerid, authParms));
        }

    }
}
