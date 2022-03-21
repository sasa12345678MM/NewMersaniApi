using System;
using System.Data;
using System.Threading.Tasks;
using Mersani.Interfaces.FinancialSetup;
using Mersani.models.FinancialSetup;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Mersani.Controllers.FinancialSetup
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinsAccountController : GeneralBaseController
    {
        protected readonly IFinisAccountRepository _finAccountRepository;

        public FinsAccountController(IFinisAccountRepository finAccountRepository)
        {
            _finAccountRepository = finAccountRepository;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetFinancialAccount([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = new DataSet();

            var texts = id.Split('-');
            string code;
            if (texts.Length == 1) code = texts[0];
            else code = texts[1];
            if (Int64.Parse(code) > 0) result = await _finAccountRepository.GetFinsAccountChildern(new FinsAccount() { ACC_NO = id }, authParms);
            else result = await _finAccountRepository.GetFinsAccount(new FinsAccount(),authParms);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostNewfinAccount([FromBody] FinsAccount finAccount)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _finAccountRepository.PostFinsAccount(finAccount, authParms));
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomerClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _finAccountRepository.DeletFinsAccount(new FinsAccount() { ACC_CODE = id }, authParms));
        }

        [HttpPost("GetAccNo2/search")]
        public async Task<ActionResult> GetAccountNo2([FromBody] FinsAccount account)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _finAccountRepository.GetAccountNoTwoByParent(account, authParms));
        }

        [HttpPost("GetAccNo3/search")]
        public async Task<ActionResult> GetAccountNo3([FromBody] FinsAccount account)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _finAccountRepository.GetAccountNoThreeByParent(account, authParms));
        }
    }
}

