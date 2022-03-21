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
    public class VoucherController : GeneralBaseController
    {
        IVoucherRepo _ivoucherRepo;

        public VoucherController(IVoucherRepo ivoucherRepo)
        {
            _ivoucherRepo = ivoucherRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> getVoucher([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ivoucherRepo.GetVoucher(id,  authParms));
        }
        [HttpGet("VocherIn/{id}")]
        public async Task<ActionResult> getVocherIn([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ivoucherRepo.GetVoucherIn(id,  authParms));
        }
        [HttpGet("All/{id}")]
        public async Task<ActionResult> getAllVocher([FromRoute]  int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ivoucherRepo.GetALLVoucher(id,  authParms));
        }
        [HttpGet("Posted/{id}")]
        public async Task<ActionResult> getPostedVocher([FromRoute]  int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ivoucherRepo.GetPostedVoucher(id,  authParms));
        }
        [HttpGet("UnPosted/{id}/{postedType}")]
        public async Task<ActionResult> GetUnPostedVoucher([FromRoute] int id, char postedType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ivoucherRepo.GetUnPostedVoucher(id, postedType, authParms));
        }
        [HttpGet("VocherOut/{id}")]
        public async Task<ActionResult> getVocherOut([FromRoute]  int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ivoucherRepo.GetVoucherOut(id,  authParms));
        }
        [HttpGet("Trans/{id}")]
        public async Task<ActionResult> GetVoucherDtl([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _ivoucherRepo.GetVoucherTrans(id, authParms));
        }
        [HttpPost]
        public async Task<ActionResult> PostAsVoucher([FromBody] Voucher entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _ivoucherRepo.PostVoucher(entities, authParms);

            return Ok(result);
        }
        [HttpPost("Posted")]
        public async Task<ActionResult> PostedVoucherHdr([FromBody] List<VoucherHdr> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _ivoucherRepo.PostedVoucherHdr(entities, authParms);

            return Ok(result);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteOwnerSetup([FromBody] List<Voucher> VoucherSetup)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _ivoucherRepo.DeletVoucher(VoucherSetup, authParms);
            return Ok(result);
        }
    }
}
