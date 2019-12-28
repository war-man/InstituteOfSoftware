using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.OrThert
{
    public class OrThertAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OrThert";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "OrThert_default",
                "OrThert/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}