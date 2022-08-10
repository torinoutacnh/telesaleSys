using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XProject.Core.Configs;

namespace XProject.WebApi.Controllers
{
    [Route("CallV1")]
    public class AutoCallV1Controller : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        public AutoCallV1Controller(IConfiguration config)
        {
            _logger = Log.Logger;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync([FromBody] CallInfo model)
        {
            var values = new Dictionary<string, string>
            {
                { "ServiceName", SystemSettingModel.CloudPhone.ServiceName },
                { "AuthUser", SystemSettingModel.CloudPhone.AuthUser },
                { "AuthKey", SystemSettingModel.CloudPhone.AuthKey },
                { "Prefix", "0" },
                { "Ext", model.Ext },
                { "PhoneName", "HMC" },
                { "PhoneNumber", model.PhoneNumber},
                { "KeySearch", Guid.NewGuid().ToString("N")}
            };

            _logger.Information($"Request CallV1 = {values}");

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://api.cloudfone.vn/api/CloudFone/AutoCall", content);

            var responseString = await response.Content.ReadAsStringAsync();

            _logger.Information($"CallV1 = {responseString}");

            return Ok(responseString);
        }

        [Route("GetCalls")]
        [HttpPost]
        public async Task<IActionResult> GetCalls([FromBody] string key)
        {
            Datum model = new Datum();
            model.key = key;
            if (model.start == "")
            {
                model.start = DateTime.Now.ToString("MM-dd-yyyy");
            }
            if (model.end == "")
            {
                model.end = DateTime.Now.ToString("MM-dd-yyyy");
            }

            var values = new Dictionary<string, string>
            {
                { "ServiceName", SystemSettingModel.CloudPhone.ServiceName },
                { "AuthUser", SystemSettingModel.CloudPhone.AuthUser },
                { "AuthKey", SystemSettingModel.CloudPhone.AuthKey },
                { "TypeGet", model.typeget },
                { "DateStart", model.start },
                { "DateEnd", model.end },
                { "CallNum", model.callnum },
                { "ReceiveNum", model.receivenum},
                { "Key", model.key},
                { "PageIndex", model.pageindex.ToString() },
                { "PageSize", model.pagesize.ToString() }
            };

            string request = $"\"ServiceName\":CF - PBX0001442,\"AuthUser\":ODS000953,\"AuthKey\":d8e8990b - 60c6 - 4b50 - 9fac - f766078a3f19,\"TypeGet\":{model.typeget},\"DateStart\":{model.start},\"DateEnd\":{model.end},\"CallNum\":{model.callnum},\"ReceiveNum\":{model.receivenum},\"key\":{model.key},\"PageIndex\":{model.pageindex},\"PageSize\":{model.pagesize}";

            _logger.Information($"Request Info CallV1 = {request}");

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://api.cloudfone.vn/api/CloudFone/GetCallHistory", content);

            var responseString = await response.Content.ReadAsStringAsync();

            _logger.Information($"Info CallV1 = {responseString}");

            return Ok(responseString);
        }

        [Route("Popup")]
        [HttpPost]
        public IActionResult GetPopUp(string ApiKey, string CallNumber, string CallName, string ReceiptNumber, string Key, string KeyRinging, string Status, string Direction, string NumberPBX, string Message, string Data)
        {
            string response = $"\"ApiKey\":{ApiKey},\"CallNumber\":{CallNumber},\"CallName\":{CallName},\"ReceiptNumber\":{ReceiptNumber},\"Key\":{Key},\"KeyRinging\":{KeyRinging},\"Status\":{Status}\"Direction\":{Direction},\"NumberPBX\":{NumberPBX},\"Message\":{Message},\"Data\":{Data}";

            _logger.Information($"Info Popup = {response}");
            return Ok();
        }
        [Route("Popup2")]
        [HttpPost]
        public async Task<IActionResult> GetPopUp2([FromBody] Root model)
        {
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
            var modelJson = JsonConvert.SerializeObject(model);
            _logger.Information($"Info popup Key = {modelJson}\n");

            #region chat SignalR
            var signalR_url = _config.GetValue<string>(
                "SignalR-config:hub_connection");

            using (var hubConnection = new HubConnection(signalR_url))
            {
                IHubProxy chatHubProxy = hubConnection.CreateHubProxy("ChatHub");
                //chatHubProxy.On("Send", message => Console.WriteLine("hello from xproject"));
                await hubConnection.Start();
                try
                {
                    if (model.Direction.ToLower().Equals("outbound"))
                    {
                        await chatHubProxy.Invoke("Send", model.CallNumber, modelJson);
                    }
                    else
                    {
                        await chatHubProxy.Invoke("Send", model.CallNumber, modelJson);

                    }

                }
                catch (Exception ex)
                {

                    _logger.Information($"Request send message = {ex.Message}");

                }
                //hubConnection.Stop();
            }
            #endregion
            return Ok(model);
        }
        [Route("Popup3")]
        [HttpPost]
        public IActionResult GetPopUp3([FromHeader] Root model)
        {

            _logger.Information($"Info status = {model.Status}");
            _logger.Information($"Info ReceiptNumber = {model.ReceiptNumber}");
            return Ok(model);
        }
        //public async Task SignalR_ChatAsync(PopupModel model)
        //{
        //   // var urlSignalR = _config.GetValue<string>(
        //      //  "SignalR-config:hub_connection");

        //}
    }
    public class CallInfo
    {
        public string Ext { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class Data
    {
        public string TotalTimeCall { get; set; }
        public string RealTimeCall { get; set; }
        public string LinkFile { get; set; }
    }

    public class Root
    {
        public string ApiKey { get; set; }
        public string CallNumber { get; set; }
        public string CallName { get; set; }
        public string ReceiptNumber { get; set; }
        public string Key { get; set; }
        public string KeyRinging { get; set; }
        public string Status { get; set; }
        public string Direction { get; set; }
        public string NumberPBX { get; set; }
        public string Message { get; set; }
        public Data Data { get; set; }
    }
    public class Datum
    {
        public Datum()
        {
            start = "";
            end = "";
            typeget = "0";
            callnum = "";
            receivenum = "";
            key = "";
            pageindex = 1;
            pagesize = 200;
        }
        public string start { get; set; }
        public string end { get; set; }
        public string typeget { get; set; }
        public string callnum { get; set; }
        public string receivenum { get; set; }
        public string key { get; set; }
        public int pageindex { get; set; }
        public int pagesize { get; set; }

    }

}
