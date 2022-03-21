using Mersani.Interfaces.Stock;
using Mersani.models.Stock;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mersani.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : GeneralBaseController
    {
        protected readonly IItemsRepo _itemsRepo;
        public ItemsController(IItemsRepo itemsRepo)
        {
            _itemsRepo = itemsRepo;
        }

        #region item
        [HttpGet("item/{id}")]
        public async Task<ActionResult> GetItems([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.GetItems(new Items() { ITEM_SYS_ID = id }, authParms));
        }
        [HttpGet("GetLastItemCode")]
        public async Task<ActionResult> GetLastItemCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.GetLastItemCode(authParms));
        }
        [HttpPost]
        public async Task<ActionResult> AddItemMasterDetail([FromBody] Items entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _itemsRepo.PostItemMasterDetails(entities, authParms);

            return Ok(result);
        }

        [HttpPost("search")]
        public async Task<ActionResult> GetItemsWithCriteria([FromBody] Items criteria)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _itemsRepo.GetItemsWithCriteria(criteria, authParms);

            return Ok(result);
        }

        [HttpPost("item")]
        public async Task<ActionResult> AddItemMaster([FromBody] Items entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _itemsRepo.PostItem(new List<Items> { entities }, authParms);

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.DeleteItemMasterDetails(new Items() { ITEM_SYS_ID = id }, authParms));
        }
        #endregion

        #region itemUnit
        [HttpGet("itemUnit/{id}")]
        public async Task<ActionResult> GetItemUnits([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.GetItemUnits(new ItemUnits() { ITU_ITEM_SYS_ID = id }, authParms));
        }
        [HttpPost("itemUnit/bulk")]
        public async Task<ActionResult> AdditemUnit([FromBody] List<ItemUnits> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _itemsRepo.PostItemUnits(entities, authParms);

            return Ok(result);
        }
        [HttpDelete("itemUnit/{id}")]
        public async Task<ActionResult> DeleteItemUnits([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.DeleteItemUnits(new ItemUnits() { ITU_SYS_ID = id }, authParms));
        }
        #endregion

        #region itemAlternative
        [HttpGet("itemAlternative/{id}")]
        public async Task<ActionResult> GetInvItemAlternative([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.GetInvItemAlternative(new invItemAlternative() { IIA_ITEM_MSTR_SYS_ID = id }, authParms));
        }
        [HttpPost("itemAlternative/bulk")]
        public async Task<ActionResult> AdditemAlternative([FromBody] List<invItemAlternative> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _itemsRepo.PostItemAlternative(entities, authParms);

            return Ok(result);
        }
        [HttpDelete("itemAlternative/{id}")]
        public async Task<ActionResult> DeleteInvItemAlternative([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.DeleteItemAlternative(new invItemAlternative() { IIA_ALTRNV_SYS_ID = id }, authParms));
        }
        #endregion

        #region itemRelated
        [HttpGet("itemRelated/{id}")]
        public async Task<ActionResult> GetInvItemRelated([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.GetInvItemRelated(new invItemRelated() { IIR_ITEM_MSTR_SYS_ID = id }, authParms));
        }
        [HttpPost("itemRelated/bulk")]
        public async Task<ActionResult> AdditemRelated([FromBody] List<invItemRelated> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _itemsRepo.PostItemRelated(entities, authParms);

            return Ok(result);
        }
        [HttpDelete("itemRelated/{id}")]
        public async Task<ActionResult> DeleteInvItemRelated([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.DeleteItemRelated(new invItemRelated() { IIR_RLTD_SYS_ID = id }, authParms));
        }
        #endregion

        #region itemBatches
        [HttpGet("itemBatches/{id}")]
        public async Task<ActionResult> GetInvItemBatches([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.GetInvItemBatches(new invitemMasterBatches() { IMB_ITEM_SYS_ID = id }, authParms));
        }

        [HttpPost("itemBatches/bulk")]
        public async Task<ActionResult> PostItemBatches([FromBody] List<invitemMasterBatches> batches)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.PostItemMasterBatches(batches, authParms));
        }

        [HttpDelete("itemBatches/{id}")]
        public async Task<ActionResult> DeleteInvItemBatches([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.DeleteItemMasterBatches(new invitemMasterBatches() { IMB_SYS_ID = id }, authParms));
        }
        #endregion

        #region itemAssempld
        [HttpGet("ItemAssempld/{id}")]
        public async Task<ActionResult> GetInvItemAssempld([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.GetInvItemAssempld(new InvItemAssempldItems() { IIS_ITEM_MSTR_SYS_ID = id }, authParms));
        }
        [HttpPost("ItemAssempld/bulk")]
        public async Task<ActionResult> PostInvItemAssempld([FromBody] List<InvItemAssempldItems> assempldItems)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.PostInvItemAssempld(assempldItems, authParms));
        }
        [HttpDelete("ItemAssempld/{id}")]
        public async Task<ActionResult> DeleteInvItemAssempld([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.DeleteInvItemAssempld(new InvItemAssempldItems() { IIS_SYS_ID = id }, authParms));
        }
        #endregion

        #region itemPrice 
        [HttpGet("ItemMasterPrices/{id}")]
        public async Task<ActionResult> GetInvItemMasterPrices([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.GetInvItemMasterPrices(new InvItemMasterPrices() { ITP_ITEM_SYS_ID = id }, authParms));
        }
        [HttpPost("ItemMasterPrices/bulk")]
        public async Task<ActionResult> PostItemMasterPrices([FromBody] List<InvItemMasterPrices> Prices)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.PostInvItemMasterPrices(Prices, authParms));
        }
        [HttpDelete("ItemMasterPrices/{id}")]
        public async Task<ActionResult> DeleteInvItemMasterPrices([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.DeleteInvItemMasterPrices(new InvItemMasterPrices() { ITP_SYS_ID = id }, authParms));
        }
        #endregion

        #region InvItemImages
        [HttpGet("ItemImages/{id}")]
        public async Task<ActionResult> GetItemImagess([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _itemsRepo.GetInvItemImages(new InvItemImages() { IMG_ITEM_SYS_ID = id }, authParms));
        }
        [HttpPost("ItemImages/bulk")]
        public async Task<ActionResult> AddItemImages([FromBody] List<InvItemImages> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _itemsRepo.PostInvItemImages(entities, authParms);

            return Ok(result);
       
        }
        [HttpDelete("ItemImages/{id}")]
        public async Task<ActionResult> DeleteItemImagess([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _itemsRepo.DeleteInvItemImages(new InvItemImages() { IMG_SYS_ID = id }, authParms));
        }
        #endregion
    }
}
