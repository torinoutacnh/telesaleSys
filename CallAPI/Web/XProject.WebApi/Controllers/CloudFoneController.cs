using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace XProject.WebApi.Controllers
{
    [Route("Call")]
    public class CloudFoneController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly ILogger _logger;

        public CloudFoneController()
        {
            _logger = Log.Logger;
        }

        [Route("ClickToCallV1")]
        [HttpPost]
        public async Task<IActionResult> CallV1(string prefix, string ext, string PhoneNumber)
        {
            string keysearch = $"CallV1-{DateTime.Now.Day}-{DateTime.Now.Month}-";
            var values = new Dictionary<string, string>
            {
                { "ServiceName", "CF-PBX0001442" },
                { "AuthUser", "ODS000953" },
                { "AuthKey", "d8e8990b-60c6-4b50-9fac-f766078a3f19" },
                { "Prefix", prefix },
                { "Ext", ext },
                { "PhoneName", "HMC" },
                { "PhoneNumber", PhoneNumber },
                { "KeySearch", keysearch+PhoneNumber}
            };

            string request = $"\"ServiceName\":CF - PBX0001442,\"AuthUser\":ODS000953,\"AuthKey\":d8e8990b - 60c6 - 4b50 - 9fac - f766078a3f19,\"Prefix\":{prefix},\"Ext\":{ext},\"PhoneNumber\":{PhoneNumber},\"KeySearch\":{keysearch + PhoneNumber}";

            _logger.Information($"Request ClickToCallV1 = {request}");

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://api.cloudfone.vn/api/CloudFone/AutoCall", content);

            var responseString = await response.Content.ReadAsStringAsync();

            _logger.Information($"Response ClickToCallV1 = {responseString}");

            return Ok(responseString);
        }

        [Route("ClickToCallV2")]
        [HttpPost]
        public async Task<IActionResult> CallV2(string ext, string phone, string headphone = "")
        {
            string keysearch = $"CallV2-{DateTime.Now.Day}-{DateTime.Now.Month}-";
            if (headphone == "")
                headphone = "GTE842899998844";
            var PhoneNumber = headphone + "/" + phone;

            var values = new Dictionary<string, string>
            {
                { "ServiceName", "CF-PBX0001442" },
                { "AuthUser", "ODS000953" },
                { "AuthKey", "d8e8990b-60c6-4b50-9fac-f766078a3f19" },
                { "Ext", ext },
                { "PhoneNumber", PhoneNumber },
                { "KeySearch", keysearch+PhoneNumber}
            };

            string request = $"\"ServiceName\":CF - PBX0001442,\"AuthUser\":ODS000953,\"AuthKey\":d8e8990b - 60c6 - 4b50 - 9fac - f766078a3f19,\"Ext\":{ext},\"PhoneNumber\":{PhoneNumber},\"KeySearch\":{keysearch + PhoneNumber}";
            _logger.Information($"Request ClickToCallV2 = {request}");

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://api.cloudfone.vn/api/CloudFone/AutoCallV2", content);

            var responseString = await response.Content.ReadAsStringAsync();

            _logger.Information($"Response ClickToCallV2 = {responseString}");

            return Ok(responseString);
        }

        [Route("Popup")]
        [HttpPost]
        public async Task<IActionResult> GetPopUp(string ApiKey = "\"\"", string CallNumber = "\"\"", string CallName = "\"\"", string ReceiptNumber = "\"\"", string Key = "\"\"", string KeyRinging = "\"\"", string Status = "\"\"", string Direction = "\"\"", string NumberPBX = "\"\"", string Message = "\"\"", string Data = "\"\"")
        {
            string response = $"\"ApiKey\":{ApiKey},\"CallNumber\":{CallNumber},\"CallName\":{CallName},\"ReceiptNumber\":{ReceiptNumber},\"Key\":{Key},\"KeyRinging\":{KeyRinging},\"Status\":{Status}\"Direction\":{Direction},\"NumberPBX\":{NumberPBX},\"Message\":{Message},\"Data\":{Data}";

            _logger.Information($"Info Popup = {response}");
            return Ok();
        }

        [Route("GMSSMS")]
        [HttpPost]
        public async Task<IActionResult> GetGMS_SMS(string GMSName, string start = "", string end = "")
        {
            string keysearch = $"CallV2-{DateTime.Now.Day}-{DateTime.Now.Month}-";
            if (start == "")
            {
                start = DateTime.Now.ToString("MM-dd-yyyy");
            }
            if (end == "")
            {
                end = DateTime.Now.ToString("MM-dd-yyyy");
            }

            var values = new Dictionary<string, string>
            {
                { "AuthUser", "ODS000953" },
                { "AuthKey", "d8e8990b-60c6-4b50-9fac-f766078a3f19" },
                { "GSMName", GMSName },
                { "DateStart", start },
                { "DateEnd", end },
                { "PageIndex", "1" },
                { "PageSize", "200" }
            };

            string request = $"\"AuthUser\":ODS000953,\"AuthKey\":d8e8990b - 60c6 - 4b50 - 9fac - f766078a3f19,\"GSMName\":{GMSName},\"DateStart\":{start},\"DateEnd\":{end},\"PageIndex\":1,\"PageSize\":200";

            _logger.Information($"Request GMS-SMS = {request}");

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://api.cloudfone.vn/api/CloudFone/GSMGetSMSMessage", content);

            var responseString = await response.Content.ReadAsStringAsync();

            _logger.Information($"Response GMS-SMS = {responseString}");

            return Ok(responseString);
        }


        [Route("GetCalls")]
        [HttpPost]
        public async Task<IActionResult> GetCalls([FromBody] Datum model)
        {
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
                { "ServiceName", "CF-PBX0001442" },
                { "AuthUser", "ODS000953" },
                { "AuthKey", "d8e8990b-60c6-4b50-9fac-f766078a3f19" },
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
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
   


}
