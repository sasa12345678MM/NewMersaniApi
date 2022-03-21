using Mersani.Interfaces.Administrator;
using Mersani.models.Administrator;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.Adminstrator
{
    public class PrinterSettingsRepository : IPrinterSettingsRepo
    {
        public async Task<DataSet> GetPrinterSettings(PrinterSetup entity, string authParms)
        {
            var query = $"SELECT * FROM GAS_PRINTER_SETUP WHERE GPS_SYS_ID = :pGPS_SYS_ID OR :pGPS_SYS_ID = 0";
            var parms = new List<OracleParameter>() {
                new OracleParameter("pGPS_SYS_ID", entity.GPS_SYS_ID)
            };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }
        public async Task<DataSet> BulkPrinterSettings(List<PrinterSetup> entities, string authParms)
        {
            foreach (var entity in entities)
            {
                if(entity.GPS_SYS_ID > 0) entity.STATE = (int)OperationType.Update;
                else entity.STATE = (int)OperationType.Add;
                entity.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;
                entity.GPS_V_CODE = OracleDQ.GetAuthenticatedUserObject(authParms).User_Act_PH;
            }
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_PRINT_SETUP_XML", entities.ToList<dynamic>(), authParms);

        }

        public async Task<DataSet> DeletePrinterSettings(PrinterSetup entity, string authParms)
        {
            entity.STATE = (int)OperationType.Delete;
            return await OracleDQ.ExcuteXmlProcAsync("PRC_GAS_PRINT_SETUP_XML", new List<dynamic>() { entity }, authParms);
        }

        public DataSet GetSystemPrinterDevices(string authParms)
        {
            var printers = PrinterSettings.InstalledPrinters;
            var ds = new DataSet();
            DataTable res = ds.Tables.Add("result");
            DataTable errorTable = ds.Tables.Add("message");

            res.Columns.Add("printer_name", typeof(string));
            foreach (string printer in printers)
            {
                res.Rows.Add(printer);
            }

            errorTable.Columns.Add("msgHead", typeof(string));
            errorTable.Columns.Add("msgBody", typeof(string));

            errorTable.Rows.Add(new Object[] { "1", "Success" });

            return ds;
        }
    }
}
