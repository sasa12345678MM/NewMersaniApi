using Mersani.Interfaces.Administrator;
using Mersani.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mersani.models.Administrator;
using SelectPdf;

namespace Mersani.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralSharedController : GeneralBaseController
    {
        protected readonly GeneralSharedRepo _GeneralSharedRepo;

        public GeneralSharedController(GeneralSharedRepo GeneralSharedRepo)
        {
            _GeneralSharedRepo = GeneralSharedRepo;
        }

        [HttpGet("currStk/{invSysId}/{itemSysId}/{batchSysId}/{uomSysId}")]
        public async Task<ActionResult> getInvItemcurrStk(int invSysId, int itemSysId, int batchSysId, int uomSysId)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralSharedRepo.getInvItemcurrStk(invSysId, itemSysId, batchSysId, uomSysId, authParms));
        }

        [HttpGet("GetLoggedInPharmacyData")]
        public async Task<ActionResult> GetLoggedInPharmacyData()
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _GeneralSharedRepo.GetLoggedInPharmacyData(authParms));
        }


        // print on server
        [HttpPost("PrintOnServer/bulk")]
        public async Task<IActionResult> PrintOnServer([FromBody] List<PrintParms> _PrintParms)
        {
            string ext = "html";
            if (!ModelState.IsValid)
                return BadRequest(GetModelStateErrors());
            try
            {
                if (ext == "pdf")
                {
                    // create a new pdf document
                    HtmlToPdf converter = new HtmlToPdf();

                    PdfDocument doc = new PdfDocument();

                    for (int i = 0; i < _PrintParms.Count; i++)
                    {
                        doc = converter.ConvertHtmlString("<html><head><title> " + _PrintParms[i].TranSerial + "</title></head><body>" + _PrintParms[i].HtmlContent + "</body></html>");

                        // save pdf document
                        if (_PrintParms[i].PrintType == 1)
                        {
                            doc.Save(_PrintParms[i].TransPrintPath + "\\" + (_PrintParms[i].TranSerial) + "_" + i + ".pdf");
                        }
                        else
                        {
                            doc.Save(_PrintParms[i].PreparePrintPath + "\\" + (_PrintParms[i].TranSerial) + "_" + i + ".pdf");
                        }

                        // close pdf document
                        doc.Close();
                    }

                }
                else
                {
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < _PrintParms.Count; i++)
                    {
                        await Task.Delay(i * 2000).ContinueWith((task) =>
                          {
                              if (_PrintParms[i].PrintType == 1)
                              {
                                  System.IO.File.WriteAllText(_PrintParms[i].TransPrintPath + "\\" + (_PrintParms[i].TranSerial) + "_" + i + "." + ext, _PrintParms[i].HtmlContent);
                              }
                              else
                              {
                                  System.IO.File.WriteAllText(_PrintParms[i].PreparePrintPath + "\\" + (_PrintParms[i].TranSerial) + "_" + i + "." + ext, _PrintParms[i].HtmlContent);
                              }
                          });

                    }

                }

                return Ok();
            }
            catch (Exception exc)
            {
                return BadRequest(GetExcErrors(exc));
            }

        }

        [HttpPost("NearbyPharmacies/search")]
        public async Task<ActionResult> GetNearbyPharmacies([FromBody] NearbyPharmaciesPosition position)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());
            return Ok(await _GeneralSharedRepo.GetNearbyPharmacies(position));
        }
    }

    public class NearbyPharmaciesPosition
    {
        public decimal LATITUDE { get; set; }
        public decimal LONGITUDE { get; set; }
    }
}