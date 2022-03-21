using Mersani.Interfaces.FinancialSetup;
using Mersani.models.FinancialSetup;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Controllers.FinancialSetup
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierClassController : GeneralBaseController
    {
        protected readonly ISupplierClassRepo _supplierClassRepo;
        public SupplierClassController(ISupplierClassRepo supplierClassRepo)
        {
            _supplierClassRepo = supplierClassRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetSupplierClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _supplierClassRepo.GetSupplierClassDataList(new SupplierClass() { FSUC_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSupplierClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _supplierClassRepo.DeleteSupplierData(new SupplierClass() { FSUC_SYS_ID = id }, authParms));
        }


        [HttpPost("bulk")]
        public async Task<ActionResult> PostSupplierClassRows([FromBody] List<SupplierClass> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _supplierClassRepo.BulkInsertUpdateSupplierData(entities, authParms));
        }
    }
}
