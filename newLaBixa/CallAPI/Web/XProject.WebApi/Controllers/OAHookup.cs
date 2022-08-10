using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using XProject.Repository.Models;

namespace XProject.WebApi.Controllers
{
    [Route("api")]

    public class OAHookup : ControllerBase
    {

        private readonly ILogger _logger;
        public OAHookup()
        {
            _logger = Log.Logger;

        }
        public string Hook([FromBody] JsonResult model,[FromHeader] object header)
        {
            _logger.Information("Hook");
            _logger.Information(model.ToString());
            _logger.Information(header.ToString());
            return "";
        }
    }
}
