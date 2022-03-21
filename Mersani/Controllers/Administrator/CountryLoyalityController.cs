using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryLoyalityController : GeneralBaseController
    {
        protected readonly CountryLoyalityRepo _CountryLoyalityRepo;
        public CountryLoyalityController(CountryLoyalityRepo CountryLoyalityRepo)
        {
            _CountryLoyalityRepo = CountryLoyalityRepo;
        }
        [HttpGet("{id}/{ParentId}")]
        public async Task<ActionResult> getFinsCustomerRelatives([FromRoute] int id, int ParentId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _CountryLoyalityRepo.geCountryLoyality(new CountryLoyalitySetup() { GCLS_SYS_ID = id,GCLS_C_SYS_ID= ParentId }, ParentId, authParms));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFinsCustomerRelatives([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _CountryLoyalityRepo.DeleteCountryLoyality(new CountryLoyalitySetup() { GCLS_SYS_ID = id }, authParms));
        }
        [HttpPost]
        public async Task<ActionResult> PostFinsCustomerRelatives([FromBody] CountryLoyalitySetup entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _CountryLoyalityRepo.PostCountryLoyality(entity, authParms));
        }
    }
}
