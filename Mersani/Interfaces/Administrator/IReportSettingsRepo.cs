using Mersani.models.Administrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
    public interface IReportSettingsRepo 
    {
        Task<DataSet> getMenuData(Menu Menu, string authParms);
        Task<DataSet> getMenuReportsData(IMenuReports IMenuReports, string authParms);
        Task<DataSet> geMenuReportParmData(IMenuReportParm IMenuReportParm, string authParms);
        Task<DataSet> SaveMenuReportsData(List<IMenuReports> entity, string authParms);
        Task<DataSet> SaveMenuReportParmData(List<IMenuReportParm> entity, string authParms);
        ////////////////////////////////////////////////
        Task<DataSet> getMenuReportUsers(IMenuReportUsers IMenuReportUsers, string authParms);
        Task<DataSet> SaveIMenuReportUsersData(List<IMenuReportUsers> entity, string authParms);

        Task<DataSet> GetMenuParamsByMenuCode(int menu_code, string authParms);
        Task<DataSet> GetMenuParamsByPath(string menu_path, string authParms);
    }
}

