using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Entity.Base_SysManage.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.BaseSysManage.Controllers
{
    using SiliconValley.InformationSystem.Business;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Util;

    public class Base_UserRoleController : Controller
    {


        private readonly Base_UserBusiness db_user;

        public Base_UserRoleController()
        {
            db_user = new Base_UserBusiness();
        }


        // GET: BaseSysManage/Base_UserRole
        public ActionResult RoleIndex()
        {
            return View();
        }
        

        /// <summary>
        /// 账号页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountIndex()
        {
            return View();
        }

        public ActionResult AccountData(int page, int limit, string empname, string empnumber)
        {

            List<Base_User> userlist = new List<Base_User>();
            if (empname == null && empnumber != null)
            {
               var templist = db_user.GetList().Where(d => d.EmpNumber == empnumber).ToList();

                if (templist != null)
                {
                    userlist.AddRange(templist);
                }

            }

            if (empnumber == null && empname != null)
            {

                ///根据员工名称查询
                ///

                var templist = db_user.GetList().ToList();

                foreach (var item in templist)
                {
                   var tempuser = db_user.ConvetToView(item);

                    if (tempuser != null && tempuser.Emp.EmpName.Contains(empname))
                    {
                        userlist.Add(item);
                    }
                }

            }

            if (empname == null && empnumber == null)
            {
               var templist = db_user.GetList().ToList();

                userlist.AddRange(templist);
            }

            var skiplist = userlist.Skip((page - 1) * limit).Take(limit).ToList();

            ///转为模型视图
            ///

            List<AccountView> accountlist = new List<AccountView>();

            foreach (var item in userlist)
            {
               var tempobj = db_user.ConvetToView(item);

                if (tempobj != null)
                {
                    accountlist.Add(tempobj);
                }
            }


            var obj = new {

                code = 0,
                msg = "",
                count= userlist.Count,
                data = accountlist

            };
            return Json(obj, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 创建账号视图
        /// </summary>
        /// <returns></returns>
        public ActionResult createAccount()
        {
            return View();
        }

        public ActionResult selectEmp()
        {
            //提供部门数据
            BaseBusiness<Department> dbdep = new BaseBusiness<Department>();

            ViewBag.Deplist = dbdep.GetList().Where(d => d.IsDel == false).ToList();
            
            return View();
        }


        /// <summary>
        /// 员工数据
        /// </summary>
        /// <returns></returns>
        public ActionResult empData(int limit, int page, int depid)
        {

            EmployeesInfoManage dbemp = new EmployeesInfoManage();

            List<EmployeesInfo> emplist = new List<EmployeesInfo>();

            if (depid == 0)
            {
                //获取所有员工

               emplist.AddRange( dbemp.GetAll());
                
            }
            else
            {
                //根据部门帅选
                emplist.AddRange(dbemp.GetEmpsByDeptid(depid));

            }

            var skiplist = emplist.Skip((page - 1) * limit).Take(limit).ToList() ;

            List<EmpDetailView> viewlist = new List<EmpDetailView>();

            SiliconValley.InformationSystem.Business.TeachingDepBusiness.TeacherBusiness dbteacher = new Business.TeachingDepBusiness.TeacherBusiness();

            foreach (var item in skiplist)
            {
                var tempobj = dbteacher.ConvertToEmpDetailView(item);

                if (tempobj != null)
                    viewlist.Add(tempobj);
            }

            var obj = new {

                code = 0,
                msg = "",
                count = emplist.Count,
                data = viewlist

            };

            return Json(obj, JsonRequestBehavior.AllowGet);
            

        }


        /// <summary>
        /// 创建账号
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="empnumber">员工编号</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult createAccount(string username, string empnumber)
        {
            AjaxResult result = new AjaxResult();


            try
            {
                db_user.createAccount(username, empnumber);

                result.ErrorCode = 200;
                result.Data = null;
                result.Msg = "正常!";
            }
            catch (Exception ex)
            {
                if (ex.Message == "该用户名已存在！")
                {
                    result.ErrorCode = 444;
                    result.Data = null;
                    result.Msg = "用户名重复!";
                }
                else
                {
                    result.ErrorCode = 500;
                    result.Data = null;
                    result.Msg = "服务器异常!";
                }

                
            }

            return Json(result, JsonRequestBehavior.AllowGet);


        }
    }
}