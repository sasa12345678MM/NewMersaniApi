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
    public class CompanyController : GeneralBaseController
    {
        protected readonly ICompanyRepo _companyRepo;
        public CompanyController(ICompanyRepo companyRepo)
        {
            _companyRepo = companyRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCompanies([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _companyRepo.GetCompany(new Company() { COMP_SYS_ID = id }, authParms));
        }

        [HttpGet("getByGroup/{id}")]
        public async Task<ActionResult> GetCompaniesByGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _companyRepo.GetCompaniesByGroup(new Company() { COMP_GROUP_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> bulkCompanies([FromBody] List<Company> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _companyRepo.BulkCompanys(entities, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCompany([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _companyRepo.DeleteCompany(new Company() { COMP_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode/{id}")]
        public async Task<ActionResult> GetLastCode( [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _companyRepo.GetLastCode(id, authParms));
        }
    }
}
