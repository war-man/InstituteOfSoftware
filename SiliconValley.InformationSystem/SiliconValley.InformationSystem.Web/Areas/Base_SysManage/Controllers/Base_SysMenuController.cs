using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Base_SysManage.Controllers
{
    public class Base_SysMenuController : BaseMvcController
    {
        // GET: Base_SysManage/Base_SysMenu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadMenus()
        {

            //加入登陆的用户是Admin

            //Base_UserBusiness userdb = new Base_UserBusiness();
            //Base_User user = userdb.GetList().Where(u => u.UserId == "Admin").FirstOrDefault();

            //SessionHelper._Session session = new SessionHelper._Session();
            //session["UserId"] = user.UserId;


            List<Menu> menus = SystemMenuManage.GetOperatorMenu();

            return Json(menus, JsonRequestBehavior.AllowGet);
        }
    }
}