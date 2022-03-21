using Mersani.Interfaces.FinancialSetup;
using Mersani.models.FinancialSetup;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mersani.Controllers.FinancialSetup
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixedAssetTypeController : GeneralBaseController
    {
        protected readonly IFixedAssetTypeRepo _fixedAssetTypeRepo;
        public FixedAssetTypeController(IFixedAssetTypeRepo FixedAssetTypeRepo)
        {
            _fixedAssetTypeRepo = FixedAssetTypeRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetFixedAssetType([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _fixedAssetTypeRepo.GetFixedAssetTypeList(new FixedAssetType() { ASSET_TYPE_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFixedAssetType([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _fixedAssetTypeRepo.DeleteFixedAssetTypeData(new FixedAssetType() { ASSET_TYPE_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostFixedAssetTypeRows([FromBody] List<FixedAssetType> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _fixedAssetTypeRepo.BulkInsertUpdateFixedAssetTypeData(entities, authParms));
        }
    }
}

