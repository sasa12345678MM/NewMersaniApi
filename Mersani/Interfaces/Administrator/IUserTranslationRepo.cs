using Mersani.models.Administrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
    public interface IUserTranslationRepo
    {
        List<UserTranslation> GetUserTranslation(int id, string authParms);
        List<UserTranslation> GetPageTranslation(int PageCode, string authParms);
        object ExportPageTranslationAr(string authParms);
        object ExportPageTranslationEn(string authParms);
        bool PostUserTranslationp(UserTranslation userGroup, string authParms);
        bool DeleteUserTranslation(int id, string authParms);
        bool UpdateUserTranslation(int id, UserTranslation userTranslation, string authParms);

    }
}
