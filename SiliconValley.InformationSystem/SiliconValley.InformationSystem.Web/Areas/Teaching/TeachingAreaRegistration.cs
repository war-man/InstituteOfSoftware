using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching
{
    public class TeachingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Teaching";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Teaching_default",
                "Teaching/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}