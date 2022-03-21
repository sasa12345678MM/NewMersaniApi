using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using Mersani.models.Administrator;
using Mersani.Interfaces.Administrator;
using System.Threading.Tasks;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : GeneralBaseController
    {
        protected readonly ICountryRepo _countryRepo;
        public CountryController(ICountryRepo countryRepo)
        {
            _countryRepo = countryRepo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCountries([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = "";// CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _countryRepo.GetCountries(id, authParms));
        }


        [HttpPost]
        public async Task<ActionResult> PostNewCountry([FromBody] Country country)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _countryRepo.PostCountry(country, authParms));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCountry([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _countryRepo.DeleteCountry(new Country() { C_SYS_ID = id }, authParms));
        }

        [HttpGet("GetLastCode")]
        public async Task<ActionResult> GetLastCode()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _countryRepo.GetLastCode(authParms));
        }
    }
}
