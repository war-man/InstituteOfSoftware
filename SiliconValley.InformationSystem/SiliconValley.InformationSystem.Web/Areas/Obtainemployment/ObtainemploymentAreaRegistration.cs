using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment
{
    public class ObtainemploymentAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Obtainemployment";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Obtainemployment_default",
                "Obtainemployment/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}