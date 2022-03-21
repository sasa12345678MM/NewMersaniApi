using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComapnyBranchesController : GeneralBaseController
    {
        protected readonly ICompanyBranchesRepo _comapnyBranchesRepo;
        public ComapnyBranchesController(ICompanyBranchesRepo comapnyBranchesRepo)
        {
            _comapnyBranchesRepo = comapnyBranchesRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCompanyBranches([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _comapnyBranchesRepo.GetBranch(new CompanyBranches() { CB_SYS_ID = id }, authParms));
        }

        [HttpGet("getByCompany/{id}")]
        public async Task<ActionResult> GetBranchesByCompany([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _comapnyBranchesRepo.GetBranchByCompany(new CompanyBranches() { CB_COMPANY_SYS_ID = id }, authParms));
        }


        [HttpPost("bulk")]
        public async Task<ActionResult> bulkCompanyBranches([FromBody] List<CompanyBranches> companyBranches)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _comapnyBranchesRepo.BulkBranches(companyBranches, authParms));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCompanyBranches([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _comapnyBranchesRepo.DeleteBranch(new CompanyBranches() { CB_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode/{id}")]
        public async Task<ActionResult> GetLastCode([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _comapnyBranchesRepo.GetLastCode(id, authParms));
        }
    }
}
