using Mersani.models.Administrator;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Administrator
{
   public interface IPrinterSettingsRepo
    {
        Task<DataSet> GetPrinterSettings(PrinterSetup entity, string authParms);
        Task<DataSet> BulkPrinterSettings(List<PrinterSetup> entity, string authParms);
        Task<DataSet> DeletePrinterSettings(PrinterSetup entity, string authParms);
        DataSet GetSystemPrinterDevices(string authParms);
    }
}
