using Mersani.Interfaces.Notifications;
using Mersani.Interfaces.PointOfSale;
using Mersani.models.Notifications;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mersani.models.Hubs
{

    public class NotificationsHub : Hub<INotificationsHub>
    {
        //public async Task BroadcastAsync(dynamic data)
        //{
        //    await Clients.All.NotificationReceivedFromHub(data);
        //}
        //public override async Task OnConnectedAsync()
        //{
        //    await Clients.All.NewUserConnected("a new user connectd");
        //}
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

    }

    public interface INotificationsHub
    {
        string GetConnectionId();
    }

    public class Notification
    {
        public string Text { get; set; }
        public string ConnectionId { get; set; }
        public DateTime DateTime { get; set; }
    }


    public class TimerManager
    {
        private Timer _timer;
        private AutoResetEvent _autoResetEvent;
        private Action _action;
        public DateTime TimerStarted { get; }
        public TimerManager(Action action)
        {
            _action = action;
            _autoResetEvent = new AutoResetEvent(false);
            _timer = new Timer(Execute, _autoResetEvent, 1000, 2000);
            TimerStarted = DateTime.Now;
        }
        public void Execute(object stateInfo)
        {
            _action();
            if ((DateTime.Now - TimerStarted).Seconds > 60)
            {
                _timer.Dispose();
            }
        }
    }

    public class DataManager
    {
        protected readonly IReminderRepo _reminderRepo;
        protected readonly IPointOfSaleRepo _pointOfSaleRepo;
        public DataManager(IReminderRepo reminderRepo) { _reminderRepo = reminderRepo;  }
        public DataManager(IPointOfSaleRepo pointOfSaleRepo) { _pointOfSaleRepo = pointOfSaleRepo; }
        public List<ReminderModel> GetRemindersData(ReminderUser user, string authParms)
        {
            var data = _reminderRepo.GetReminders(user, authParms);
            return data;
        }

        public List<dynamic> GetSalesOrersReminder(int phramcyId, string authParms)
        {
            var data = _pointOfSaleRepo.GetReminders(phramcyId, authParms);
            return data;
        }
    }

    public class ReminderModel
    {
        public char? RH_FRZ_Y_N { get; set; }
        public char? RD_FRZ_Y_N { get; set; }
        public char? RU_STARTUP_Y_N { get; set; }
        public char? RU_EMAIL_Y_N { get; set; }
        public char? RU_SMS_Y_N { get; set; }
        public char? USR_FRZ_Y_N { get; set; }
        public string TXN_TYPE { get; set; }
        public DateTime? TXN_DATE { get; set; }
        public int? RH_SYS_ID { get; set; }
        public int? RD_SYS_ID { get; set; }
        public int? RD_RH_SYS_ID { get; set; }
        public int? RD_APP_CODE { get; set; }
        public int? RU_SYS_ID { get; set; }
        public int? RU_RD_SYS_ID { get; set; }
        public int? RU_USR_CODE { get; set; }
        public int? RU_LEVEL { get; set; }
        public int? RU_BEF_DAYS { get; set; }
        public int? RU_AFT_DAYS { get; set; }
        public int? RU_MNU_CODE { get; set; }
        public int? USR_CODE { get; set; }
        public string USR_LOGIN { get; set; }
        public string USR_EMAIL_ID { get; set; }
        public string USR_DEF_LANG { get; set; }
        public string RH_CODE { get; set; }
        public string USR_MOB { get; set; }
        public string USR_TEL { get; set; }
        public string RD_DEC_AR { get; set; }
        public string RD_DEC_EN { get; set; }
        public string USR_FULL_NAME_AR { get; set; }
        public string USR_FULL_NAME_EN { get; set; }
        public string RU_V_CODE { get; set; }
        public string TXN_CODE { get; set; }
        public string RH_DESC_AR { get; set; }
        public string RH_DESC_EN { get; set; }
        public string MNU_PATH { get; set; }
    }
}
