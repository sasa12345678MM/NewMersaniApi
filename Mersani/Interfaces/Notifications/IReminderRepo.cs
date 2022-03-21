using Mersani.models.Hubs;
using Mersani.models.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Notifications
{
    public interface IReminderRepo
    {
        // head
        Task<DataSet> GetReminderHeaders(ReminderHeader header, string authParms);
        Task<DataSet> BulkReminderHeaders(List<ReminderHeader> headers, string authParms);
        Task<DataSet> DeleteReminderHeader(ReminderHeader header, string authParms);


        // details
        Task<DataSet> GetReminderDetails(ReminderDetail detail, string authParms);
        Task<DataSet> BulkReminderDetails(List<ReminderDetail> details, string authParms);
        Task<DataSet> DeleteReminderDetail(ReminderDetail detail, string authParms);


        // users
        Task<DataSet> GetReminderUsers(ReminderUser user, string authParms);
        Task<DataSet> BulkReminderUsers(List<ReminderUser> users, string authParms);
        Task<DataSet> DeleteReminderUser(ReminderUser user, string authParms);


        Task<DataSet> GetNotifications(ReminderUser user, string authParms);
        List<ReminderModel> GetReminders(ReminderUser user, string authParms);

    }
}
