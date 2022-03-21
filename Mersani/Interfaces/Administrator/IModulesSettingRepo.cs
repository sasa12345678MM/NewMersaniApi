using Mersani.models.Administrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
    public interface IModulesSettingRepo
    {
        Task<DataSet> GetModulesSetting(ModulesSetting entity, string authParms);
        Task<DataSet> BulkModulesSetting(List<ModulesSetting> entities, string authParms);
        Task<DataSet> DeleteModulesSetting(ModulesSetting entity, string authParms);
        Task<DataSet> GetSettingByType(string type, string authParms);
    }
}
