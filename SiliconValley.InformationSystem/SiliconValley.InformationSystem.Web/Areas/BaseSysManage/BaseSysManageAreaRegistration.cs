using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.BaseSysManage
{
    public class BaseSysManageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BaseSysManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BaseSysManage_default",
                "BaseSysManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}