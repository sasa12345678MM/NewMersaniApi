using System.Threading.Tasks;
using Mersani.Interfaces.FinancialSetup;
using Mersani.models.FinancialSetup;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Mersani.Controllers.FinancialSetup
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankSetupController : GeneralBaseController
    {
        protected readonly IBankSetupRepo _bankSetupRepo;
        public BankSetupController(IBankSetupRepo bankSetupRepo)
        {
            _bankSetupRepo = bankSetupRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBankSetup(int id )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _bankSetupRepo.GetBankSetup(id, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostNewBankSetup([FromBody] BankSetup bank)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _bankSetupRepo.PostBankSetup(bank, authParms);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBankSetup([FromRoute] int id, [FromBody] BankSetup entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;

            if (id == entity.FB_BANK_CODE)
            {
                string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
                result = await _bankSetupRepo.PostBankSetup(entity, authParms);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBankSetup([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            if (id > 0)
            {
                result = await _bankSetupRepo.DeletBankSetup(id, authParms);
            }

            return Ok(result);
        }
    }
}
