using Mersani.Interfaces.Sales;
using Mersani.models.Sales;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mersani.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesInvoicesController : GeneralBaseController 
    {
        protected readonly ISalesInvoicesRepo _invoicesRepo;

        public SalesInvoicesController(ISalesInvoicesRepo invoicesRepo)
        {
            _invoicesRepo = invoicesRepo;
        }

        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetInvoicesMaster(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.GetInvoicesMaster(new SalesInvoices() { INVSH_SYS_ID = id }, authParms));
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetInvoicesDetails(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.GetInvoicesDetails(new SalesInvoices() { INVSH_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastInvoiceCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.GetInvoicesLastCode(authParms));
        }

        [HttpPost("GetNonPostedInvoices/search")]
        public async Task<ActionResult> GetNonPostedInvoices([FromBody] SalesInvoices entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.GetNonPostedInvoices(entity, authParms));
        }


        [HttpPost("accounts/search")]
        public async Task<ActionResult> GetDeafultAccountsForSales([FromBody] SalesInvoices entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.GetDefaultAccountsForSales(entity, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostInvoice([FromBody] SalesInvoicesData entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.PostInvoicesMasterDetails(entity, authParms));
        }

        [HttpPost("posting/bulk")]
        public async Task<ActionResult> PostingInvoicesList([FromBody] List<SalesInvoices> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.BulkSalesPostingInvoices(entities, authParms));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvoice([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.DeleteInvoicesMasterDetails(new SalesInvoiceItems() { INVSI_INVSH_SYS_ID = id }, 1, authParms));
        }

        [HttpDelete("item/{id}")]
        public async Task<ActionResult> DeleteInvoiceItem([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _invoicesRepo.DeleteInvoicesMasterDetails(new SalesInvoiceItems() { INVSI_SYS_ID = id }, 2, authParms));
        }
    }
}
