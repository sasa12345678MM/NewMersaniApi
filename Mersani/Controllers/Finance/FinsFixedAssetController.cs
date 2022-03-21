using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using Mersani.Interfaces.Finance;
using Mersani.models.Finance;
using System.Threading.Tasks;
using Mersani.Utility.Exceptions;
using System.Collections.Generic;

namespace Mersani.Controllers.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinsFixedAssetController : GeneralBaseController
    {
        protected readonly IFinsFixedAssetRepo _FinsFixedAssetRepo;
        public FinsFixedAssetController(IFinsFixedAssetRepo FinsFixedAssetRepo)
        {
            _FinsFixedAssetRepo = FinsFixedAssetRepo;
        }

        [HttpGet("{id}/{postedType}")]
        public async Task<ActionResult> getFinsFixedAsset([FromRoute] int id,string postedType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _FinsFixedAssetRepo.GetFinsFixedAssetTrans(id, postedType, authParms));
        }
        [HttpGet("Sale/{id}")]
        public async Task<ActionResult> getSaleFinsFixedAsset([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _FinsFixedAssetRepo.GetSaleFinsFixedAssetTrans(id, authParms));
        }
        [HttpGet("UnSale/{id}")]
        public async Task<ActionResult> getUnSaleFinsFixedAsset([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _FinsFixedAssetRepo.GetUnSaleFinsFixedAssetTrans(id, authParms));
        }
        [HttpGet("DefaultAccount")]
        public async Task<ActionResult> GetDefaultFixedAssetAccount()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _FinsFixedAssetRepo.GetDefaultFixedAssetAccount(authParms));
        }
        
        [HttpPost]
        public async Task<ActionResult> AddFinsFixedAsset([FromBody] List<FinsFixedAsset> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _FinsFixedAssetRepo.AddFinsFixedAsset(entities, authParms);

            return Ok(result);
        }
        [HttpPost("Sale")]
        public async Task<ActionResult> SaleFinsFixedAsset([FromBody] List<FinsFixedAsset> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _FinsFixedAssetRepo.SaleFinsFixedAsset(entities, authParms);

            return Ok(result);
        }
        [HttpPost("Post")]
        public async Task<ActionResult> PostFinsFixedAsset([FromBody] List<FinsFixedAsset> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _FinsFixedAssetRepo.PostFinsFixedAsset(entities, authParms);

            return Ok(result);
        }
        //[HttpDelete]
        //public async Task<ActionResult> DeleteOwnerSetup([FromBody] List<FinsFixedAsset> VoucherSetup)
        //{
        //    if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
        //    string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
        //    var result = await _FinsFixedAssetRepo.DeletFinsFixedAsset(VoucherSetup, authParms);
        //    return Ok(result);
        //}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletFinsFixedAsset([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _FinsFixedAssetRepo.DeletFinsFixedAsset(new FinsFixedAsset() { ASSET_SYS_ID = id }, authParms);
            return Ok(result);
        }
        [HttpGet("AssetDep/{id}")]
        public async Task<ActionResult> GetFinsFixedAssetDep([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _FinsFixedAssetRepo.GetFinsFixedAssetDep(id, authParms));
        }
        [HttpGet("excDepretiation")]
        public ActionResult ExecfixedAssetDepretiation()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok( _FinsFixedAssetRepo.ExecfixedAssetDepretiation( authParms));
        }
        [HttpGet("Depretiation/{id}")]
        public async Task<ActionResult> GetfixedAssetDepretiation(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _FinsFixedAssetRepo.GetfixedAssetDepretiation(id, authParms));
        }
        [HttpPost("PostDepretiation")]
        public async Task<ActionResult> PostFinsFixedAssetDepr([FromBody] List<FinsFixedAssetDepr> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _FinsFixedAssetRepo.PostFinsFixedAssetDepr(entities, authParms);

            return Ok(result);
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _FinsFixedAssetRepo.GetLastCode(authParms));
        }

    }
}
