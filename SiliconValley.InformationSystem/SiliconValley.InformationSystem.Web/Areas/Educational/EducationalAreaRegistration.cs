using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational
{
    public class EducationalAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Educational";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Educational_default",
                "Educational/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}