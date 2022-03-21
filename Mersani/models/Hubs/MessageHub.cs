using Mersani.models.Users;
using Mersani.Oracle;
using Mersani.Utility;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mersani.models.Hubs
{
    public class MessageHub : Hub
    {
        private static Dictionary<string, string> connections = new Dictionary<string, string>();
        private static Dictionary<string, string> _active_users = new Dictionary<string, string>();
        public MessageHubHelper _msgHelper { get; }
        public MessageHub(MessageHubHelper msgHelper)
        {
            _msgHelper = msgHelper;
        }

        public string GetAuthParms()
        {
            string accesstoken = Context.GetHttpContext().Request.Query["access_token"].ToString(); ;

            Context.GetHttpContext().Request.Headers["Authorization"] = accesstoken;

            string authParms = CustomAuth.getTokenParmsAuthorization(Context.GetHttpContext().Request.HttpContext);

            return authParms;
        }

        public override Task OnConnectedAsync()
        {
            string authParms = GetAuthParms();
            var userId = getUserId(); //OracleDQ.GetAuthenticatedUserObject(secParms)?.UserCode.ToString();
            string userType = getUserType();//OracleDQ.GetAuthenticatedUserObject(secParms)?.UserType == "C" ? "C" : "U";
            //// check for conection
            //this._msgHelper.ConnectUser(userId,Context.ConnectionId,secParms);
            var data = new DataSet();
            if (userId.Length < 9)
            {
                data = _msgHelper.GetUserDetail(userId, authParms, userType).Result;
            }
            if (String.IsNullOrEmpty(connections.GetValueOrDefault($"{userType}_{userId}")))
            {
                connections.Add($"{userType}_{userId}", Context.ConnectionId);
            }
            if (String.IsNullOrEmpty(_active_users.GetValueOrDefault($"{userType}_{userId}")))
            {
                if (data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
                {
                    DataRow userItem = data.Tables[0].Rows[0];
                    string user = userType + ',' + userItem["USR_CODE"].ToString() + ',' + userItem["USR_LOGIN"].ToString() + ',' + userItem["USR_FULL_NAME_AR"].ToString() + ',' + userItem["USR_FULL_NAME_EN"].ToString();
                    _active_users.Add($"{userType}_{userId}", user);
                }
                else
                {
                    string user = userType + ',' + userId + ',' + userId + ',' + userId + ',' + userId;
                    _active_users.Add($"{userType}_{userId}", user);
                }
            }

            GetAllActiveUsers();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string userId = getUserId();// OracleDQ.GetAuthenticatedUserObject(authParms)?.UserCode.ToString();
            string userType = getUserType();//OracleDQ.GetAuthenticatedUserObject(authParms)?.UserType == "C" ? "C" : "U";

            //this._msgHelper.DisConnectUser(userId, Context.ConnectionId, authParms);
            if (!String.IsNullOrEmpty(connections.GetValueOrDefault($"{userType}_{userId}")))
            {
                connections.Remove($"{userType}_{userId}");
            }

            if (!String.IsNullOrEmpty(_active_users.GetValueOrDefault($"{userType}_{userId}")))
            {
                _active_users.Remove($"{userType}_{userId}");
            }

            GetAllActiveUsers();
            return base.OnDisconnectedAsync(exception);
        }

        //return list of all active connections
        public List<string> GetAllActiveConnections()
        {
            return _active_users.Values.ToList();
        }
        // get userID
        public string getUserId()
        {
            var userId = Context.GetHttpContext().Request.Query["userCode"].ToString();
            return userId;
        }

        // get Username 
        public string getUserType()
        {
            var userName = Context.GetHttpContext().Request.Query["userType"].ToString();
            return userName;
        }

        // send general message to all active clients...
        public async Task GeneralMessage(TktChat msg)
        {
            await Clients.All.SendAsync("GeneralMessage", msg);
        }

        // get all active users connected in system
        public Task GetAllActiveUsers()
        {
            return Clients.All.SendAsync("ActiveUsers", GetAllActiveConnections());
        }

        public bool HasKey(object objectToCheck, string key)
        {
            var type = objectToCheck.GetType();
            return type.GetProperty(key) != null;
        }

        public TktChat GenerateValueKindObject(dynamic data)
        {
            //your result
            var responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(data.ToString());
            TktChat msg = new TktChat();
            //change like below
            var d = JsonDocument.Parse(data.ToString());  //JsonDocument.Parse(reader.ReadToEnd())
            var result = d.RootElement.EnumerateObject();
            //foreach (var r in result)
            foreach (var item in responseData)
            {

                //if (r.Value.ValueKind == JsonValueKind.String)
                //{
                //    str = 
                //    //msg[r.Name] = (string)stringValue;
                //}
                //if (r.Value.ValueKind == JsonValueKind.Number)
                //{
                //    val = r.Value.GetDouble();
                //    //msg[r.Name] = (int)stringValue;
                //}

                //if (r.Value.ValueKind == JsonValueKind.Null || r.Value.ValueKind == JsonValueKind.Undefined)
                //{
                //    obj = r.Value.GetDouble();
                //    //msg[r.Name] = stringValue;
                //}

                switch (item.Key)
                {
                    case "TC_SYS_ID":
                        msg.TC_SYS_ID = -1;//Int32.Parse(item.Value.GetDouble());
                        break;
                    case "clientuniqueid":
                        msg.clientuniqueid = item.Value.GetString();
                        break;
                    case "TC_SNDR_TYPE":
                        msg.TC_SNDR_TYPE = item.Value.GetString();
                        break;
                    case "TC_RCVR_TYPE":
                        msg.TC_RCVR_TYPE = item.Value.GetString();
                        break;
                    case "TC_TYPE":
                        msg.TC_TYPE = item.Value.GetString();
                        break;
                    case "TC_DATE":
                        msg.TC_DATE = DateTime.Now;//item.Value.GetString();
                        break;
                    case "TC_MESSAGE":
                        msg.TC_MESSAGE = item.Value.GetString();
                        break;
                    case "TC_ATTACHMENT":
                        msg.TC_ATTACHMENT = item.Value.GetString();
                        break;
                    case "TC_SENDER":
                        msg.TC_SENDER = item.Value.GetString();
                        break;
                    case "TC_RECEIVER":
                        msg.TC_RECEIVER = item.Value != null ? item.Value.GetString() : null;
                        break;
                }
            }

            return msg;

        }

        // send message to one selected user onlyyyyyy as private message
        public async Task SendPrivateMessage(dynamic data)
        {
            TktChat message = new TktChat();
            if (data is TktChat) message = data;
            else message = GenerateValueKindObject(data);
            string authParms = GetAuthParms();
            string userId = getUserId();//OracleDQ.GetAuthenticatedUserObject(authParms)?.UserCode.ToString();
            string userType = getUserType();//OracleDQ.GetAuthenticatedUserObject(authParms)?.UserType == "C" ? "C" : "U";
            UserData user = new UserData();

            string receiver = "";
            if (userType == "C")
            {
                if (message.TC_RECEIVER == null || message.TC_RECEIVER == "")
                {
                    var conn_users = connections.Where(i => i.Key.StartsWith("U_")).ToDictionary(i => i.Key, i => i.Value);
                    if (conn_users.Count > 0)
                    {
                        foreach (string key in OracleDQ.RandomKeys(conn_users).Take(1))
                        {
                            receiver = key;
                        }
                    }
                    else
                    {
                        user = OracleDQ.GetData<UserData>("SELECT * FROM ( SELECT * FROM GAS_USR WHERE USR_ROLE_SYS_ID = 21 ORDER BY dbms_random.VALUE ) WHERE ROWNUM = 1", "", public_: true).First();
                        receiver = $"{message.TC_RCVR_TYPE}_{user.USR_CODE}";
                    }
                }
                else
                {
                    receiver = $"{userType}_{message.TC_RECEIVER}";
                }
            }
            else
            {
                receiver = $"{message.TC_RCVR_TYPE}_{message.TC_RECEIVER}";
            }

            message.TC_RECEIVER = receiver.Substring(2);
            // save your message
            await _msgHelper.saveMessage(message, authParms);

            string currentconnection = connections.GetValueOrDefault(receiver);//message.TC_RECEIVER);

            if (!String.IsNullOrEmpty(currentconnection))
            {
                await Clients.Client(currentconnection).SendAsync("PrivateMessage", message);
            }
            else
            {
                string currconn = connections.GetValueOrDefault($"{message.TC_SNDR_TYPE}_{message.TC_SENDER}");
                await Clients.Client(currconn).SendAsync("PrivateMessage", new TktChat()
                {
                    clientuniqueid = DateTime.Now.ToLongTimeString(),
                    TC_RECEIVER = message.TC_SENDER,
                    TC_SENDER = user.USR_CODE.ToString(),
                    TC_DATE = DateTime.Now,
                    TC_MESSAGE = "No One Of Support Available Rightnow",
                    TC_SNDR_TYPE = "U",
                    TC_RCVR_TYPE = "C",
                    TC_TYPE = "R",
                    TC_SYS_ID = -1,
                    SNDR_CHAR_AR = user.USR_FULL_NAME_AR[0],
                    SNDR_CHAR_EN = user.USR_FULL_NAME_EN[0],
                    TC_ATTACHMENT = null,
                    TC_MSG_TYPE = null
                });
            }
        }
         
    }
}
