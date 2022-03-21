using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTranslationController : GeneralBaseController
    {
        public readonly IUserTranslationRepo _IUserTranslationRepo;

        public UserTranslationController(IUserTranslationRepo _UserTranslationRepo)
        {
            _IUserTranslationRepo = _UserTranslationRepo;
        }
        [HttpGet("{id}")]
        public ActionResult GetUserTranslation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(_IUserTranslationRepo.GetUserTranslation(id, authParms));
        }
        [HttpGet("Page/{id}")]
        public ActionResult GetPageTranslation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelStateErrors());
            }
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(_IUserTranslationRepo.GetPageTranslation(id, authParms));
        }

        [HttpPost]
        public ActionResult PostNewTranslation([FromBody] UserTranslation _UserTranslation)
        {
            if (!ModelState.IsValid) 
                return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(_IUserTranslationRepo.PostUserTranslationp(_UserTranslation, authParms));
        }

        [HttpPut("{id}")]
        public ActionResult UpdateTranslation([FromRoute] int id, [FromBody] UserTranslation _UserTranslation)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;

            if (id == _UserTranslation.LABEL_CODE)
            {
                string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

                if (_UserTranslation.LABEL_CODE > 0)
                {
                    result = _IUserTranslationRepo.UpdateUserTranslation(id, _UserTranslation, authParms);
                }
            }
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteUserGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            if (id > 0)
            {
                result = _IUserTranslationRepo.DeleteUserTranslation(id, authParms);
            }

            return Ok(result);
        }

        [HttpGet("ExportPageTranslationAr")]
        public ActionResult ExportPageTranslationAr()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(new List<dynamic>() { _IUserTranslationRepo.ExportPageTranslationAr(authParms) });
        }

        [HttpGet("ExportPageTranslationEN")]
        public ActionResult ExportPageTranslationEn()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(new List<dynamic>() { _IUserTranslationRepo.ExportPageTranslationEn(authParms) });
        }

    }
}
