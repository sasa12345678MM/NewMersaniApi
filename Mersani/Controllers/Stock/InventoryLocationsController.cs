using Mersani.Utility;
using Mersani.models.Stock;
using System.Threading.Tasks;
using Mersani.Interfaces.Stock;
using Microsoft.AspNetCore.Mvc;

namespace Mersani.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryLocationsController : GeneralBaseController
    {
        protected readonly IInventoryLocationsRepo _inventoryLocationsRepo;
        public InventoryLocationsController(IInventoryLocationsRepo inventoryLocationsRepo)
        {
            _inventoryLocationsRepo = inventoryLocationsRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetInventoryLoactions([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _inventoryLocationsRepo.GetInventoryLocations(new InventoryLocations() { IIL_MST_INV_SYS_ID = id }, authParms));
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult> GetInventoryLoactionsById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _inventoryLocationsRepo.GetInventoryLocationsById(new InventoryLocations() { IIL_LOC_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInventory([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _inventoryLocationsRepo.DeleteInventoryLocations(new InventoryLocations() { IIL_LOC_SYS_ID = id }, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostInventory([FromBody] InventoryLocations entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _inventoryLocationsRepo.PostInventoryLocations(entity, authParms));
        }

        [HttpGet("GetLastCode/{id}")]
        public async Task<ActionResult> GetLastCode( [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _inventoryLocationsRepo.GetLastCode(id, authParms));
        }
    }
}
