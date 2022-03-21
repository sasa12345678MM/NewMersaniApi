using Mersani.Utility;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Mersani.models.FinancialSetup;
using Mersani.Interfaces.FinancialSetup;

namespace Mersani.Controllers.FinancialSetup
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerSupplierController : GeneralBaseController
    {
        protected readonly OwnerSupplierRepo _OwnerSupplierRepo;
        public OwnerSupplierController(OwnerSupplierRepo OwnerSupplierRepo)
        {
            _OwnerSupplierRepo = OwnerSupplierRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOwnerSupplier([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _OwnerSupplierRepo.GetOwnerSupplierDataList(new OwnerSupplier() { FOS_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOwnerSupplierClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _OwnerSupplierRepo.DeleteOwnerSupplierData(new OwnerSupplier() { FOS_SYS_ID = id }, authParms));
        }


        [HttpPost]
        public async Task<ActionResult> PostOwnerSupplierClassRows([FromBody] OwnerSupplier entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _OwnerSupplierRepo.BulkInsertUpdateOwnerSupplierData(entity, authParms));
        }
        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _OwnerSupplierRepo.GetLastCode(authParms));
        }
    }
}
