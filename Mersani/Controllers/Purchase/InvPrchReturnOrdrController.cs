using Mersani.Interfaces.Purchase;
using Mersani.models.Purchase;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Purchase
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvPrchReturnOrdrController : GeneralBaseController
    {
        protected readonly InvPrchReturnOrdrRepo _InvPrchReturnOrdrRepo;

        public InvPrchReturnOrdrController(InvPrchReturnOrdrRepo InvPrchReturnOrdrRepo)
        {
            _InvPrchReturnOrdrRepo = InvPrchReturnOrdrRepo;
        }

        [HttpGet("master/{id}/{PostedType}")]
        public async Task<ActionResult> GetInvoicesMaster([FromRoute] int id, string PostedType)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _InvPrchReturnOrdrRepo.GetInvPrchReturnHdr(new InvPrchReturnOrdrHdr() { IPROH_SYS_ID = id }, PostedType, authParms));
        }

        [HttpGet("Dtl/{id}")]
        public async Task<ActionResult> GetInvoicesReturnDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _InvPrchReturnOrdrRepo.GetInvPrchReturnOrdrDtl(new InvPrchReturnOrdrDtl() { IPROD_IPROH_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastInvoicesReturnCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _InvPrchReturnOrdrRepo.GetInvPrchLastCode(authParms));
        }


        [HttpPost]
        public async Task<ActionResult> PostInvoicesReturnData([FromBody] PurchaseReturnOrderData entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _InvPrchReturnOrdrRepo.SaveInvPrchHdrandItem(entity, authParms));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDeleveryNoteMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _InvPrchReturnOrdrRepo.DeleteInvPrchReturn(new InvPrchReturnOrdrHdr() { IPROH_SYS_ID = id }, authParms));
        }
        [HttpPost("posting")]
        public async Task<ActionResult> PostingInvoicesList([FromBody] List<InvPrchReturnOrdrHdr> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _InvPrchReturnOrdrRepo.InvPrchReturnPosting(entities, authParms));
        }
        [HttpPost("accounts/search")]
        public async Task<ActionResult> GetDeafultAccountsForPurchase([FromBody] InvPrchReturnOrdrHdr entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _InvPrchReturnOrdrRepo.GetDefaultAccountsForPurchase(entity, authParms));
        }

    }
}
