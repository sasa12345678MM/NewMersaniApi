using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Mersani.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFilesController : GeneralBaseController
    {

        [HttpPost("Upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = Path.Combine("Resources", "Files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!Directory.Exists(pathToSave)) Directory.CreateDirectory(pathToSave);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    if (Directory.Exists(fullPath)) return BadRequest("File Exists");
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath, result = new List<dynamic>() { new { dbPath } }, message = new { msgHead = "1", msgBody = "Done" } });
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
                var formCollection = await Request.ReadFormAsync();
                var files = formCollection.Files;
                var folderName = Path.Combine("Resources", "Files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (files.Any(f => f.Length == 0)) return BadRequest("No Files Upload!");

                if (!Directory.Exists(pathToSave)) Directory.CreateDirectory(pathToSave);
                List<string> dbPaths = new List<string>();
                foreach (var file in files)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
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
    }
}
