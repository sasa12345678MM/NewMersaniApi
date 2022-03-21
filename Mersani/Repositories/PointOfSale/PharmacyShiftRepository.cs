using Mersani.Interfaces.PointOfSale;
using Mersani.models.PointOfSale;
using Mersani.Oracle;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Repositories.PointOfSale
{
    public class PharmacyShiftRepository : IPharmacyShiftRepo
    {
        public async Task<DataSet> CheckUserShiftForPharmacy(PharmacyShiftActivation data, string authParms)
        {
            return await OracleDQ.ExcuteSelectizeProcAsync("PRC_CHECK_PHARMACY_SHIFT", new List<dynamic>() { data }, authParms);
        }

        public async Task<DataSet> DeletePharmacyShiftMasterDetails(PharmacyShiftDetail entity, int type, string authParms)
        {
            var authP = OracleDQ.GetAuthenticatedUserObject(authParms);
            entity.CURR_USER = authP.UserCode.Value;
            entity.STATE = (int)OperationType.Delete;

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_Hdr", new List<dynamic>() { entity });
            parameters.Add("xml_document_Dtl", new List<dynamic>() { });
            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_POS_SHIFTS_SETUP_XML", parameters, authParms);
        }

        public async Task<DataSet> GetPharmacyLastShiftCashLeft(int pharmacyId, string authParms)
        {
            var query = $"SELECT FN_GET_PHARM_CASH_LEFT({pharmacyId}) AS CASH_LEFT FROM DUAL";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetPharmacyShiftDetails(PharmacyShiftMaster entity, string authParms)
        {
            var query = $"select * from POS_SHIFTS_SETUP_DTL where PSD_PSH_SYS_ID = :pPSD_PSH_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pPSD_PSH_SYS_ID", entity.PSH_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> GetPharmacyShiftMaster(PharmacyShiftMaster entity, string authParms)
        {
            var query = $"select * from  POS_SHIFTS_SETUP_HDR where PSH_PHARM_SYS_ID = :pPSH_PHARM_SYS_ID";
            var parms = new List<OracleParameter>() { new OracleParameter("pPSH_PHARM_SYS_ID", entity.PSH_PHARM_SYS_ID) };
            return await OracleDQ.ExcuteGetQueryAsync(query, parms, authParms, CommandType.Text);
        }

        public async Task<DataSet> PostPharmacyShiftMasterDetails(PharmacyShiftData entity, string authParms)
        {
            var authData = OracleDQ.GetAuthenticatedUserObject(authParms);

            //hdr
            entity.MASTER.CURR_USER = authData.UserCode.Value;
            if (entity.MASTER.PSH_SYS_ID > 0) entity.MASTER.STATE = (int)OperationType.Update;
            else entity.MASTER.STATE = (int)OperationType.Add;

            // dtl 
            for (int i = 0; i < entity.DETAILS.Count; i++)
            {
                entity.DETAILS[i].PSD_PSH_SYS_ID = entity.MASTER.PSH_SYS_ID;
                entity.DETAILS[i].CURR_USER = authData.UserCode;
                if (entity.DETAILS[i].PSD_SYS_ID > 0) entity.DETAILS[i].STATE = (int)OperationType.Update;
                else entity.DETAILS[i].STATE = (int)OperationType.Add;
            }

            Dictionary<string, List<dynamic>> parameters = new Dictionary<string, List<dynamic>>();
            parameters.Add("xml_document_h", new List<dynamic>() { entity.MASTER });
            parameters.Add("xml_document_d", entity.DETAILS.ToList<dynamic>());

            return await OracleDQ.ExcuteMasterDetailsXMLAsync("PRC_POS_SHIFTS_SETUP_XML", parameters, authParms);
        }

        public async Task<DataSet> PostPharmacyShift(PharmacyShiftActivation shift, string authParms)
        {
            if (shift.PSA_SYS_ID > 0) shift.STATE = (int)OperationType.Update;
            else shift.STATE = (int)OperationType.Add;
            shift.CURR_USER = OracleDQ.GetAuthenticatedUserObject(authParms).UserCode;

            return await OracleDQ.ExcuteXmlProcAsync("PRC_POS_SHIFT_ACTIVATION_XML", new List<dynamic>() { shift }, authParms);
        }

        public async Task<DataSet> GetCashierShiftTotals(int shiftId, string authParms)
        {
            var query = $"SELECT NVL(SUM (PCH_TOTAL_AMOUNT),0) AS TOTAl_SALES, NVL(SUM (PCH_CASH_PAYMENT),0) AS TOTAL_CASH, " +
                $" NVL(SUM (PCH_CARD_PAYMENT),0) AS TOTAL_CARD, FN_GET_SHIFT_TOTAL_EXPENSES({shiftId}) AS TOTAL_EXPENSES " +
                $" FROM POS_CHASHER_HDR WHERE PCH_PSA_SYS_ID = {shiftId}";
            return await OracleDQ.ExcuteGetQueryAsync(query, null, authParms, CommandType.Text);
        }
    }
}
