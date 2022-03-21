using Mersani.Interfaces.FinancialSetup;
using Mersani.Interfaces.Website.CusAuth;
using Mersani.models.FinancialSetup;
using Mersani.models.website;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mersani.Controllers.Website
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebAuthController : GeneralBaseController
    {

        public IWebAuthRepo _authRepo { get; private set; }

        public WebAuthController(IWebAuthRepo authRepo)
        {
            _authRepo = authRepo;
            
        }


        [HttpPost("RegisterCusomer")]
        public async Task<ActionResult> RegisterCusomer([FromBody] Customer entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = "";//not work inside, not important
            return Ok(await _authRepo.Register(entity, authParms));
        }

        [HttpPost("UpdateCusomer")]
        public async Task<ActionResult> UpdateCusomer([FromBody] Customer entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = "";//not work inside, not important
            return Ok(await _authRepo.UpdateCustomer(entity, authParms));
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] WebLoginModel entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            return Ok(await _authRepo.Login(entity));//new { res = res, token = token });
        }

    }

}
