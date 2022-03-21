using Mersani.Interfaces.Stock;
using Mersani.models.Stock;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemsController : GeneralBaseController
    {
        protected readonly IInventoryItemsRepo _inventoryItemsRepo;
        public InventoryItemsController(IInventoryItemsRepo inventoryItemsRepo)
        {
            _inventoryItemsRepo = inventoryItemsRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetInventoryItems([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _inventoryItemsRepo.GetInventoryItems(new InventoryItems() { III_INV_SYS_ID = id }, authParms));
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult> GetInventoryItemById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _inventoryItemsRepo.GetInventoryItemsById(new InventoryItems() { III_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInventoryItem([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _inventoryItemsRepo.DeleteInventoryItems(new InventoryItems() { III_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostInventoryItem([FromBody] List<InventoryItems> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _inventoryItemsRepo.BulkInventoryItems(entities, authParms));
        }
    }
}
