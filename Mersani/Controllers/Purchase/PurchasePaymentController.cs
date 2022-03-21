using Mersani.Interfaces.Purchase;
using Mersani.models.Purchase;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mersani.Controllers.Purchase
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasePaymentController : GeneralBaseController
    {
        protected readonly IPurchasePaymentRepo _paymentRepo;

        public PurchasePaymentController(IPurchasePaymentRepo paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetPurchasePaymentMaster(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _paymentRepo.GetPurchasePaymentMaster(new P_PaymentMaster() { P_PAY_SYS_ID = id }, authParms));
        }

        [HttpPost("details/{id}")]
        public async Task<ActionResult> GetPurchasePaymentDetails([FromBody] P_PaymentMaster entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _paymentRepo.GetPurchasePaymentDetails(entity, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastPaymentCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _paymentRepo.GetPaymentLastCode(authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostInvoice([FromBody] PurchasePayment entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _paymentRepo.PostPurchasePaymentMasterDetails(entity, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvoice([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _paymentRepo.DeletePurchasePaymentMasterDetails(new P_PaymentDetails() { P_PAY_MST_SYS_ID = id }, 1, authParms));
        }

        [HttpDelete("item/{id}")]
        public async Task<ActionResult> DeleteInvoiceItem([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _paymentRepo.DeletePurchasePaymentMasterDetails(new P_PaymentDetails() { P_PAY_DTLS_SYS_ID = id }, 2, authParms));
        }
    }
}
