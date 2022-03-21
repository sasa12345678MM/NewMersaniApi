using Mersani.Utility;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mersani.models.FinancialSetup;
using Mersani.Interfaces.FinancialSetup;

namespace Mersani.Controllers.FinancialSetup
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : GeneralBaseController
    {
        protected readonly ICustomerRepo _customerRepo;
        public CustomerController(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomerClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerRepo.GetCustomerDataList(new Customer() { CUST_SYS_ID = id }, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomerClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerRepo.DeleteCustomer(new Customer() { CUST_SYS_ID = id }, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostSupplierClassRows([FromBody] Customer entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerRepo.BulkInsertUpdateCustomer(entity, authParms));
        }

        [HttpPost("SavePOSCustomer")]
        public async Task<ActionResult> SavePOSCustomer([FromBody] Customer entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerRepo.SavePOSCustomer(entity, authParms));
        }


        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _customerRepo.GetLastCode(authParms));
        }

        [HttpGet("GetPOSLastCode")]
        public async Task<ActionResult> GetPOSLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _customerRepo.GetPOSLastCode(authParms));
        }

        [HttpGet("address/{id}/{ParentId}")]
        public async Task<ActionResult> getFinsCustomerAddresses([FromRoute] int id, int ParentId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerRepo.getFinsCustomerAddresses(new FinsCustomerAddresses() { FCA_SYS_ID = id }, ParentId, authParms));
        }

        [HttpDelete("address/{id}")]
        public async Task<ActionResult> DeleteFinsCustomerAddresses([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerRepo.DeleteFinsCustomerAddresses(new FinsCustomerAddresses() { FCA_SYS_ID = id }, authParms));
        }

        [HttpPost("address")]
        public async Task<ActionResult> PostFinsCustomerAddresses([FromBody] FinsCustomerAddresses entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerRepo.PostFinsCustomerAddresses(entity, authParms));
        }
        [HttpGet("Relatives/{id}/{ParentId}")]
        public async Task<ActionResult> getFinsCustomerRelatives([FromRoute] int id, int ParentId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerRepo.getFinsCustomerRelatives(new FinsCustomerRelatives() { FCR_SYS_ID = id }, ParentId, authParms));
        }

        [HttpDelete("Relatives/{id}")]
        public async Task<ActionResult> DeleteFinsCustomerRelatives([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerRepo.DeleteFinsCustomerRelatives(new FinsCustomerRelatives() { FCR_SYS_ID = id }, authParms));
        }

        [HttpPost("Relatives")]
        public async Task<ActionResult> PostFinsCustomerRelatives([FromBody] FinsCustomerRelatives entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _customerRepo.PostFinsCustomerRelatives(entity, authParms));
        }

        [HttpGet("mobile/{mobile}")]
        public async Task<ActionResult> getCustomerByMobile([FromRoute] string mobile)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _customerRepo.getCustomerByMobile(mobile, authParms));
        }
    }
}
