using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mersani.Interfaces.FinancialSetup;
using Mersani.models.FinancialSetup;
using Mersani.Repositories.FinancialSetup;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mersani.Controllers.FinancialSetup
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinsAccountClassController : GeneralBaseController
    {
        IFinsAccountClassRepository FINS_ACC_CLASSRepository;

        public FinsAccountClassController(IFinsAccountClassRepository _FINS_ACC_CLASSRepository)
        {
            FINS_ACC_CLASSRepository = _FINS_ACC_CLASSRepository;
        }
        [HttpGet("{id}")]
        public ActionResult GetFinancialFinisAccountClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(FINS_ACC_CLASSRepository.GetFINS_ACC_CLASSDetails(id, authParms));

        }

        [HttpPost]
        public ActionResult PostNewfinAccountClass([FromBody] FinsAcountClass FinisAccountClass)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(FINS_ACC_CLASSRepository.PostNewFINS_ACC_CLASS(FinisAccountClass, authParms));
        }

        [HttpPut("{id}")]
        public ActionResult UpdatefinAccountClass([FromRoute] int id, [FromBody] FinsAcountClass FinisAccountClass)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;

            if (id == FinisAccountClass.ACC_CLASS_CODE)
            {
                string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

                if (FinisAccountClass.ACC_CLASS_CODE > 0)
                {
                    result = FINS_ACC_CLASSRepository.UpdateFINS_ACC_CLASS(id, FinisAccountClass, authParms);
                }
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public ActionResult DeletefinAccountClass([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            if (id > 0)
            {
                result = FINS_ACC_CLASSRepository.DeleteFINS_ACC_CLASS(id, authParms);
            }

            return Ok(result);
        }
    }
}

