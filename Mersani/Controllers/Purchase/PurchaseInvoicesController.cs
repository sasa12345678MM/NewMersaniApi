using Mersani.Interfaces.Purchase;
using Mersani.models.Purchase;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mersani.Controllers.Purchase
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseInvoicesController : GeneralBaseController
    {
        protected readonly IPurchaseInvoicesRepo _invoicesRepo;

        public PurchaseInvoicesController(IPurchaseInvoicesRepo invoicesRepo)
        {
            _invoicesRepo = invoicesRepo;
        }

        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetInvoicesMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.GetInvoicesMaster(new PurchaseInvoices() { INVH_SYS_ID = id }, authParms));
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetInvoicesDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.GetInvoicesDetails(new PurchaseInvoices() { INVH_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastInvoiceCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.GetInvoicesLastCode(authParms));
        }

        [HttpPost("accounts/search")]
        public async Task<ActionResult> GetDefaultAccountsForPurchase([FromBody] PurchaseInvoices entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.GetDefaultAccountsForPurchase(entity, authParms));
        }

        [HttpPost("invoices/search")]
        public async Task<ActionResult> GetInvoicesMasterDataSearch()
        {

            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            var entity = Request.Form;

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.GetInvoicesMasterDataSearch(entity, authParms));
        }

        [HttpPost("GetNonPostedInvoices/search")]
        public async Task<ActionResult> GetNonPostedInvoices([FromBody] PurchaseInvoices entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.GetNonPostedInvoices(entity, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostInvoice([FromBody] PurchaseInvoicesData entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.PostInvoicesMasterDetails(entity, authParms));
        }

        [HttpPost("posting/bulk")]
        public async Task<ActionResult> PostingInvoicesList([FromBody] List<PurchaseInvoices> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.BulkPurchasePostingInvoices(entities, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvoice([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.DeleteInvoicesMasterDetails(new PurchaseInvoiceItems() { INVI_INVH_SYS_ID = id }, 1, authParms));
        }

        [HttpDelete("item/{id}")]
        public async Task<ActionResult> DeleteInvoiceItem([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.DeleteInvoicesMasterDetails(new PurchaseInvoiceItems() { INVI_SYS_ID = id }, 2, authParms));
        }
    }
}
