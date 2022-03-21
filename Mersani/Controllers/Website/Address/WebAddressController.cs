using Mersani.Interfaces.Administrator;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Website.Address
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebAddressController : GeneralBaseController
    {
        
        protected readonly ICountryRepo _countryRepo;
        public WebAddressController(ICountryRepo countryRepo)
        {
            _countryRepo = countryRepo;
        }

        [HttpGet("GetCountryByName/{name}")]
        public async Task<ActionResult> GetCountryByName([FromRoute] string name)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _countryRepo.GetCountryByName(name, authParms));
        }
     

    }
}
