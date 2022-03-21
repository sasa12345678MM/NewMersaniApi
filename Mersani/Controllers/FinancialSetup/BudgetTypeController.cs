using Mersani.Interfaces.FinancialSetup;
using Mersani.models.FinancialSetup;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.FinancialSetup
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetTypeController : GeneralBaseController
    {
        protected readonly IBudgetTypeRepo _budgetTypeRepo;

        public BudgetTypeController(IBudgetTypeRepo budgetTypeRepo)
        {
            _budgetTypeRepo = budgetTypeRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBudgetType([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            var result = await _budgetTypeRepo.GetBudgetType(id, authParms);


            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult> PostNewBudgetType([FromBody] BudgetType user)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var result = await _budgetTypeRepo.PostNewBudgetType(user, authParms);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBudgetType([FromRoute] int id, [FromBody] BudgetType entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;

            if (id == entity.BDG_CODE)
            {
                string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
                result = await _budgetTypeRepo.PostNewBudgetType(entity, authParms);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBudgetType([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            if (id > 0)
            {
                result = await _budgetTypeRepo.DeleteBudgetType(id, authParms);
            }

            return Ok(result);
        }
    }
}
