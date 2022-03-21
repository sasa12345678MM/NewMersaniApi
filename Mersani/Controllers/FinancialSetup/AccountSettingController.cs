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
    public class AccountSettingController : GeneralBaseController
    {

        protected readonly IAccountSettingRepo _accountSettingRepo;
        public AccountSettingController(IAccountSettingRepo accountSettingRepo)
        {
            _accountSettingRepo = accountSettingRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomerClass([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _accountSettingRepo.GetAccountSetting(new AccountSetting() { ACC_SET_V_CODE = id }, authParms));
        }

        [HttpGet("view/{id}")]
        public async Task<ActionResult> GetActivityViewData([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _accountSettingRepo.GetActivityViewData(authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomerClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _accountSettingRepo.DeleteAccountSetting(new AccountSetting() { ACC_SET_SYS_ID = id }, authParms));

        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostSupplierClassRows([FromBody] List<AccountSetting> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _accountSettingRepo.BulkInsertUpdateAccountSetting(entities, authParms));
        }

    }
}
