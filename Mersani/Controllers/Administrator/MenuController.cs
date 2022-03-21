using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;
using System.Threading.Tasks;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : GeneralBaseController
    {
        protected readonly IMenuRepo _menuRepo;
        public MenuController(IMenuRepo menuRepo)
        {
            _menuRepo = menuRepo;
        }

        [HttpGet("{id}")]
        public ActionResult GetMenu([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(_menuRepo.GetMenu(id, authParms));
        }

        [HttpGet("GetUserMenu/{id}")]
        public ActionResult GetUserMenu([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(_menuRepo.GetUserMenu(id, authParms));
        }

        [HttpPost]
        public ActionResult PostNewMenu([FromBody] Menu menu)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(_menuRepo.PostNewMenu(menu, authParms));
        }

        [HttpPut("{id}")]
        public ActionResult UpdateMenu([FromRoute] int id, [FromBody] Menu menu)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;

            if (id == menu.MNU_CODE)
            {
                string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

                if (menu.MNU_CODE > 0)
                {
                    result = _menuRepo.UpdateMenu(id, menu, authParms);
                }
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteMenu([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            if (id > 0)
            {
                result = _menuRepo.DeleteMenu(id, authParms);
            }

            return Ok(result);
        }

        #region Report Menu
        [HttpGet("GetReportMenus/{id}")]
        public async Task<ActionResult> GetReportMenus([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _menuRepo.GetReportMenus(id, authParms));
        }

        [HttpGet("GetReportMenuDetails/{id}")]
        public async Task<ActionResult> GetReportMenuDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _menuRepo.GetReportMenuDetails(id, authParms));
        }
        #endregion
    }
}
