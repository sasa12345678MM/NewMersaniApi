using Mersani.Interfaces.CallCenter;
using Mersani.Interfaces.Website.WebShoping;
using Mersani.models.CostCenter;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.CallCenter
{
    [Route("api/[controller]")]
    [ApiController]
    public class TktSalesOrderController : GeneralBaseController
    {
        protected readonly TktSalesOrderRepo _TktSalesOrderRepo;
        IWebShopingRepo _shoppingRepo;
        public TktSalesOrderController(TktSalesOrderRepo TktSalesOrderRepo, IWebShopingRepo shoppingRepo)
        {
            _TktSalesOrderRepo = TktSalesOrderRepo;
            _shoppingRepo = shoppingRepo;
        }

        [HttpGet("hdr/{id}")]
        public async Task<ActionResult> GetSalesOrdersMaster([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _TktSalesOrderRepo.GetTktSalesOrderHdr(new TktSalesOrderHdr() { TSOH_SYS_ID = id }, authParms));
        }
        [HttpGet("GetTktSalesOrderByCustomerId/{cusrtomerid}")]
        public async Task<ActionResult> GetTktSalesOrderByCustomerId([FromRoute] int cusrtomerid)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _TktSalesOrderRepo.GetTktSalesOrderByCustomerId(cusrtomerid, authParms));
        }
        [HttpPost]
        public async Task<ActionResult> PostSalesOrder([FromBody] TktSalesOrder entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            if (entity.TKTSALESORDERHDR.TSOH_PAYMENT_Y_N=='Y')
            {
                entity.paymentDetails.TSOP_CAPTURED = "?";
            var res=   await _shoppingRepo.AddPaymentDetails(entity.paymentDetails, authParms);
                if (res.Tables.Count>0)
                {

                }
            }
            return Ok(await _TktSalesOrderRepo.PostTktSalesOrderHdrDtl(entity, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSalesOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _TktSalesOrderRepo.DeleteTktSalesOrderHdr(new TktSalesOrderHdr() { TSOH_SYS_ID = id }, 1, authParms));
        }

        [HttpGet("Dtl/{id}")]
        public async Task<ActionResult> GetSalesOrdersDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _TktSalesOrderRepo.GetSalesOrderDtl(new TktSalesOrderDtl() { TSOD_TSOH_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastSalesOrderCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _TktSalesOrderRepo.GetTktSalesOrderLastCode(authParms));
        }
          
           ////////////////////////////////////////
    
        [HttpGet("Detail/{id}/{ParentId}")]
        public async Task<ActionResult> GetTicketDetail([FromRoute] int id, int ParentId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _TktSalesOrderRepo.GetSalesOrderDetail(new TktSalesOrderDetail() { TSOL_SYS_ID = id, TSOL_SOH_SYS_ID = ParentId }, authParms));
        }
        [HttpPost("Detail")]
        public async Task<ActionResult> SaveTicketDetail([FromBody] List<TktSalesOrderDetail> entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            
            
            return Ok(await _TktSalesOrderRepo.SaveSalesOrderDetail(entity, authParms));
        }
        [HttpGet("UnSold/{id}/{Type}")]
        public async Task<ActionResult> GetTktUnSoldOrder([FromRoute] int id, string Type)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _TktSalesOrderRepo.GetTktUnSoldOrder(id, Type, authParms));
        }
        [HttpPost("Status")]
        public async Task<ActionResult> SaveTicketDetail([FromBody] TktSalesOrderHdr entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _TktSalesOrderRepo.SaveSalesOrderStatus(new List<TktSalesOrderHdr>{ entity}, authParms));
        }
        ////////////////////////////////////////////////////////////
        [HttpGet("Log/{id}")]
        public async Task<ActionResult> GetTicketMasterLogData([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _TktSalesOrderRepo.GetTktSalesOrderHdrLog(new TktSalesOrderHdrLog() { TSOHL_TSOH_SYS_ID = id }, authParms));
        }
        [HttpPost("CheckItemInStock/search")]
        public async Task<ActionResult> CheckItemInStock([FromBody] CheckInStockObj entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _TktSalesOrderRepo.CheckItemInStock(entity, authParms));
        }

    }
}