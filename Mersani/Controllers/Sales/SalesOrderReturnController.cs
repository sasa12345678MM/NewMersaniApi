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
    public class SalesOrderReturnController : GeneralBaseController
    {
        protected readonly ISalesOrderReturnRepo _salesOrderReturnRepo; 
        public SalesOrderReturnController(ISalesOrderReturnRepo salesOrderReturnRepo)
        {
            _salesOrderReturnRepo = salesOrderReturnRepo;
        }

        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetSalesOrderReturnMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderReturnRepo.GetSalesOrderReturnMaster(new SalesOrderReturnMaster() { SROH_SYS_ID = id }, authParms));
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetSalesOrderReturnDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderReturnRepo.GetSalesOrderReturnDetails(new SalesOrderReturnMaster() { SROH_SYS_ID = id }, authParms));
        }

        [HttpGet("GetDetailsBySalesOrderCode/{id}")]
        public async Task<ActionResult> GetDetailsBySalesOrderCode([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderReturnRepo.GetSalesOrderDetailsByCode(id, authParms));
        }


        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastSalesOrderReturnCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderReturnRepo.GetSalesOrderReturnLastCode(authParms));
        }

        [HttpPost("GetNonApprovedOrders/search")]
        public async Task<ActionResult> GetNonApprovedOrders([FromBody] SalesOrderReturnMaster entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderReturnRepo.GetNonApprovedOrders(entity, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostSalesOrderReturn([FromBody] SalesOrderReturn entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderReturnRepo.PostSalesOrderReturnMasterDetails(entity, authParms));
        }

        [HttpPost("posting/bulk")]
        public async Task<ActionResult> PostingSalesOrderReturnList([FromBody] List<SalesOrderReturnMaster> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderReturnRepo.BulkSalesApprovedOrders(entities, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSalesOrderReturn([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderReturnRepo.DeleteSalesOrderReturnMasterDetails(new SalesOrderReturnDetails() { SROD_SROH_SYS_ID = id }, 1, authParms));
        }

        [HttpDelete("item/{id}")]
        public async Task<ActionResult> DeleteSalesOrderReturnItem([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _salesOrderReturnRepo.DeleteSalesOrderReturnMasterDetails(new SalesOrderReturnDetails() { SROD_SYS_ID = id }, 2, authParms));
        }
    }
}
