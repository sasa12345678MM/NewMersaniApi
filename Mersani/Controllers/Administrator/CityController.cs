using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;
using System.Threading.Tasks;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : GeneralBaseController
    {
        protected readonly ICityRepo _cityRepo;
        public CityController(ICityRepo cityRepo)
        {
            _cityRepo = cityRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCountries([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = "";//CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _cityRepo.GetCity(id, authParms));
        }
        [HttpPost]
        public async Task<ActionResult> PostCity([FromBody] City city)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _cityRepo.PostCity(city, authParms));
        }

       
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCity([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _cityRepo.DeleteCity(new City() { CITY_SYS_ID = id }, authParms));
        }


        [HttpGet("GetLastCode/{id}")]
        public async Task<ActionResult> GetLastCode([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _cityRepo.GetLastCode(id, authParms));
        }
    }
}
