using Mersani.models.Users;
using Mersani.Utility;
using Mersani.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Mersani.Oracle;

namespace Mersani.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : GeneralBaseController
    {
        protected readonly IUsersRepository _usersRepository;
        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _usersRepository.GetUserData(id, authParms));
        }

        [HttpPost]
        public async Task<ActionResult> PostNewUser([FromBody] UserData user)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _usersRepository.PostUserData(user, authParms));
        }

        [HttpPut("UploadProfilePhoto/{id}")]
        public async Task<ActionResult> UploadProfilePhoto([FromRoute] int id, [FromBody] UserData entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            bool result = false;

            if (id == entity.USR_CODE)
            {
                string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

                if (entity.USR_CODE > 0)
                {
                    result = await _usersRepository.UploadProfileImg(entity, authParms);
                }
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _usersRepository.DeleteUserData(new UserData() { USR_CODE = id }, authParms));
        }

        [HttpPut("UpdateUserProfile/{id}")]
        public async Task<ActionResult> UpdateUserProfile([FromRoute] int id, [FromBody] UserData entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _usersRepository.UpdateUserProfileData(entity, authParms));
        }

        #region upload images
        [HttpPost("Upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
                var fileName = OracleDQ.GetEncryptedFileName(authParms);

                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = Path.Combine("Resources", "Files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!Directory.Exists(pathToSave)) Directory.CreateDirectory(pathToSave);

                if (file.Length > 0)
                {
                    //var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var ext =Path.GetExtension(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
                    var fullPath = Path.Combine(pathToSave, fileName + ext);
                    var dbPath = Path.Combine(folderName, fileName + ext);
                    if (Directory.Exists(fullPath)) return BadRequest("File Exists");
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { result = new List<dynamic>() { new { dbPath } }, message = new List<dynamic>() { new { msgHead = "1", msgBody = "Done" } } });
                }
                else
                {
                    return BadRequest("you don't authorize to upload");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpPost("UploadMulti"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadMulti()
        {
            try
            {

                string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
                //List<UpladFile> UpladFile = _usersRepository.GetEncryptedFileName(authParms);

                var formCollection = await Request.ReadFormAsync();
                var files = formCollection.Files;
                var folderName = Path.Combine("Resources", "Files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (files.Any(f => f.Length == 0)) return BadRequest("No Files Upload!");

                if (!Directory.Exists(pathToSave)) Directory.CreateDirectory(pathToSave);
                List<string> dbPaths = new List<string>();
                foreach (var file in files)
                {
                    //List<UpladFile> UpladFile = _usersRepository.GetEncryptedFileName(authParms);
                    var fileName = OracleDQ.GetEncryptedFileName(authParms);
                    var ext = Path.GetExtension(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
                    var fullPath = Path.Combine(pathToSave, fileName + ext);
                    var dbPath = Path.Combine(folderName, fileName +  ext);
                    //var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    //var fullPath = Path.Combine(pathToSave, fileName);
                    //var dbPath = Path.Combine(folderName, fileName);
                    if (Directory.Exists(fullPath)) continue;
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    dbPaths.Add(dbPath);
                }
                return Ok(new { dbPaths });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        #endregion
    }
}
