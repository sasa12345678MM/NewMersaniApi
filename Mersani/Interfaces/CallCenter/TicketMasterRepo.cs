using Mersani.models.CostCenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.CallCenter
{
   public interface ITicketMasterRepo
    {
        Task<DataSet> GetTicketMasterData(TicketMaster entity, string authParms); 
        Task<DataSet> GetTicketMasterDataByCustomer(string authParms); 
         Task<DataSet> GetLastCode(string authParms);
        Task<DataSet> UpdateTicketMaster(TicketMaster entity, string authParms);
        Task<DataSet> deleteTicketMaster(TicketMaster entity, string authParms);


        Task<DataSet> GetTicketDetail(TktTicketDetail entity, string authParms);
        Task<DataSet> SaveTicketDetail(List<TktTicketDetail> entity, string authParms);

        ///////////////
        Task<DataSet> getUnAnswerdTickedMaster(int id ,string calltype, string authParms);

        Task<DataSet> SaveTicketMasteDetail(TktTicketData entity, string authParms);
        ///////////////////////////////////////
        Task<DataSet> GetTicketMasterLogData(TicketMasterLog entity, string authParms);

    }
}
