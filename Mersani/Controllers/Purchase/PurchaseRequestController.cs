using Mersani.Interfaces.Purchase;
using Mersani.models.Purchase;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mersani.Controllers.Purchase
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseRequestController : GeneralBaseController
    {
        protected readonly IPurchaseRequestRepo _PurchaseRequestRepo;

        public PurchaseRequestController(IPurchaseRequestRepo PurchaseRequestRepo)
        {
            _PurchaseRequestRepo = PurchaseRequestRepo;
        }

        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetPurchaseRequestMaster(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PurchaseRequestRepo.GetPurchaseRequestMaster(new PurchaseRequestMaster() { IPRH_SYS_ID = id }, authParms));
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetPurchaseDetails(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PurchaseRequestRepo.GetPurchaseRequestDetails(new PurchaseRequestMaster() { IPRH_SYS_ID = id }, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostPurchase([FromBody] PurchaseRequest entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PurchaseRequestRepo.PostPurchaseRequestMasterDetails(entity, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastInvoiceCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PurchaseRequestRepo.GetPurchaseRequestLastCode(authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePurchase([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PurchaseRequestRepo.DeletePurchaseRequestMasterDetails(new PurchaseRequestDetails() { IPRD_IPRH_SYS_ID = id }, 1, authParms));
        }

        [HttpDelete("item/{id}")]
        public async Task<ActionResult> DeletePurchaseItem([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PurchaseRequestRepo.DeletePurchaseRequestMasterDetails(new PurchaseRequestDetails() { IPRD_SYS_ID = id }, 2, authParms));
        }

        [HttpGet("PendingForOwner/{id}")]
        public async Task<ActionResult> GetPurchaseRequestPendingForOwner(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PurchaseRequestRepo.GetOwnerApprovedRequestMaster(new PurchaseRequestMaster() { IPRH_SYS_ID = id }, authParms));
        }

        [HttpGet("PendingForCompany/{id}")]
        public async Task<ActionResult> GetPurchaseRequestPendingForCompany(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PurchaseRequestRepo.GetCompanyApprovedRequestMaster(new PurchaseRequestMaster() { IPRH_SYS_ID = id }, authParms));
        }


        [HttpPost("GetRequestsForDashboard/search")]
        public async Task<ActionResult> GetRequestsForDashboard(PurchaseRequestDashboard searchCriteria)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PurchaseRequestRepo.GetRequestsForDashboard(searchCriteria, authParms));
        }

        [HttpPost("GetPurchaseBasicQty/search")]
        public async Task<ActionResult> GetPurchaseBasicQty(PurchaseRequestDetails searchCriteria)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PurchaseRequestRepo.GetPurchaseBasicQty(searchCriteria, authParms));
        }

    }
}
