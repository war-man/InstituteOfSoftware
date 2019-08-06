using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality
{
    public class TeachingqualityAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Teachingquality";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Teachingquality_default",
                "Teachingquality/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}