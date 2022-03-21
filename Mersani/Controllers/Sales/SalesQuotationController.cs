using Mersani.Interfaces.Sales;
using Mersani.models.Sales;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesQuotationController : GeneralBaseController
    {
        protected readonly ISalesQuotationRepo _salesQuotationRepo;

            public SalesQuotationController(ISalesQuotationRepo salesQuotationRepo)
        {
            _salesQuotationRepo = salesQuotationRepo;
        }

        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetInvoicesMaster([FromRoute] int id, string PostedType)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesQuotationRepo.GetSalesquotationHdr(new IsalesquotationMaster() { SQH_SYS_ID = id }, PostedType, authParms));
        }

        [HttpGet("Detail/{id}")]
        public async Task<ActionResult> GetSalesInvoicesReturnDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesQuotationRepo.GetSalesquotationDetails(new IsalesquotationDetails() { SQD_SQH_SYS_ID = id }, authParms));
        }
        [HttpGet("Terms/{id}")]
        public async Task<ActionResult> GetSalesInvoicesReturnTerms([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesQuotationRepo.GetSalesquotationTerms(new IsalesquotationTerms() { SQT_SQH_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastSalesInvoicesReturnCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesQuotationRepo.GetSalesquotationLastCode(authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostSalesInvoicesReturnData([FromBody] ISalesQuotation entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesQuotationRepo.SaveInvoicesHdrandDetails(entity, authParms));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDeleveryNoteMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _salesQuotationRepo.DeleteSalesquotation(new IsalesquotationMaster() { SQH_SYS_ID = id }, authParms));
        }
        [HttpPost("posting")]
        public async Task<ActionResult> PostingInvoicesList([FromBody] List<IsalesquotationMaster> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesQuotationRepo.SalesquotationPosting(entities, authParms));
        }
       

    }
}
