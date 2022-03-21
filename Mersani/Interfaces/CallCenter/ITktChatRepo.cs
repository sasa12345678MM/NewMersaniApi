using Mersani.models.Hubs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.CallCenter
{
    public interface ITktChatRepo
    {

        Task<DataSet> GetChatHistory(TktChat chat, string authParms);
        Task<DataSet> GetChatHistoryForCustomer(string sender, string authParms);
        Task<DataSet> GetChatRecieversHistory(TktChat chat, string authParms);
    }
}
