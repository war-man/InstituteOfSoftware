using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.MyEducation
{
    public class MyEducationAreaRegistration : AreaRegistration 
    {
        //学历管理
        public override string AreaName 
        {
            get 
            {
                return "MyEducation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MyEducation_default",
                "MyEducation/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}