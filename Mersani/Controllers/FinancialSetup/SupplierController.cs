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
    public class SupplierController : GeneralBaseController
    {
        protected readonly ISupplierRepo _supplierRepo;
        public SupplierController(ISupplierRepo supplierRepo)
        {
            _supplierRepo = supplierRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetSupplier([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _supplierRepo.GetSupplierDataList(new Supplier() { SUPP_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSupplierClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _supplierRepo.DeleteSupplierData(new Supplier() { SUPP_SYS_ID = id }, authParms));
        }


        [HttpPost]
        public async Task<ActionResult> PostSupplierClassRows([FromBody] Supplier entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _supplierRepo.BulkInsertUpdateSupplierData(entity, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _supplierRepo.GetLastCode(authParms));
        }
    }
}
