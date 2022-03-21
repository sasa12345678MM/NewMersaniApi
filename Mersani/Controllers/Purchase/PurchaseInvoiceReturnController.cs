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
    public class PurchaseInvoiceReturnController : GeneralBaseController
    {
        protected readonly PurchaseInvoicesReturnRepo _InvoicesReturnRepo;

        public PurchaseInvoiceReturnController(PurchaseInvoicesReturnRepo InvoicesReturnRepo)
        {
            _InvoicesReturnRepo = InvoicesReturnRepo;
        }

        [HttpGet("master/{id}/{PostedType}")]
        public async Task<ActionResult> GetInvoicesMaster([FromRoute] int id,string PostedType)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _InvoicesReturnRepo.GetInvoicesReturnHdr(new InvoicesReturnHead() { RIH_SYS_ID = id }, PostedType, authParms));
        }

        [HttpGet("Item/{id}")]
        public async Task<ActionResult> GetInvoicesReturnDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _InvoicesReturnRepo.GetInvoicesReturnItem(new InvoicesReturnItem() { RII_RIH_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastInvoicesReturnCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _InvoicesReturnRepo.GetInvoicesLastCode(authParms));
        }

        [HttpPost("GetNonPostedInvoices/search")]
        public async Task<ActionResult> GetNonPostedInvoicesReturn([FromBody] InvoicesReturnHead entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _InvoicesReturnRepo.GetNonPostedInvoicesReturn(entity, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostInvoicesReturnData([FromBody] InvoiceReturnData entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _InvoicesReturnRepo.SaveInvoicesHdrandItem(entity, authParms));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDeleveryNoteMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _InvoicesReturnRepo.DeleteInvoicesReturn(new InvoicesReturnHead() { RIH_SYS_ID = id }, authParms));
        }
        [HttpPost("posting")]
        public async Task<ActionResult> PostingInvoicesList([FromBody] List<InvoicesReturnHead> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _InvoicesReturnRepo.InvoicesReturnPosting(entities, authParms));
        }
        [HttpPost("accounts/search")]
        public async Task<ActionResult> GetDeafultAccountsForPurchase([FromBody] InvoicesReturnHead entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _InvoicesReturnRepo.GetDefaultAccountsForPurchase(entity, authParms));
        }

    }
}
