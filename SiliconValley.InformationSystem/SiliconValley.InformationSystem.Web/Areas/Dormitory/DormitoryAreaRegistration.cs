using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Dormitory
{
    public class DormitoryAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Dormitory";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Dormitory_default",
                "Dormitory/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}