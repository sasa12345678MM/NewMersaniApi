using Mersani.models.Auth;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Auth
{
    public interface IAuthRepository
    {
        Task<DataSet> Login(UserLoginModal user);

        Task<DataSet> GetActivityView(string userActivityCode, int userCode, string authParms);
    }
}
