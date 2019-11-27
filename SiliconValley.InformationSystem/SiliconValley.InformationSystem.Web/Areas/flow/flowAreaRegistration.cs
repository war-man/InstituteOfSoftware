using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.flow
{
    public class flowAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "flow";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "flow_default",
                "flow/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}