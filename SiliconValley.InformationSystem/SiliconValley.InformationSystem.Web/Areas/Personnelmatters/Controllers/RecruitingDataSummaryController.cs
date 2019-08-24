using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    using SiliconValley.InformationSystem.Business.RecruitingDataSummaryBusiness;
    using SiliconValley.InformationSystem.Business.TalentDemandPlanBusiness;
    using SiliconValley.InformationSystem.Business.PositionBusiness;
    using SiliconValley.InformationSystem.Business.DepartmentBusiness;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Business.RecruitPhoneTraceBusiness;

    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Util;
    using System.Text;
    using System.IO;
    using SiliconValley.InformationSystem.Business.Common;
    using SiliconValley.InformationSystem.Entity.Base_SysManage;
    using System.Data;

    public class RecruitingDataSummaryController : Controller
    {
        // GET: Personnelmatters/RecruitingDataSummary
        public ActionResult RecruitIndex()
        {
            return View();
        }
        //获取人才需求计划
        public ActionResult GetTalantDemandData(int page, int limit) {
            TalentDemandPlanManage tdpmanage = new TalentDemandPlanManage();
            var list = tdpmanage.GetList();
            var mytdplist = list.OrderBy(r => r.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var newlist = from tdp in mytdplist
                          select new {
                              tdp.Id,
                              dname = GetDept((int)tdp.DeptId).DeptName,
                              pname = GetPosition((int)tdp.Pid).PositionName,
                              tdp.DemandPersonNum,
                              tdp.PlanEntryTime,
                              tdp.PositionStatement,
                              tdp.PositionRequest,
                              tdp.RecruitReason,
                              empname = GetEmp(tdp.EmployeeId).EmpName,
                              tdp.ApplyTime,
                              tdp.Remark,
                              tdp.IsDel
                          };
            var newobj = new
            {
                code = 0,
                msg = "",
                count = list.Count(),
                data = newlist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }

        //获取招聘电话追踪数据
        public ActionResult GetTraceData(int page, int limit)
        {
            RecruitPhoneTraceManage rptmanage = new RecruitPhoneTraceManage();
            var rdslist = rptmanage.GetList();
            var myrdslist = rdslist.OrderBy(r => r.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var newlist = from rds in myrdslist
                          select new
                          {
                              rds.Id,
                              rds.Name,
                              pname = GetPosition((int)rds.Pid).PositionName,
                              rds.PhoneNumber,
                              rds.TraceTime,
                              rds.Channel,
                              rds.ResumeType,
                              rds.PhoneCommunicateResult,
                              rds.FirstInterviewDate,
                              rds.IsInterview,
                              rds.FirstInterviewResult,
                              rds.RetestDate,
                              rds.RetestResult,
                              rds.OfferGiveTime,
                              rds.PlanEntryTime,
                              rds.IsEntry,
                              rds.Remark
                          };
            var newobj = new
            {
                code = 0,
                msg = "",
                count = rdslist.Count(),
                data = newlist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }

        //获取月度招聘数据汇总
        public ActionResult GetRecruitData(int page, int limit) {
            AddRecruitData();
            RecruitingDataSummaryManage rdsmanage = new RecruitingDataSummaryManage();
            var rdslist = rdsmanage.GetList();
            var myrdslist = rdslist.OrderBy(r => r.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var newobj = new
            {
                code = 0,
                msg = "",
                count = rdslist.Count(),
                data = rdslist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 获取部门对象
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public Department GetDept(int deptid) {
            DepartmentManage dmanage = new DepartmentManage();
            var deptobj = dmanage.GetEntity(deptid);
            return deptobj;
        }

        /// <summary>
        ///获取所属岗位对象
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Position GetPosition(int pid)
        {
            PositionManage pmanage = new PositionManage();
            var str = pmanage.GetEntity(pid);
            return str;
        }

        /// <summary>
        /// 获取员工（人才需求计划表-负责人）
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public EmployeesInfo GetEmp(string empid) {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var emp = empmanage.GetEntity(empid);
            return emp;
        }

        /// <summary>
        /// 根据部门名称获取部门编号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetDeptidByName(string name) {
            DepartmentManage dmanage = new DepartmentManage();
            var dept = dmanage.GetList().Where(s => s.DeptName == name).FirstOrDefault();
            return dept.DeptId;
        }
        /// <summary>
        /// 根据岗位名称获取岗位编号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetPidByName(string name) {
            PositionManage pmanage = new PositionManage();
            var pid = pmanage.GetList().Where(p => p.PositionName == name).FirstOrDefault().Pid;
            return pid;
        }
        /// <summary>
        /// 根据员名称获取员工编号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetEmpidByName(string name) {
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var empid = emanage.GetList().Where(e => e.EmpName == name).FirstOrDefault().EmployeeId;
            return empid;
        }


        //添加人才需求计划
        public ActionResult AddTalantDemand() {
            return View();
        }
        //人才需求添加页绑定负责人属性下拉框
        public ActionResult BindEmpSelect() {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var emp = empmanage.GetList();
            var newobj = new {
                code = 0,
                msg = "",
                count = emp.Count(),
                data = emp
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddTalantDemand(TalentDemandPlan tdp) {
            TalentDemandPlanManage tdpmanage = new TalentDemandPlanManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                tdp.IsDel = false;
                tdpmanage.Insert(tdp);
                AjaxResultxx = tdpmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = tdpmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //人才需求计划的是否完成招聘属性修改
        public ActionResult EditTdpIsdel(int id, bool isdel)
        {
            TalentDemandPlanManage tdpmanage = new TalentDemandPlanManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var tdp = tdpmanage.GetEntity(id);
                tdp.IsDel = isdel;
                tdpmanage.Update(tdp);
                AjaxResultxx = tdpmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = tdpmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //导入excel文件显示页面
        public ActionResult AddExcelFile() {
            return View();
        }
        //获取excel里的文件
        public List<TalentDemandPlan> GetExcelFile() {
            List<TalentDemandPlan> tdplist = new List<TalentDemandPlan>();
            string namef = SessionHelper.Session["filename"].ToString();
               var t = AsposeOfficeHelper.ReadExcel(namef, false);
            foreach (DataRow item in t.Rows)
            {
                TalentDemandPlan tdp = new TalentDemandPlan();
                tdp.DeptId = GetDeptidByName(item["部门"].ToString());
                tdp.Pid = GetPidByName(item["岗位名称"].ToString());
                tdp.DemandPersonNum = Convert.ToInt32(item["需求人数"]);
                tdp.EmployeeId = GetEmpidByName(item["负责人"].ToString());
                tdp.ApplyTime = Convert.ToDateTime(item["需求申请时间"]);
                tdp.PlanEntryTime = Convert.ToDateTime(item["预计入职时间"]);
                tdp.PositionStatement = item["岗位职责"].ToString();
                tdp.PositionRequest = item["岗位要求"].ToString();
                tdp.RecruitReason =item["招聘原因"].ToString();
                tdp.IsDel = Convert.ToBoolean(item["是否完成招聘"]);
                tdp.Remark = item["备注"].ToString();
                tdplist.Add(tdp);

            }
            return tdplist;
        }
        //一个删除文件的方法
        public void DeleteFile()
        {
            var namef = SessionHelper.Session["filename"];
            if (namef != null)
            {
                FileInfo fi = new FileInfo(namef.ToString());
                bool ishave = fi.Exists;
                if (ishave)
                {
                    fi.Delete();
                }
            }
        }
        //显示出上传过来的excel文件
        public ActionResult ShowUploadFile() {
            StringBuilder ProName = new StringBuilder();
            try
            {
                HttpPostedFileBase file = Request.Files["file"];
                string fname = Request.Files["file"].FileName; //获取上传文件名称（包含扩展名）
                string f = Path.GetFileNameWithoutExtension(fname);//获取文件名称
                string name = Path.GetExtension(fname);//获取扩展名
                string pfilename = AppDomain.CurrentDomain.BaseDirectory + "uploadXLSXfile/TalantDemandPlanfile/";//获取当前程序集下面的uploads文件夹中的excel文件夹目录
                string completefilePath = f + DateTime.Now.ToString("yyyyMMddhhmmss") + name;//将上传的文件名称转变为当前项目名称 
                ProName.Append(Path.Combine(pfilename, completefilePath));//合并成一个完整的路径;
                file.SaveAs(ProName.ToString());//上传文件   
                SessionHelper.Session["filename"] = ProName.ToString();
                List<TalentDemandPlan> tdplist = GetExcelFile();
                if (tdplist.Count > 0)//如果拿到值说明文件格式是可以读取的
                {
                    var mydata = tdplist.Select(s => new
                    {
                        #region
                        s.Id,
                        dname = GetDept((int)s.DeptId).DeptName,
                        pname = GetPosition((int)s.Pid).PositionName,
                        s.DemandPersonNum,
                        s.PlanEntryTime,
                        s.PositionStatement,
                        s.PositionRequest,
                        s.RecruitReason,
                        empname = GetEmp(s.EmployeeId).EmpName,
                        s.ApplyTime,
                        s.Remark,
                        s.IsDel
                        #endregion
                    });
                    var jsondata = new
                    {
                        code = "",
                        msg = "ok",
                        data = mydata,
                    };
                    return Json(jsondata, JsonRequestBehavior.AllowGet);
                }
                else //该文件格式不正确
                {
                    var jsondata = new
                    {
                        code = "",
                        msg = "文件格式错误",
                        data = "",
                    };
                    DeleteFile();//如果格式不符合规范则删除上传的文件
                    return Json(jsondata, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ee)
            {
                BusHelper.WriteSysLog(ee.Message, EnumType.LogType.上传文件);
                var jsondata = new
                {
                    code = "",
                    msg = ee.Message,
                    data = "",
                };
                return Json(jsondata, JsonRequestBehavior.AllowGet);
            }

        }


        //添加招聘电话追踪信息
        public ActionResult Addrpt() {
            return View();
        }
        //添加招聘电话追踪记录页面绑定岗位属性下拉框
        public ActionResult BindPidSelect()
        {
            PositionManage pmanage = new PositionManage();
            var emp = pmanage.GetList();
            var newobj = new
            {
                code = 0,
                msg = "",
                count = emp.Count(),
                data = emp
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Addrpt(RecruitPhoneTrace rpt) {
            RecruitPhoneTraceManage rmanage = new RecruitPhoneTraceManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                rmanage.Insert(rpt);
                AjaxResultxx = rmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = rmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //招聘电话追踪记录的是否入职属性修改
        public ActionResult EditRptIsentry(int id, bool isdel) {
            RecruitPhoneTraceManage rmanage = new RecruitPhoneTraceManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var rpt = rmanage.GetEntity(id);
                rpt.IsEntry = isdel;
                rmanage.Update(rpt);
                AjaxResultxx = rmanage.Success();

            }
            catch (Exception ex)
            {
                AjaxResultxx = rmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //招聘电话追踪记录的是否面试属性修改
        public ActionResult EditRptIsInterview(int id, bool isdel) {
            RecruitPhoneTraceManage rmanage = new RecruitPhoneTraceManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var rpt = rmanage.GetEntity(id);
                rpt.IsInterview = isdel;
                rmanage.Update(rpt);
                AjaxResultxx = rmanage.Success();

            }
            catch (Exception ex)
            {
                AjaxResultxx = rmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }


        //月度招聘数据汇总添加
        public AjaxResult AddRecruitData() {
            var AjaxResultxx = new AjaxResult();
            TalentDemandPlanManage tdp = new TalentDemandPlanManage();
            RecruitPhoneTraceManage rpt = new RecruitPhoneTraceManage();
            try
            {
               
            }
            catch (Exception)
            {

                throw;
            }
            return AjaxResultxx;
        }

       
    }
}