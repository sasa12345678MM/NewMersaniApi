using Mersani.Utility;
using Mersani.models.Stock;
using System.Threading.Tasks;
using Mersani.Interfaces.Stock;
using Microsoft.AspNetCore.Mvc;

namespace Mersani.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : GeneralBaseController
    {
        protected readonly IInventoryRepo _inventoryRepo;
        public InventoryController(IInventoryRepo inventoryRepo)
        {
            _inventoryRepo = inventoryRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetInventory([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _inventoryRepo.GetInventoryData(new Inventory() { IIM_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInventory([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _inventoryRepo.DeleteInventoryData(new Inventory() { IIM_SYS_ID = id }, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostInventory([FromBody] Inventory entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _inventoryRepo.PostInventoryData(entity, authParms));
        }

        [HttpGet("getById/{id}")]
        public async Task<ActionResult> GetInventoryById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _inventoryRepo.GetInventoryDataById(id, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _inventoryRepo.GetLastCode(authParms));
        }
    }
}
