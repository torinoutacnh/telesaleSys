using Invedia.Core.EnvUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using XProject.Contract.Repository.Infrastructure;
using XProject.Contract.Service;
using XProject.Repository.Infrastructure;

namespace XProject.WebApi.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly ICallsHistoryService _callsHistoryService;
        private readonly ILogger _logger;
        public HomeController(IServiceProvider serviceProvider)
        {
            _logger = Log.Logger;
            _callsHistoryService = serviceProvider.GetRequiredService<ICallsHistoryService>();
        }

        public IActionResult Index()
        {
            //var message = "[{\"Name\":\"\",\"Ext\":\"101\",\"Status\":\"\"},{\"Name\":\"\",\"Ext\":\"102\",\"Status\":\"\"},{\"Name\":\"\",\"Ext\":\"103\",\"Status\":\"\"},{\"Name\":\"admin\",\"Ext\":\"104\",\"Status\":\"Online\"},{\"Name\":\"\",\"Ext\":\"105\",\"Status\":\"\"},{\"Name\":\"\",\"Ext\":\"106\",\"Status\":\"\"},{\"Name\":\"\",\"Ext\":\"107\",\"Status\":\"\"},{\"Name\":\"\",\"Ext\":\"108\",\"Status\":\"\"}]";

            //    var myDeserializedClass = JsonConvert.DeserializeObject<List<Staff>>(message);
            //foreach (var item in myDeserializedClass)
            //{
            //    _logger.Information(item.Name+" "+item.Status+" "+item.Ext);
            //}
            var info = $"Service is running normally on {EnvHelper.CurrentEnvironment}...";
            return Ok(info);
        }

        [HttpGet("/version")]
        public ActionResult<string> GetVersion()
        {
            var version = typeof(Program).Assembly.GetName().Version?.ToString();
            _callsHistoryService.GetAll();
            _logger.Information($"Version = {version}");
            return version;
        }
    }
    public class Staff
    {
        public string Name { get; set; }
        public string Ext { get; set; }
        public string Status { get; set; }
    }
}