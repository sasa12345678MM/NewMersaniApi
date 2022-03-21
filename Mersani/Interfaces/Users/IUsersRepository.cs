using Mersani.models.Users;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Repositories
{
    public interface IUsersRepository
    {
        Task<bool> UploadProfileImg(UserData user, string authParms);
        Task<DataSet> UpdateUserProfileData(UserData user, string authParms);
        List<UpladFile> GetEncryptedFileName( string authParms);

        // new rules
        Task<DataSet> GetUserData(int UserCode, string authParms);
        Task<DataSet> PostUserData(UserData user, string authParms);
        Task<DataSet> DeleteUserData(UserData user, string authParms);
    }
}
