using Mersani.Interfaces.Notifications;
using Mersani.models.Hubs;
using Mersani.models.Notifications;
using Mersani.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Controllers.Notifications
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : GeneralBaseController
    {
        protected readonly IReminderRepo _reminderRepo;
        private IHubContext<NotificationsHub> _hubContext;

        public ReminderController(IReminderRepo reminderRepo, IHubContext<NotificationsHub> hubContext)
        {
            _reminderRepo = reminderRepo;
            _hubContext = hubContext;
        }


        #region header
        [HttpGet("head/{id}")]
        public async Task<ActionResult> GetReminderHeaders([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _reminderRepo.GetReminderHeaders(new ReminderHeader() { RH_SYS_ID = id }, authParms));
        }

        [HttpPost("head/bulk")]
        public async Task<ActionResult> bulkReminderHeaders([FromBody] List<ReminderHeader> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _reminderRepo.BulkReminderHeaders(entities, authParms));
        }

        [HttpDelete("head/{id}")]
        public async Task<ActionResult> DeleteReminderHeader([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _reminderRepo.DeleteReminderHeader(new ReminderHeader() { RH_SYS_ID = id }, authParms));
        }
        #endregion

        #region detail
        [HttpGet("detail/{id}")]
        public async Task<ActionResult> GetReminderDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _reminderRepo.GetReminderDetails(new ReminderDetail() { RD_RH_SYS_ID = id }, authParms));
        }

        [HttpPost("detail/bulk")]
        public async Task<ActionResult> bulkReminderDetails([FromBody] List<ReminderDetail> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _reminderRepo.BulkReminderDetails(entities, authParms));
        }

        [HttpDelete("detail/{id}")]
        public async Task<ActionResult> DeleteReminderDetail([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _reminderRepo.DeleteReminderDetail(new ReminderDetail() { RD_SYS_ID = id }, authParms));
        }
        #endregion

        #region user
        [HttpGet("user/{id}")]
        public async Task<ActionResult> GetReminderUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _reminderRepo.GetReminderUsers(new ReminderUser() { RU_RD_SYS_ID = id }, authParms));
        }

        [HttpPost("user/bulk")]
        public async Task<ActionResult> bulkReminderUsers([FromBody] List<ReminderUser> entities)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _reminderRepo.BulkReminderUsers(entities, authParms));
        }

        [HttpDelete("user/{id}")]
        public async Task<ActionResult> DeleteReminderUser([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);

            return Ok(await _reminderRepo.DeleteReminderUser(new ReminderUser() { RU_SYS_ID = id }, authParms));
        }
        #endregion


        [HttpPost]
        public async Task SendMessage(Notification message)
        {
            //additional business logic 

            await _hubContext.Clients.All.SendAsync("messageReceivedFromApi", message);

            //additional business logic 
        }

        [HttpPost("notify/search")]
        public async Task<ActionResult> GetNotifications([FromBody] ReminderUser reminderUser)
        {
            if (!ModelState.IsValid) return BadRequest(GetModelStateErrors());

            string authParms = CustomAuth.getTokenParmsAuthorization(Request.HttpContext);
            var data = await _reminderRepo.GetNotifications(reminderUser, authParms);
            var timerManager = new TimerManager(() => { });
            if (reminderUser.CONNECTION_ID != null)
                timerManager = new TimerManager(() => _hubContext.Clients.Client(reminderUser.CONNECTION_ID).SendAsync("transferchartdata", new DataManager(_reminderRepo).GetRemindersData(reminderUser, authParms)));
            return Ok(data);
        }
    }

}
