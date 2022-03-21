using Mersani.Interfaces.PointOfSale;
using Mersani.models.Hubs;
using Mersani.models.PointOfSale;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Mersani.Controllers.PointOfSale
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointOfSaleController : GeneralBaseController
    {
        protected readonly IPointOfSaleRepo _PointOfSaleRepo;
        private IHubContext<NotificationsHub> _hubContext;
        public PointOfSaleController(IPointOfSaleRepo PointOfSaleRepo, IHubContext<NotificationsHub> hubContext)
        {
            _PointOfSaleRepo = PointOfSaleRepo;
            _hubContext = hubContext;
        }

        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetPointOfSaleHDR([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.GetPointOfSaleMaster(new PointOfSaleMASTER() { PCH_SYS_ID = id }, authParms));
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetPointOfSaleDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.GetPointOfSaleDetails(new PointOfSaleDetails() { PCD_PCH_SYS_ID = id }, authParms));
        }


        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetPointOfSaleLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.GetPointOfSaleLastCode(authParms));
        }

        [HttpGet("GetDefaultAccounts")]
        public async Task<ActionResult> GetDefaultAccounts()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.GetDefaultAccounts(authParms));
        }

        [HttpGet("GetDefaultPrinters")]
        public async Task<ActionResult> GetDefaultPrinters()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.GetDefaultPrinters(authParms));
        }

        [HttpGet("GetSalesOrdersNotifactions/{id}/{connId}")]
        public async Task<ActionResult> GetSalesOrdersNotifactions([FromRoute] int id, string connId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var data = await _PointOfSaleRepo.GetSalesOrdersNotifactions(id, authParms);

            var timerManager = new TimerManager(() => _hubContext.Clients.Client(connId).SendAsync("transfersalesordersdata", new DataManager(_PointOfSaleRepo).GetSalesOrersReminder(id, authParms)));
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult> PostPointOfSaleHDRDetails([FromBody] PointOfSaleData entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.PostPointOfSaleMASTERDetails(entity, authParms));
        }

        [HttpPost("posting/bulk")]
        public async Task<ActionResult> BulkPointOfSale([FromBody] List<PointOfSaleMASTER> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.BulkPointOfSale(entities, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePointOfSaleHDRDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _PointOfSaleRepo.DeletePointOfSaleMASTERDetails(new PointOfSaleDetails() { PCD_PCH_SYS_ID = id }, 1, authParms));
        }



        [HttpGet("GetItemByBarcode/{barcode}/{invId}")]
        public async Task<ActionResult> GetItemByBarcode([FromRoute] string barcode, int invId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.GetItemDataByBarCode(barcode, invId, authParms));
        }

        [HttpGet("GetItemByInventory/{invId}")]
        public async Task<ActionResult> GetItemByInventory([FromRoute] int invId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.GetItemsByInventory(invId, authParms));
        }
        [HttpGet("PosShiftsExpenses/{id}")]
        public async Task<ActionResult> getPosShiftsExpenses([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.getPosShiftsExpenses(new IPosShiftsExpenses() { PSE_PSA_SYS_ID = id }, authParms));
        }

        [HttpPost("PosShiftsExpenses")]
        public async Task<ActionResult> SavePosShiftsExpenses([FromBody] List<IPosShiftsExpenses> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.SavePosShiftsExpenses(entities, authParms));
        }

        [HttpGet("GetNearbyPharamciesItem/{id}")]
        public async Task<ActionResult> GetNearbyPharamciesItem([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.GetItemWithinNearbyPharamcies(id, authParms));
        }

        [HttpGet("GetPOSInvPromotions/{id}")]
        public async Task<ActionResult> GetPOSInvPromotions([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.GetPOSInvPromotions(id, authParms));
        }

        [HttpGet("CheckItemDiscount/{item}/{qty}")]
        public async Task<ActionResult> CheckItemDiscount([FromRoute] int item, decimal qty)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.CheckItemDiscount(item, qty, authParms));
        }

        [HttpGet("CheckInvoiceDiscount/{total}")]
        public async Task<ActionResult> CheckInvoiceDiscount([FromRoute] decimal total)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.CheckInvoiceDiscount(total, authParms));
        }


        [HttpGet("CheckOnline")]
        public async Task<ActionResult> CheckLinkConnection()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            return Ok(await _PointOfSaleRepo.CheckLinkConnection());
        }

        [HttpPost("SyncDataToLive")]
        public async Task<ActionResult> SyncDataToLive([FromBody] object dynamic)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _PointOfSaleRepo.SyncDataFromLocalToLive(authParms));
        }

    }
}
