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
    public class SalesPaymentController : GeneralBaseController
    {
        protected readonly ISalesPaymentRepo _paymentRepo;

        public SalesPaymentController(ISalesPaymentRepo paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetSalesPaymentMaster(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _paymentRepo.GetSalesPaymentMaster(new S_PaymentMaster() { S_PAY_SYS_ID = id }, authParms));
        }

        [HttpPost("details/{id}")]
        public async Task<ActionResult> GetSalesPaymentDetails([FromBody] S_PaymentMaster entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _paymentRepo.GetSalesPaymentDetails(entity, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastInvoiceCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _paymentRepo.GetPaymentLastCode(authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostInvoice([FromBody] SalesPayment entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _paymentRepo.PostSalesPaymentMasterDetails(entity, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvoice([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _paymentRepo.DeleteSalesPaymentMasterDetails(new S_PaymentDetails() { S_PAY_MST_SYS_ID = id }, 1, authParms));
        }

        [HttpDelete("item/{id}")]
        public async Task<ActionResult> DeleteInvoiceItem([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _paymentRepo.DeleteSalesPaymentMasterDetails(new S_PaymentDetails() { S_PAY_DTLS_SYS_ID = id }, 2, authParms));
        }

        [HttpPost("posting/bulk")]
        public async Task<ActionResult> PostingSalesPaymentsList([FromBody] List<S_PaymentMaster> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _paymentRepo.BulkSalesApprovedPayments(entities, authParms));
        }
    }
}
