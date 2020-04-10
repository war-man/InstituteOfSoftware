using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Courseware
{
    public class CoursewareAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Courseware";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Courseware_default",
                "Courseware/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}