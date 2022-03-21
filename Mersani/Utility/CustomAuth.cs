using Mersani.Utility.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mersani.Utility
{
    public class CustomAuth : ActionFilterAttribute
    {
        public static string getTokenParmsAuthorization(HttpContext _HttpContext)
        {
            string parmString = "";
            string authHeader = _HttpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Bearer"))
            {
                string encodedParms = authHeader.Substring("Bearer ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                parmString = encoding.GetString(Convert.FromBase64String(encodedParms));
            }
            else
            {
                //Handle what happens if that isn't the case
                throw new NotFoundException($"Not Authorized User, Please Login First!");
            }
            return parmString;
        }

        public static string encodingToken(string toEncode)
        {
            byte[] bytes = Encoding.GetEncoding(28591).GetBytes(toEncode);
            string toReturn = Convert.ToBase64String(bytes);
            return toReturn;
        }
    }
}
