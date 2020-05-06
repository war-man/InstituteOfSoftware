﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.UserManeger;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Depository.CellPhoneSMS;

using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Web.Common;
using System.Drawing;
using System.IO;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
namespace SiliconValley.InformationSystem.Web.Controllers
{
    //  /Login/LoginIndex
    [IgnoreLogin]
    public class LoginController : BaseMvcController
    {
        UsersInfoManeger userinfo = new UsersInfoManeger();
        EmployeesInfoManage empmanage = new EmployeesInfoManage();
        // GET: Login
        //登录页面
        public ActionResult LoginIndex()
        {
            return View();
        }
        //登录方法
        public ActionResult LoginFunction(Base_User u, string loginType, string mobile, string smsCaptcha, string code)
        {

            ErrorResult err = new ErrorResult();
            try
            {

                //账号密码登录
                if (loginType == "account")
                {
                    string pwd = Util.Extention.ToMD5String(u.Password);
                    Base_User findu = userinfo.GetList().Where(find => find.UserName == u.UserName && find.Password == pwd).FirstOrDefault();

                    if (findu != null)
                    {
                        SessionHelper sessionHelper = new SessionHelper();

                        SessionHelper.Session["UserId"] = findu.UserId;
                        err.Success = true;
                        err.Msg = "登陆成功!";
                        err.Data = "/Base_SysManage/Base_SysMenu/Index";

                        //获取权限

                        var permisslist = PermissionManage.GetOperatorPermissionValues();

                        SessionHelper.Session["OperatorPermission"] = permisslist;


                        return Json(err, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        err.Msg = "用户名或密码错误";
                    }
                }

                //手机短信验证码登录
                if (loginType == "phone")
                {

                    EmployeesInfo emp = empmanage.GetList().Where(e => e.Phone == mobile).FirstOrDefault();

                    if (emp != null && SessionHelper.Session["code"].ToString() == smsCaptcha)
                    {                      

                        Base_User myuser = userinfo.GetList().Where(user => user.EmpNumber == emp.EmployeeId).FirstOrDefault();

                        SessionHelper.Session["UserId"] = myuser.UserId;
                        err.Success = true;
                        err.Msg = "登陆成功!";
                        err.Data = "/Base_SysManage/Base_SysMenu/Index";

                        //获取权限

                        var permisslist = PermissionManage.GetOperatorPermissionValues();

                        SessionHelper.Session["OperatorPermission"] = permisslist;


                        return Json(err, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        err.Msg = "用户名或密码错误";
                    }
                }
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message, EnumType.LogType.系统异常);
            }
            return Json(err, JsonRequestBehavior.AllowGet);
        }

        //手机短信实例
        public ActionResult PhoneSMS()
        {
            string number = "13204961361";
            string smsText = "达磊，下雨了！！！";
            string t = PhoneMsgHelper.SendMsg(number, smsText);
            return View();
        }

        /// <summary>
        /// 产生手机登录验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCode()
        {
            string code = LoginHelper.Code(4);
            //将验证码存储到Session中
            SessionHelper.Session["code"] = code;
            Bitmap image = LoginHelper.CreateCodeImage(code);
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] bytes = ms.ToArray();
            return File(bytes, "image/Jpeg");
        }
    }
}