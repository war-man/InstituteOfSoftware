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
                emp.Sex,
                emp.EmpName,
                emp.EntryTime,
                emp.Education,
                emp.PositiveDate,
                 dname=empmanage.GetDept(emp.PositionId).DeptName,
                 pname=empmanage.GetPosition(emp.PositionId).PositionName,
                 emp.ProbationSalary,
                 emp.Salary,
            };
            return Json(empobj, JsonRequestBehavior.AllowGet);
        }

        //转正申请
        public ActionResult PositiveApply() {
            // string eid = Session["loginname"].ToString();//填写申请的员工即当前登录的员工
            string eid = "201908150004";//为测试，暂时设置的死数据
            ViewBag.eid = eid;
            return View();
        }
        // 转正申请提交
        [HttpPost]
        public ActionResult PositiveApply(ApplyForFullMember affm) {
            ApplyForFullMemberManage amanage = new ApplyForFullMemberManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                affm.ApplicationDate = DateTime.Now;  //转正申请时间默认当前提交时间         
                affm.IsApproval = false;//默认为未审批状态
                affm.IsPass = false;//表示申请单默认未通过状态
                amanage.Insert(affm);
                AjaxResultxx = amanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = amanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }


        //离职申请
        public ActionResult DimissionApply() {
            // string eid = Session["loginname"].ToString();//填写申请的员工即当前登录的员工
            string eid = "201909040025";//为测试，暂时设置的死数据
            ViewBag.eid = eid;
            return View();
        }
        //离职申请提交
        [HttpPost]
        public ActionResult DimissionApply(DimissionApply da) {
            DimissionApplyManage damanage = new DimissionApplyManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                da.DimissionDate = DateTime.Now;//离职申请时间默认当前提交时间
                da.IsApproval = false;//默认为未审批状态
                da.IsPass = false;//表示申请单默认未通过状态
                damanage.Insert(da);
                AjaxResultxx = damanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = damanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }


        //转岗申请
        public ActionResult TransferPositionApply() {
            // string eid = Session["loginname"].ToString();//填写申请的员工即当前登录的员工
            string eid = "201909040025";//为测试，暂时设置的死数据
            ViewBag.eid = eid;
            return View();
        }
        //转岗提交申请
        [HttpPost]
        public ActionResult TransferPositionApply(JobTransferAppply jta) {
            JobTransferApplyManage jtamanage = new JobTransferApplyManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                jta.ApplicationTime = DateTime.Now;
                jta.IsPass = false;
                jta.IsApproval = false;
                jtamanage.Insert(jta);
                AjaxResultxx = jtamanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = jtamanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }


        //加薪申请
        public ActionResult RaisesApply()
        {
            // string eid = Session["loginname"].ToString();//填写申请的员工即当前登录的员工
            string eid = "201909040025";//为测试，暂时设置的死数据
            ViewBag.eid = eid;
            return View();
        }
        //加薪提交申请
        [HttpPost]
        public ActionResult RaisesApply(SalaryRaiseApply sra)
        {
            SalaryRaiseApplyManage sramanage = new SalaryRaiseApplyManage();
            var AjaxResultxx = new AjaxResult();
            try
            {

                sramanage.Insert(sra);
                AjaxResultxx = sramanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = sramanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

    }
}