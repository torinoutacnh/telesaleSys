using Microsoft.AspNet.SignalR;

using Microsoft.Owin.Cors;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Labixa.Startup))]
namespace Labixa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration { };
                map.RunSignalR(hubConfiguration);
            });
            ConfigureAuth(app);
        }
    }
}
