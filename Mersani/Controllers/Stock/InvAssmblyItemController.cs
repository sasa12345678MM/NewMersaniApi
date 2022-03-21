using Mersani.Interfaces.Stock;
using Mersani.models.Stock;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvAssmblyItemController : GeneralBaseController
    {
        protected readonly InvAssmblyItemRepo _InvAssmblyItemRepo;
        public InvAssmblyItemController(InvAssmblyItemRepo InvAssmblyItemRepo)
        {
            _InvAssmblyItemRepo = InvAssmblyItemRepo;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetInvAssmblyItemHdr([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _InvAssmblyItemRepo.GetInvAssmblyItemHdr(new InvAssmblyItemHdr() { IAIH_SYS_ID = id }, authParms));
        }
        [HttpPost]
        public async Task<ActionResult> AddInvAssmblyItemMasterDetails([FromBody] InvAssmblyItmData entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _InvAssmblyItemRepo.PostInvAssmblyItemMasterDetails(entities, authParms);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvAssmblyItemMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _InvAssmblyItemRepo.DeleteInvAssmblyItemMasterDetails(new InvAssmblyItemHdr() { IAIH_SYS_ID = id }, authParms));
        }
        [HttpGet("Dtl/{id}")]
        public async Task<ActionResult> GetinvInvAssmblyItemDtls([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _InvAssmblyItemRepo.GetinvInvAssmblyItemDtls(new invAssmblyItemHDtl() { IAID_HDR_SYS_ID = id }, authParms));
        }
        [HttpGet("GetLastCode/{type}")]
        public async Task<ActionResult> GetLastCode(string type)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _InvAssmblyItemRepo.GetLastCode(type,authParms));
        }

    }
}
