using System.Web.Mvc;

namespace Labixa.Areas.Autocall
{
    public class AutocallAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Autocall";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Autocall_default",
                "Autocall/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}