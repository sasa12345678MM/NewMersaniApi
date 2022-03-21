using Mersani.Interfaces.Auth;
using Mersani.models.Auth;
using Mersani.models.Users;
using Mersani.Oracle;
using Mersani.Repositories.Auth;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : GeneralBaseController
    {
        IAuthRepository _authRepository;
        IEmailService _emailService;


        public AuthController(IAuthRepository authRepository, IEmailService emailService)
        {
            _authRepository = authRepository;
            _emailService = emailService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] UserLoginModal loginModal)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            return Ok(await _authRepository.Login(loginModal));
        }
 
        [HttpPost("GetActivityView/search")]
        public async Task<IActionResult> GetActivityView([FromBody] AuthParams auth)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _authRepository.GetActivityView(auth.User_Act_PH, auth.UserCode.Value ,authParms));
        }
        private string MakeRandomString(int size)
        {
            Random rand = new Random();
            // Characters we will use to generate this random string.
            char[] allowableChars = "ABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray();

            // Start generating the random string.
            string activationCode = string.Empty;
            for (int i = 0; i <= size - 1; i++)
            {
                activationCode += allowableChars[rand.Next(allowableChars.Length - 1)];
            }

            // Return the random string in upper case.
            return activationCode.ToUpper();
        }

        [HttpPost("sendMail")]
        public async Task<IActionResult> SendMail([FromBody] Email email)
        {
            //if (email.TO!=null)
            //{
                string code = MakeRandomString(5);
                email.USERNAME = "Mirsani";
                email.SUBJECT = "Your code For Email confirmation";
                email.MESSAGE = code;
                bool  res = await _emailService.SendAsync(email, new AppSettings() { });
                return Ok(new {code= code,res= res });

            //}
            //else
            //{
            //var user = GetDataByUserName(email.USERNAME);
            //if (user.USR_EMAIL_ID == null) return Ok(false);
            //email.TO = user.USR_EMAIL_ID;
            //email.SUBJECT = "Mirsani Reset Password";
            //email.MESSAGE = $"Your Backup Password is : {user.USR_PW} \n Best Regards,,";        
            //    return Ok(await _emailService.SendAsync(email, new AppSettings() { }));
            //}      
        }

        [NonAction]
        private UserData GetDataByUserName(string username)
        {
            var user = OracleDQ.GetData<UserData>("SELECT * FROM GAS_USR WHERE USR_LOGIN = :pUserName", "UserCode/1,UserGroup/1", new { pUserName = username }, CommandType.Text);
            return user.Find(el => el.USR_LOGIN == username);
        }
    }
}
