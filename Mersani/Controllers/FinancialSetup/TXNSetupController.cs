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
    public class TXNSetupController : GeneralBaseController
    {
        protected readonly ITNXSetupRepo _voucherTypeRepo;
        public TXNSetupController(ITNXSetupRepo voucherTypeRepo)
        {
            _voucherTypeRepo = voucherTypeRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetVoucherType([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _voucherTypeRepo.GetTXNSetupDataList(new TXNSetup() { FTS_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostNewVoucherType([FromBody] List<TXNSetup> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _voucherTypeRepo.BulkInsertUpdateTXNSetup(entities, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVoucherType([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _voucherTypeRepo.DeleteTXNSetupData(new TXNSetup() { FTS_SYS_ID = id }, authParms));
        }
    }
}
