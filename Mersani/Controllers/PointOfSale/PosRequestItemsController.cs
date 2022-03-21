using Mersani.Interfaces.PointOfSale;
using Mersani.models.PointOfSale;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mersani.Controllers.PointOfSale
{
    [Route("api/[controller]")]
    [ApiController]
    public class PosRequestItemsController : GeneralBaseController
    {
        protected readonly IPosRequestItemsRepo _posRequestItemsRepo;

        public PosRequestItemsController(IPosRequestItemsRepo posRequestItemsRepo)
        {
            _posRequestItemsRepo = posRequestItemsRepo;
        }

        [HttpGet("master/{id}")]
        public async Task<ActionResult> GetPosRequestPosRequestMaster(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _posRequestItemsRepo.GetPosRequestItemsMaster(new PosRequestItemsMaster() { PRIH_SYS_ID = id }, authParms));
        }

        [HttpGet("masterForPending/{id}")]
        public async Task<ActionResult> GetPosRequestItemsMasterForPending(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _posRequestItemsRepo.GetPosRequestItemsMasterForPending(new PosRequestItemsMaster() { PRIH_SYS_ID = id }, authParms));
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult> GetPosRequestItemsDetails(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _posRequestItemsRepo.GetPosRequestItemsDetails(new PosRequestItemsMaster() { PRIH_SYS_ID = id }, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostPosRequestItemsMasterDetails([FromBody] PosRequestItems entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _posRequestItemsRepo.PostPosRequestItemsMasterDetails(entity, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastPosRequestCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _posRequestItemsRepo.GetPosRequestItemsLastCode(authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePosRequest([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _posRequestItemsRepo.DeletePosRequestItemsMasterDetails(new PosRequestItemsDetails() { PRID_PRIH_SYS_ID = id }, 1, authParms));
        }

        [HttpDelete("item/{id}")]
        public async Task<ActionResult> DeletePosRequestItemRepo([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _posRequestItemsRepo.DeletePosRequestItemsMasterDetails(new PosRequestItemsDetails() { PRID_SYS_ID = id }, 2, authParms));
        }

        [HttpGet("PendingForApprove/{id}")]
        public async Task<ActionResult> GetPurchaseRequestPendingForApprove(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _posRequestItemsRepo.GetPosRequestItemsPendingForApproval(new PosRequestItemsMaster() { PRIH_SNDR_PHRM_SYS_ID = id }, authParms));
        }

        [HttpGet("PendingForConfirm/{id}")]
        public async Task<ActionResult> GetPosRequestItemsPendigForConfirm(int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _posRequestItemsRepo.GetPosRequestItemsPendigForConfirm(new PosRequestItemsMaster() { PRIH_RQSTR_PHRM_SYS_ID = id }, authParms));
        }

    }
}
