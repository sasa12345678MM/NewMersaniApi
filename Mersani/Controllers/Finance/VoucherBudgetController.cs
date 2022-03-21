using Mersani.Interfaces.Finance;
using Mersani.models.Finance;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherBudgetController : GeneralBaseController
    {
        IVoucherBudgetRepo _iVoucherBudgetRepo;

        public VoucherBudgetController(IVoucherBudgetRepo iVoucherBudgetRepo)
        {
            _iVoucherBudgetRepo = iVoucherBudgetRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> getVoucherBudget([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _iVoucherBudgetRepo.GetVoucherBudget(id, authParms));
        }

        [HttpGet("All/{id}")]
        public async Task<ActionResult> getAllVocher([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _iVoucherBudgetRepo.GetALLVoucherBudget(id, authParms));
        }
        [HttpGet("Posted/{id}")]
        public async Task<ActionResult> getPostedVocher([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _iVoucherBudgetRepo.GetPostedVoucherBudget(id, authParms));
        }
        [HttpGet("UnPosted/{id}/{postedType}")]
        public async Task<ActionResult> GetUnPostedVoucherBudget([FromRoute] int id, char postedType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _iVoucherBudgetRepo.GetUnPostedVoucherBudget(id, postedType, authParms));
        }

        [HttpGet("Trans/{id}")]
        public async Task<ActionResult> GetVoucherBudgetDtl([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _iVoucherBudgetRepo.GetVoucherBudgetTrans(id, authParms));
        }
        [HttpPost]
        public async Task<ActionResult> PostAsVoucherBudget([FromBody] VoucherBudget entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _iVoucherBudgetRepo.PostVoucherBudget(entities, authParms);

            return Ok(result);
        }
        [HttpPost("Posted")]
        public async Task<ActionResult> PostedVoucherBudgetHdr([FromBody] List<VoucherBudgetHdr> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _iVoucherBudgetRepo.PostedVoucherBudgetHdr(entities, authParms);

            return Ok(result);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteOwnerSetup([FromBody] VoucherBudget VoucherBudgetSetup)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _iVoucherBudgetRepo.deleteVoucherBudget(VoucherBudgetSetup, authParms);
            return Ok(result);
        }
        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _iVoucherBudgetRepo.GetLastCode(authParms));
        }
    }
}
