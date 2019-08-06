using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters
{
    public class PersonnelmattersAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Personnelmatters";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Personnelmatters_default",
                "Personnelmatters/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}