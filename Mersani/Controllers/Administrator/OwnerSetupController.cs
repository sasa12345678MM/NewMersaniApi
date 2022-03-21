using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerSetupController : GeneralBaseController
    {
        protected readonly IOwnerSetupRepo _OwnerSetupRepo;
        public OwnerSetupController(IOwnerSetupRepo ownerSetupRepo)
        {
            _OwnerSetupRepo = ownerSetupRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOwnerSetup(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _OwnerSetupRepo.GetOwnerSetup(id, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostOwnerSetup([FromBody] List<OwnerSetup> ownerSetup)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _OwnerSetupRepo.PostOwnerSetup(ownerSetup, authParms);
            return Ok(result);
        }
   
     

        [HttpPost("Delete")]
        public async Task<ActionResult> DeleteOwnerSetup([FromBody] List<OwnerSetup> ownerSetup)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _OwnerSetupRepo.DeletOwnerSetup(ownerSetup, authParms);
            return Ok(result);
        }
        [HttpGet("mobile/{mobile}")]
        public async Task<ActionResult> getOwnerByMobile([FromRoute] string mobile)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _OwnerSetupRepo.getOwnerByMobile(mobile, authParms));
        }

        [HttpGet("OwnerInsCo/{id}")]
        public async Task<ActionResult> GetGasCInsCompany([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _OwnerSetupRepo.GetGasCInsCompany(new gasOwnerInsCo() { GOIC_OWNER_SYS_ID = id }, authParms));
        }
        [HttpPost("OwnerInsCo")]
        public async Task<ActionResult> PostGasCInsCompany([FromBody] List<gasOwnerInsCo> gasOwnerInsCo)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _OwnerSetupRepo.PostGasCInsCompany(gasOwnerInsCo, authParms);
            return Ok(result);
        }

    }
}
