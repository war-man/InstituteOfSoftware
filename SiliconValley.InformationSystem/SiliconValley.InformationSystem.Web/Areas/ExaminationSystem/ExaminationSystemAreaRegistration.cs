using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.ExaminationSystem
{
    public class ExaminationSystemAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ExaminationSystem";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ExaminationSystem_default",
                "ExaminationSystem/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}