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
    public class CustomerClassController : GeneralBaseController
    {
        protected readonly ICustomerClassRepo _customerClassRepo;
        public CustomerClassController(ICustomerClassRepo customerClassRepo)
        {
            _customerClassRepo = customerClassRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomerClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerClassRepo.GetCustomerClassDataList(new CustomerClass() { FCUC_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomerClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerClassRepo.DeleteCustomerData(new CustomerClass() { FCUC_SYS_ID = id }, authParms));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> PostSupplierClassRows([FromBody] List<CustomerClass> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerClassRepo.BulkInsertUpdateCustomerData(entities, authParms));
        }

    }
}
