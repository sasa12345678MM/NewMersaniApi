using Mersani.models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace Mersani.Utility
{
    public class GeneralBaseController : ControllerBase
    {

        public UserData _user { get; set; }

        public GeneralBaseController()
        {

        }

        public GeneralBaseController(UserData user)
        {
            _user = user;
        }

        [NonAction]
        public BadRequestObjectResult GetModelStateErrors()
        {
            string errorsMsg = "";
            BadRequestObjectResult res;
            foreach (ModelError error
                
                in ModelState.Keys.Select(k => ModelState[k].Errors).First())
            {
                if (!String.IsNullOrEmpty(error.ErrorMessage))
                {
                    errorsMsg = errorsMsg + error.ErrorMessage;
                }
                if (error.Exception != null)
                {
                    errorsMsg = errorsMsg + error.Exception.Message;
                }
            }
            res = new BadRequestObjectResult(errorsMsg);

            return res;
        }
        [NonAction]
        public BadRequestObjectResult RetCutomErrors(string errorsMsgAr, string errorsMsgEn)
        {
            string errorsMsg = "";
            BadRequestObjectResult res;
            errorsMsg = errorsMsgAr + errorsMsg;
            res = new BadRequestObjectResult(errorsMsg);

            return res;
        }
        [NonAction]
        public BadRequestObjectResult GetExcErrors(Exception exc)
        {
            string errorsMsg = "";
            BadRequestObjectResult res;
            errorsMsg = exc.Message + Environment.NewLine + exc.StackTrace;
            res = new BadRequestObjectResult(errorsMsg);

            return res;
        }



    }
}
