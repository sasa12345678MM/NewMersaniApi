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
    public class PurchaseOrderController : GeneralBaseController
    {
        protected readonly IPurchaseOrderRepo _purchaseOrderRepo; 
        public PurchaseOrderController(IPurchaseOrderRepo purchaseOrderRepo)
        {
            _purchaseOrderRepo = purchaseOrderRepo;
        }

        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetInvoicesMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _purchaseOrderRepo.GetPurchaseOrderMaster(new PurchaseOrderMaster() { IPOH_SYS_ID = id }, authParms));
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetInvoicesDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _purchaseOrderRepo.GetPurchaseOrderDetails(new PurchaseOrderMaster() { IPOH_SYS_ID = id }, authParms));
        }

        [HttpGet("GetDetailsByRequest/{id}")]
        public async Task<ActionResult> GetDetailsByRequest([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _purchaseOrderRepo.GetOrderDetailsByRequest(id, authParms));
        }


        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastInvoiceCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _purchaseOrderRepo.GetPurchaseOrderLastCode(authParms));
        }

        [HttpPost("GetNonApprovedOrders/search")]
        public async Task<ActionResult> GetNonApprovedOrders([FromBody] PurchaseOrderMaster entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _purchaseOrderRepo.GetNonApprovedOrders(entity, authParms));
        }


        [HttpPost]
        public async Task<ActionResult> PostInvoice([FromBody] PurchaseOrder entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _purchaseOrderRepo.PostPurchaseOrderMasterDetails(entity, authParms));
        }

        [HttpPost("posting/bulk")]
        public async Task<ActionResult> PostingInvoicesList([FromBody] List<PurchaseOrderMaster> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _purchaseOrderRepo.BulkPurchaseApprovedOrders(entities, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvoice([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _purchaseOrderRepo.DeletePurchaseOrderMasterDetails(new PurchaseOrderDetails() { IPOD_IPOH_SYS_ID = id }, 1, authParms));
        }

        [HttpDelete("item/{id}")]
        public async Task<ActionResult> DeleteInvoiceItem([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _purchaseOrderRepo.DeletePurchaseOrderMasterDetails(new PurchaseOrderDetails() { IPOD_SYS_ID = id }, 2, authParms));
        }
    }
}
