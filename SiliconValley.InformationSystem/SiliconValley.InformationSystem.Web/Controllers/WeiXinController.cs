using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Entity.Base_SysManage.ViewEntity;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Controllers
{

    public class WeiXinController : BaseMvcController
    {
        private readonly Base_UserBusiness db_account;

        public WeiXinController()
        {
            db_account = new Base_UserBusiness();
        }

        // GET: WeiXin
        public ActionResult WXLogin(string code)
        {
            QQ_callback callback = new QQ_callback();

            Weixin_info userinfo = callback.getWeixinUserInfoJSON(code);

            //匹配账号 

            var account = db_account.GetList().Where(d => d.WX_Unionid == userinfo.Unionid).FirstOrDefault();

            if (account == null) return View("WXLoginError","login404");

            //记录登录
            SessionHelper.Session["UserId"] = account.UserId;

            var permisslist = PermissionManage.GetOperatorPermissionValues();

            SessionHelper.Session["OperatorPermission"] = permisslist;

            return Redirect("/Base_SysManage/Base_SysMenu/Index");
        }

     
        [CheckLogin]
        [HttpGet]
        public ActionResult BindingWX()
        {

            return View();
        }

        [CheckLogin]
        public ActionResult DoBindingWX(string code)
        {

            QQ_callback callback = new QQ_callback();

            Weixin_info userinfo = callback.getWeixinUserInfoJSON(code);

            var currentUser = Base_UserBusiness.GetCurrentUser();

            if(!string.IsNullOrEmpty(currentUser.WX_Unionid)) return View("WXLoginError", "binding402");

            //存入信息
            currentUser.WX_Unionid = userinfo.Unionid;

            db_account.Update(currentUser);

            //跳转登录

            return RedirectToAction("LoginIndex","Login");

        }

        public ActionResult WXLoginError()
        {

            return View();
        }
    }
}