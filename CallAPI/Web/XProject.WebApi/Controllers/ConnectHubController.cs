using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace XProject.WebApi.Controllers
{
    [Route("ConnectHub")]
    public class ConnectHubController : ControllerBase
    {
        private readonly ILogger _logger;

        public ConnectHubController()
        {
            _logger = Log.Logger;
        }
        [Route("Send")]
        [HttpPost]
        public async Task<IActionResult> IndexAsync()
        {
            using (var hubConnection = new HubConnection("http://45.119.82.72:8011/signalr"))
            {
                IHubProxy chatHubProxy = hubConnection.CreateHubProxy("ChatHub");
                //chatHubProxy.On("Send", message => Console.WriteLine("hello from xproject"));
                await hubConnection.Start();
                try
                {
                await chatHubProxy.Invoke("Send","108","Hello from xproject");

                }
                catch (Exception ex)
                {

                    _logger.Information($"Request send message = {ex.Message}");
                    _logger.Information($"Request send message = {ex.Data}");

                }
                //hubConnection.Stop();
            }
            return Ok("Success");
        }
    }
}
