using Microsoft.AspNet.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Service;

namespace XProject.Service.JobHandler
{
    public interface IStartJob
    {
        Task Execute();
    }
    public class StartJob : IStartJob
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly IWorking_DailyService _Working_DailyService;
        private readonly IBrandService _BrandService;

        public StartJob(IConfiguration config, IServiceProvider serviceProvider)
        {
            _logger = Log.Logger;
            _config = config;
            _Working_DailyService = serviceProvider.GetRequiredService<IWorking_DailyService>();
            _BrandService = serviceProvider.GetRequiredService<IBrandService>();
        }
        public async Task Execute()
        {
            var listBrand = _BrandService.GetAll();
            // test socket
            #region chat SignalR
            var signalR_url = _config.GetValue<string>(
                "SignalR-config:hub_connection");

            using (var hubConnection = new HubConnection(signalR_url))
            {
                IHubProxy chatHubProxy = hubConnection.CreateHubProxy("ChatHub");
                //chatHubProxy.On("Send", message => Console.WriteLine("hello from xproject"));
                chatHubProxy.On("listInformation", message =>
                {
                    _Working_DailyService.PushQueue(JsonConvert.SerializeObject(message));
                    _logger.Information(JsonConvert.SerializeObject(message));
                });
                await hubConnection.Start();
                //try
                //{
                    foreach (var item in listBrand)
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            var ii = 100 + i;
                            chatHubProxy.Invoke("StatusWork", "", ii.ToString(),"", item.BrandId.ToString()).Wait(10);
                            _logger.Information(item.BrandId.ToString()+"  ***** ");
                            _logger.Information(ii+"  ***** ");

                        }

                    }
                //}
                //catch (Exception ex)
                //{
                //    _logger.Information($"Request send message = {ex.Message}");

                //}
                var a = chatHubProxy.Invoke("GetList").Wait(100);
            }
            //hubConnection.Stop();
        }
        #endregion

    }
}
