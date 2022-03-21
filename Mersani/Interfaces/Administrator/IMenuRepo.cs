using Mersani.models.Administrator;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
    public interface IMenuRepo
    {
        List<Menu> GetMenu(int id, string authParms);
        List<Menu> GetUserMenu(int userId, string authParms);
        bool PostNewMenu(Menu menu, string authParms);
        bool UpdateMenu(int id, Menu menu, string authParms);
        bool DeleteMenu(int id, string authParms);

        Task<DataSet> GetReportMenus(int menuCode, string authParms);
        Task<DataSet> GetReportMenuDetails(int menuCode, string authParms);
    }
}
