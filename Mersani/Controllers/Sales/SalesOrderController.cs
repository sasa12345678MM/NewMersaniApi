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
    public class SalesOrderController : GeneralBaseController
    {
        protected readonly ISalesOrderRepo _salesOrderRepo;
        public SalesOrderController(ISalesOrderRepo salesOrderRepo)
        {
            _salesOrderRepo = salesOrderRepo;
        }

        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetSalesOrdersMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderRepo.GetSalesOrderMaster(new SalesOrderMaster() { SOH_SYS_ID = id }, authParms));
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetSalesOrdersDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderRepo.GetSalesOrderDetails(new SalesOrderMaster() { SOH_SYS_ID = id }, authParms));
        }

        [HttpGet("GetOrderDetailsByQuotation/{id}")]
        public async Task<ActionResult> GetOrderDetailsByQuotation([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderRepo.GetOrderDetailsByQuotation(id, authParms));
        }


        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastSalesOrderCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderRepo.GetSalesOrderLastCode(authParms));
        }

        [HttpPost("GetNonApprovedOrders/search")]
        public async Task<ActionResult> GetNonApprovedOrders([FromBody] SalesOrderMaster entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderRepo.GetNonApprovedOrders(entity, authParms));
        }


        [HttpPost]
        public async Task<ActionResult> PostSalesOrder([FromBody] SalesOrder entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderRepo.PostSalesOrderMasterDetails(entity, authParms));
        }

        [HttpPost("posting/bulk")]
        public async Task<ActionResult> PostingSalesOrdersList([FromBody] List<SalesOrderMaster> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderRepo.BulkSalesApprovedOrders(entities, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSalesOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderRepo.DeleteSalesOrderMasterDetails(new SalesOrderDetails() { SOD_SOH_SYS_ID = id }, 1, authParms));
        }

        [HttpDelete("item/{id}")]
        public async Task<ActionResult> DeleteSalesOrderItem([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderRepo.DeleteSalesOrderMasterDetails(new SalesOrderDetails() { SOD_SYS_ID = id }, 2, authParms));
        }
    }
}
