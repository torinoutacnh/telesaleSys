using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
namespace XProject.WebApi.Controllers
{

    [Route("Hook")]
    public class OAHookup : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public OAHookup(IConfiguration config)
        {
            _logger = Log.Logger;
            _config = config;


        }
        [Route("OAZalo")]
        [HttpPost]
        public string Hook([FromBody] object model, [FromHeader] string X_ZEvent_Signature)
        {
            _logger.Information(X_ZEvent_Signature.ToString());
            X_ZEvent_Signature = Request.Headers["X-ZEvent-Signature"].ToString();
            _logger.Information("Hook");
            _logger.Information(model.ToString());
            _logger.Information(X_ZEvent_Signature.ToString());
            #region đưa vô queue để add vô Database và kiểm tra nếu zaloID chưa có update sdt thì sẽ gửi 1 api gửi yêu cầu cập nhật thông tin qua zalo
            //
            // nếu event_name = user_asking_product
            //     {
            //          "app_id": "360846524894093967",
            //          "product": {
            //          "thumbnail": "image_url_of_product",
            //          "description": "message_from_user",
            //          "url": "url_lead_to_product",
            //          "title": "product_name"
            //              },
            //          "sender": {
            //          "id": "246845883529197922"
            //          },
            //          "recipient": {
            //          "id": "388613280879808645"
            //          },
            //          "event_name": "user_asking_product",
            //          "message": {
            //          "text": "product_name",
            //          "msg_id": "b403772854c7b19e8d7"
            //          },
            //          "timestamp": "1540361303693"
            //      }
            //
            #endregion
            return "";
        }
        [Route("OAZaloV2")]
        [HttpPost]
        public async Task<string> Hook_V2Async([FromBody] JObject obj)
        {
            Dictionary<string, string> headerdata = new Dictionary<string, string>();
            foreach (var item in Request.Headers.Keys)
            {
                headerdata.Add(item, Request.Headers[item]);
            };
            //a = (HeaderDictionary)Request.Headers;
            _logger.Information("Hook");
            _logger.Information(JsonConvert.SerializeObject(headerdata));
            _logger.Information(obj.ToString());
            // _logger.Information(obj["event_name"].ToString());

            

            #region đưa vô queue để add vô Database và kiểm tra nếu zaloID chưa có update sdt thì sẽ gửi 1 api gửi yêu cầu cập nhật thông tin qua zalo
            //
            // nếu event_name = user_asking_product
            //     {
            //          "app_id": "360846524894093967",
            //          "product": {
            //          "thumbnail": "image_url_of_product",
            //          "description": "message_from_user",
            //          "url": "url_lead_to_product",
            //          "title": "product_name"
            //              },
            //          "sender": {
            //          "id": "246845883529197922"
            //          },
            //          "recipient": {
            //          "id": "388613280879808645"
            //          },
            //          "event_name": "user_asking_product",
            //          "message": {
            //          "text": "product_name",
            //          "msg_id": "b403772854c7b19e8d7"
            //          },
            //          "timestamp": "1540361303693"
            //      }
            //
            #endregion
            return "";
        }
    }
}
