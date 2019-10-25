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
    using SiliconValley.InformationSystem.Business.EducationalBusiness;
    using System.Text;
    using System.IO;
    using SiliconValley.InformationSystem.Business.Common;

    public class ApprovalManagementController : Controller
    {
        // GET: Personnelmatters/ApprovalManagement
        public ActionResult ApprovalIndex()//审批管理
        {
            string eid = "201908220012";//为测试，暂时设置的死数据
            ViewBag.eid = eid;
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
            string eid = "201908230013";//为测试，暂时设置的死数据
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
            string eid = "201908230013";//为测试，暂时设置的死数据
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
            string eid = "201908230013";//为测试，暂时设置的死数据
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
                sra.IsPass = false;
                sra.IsApproval = false;
                sramanage.Insert(sra);
                AjaxResultxx = sramanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = sramanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }


        //加班申请
        public ActionResult OvertimeApply() {
            // string eid = Session["loginname"].ToString();//填写申请的员工即当前登录的员工
            string eid = "201908150003";//为测试，暂时设置的死数据
            ViewBag.eid = eid;
            BeOnDutyManeger bodmanage = new BeOnDutyManeger();
            var typelist = bodmanage.GetList();
            ViewBag.overtimetype = new SelectList(typelist,"Id","TypeName");
            return View();
        }
        //加班申请提交
        [HttpPost]
        public ActionResult OvertimeApply(OvertimeRecord or) {
            OvertimeRecordManage ormanage = new OvertimeRecordManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                or.IsNoDaysOff = false;//默认为调休
                or.IsPassYear = false;//默认未过年限
                or.IsPass = false;//默认审批未通过
                or.IsApproval = false;//默认未审批
                ormanage.Insert(or);
                AjaxResultxx = ormanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = ormanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  //是否可申请调休的验证（当上月有提前调休记录，且还未补班的情况下，不能再调休）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckIsAllowDaysoff(string id) {
            DaysOffManage dfmanage = new DaysOffManage();
            OvertimeRecordManage otrmanage = new OvertimeRecordManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                //计算该员工的审批已通过的作为调休的加班总时间（可调休总时间）
                var sumdaysofftime = otrmanage.GetList().Where(s => s.EmployeeId == id && s.IsPass == true && s.IsNoDaysOff == false).ToList().Sum(s => s.Duration);
                //计算该员工已经调休了的总时间
                var sumdaysoff = dfmanage.GetList().Where(s => s.EmployeeId == id && s.IsPass == true).ToList().Sum(s => s.Duration);
               
                if ((decimal)sumdaysofftime-(decimal)sumdaysoff<0 ) {//表示有提前调休的情况
                    //找到上个月提前调休的记录
                    var lastdaysoff = dfmanage.GetList().LastOrDefault();
                    int lastmonth = DateTime.Parse(lastdaysoff.StartTime.ToString()).Month;
                    int nowmonth = DateTime.Now.Month;
                    if (nowmonth-lastmonth==1) {//表示是上个月
                        AjaxResultxx = dfmanage.Success();
                    }

                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = dfmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);

        }
        //调休申请
        public ActionResult DaysOffApply() {
            // string eid = Session["loginname"].ToString();//填写申请的员工即当前登录的员工
            string eid = "201908150003";//为测试，暂时设置的死数据
            ViewBag.eid = eid;
            return View();
        }
        //调休申请提交
        [HttpPost]
        public ActionResult DaysOffApply(DaysOff leave) {
            DaysOffManage dmanage = new DaysOffManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                if (leave.Image != "undefined")
                {
                    #region 图片上传
                    StringBuilder ProName = new StringBuilder();
                    HttpPostedFileBase file = Request.Files["Image"];
                    string fname = file.FileName; //获取上传文件名称（包含扩展名）
                    string f = Path.GetFileNameWithoutExtension(fname);//获取文件名称
                    string name = Path.GetExtension(fname);//获取扩展名
                    string pfilename = AppDomain.CurrentDomain.BaseDirectory + "uploadXLSXfile/DaysOffImage/";//获取当前程序集下面的uploads文件夹中的文件夹目录

                    string completefilePath = DateTime.Now.ToString("yyyyMMddhhmmss") + name;//将上传的文件名称转变为当前项目名称
                    ProName.Append(Path.Combine(pfilename, completefilePath));//合并成一个完整的路径;
                    file.SaveAs(ProName.ToString());//上传文件   
                    #endregion
                    leave.Image = completefilePath;
                    leave.IsApproval = false;
                    leave.IsPass = false;
                    leave.IsPassYear = false;
                    dmanage.Insert(leave);
                }
                else {
                    leave.Image = null;
                    leave.IsApproval = false;
                    leave.IsPass = false;
                    leave.IsPassYear = false;
                    dmanage.Insert(leave);
                }
                   AjaxResultxx = dmanage.Success(leave);
            }
            catch (Exception ex)
            {
                AjaxResultxx = dmanage.Error(ex.Message);
                BusHelper.WriteSysLog(ex.Message, SiliconValley.InformationSystem.Entity.Base_SysManage.EnumType.LogType.上传文件);
               
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
         
        }


        //请假申请
        public ActionResult LeaveApply() {
            // string eid = Session["loginname"].ToString();//填写申请的员工即当前登录的员工
            string eid = "201908150003";//为测试，暂时设置的死数据
            ViewBag.eid = eid;
            LeaveTypeManage ltype = new LeaveTypeManage();
            var leavetypelist = ltype.GetList();
            ViewBag.leaveType = new SelectList(leavetypelist,"TypeId","TypeName");
           
            return View();
        }

    }
}