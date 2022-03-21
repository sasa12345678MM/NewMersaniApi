using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Oracle;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblArchivesController : GeneralBaseController
    {
        public TblArchivesRepo _TblArchivesRepo;
        public Boolean CopyFlag = false;
        public TblArchivesController(TblArchivesRepo TblArchivesRepo)
        {
            this._TblArchivesRepo = TblArchivesRepo;
        }
        [HttpGet("{id}/{name}")]
        public async Task<ActionResult> GetTblArchives([FromRoute] int id, string name)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string Authparms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            return Ok(await _TblArchivesRepo.GetTblArchives(new TblArchives() { ARCH_PARENT_TBL_SYS_ID = id, ARCH_PARENT_TBL_NAME = name }, Authparms));
        }
        [HttpPost]
        public async Task<ActionResult> PostTblArchives([FromBody] List<TblArchives> entity)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string AuthParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _TblArchivesRepo.PostTblArchives(entity, AuthParms));


        }
        [HttpDelete("id")]
        public async Task<ActionResult> DeleteTblArchives([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            String AuthParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _TblArchivesRepo.DeleteTblArchives(new TblArchives() { ARCH_SYS_ID = id }, AuthParms));
        }

        [HttpGet("scan")]
        public ActionResult getscanArchives()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            string Authparms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            //System.Diagnostics.Process scan = new System.Diagnostics.Process();
            //scan.StartInfo.FileName = @"C:\Windows\archiving\scan\scan.Exe";
            ////scan.StartInfo.Arguments = @"C:\Windows\archiving\scan\scan.Exe";
            //scan.Start();

            try
            {
                using (Process myProcess = new Process())
                {
                    
                    string SourcefileName = "image.pdf";
                    string sourceFolder = @"C:\Windows\archiving";
                    string sourceFullPath = System.IO.Path.Combine(sourceFolder, SourcefileName);
                    string[] pdfList = Directory.GetFiles(sourceFolder, "*.pdf");
                    foreach (string f in pdfList)
                    {
                        System.IO.File.Delete(f);
                    }
                    //////////////////////////////////////////////
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = @"C:\Windows\archiving\scan\scan.Exe";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                    //////////////////////////////////////////////
                    //string targetPath = @"C:\Windows\archiving\scancopy";
                    var folderTosave = Path.Combine("Resources", "Scan");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderTosave);
                    // Use Path class to manipulate file and directory paths.
                    var fileNameToSave = OracleDQ.GetEncryptedFileName(Authparms);
                    System.IO.Directory.CreateDirectory(pathToSave);
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }
                    var fullPath = Path.Combine(pathToSave, fileNameToSave + ".pdf");
                    var dbPath = Path.Combine(folderTosave, fileNameToSave + ".pdf");
                    
                    int x = 0;
                    while (!System.IO.File.Exists(sourceFullPath) && x <=15)
                    {
                        Task.Delay(5000).Wait();
                        x += 1;
                    }
                    if (System.IO.File.Exists(sourceFullPath))
                    {
                    System.IO.File.Copy(sourceFullPath, fullPath, true);
                    return Ok(new { dbPath });

                    }
                    else
                    {
                        return StatusCode(500, $"Can not Scan File ");
                    }
                 

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }

        }
     

    }
}

// Example


//string fileName = "image.pdf";
//string sourcePath = @"C:\Windows\archiving";
//string targetPath = @"C:\Windows\archiving\scancopy";

//// Use Path class to manipulate file and directory paths.
//string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
//string destFile = System.IO.Path.Combine(targetPath, fileName);
//System.IO.Directory.CreateDirectory(targetPath);
//System.IO.File.Copy(sourceFile, destFile, true);

//if (System.IO.Directory.Exists(sourcePath))
//{
//    string[] files = System.IO.Directory.GetFiles(sourcePath);
//    foreach (string s in files)
//    {
//        // Use static Path methods to extract only the file name from the path.
//        fileName = System.IO.Path.GetFileName(s);
//        destFile = System.IO.Path.Combine(targetPath, fileName);
//        System.IO.File.Copy(s, destFile, true);
//    }
//}
//else
//{
//    Console.WriteLine("Source path does not exist!");
//}

// Keep console window open in debug mode.