using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Entity.Base_SysManage.ViewEntity;
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
        public ActionResult Index(string code)
        {
            QQ_callback callback = new QQ_callback();

            Weixin_info userinfo = callback.getWeixinUserInfoJSON(code);

            //匹配账号

            


            return View();
        }
    }
}