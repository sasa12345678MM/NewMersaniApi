using Mersani.Interfaces.HR;
using Mersani.models.HR;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.HR
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrLoanTypeController : GeneralBaseController
    {
        protected readonly LoanTypeRepo _loanTypeRepo;
        public HrLoanTypeController(LoanTypeRepo loanTypeRepo)
        {
            _loanTypeRepo = loanTypeRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetHrLoanType([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _loanTypeRepo.GetHrLoansTypeData(id, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostLoanType([FromBody] List<HrLoansType> loanType)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _loanTypeRepo.PostHrLoansTypeData(loanType, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLoanType([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _loanTypeRepo.DeleteHrLoansTypeData(new HrLoansType() { HRLT_SYS_ID = id }, authParms));
        }



    }
}
