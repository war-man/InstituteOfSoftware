using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    using SiliconValley.InformationSystem.Business.SchoolAttendanceManagementBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Util;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;

    public class ApprovalManagementController : Controller
    {
        // GET: Personnelmatters/ApprovalManagement
        public ActionResult ApprovalIndex()//审批管理
        {
            return View();
        }
        //根据编号获取某员工信息
        public ActionResult GetEmpid(string id) {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var emp = empmanage.GetEntity(id);
            var empobj = new {
                emp.EmployeeId,
                emp.EmpName,
                emp.EntryTime,
                 dname=empmanage.GetDept(emp.PositionId).DeptName,
                 pname=empmanage.GetPosition(emp.PositionId).PositionName,
            };
            return Json(empobj, JsonRequestBehavior.AllowGet);
        }

        //转正申请
        public ActionResult PositiveApply() {
            // string eid = Session["loginname"].ToString();//填写申请的员工即当前登录的员工
            string eid = "201908150002";//为测试，暂时设置的死数据
            ViewBag.eid = eid;
            return View();
        }
        /// <summary>
        /// 转正申请提交
        /// </summary>
        /// <param name="affm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PositiveApply(ApplyForFullMember affm) {
            ApplyForFullMemberManage amanage = new ApplyForFullMemberManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                affm.ApplicationDate = DateTime.Now;           
                affm.IsDel = true;//表示申请单默认未通过状态
                amanage.Insert(affm);
                AjaxResultxx = amanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = amanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }
    }
}