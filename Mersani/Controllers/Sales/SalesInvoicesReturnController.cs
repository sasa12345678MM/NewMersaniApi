using Mersani.Interfaces.Sales;
using Mersani.models.Sales;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesInvoicesReturnController : GeneralBaseController
    {
        protected readonly SalesInvoicesReturnRepo _SalesInvoicesReturnRepo; 

        public SalesInvoicesReturnController(SalesInvoicesReturnRepo SalesInvoicesReturnRepo)
        {
            _SalesInvoicesReturnRepo = SalesInvoicesReturnRepo;
        }

        [HttpGet("master/{id}/{PostedType}")]
        public async Task<ActionResult> GetInvoicesMaster([FromRoute] int id, string PostedType)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesInvoicesReturnRepo.GetSalesInvoicesReturnHdr(new SalesInvoicesReturnHead() { RIH_SYS_ID = id }, PostedType, authParms));
        }

        [HttpGet("Item/{id}")]
        public async Task<ActionResult> GetSalesInvoicesReturnDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesInvoicesReturnRepo.GetSalesInvoicesReturnItem(new SalesInvoicesReturnItem() { RII_RIH_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastSalesInvoicesReturnCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesInvoicesReturnRepo.GetInvoicesLastCode(authParms));
        }

        [HttpPost("GetNonPostedInvoices/search")]
        public async Task<ActionResult> GetNonPostedSalesInvoicesReturn([FromBody] SalesInvoicesReturnHead entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesInvoicesReturnRepo.GetNonPostedSalesInvoicesReturn(entity, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostSalesInvoicesReturnData([FromBody] SalesInvoiceReturnData entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesInvoicesReturnRepo.SaveInvoicesHdrandItem(entity, authParms));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDeleveryNoteMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _SalesInvoicesReturnRepo.DeleteSalesInvoicesReturn(new SalesInvoicesReturnHead() { RIH_SYS_ID = id }, authParms));
        }
        [HttpPost("posting")]
        public async Task<ActionResult> PostingInvoicesList([FromBody] List<SalesInvoicesReturnHead> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesInvoicesReturnRepo.SalesInvoicesReturnPosting(entities, authParms));
        }
        [HttpPost("accounts/search")]
        public async Task<ActionResult> GetDeafultAccountsForPurchase([FromBody] SalesInvoicesReturnHead entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _SalesInvoicesReturnRepo.GetDefaultAccountsForPurchase(entity, authParms));
        }

    }
}
