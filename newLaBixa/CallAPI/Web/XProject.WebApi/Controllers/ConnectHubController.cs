using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace XProject.WebApi.Controllers
{
    [Route("ConnectHub")]
    public class ConnectHubController : ControllerBase
    {
        [Route("Send")]
        [HttpPost]
        public async Task<IActionResult> IndexAsync()
        {
            using (var hubConnection = new HubConnection("http://45.119.82.72:8011/signalr"))
            {
                IHubProxy chatHubProxy = hubConnection.CreateHubProxy("chathub");
                //chatHubProxy.On("Send", message => Console.WriteLine("hello from xproject"));
                await hubConnection.Start();
                await chatHubProxy.Invoke("send", new { name = "108", message = "Hello from xproject" });
                //hubConnection.Stop();
            }
            return Ok("Success");
        }
    }
}
