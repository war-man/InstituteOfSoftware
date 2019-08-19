using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.CourseSyllabus
{
    public class CourseSyllabusAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CourseSyllabus";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CourseSyllabus_default",
                "CourseSyllabus/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}