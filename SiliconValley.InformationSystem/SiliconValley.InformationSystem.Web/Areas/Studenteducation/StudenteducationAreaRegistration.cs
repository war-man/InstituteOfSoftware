using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Studenteducation
{
    public class StudenteducationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Studenteducation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Studenteducation_default",
                "Studenteducation/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}