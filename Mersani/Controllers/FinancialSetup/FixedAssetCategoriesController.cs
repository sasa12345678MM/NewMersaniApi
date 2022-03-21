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
    public class FixedAssetCategoriesController : GeneralBaseController
    {
        protected readonly IFixedAssetCategoriesRepo _fixedAssetCategoriesRepo;
        public FixedAssetCategoriesController(IFixedAssetCategoriesRepo FixedAssetCategoriesRepo)
        {
            _fixedAssetCategoriesRepo = FixedAssetCategoriesRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetFixedAssetCategories([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _fixedAssetCategoriesRepo.GetFixedAssetCategoriesList(new FixedAssetCategories() { ASSET_CTGRY_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFixedAssetCategories([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _fixedAssetCategoriesRepo.DeleteFixedAssetCategoriesData(new FixedAssetCategories() { ASSET_CTGRY_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostFixedAssetCategoriesRows([FromBody] List<FixedAssetCategories> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _fixedAssetCategoriesRepo.BulkInsertUpdateFixedAssetCategoriesData(entities, authParms));
        }
    }
}





