using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacySetupController : ControllerBase
    {
        protected IPharmacySetupRepo _PharmacySetupRepo;
        public PharmacySetupController(IPharmacySetupRepo PharmacySetupRepo)
        {
            _PharmacySetupRepo = PharmacySetupRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetIPharmacySetup(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PharmacySetupRepo.GetPharmacySetup(id, authParms));
        }
        [HttpGet("Owner/{id}")]
        public async Task<ActionResult> GetOwnerPharmacySetup(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PharmacySetupRepo.GetOwnerPharmacySetup(id, authParms));
        }
        [HttpPost]
        public async Task<ActionResult> PostIPharmacySetup([FromBody] List<PharmacySetup> PharmacySetup)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _PharmacySetupRepo.PostPharmacySetup(PharmacySetup, authParms);
            return Ok(result);
        }

        private ModelStateDictionary GetModelStateErrors()
        {
            throw new NotImplementedException();
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> DeleteIPharmacySetup([FromBody] List<PharmacySetup> PharmacySetup)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _PharmacySetupRepo.DeletPharmacySetup(PharmacySetup, authParms);
            return Ok(result);
        }
        [HttpGet("GetLastCode/{id}")]
        public async Task<ActionResult> GetLastCode(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PharmacySetupRepo.GetLastCode(id,authParms));
        }

    }
}