using SiliconValley.InformationSystem.Business.Channel;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.Consult_Business;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness;
using SiliconValley.InformationSystem.Business.EmpTransactionBusiness;
using SiliconValley.InformationSystem.Business.FinanceBusiness;
using SiliconValley.InformationSystem.Business.SchoolAttendanceManagementBusiness;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    public class EmpTransactionRecordController : Controller
    {
        // GET: Personnelmatters/EmpTransactionRecord
        public ActionResult EmpTransactionRecordIndex()
        {
            return View();
        }

        //获取员工异动表数据
        public ActionResult GetEtrData(int page, int limit)
        {
            EmpTransactionManage etmanage = new EmpTransactionManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var list = etmanage.GetList().Where(s=>s.IsDel==false).ToList();
            var mylist = list.OrderByDescending(e => e.TransactionId).Skip((page - 1) * limit).Take(limit).ToList();
            var etlist = from e in mylist
                         select new
                         {
                             e.TransactionId,
                             empName = emanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             type = emanage.GetETById(e.TransactionType).MoveTypeName,
                             e.TransactionTime,
                             predname = e.PreviousDept == null ? null : emanage.GetDeptById((int)e.PreviousDept).DeptName,
                             prepname = e.PreviousPosition == null ? null : emanage.GetPobjById((int)e.PreviousPosition).PositionName,
                             nowdname = e.PresentDept == null ? null : emanage.GetDeptById((int)e.PresentDept).DeptName,
                             nowpname = e.PresentPosition == null ? null : emanage.GetPobjById((int)e.PresentPosition).PositionName,
                             e.Remark,
                             e.PreviousSalary,
                             e.PresentSalary,
                             e.Reason,
                             e.IsDel
                         };
            var newobj = new
            {
                code = 0,
                msg = "",
                count = list.Count(),
                data = etlist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);


        }
        //编辑员工异动信息      
        public ActionResult EditETR(int id)
        {
            EmpTransactionManage etmanage = new EmpTransactionManage();
            var et = etmanage.GetEntity(id);
            ViewBag.id = id;
            return View(et);
        }
        //根据编号获取异动对象信息
        public ActionResult GetertById(int id)
        {
            EmpTransactionManage etmanage = new EmpTransactionManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var e = etmanage.GetEntity(id);
            var empobj = new
            {
                #region 获取属性值 
                e.TransactionId,
                empName = emanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                esex = emanage.GetInfoByEmpID(e.EmployeeId).Sex,
                dname = emanage.GetDept(emanage.GetInfoByEmpID(e.EmployeeId).PositionId).DeptName,
                pname = emanage.GetPosition(emanage.GetInfoByEmpID(e.EmployeeId).PositionId).PositionName,
                EntryTime = emanage.GetInfoByEmpID(e.EmployeeId).EntryTime,
                education = emanage.GetInfoByEmpID(e.EmployeeId).Education,
                positiveDate = emanage.GetInfoByEmpID(e.EmployeeId).PositiveDate,
                type = emanage.GetETById(e.TransactionType).MoveTypeName,
                e.TransactionTime,
                predname = e.PreviousDept == null ? null : emanage.GetDeptById((int)e.PreviousDept).DeptName,
                prepname = e.PreviousPosition == null ? null : emanage.GetPobjById((int)e.PreviousPosition).PositionName,
                nowdname = e.PresentDept == null ? null : emanage.GetDeptById((int)e.PresentDept).DeptName,
                nowpname = e.PresentPosition == null ? null : emanage.GetPobjById((int)e.PresentPosition).PositionName,
                e.Remark,
                e.PreviousSalary,
                e.PresentSalary,
                e.Reason,
                e.IsDel
                #endregion
            };

            return Json(empobj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditETR(EmpTransaction et)
        {
            EmpTransactionManage etmanage = new EmpTransactionManage();
            var ajaxresult = new AjaxResult();
            var e = etmanage.GetEntity(et.TransactionId);
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            MoveTypeManage mt = new MoveTypeManage();

            try
            {
                e.TransactionTime = et.TransactionTime;
                e.Remark = et.Remark;
                etmanage.Update(e);
                ajaxresult = etmanage.Success();
                try
                {
                    var mtype1 = mt.GetList().Where(s => s.MoveTypeName == "转正").FirstOrDefault().ID;
                    // var mtype2 = mt.GetList().Where(s => s.MoveTypeName == "离职").FirstOrDefault().ID;
                    var mtype3 = mt.GetList().Where(s => s.MoveTypeName == "调岗").FirstOrDefault().ID;
                    var myype4 = mt.GetList().Where(s => s.MoveTypeName == "加薪").FirstOrDefault().ID;
                    var emp = empmanage.GetEntity(e.EmployeeId);
                    if (ajaxresult.Success && e.TransactionType == mtype1)//当异动时间修改好之后且是转正异动的情况下将该员工的转正日期修改
                    {
                        emp.PositiveDate = e.TransactionTime;
                        empmanage.Update(emp);
                        ajaxresult = empmanage.Success();
                    }
                    //else if (ajaxresult.Success && e.TransactionType == mtype2)//当异动时间修改好之后且是离职异动的情况下将该员工的在职状态改为离职状态
                    //{
                    //    emp.IsDel = true;
                    //    empmanage.Update(emp);
                    //    ajaxresult = empmanage.Success();

                    //}
                    else if (ajaxresult.Success && e.TransactionType == mtype3)//当异动时间修改好之后且是调岗异动的情况下将该员工的工资及岗位进行修改
                    {
                        emp.PositionId = (int)e.PresentPosition;
                        if (emp.PositiveDate == null)
                        {
                            emp.ProbationSalary = e.PresentSalary;
                        }
                        else
                        {
                            emp.Salary = e.PresentSalary;
                        }
                        empmanage.Update(emp);
                        ajaxresult = empmanage.Success();
                    }
                    else if (ajaxresult.Success && e.TransactionType == myype4)
                    {
                        if (ajaxresult.Success)
                        {//异动添加成功后将员工表中的员工工资也改变    
                            if (emp.PositiveDate == null)
                            {
                                emp.ProbationSalary = et.PresentSalary;
                            }
                            else
                            {
                                emp.Salary = et.PresentSalary;
                            }
                            empmanage.Update(emp);
                            ajaxresult = empmanage.Success();
                        }
                    }

                }
                catch (Exception ex)
                {
                    ajaxresult = empmanage.Error(ex.Message);
                }

            }
            catch (Exception ex)
            {
                ajaxresult = etmanage.Error(ex.Message);
            }

            return Json(ajaxresult, JsonRequestBehavior.AllowGet);
        }
        //员工异动详情信息
        public ActionResult EmpETRDetail(int id)
        {
            EmpTransactionManage etmanage = new EmpTransactionManage();
            var et = etmanage.GetEntity(id);
            ViewBag.id = id;
            return View(et);
        }

        //异动信息添加
        public ActionResult AddTransactionInfo() {
            MoveTypeManage mt = new MoveTypeManage();
            var mtlist = mt.GetList().Where(s=>s.IsDel==false).ToList();
            ViewBag.etrType = mtlist;
            return View();
        }
       
        /// <summary>
        ///  异动信息添加时选择员工
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectEmp() {
            return View();
        }
        public ActionResult GetEmpData(int page, int limit)
        {
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var list = empinfo.GetList().Where(e => e.IsDel == false).ToList();
           
            var mylist = list.OrderBy(e => e.EmployeeId).Skip((page - 1) * limit).Take(limit).ToList();
            var newlist = from e in mylist
                          select new
                          {
                              #region 获取属性值 
                              e.EmployeeId,
                              e.DDAppId,
                              e.EmpName,
                              Position = empinfo.GetPosition((int)e.PositionId).PositionName,
                              Depart = empinfo.GetDept((int)e.PositionId).DeptName,
                              e.PositionId,
                              empinfo.GetDept((int)e.PositionId).DeptId,
                              e.Sex,
                              e.Age,
                              e.Nation,
                              e.Phone,
                              e.IdCardNum,
                              e.ContractStartTime,
                              e.ContractEndTime,
                              e.EntryTime,
                              e.Birthdate,
                              e.Birthday,
                              e.PositiveDate,
                              e.UrgentPhone,
                              e.DomicileAddress,
                              e.Address,
                              e.Education,
                              e.MaritalStatus,
                              e.IdCardIndate,
                              e.PoliticsStatus,
                              e.WorkExperience,
                              e.ProbationSalary,
                              e.Salary,
                              e.SSStartMonth,
                              e.BCNum,
                              e.Material,
                              e.Remark,
                              e.IsDel
                              #endregion

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
        /// <summary>
        /// 提交异动信息的添加
        /// </summary>
        /// <param name="etr"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddEtrInfo(EmpTransaction etr)
        {
            var ajaxresult = new AjaxResult();
            EmpTransactionManage etrmanage = new EmpTransactionManage();
            MoveTypeManage mtmanage = new MoveTypeManage();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            try
            {
                etr.IsDel = false;
                etrmanage.Insert(etr);
                ajaxresult = etrmanage.Success();
            }
            catch (Exception ex)
            {
                ajaxresult = etrmanage.Error(ex.Message);
            }

            if (ajaxresult.Success) {
                var mtname = mtmanage.GetList().Where(s => s.IsDel == false && s.ID == etr.TransactionType).FirstOrDefault().MoveTypeName;
                if (mtname.Equals("离职")) {
                    #region 员工表（及相关子表）修改（离职）
                    var emp = empmanage.GetEntity(etr.EmployeeId);
                    emp.IsDel = true;
                    empmanage.Update(emp);
                    ajaxresult = empmanage.Success();

                    if (ajaxresult.Success)
                    {
                        var dname = empmanage.GetDept(emp.PositionId).DeptName;
                        var pname = empmanage.GetPosition(emp.PositionId).PositionName;
                        if (dname.Equals("就业部"))
                        {
                            EmploymentStaffBusiness esmanage = new EmploymentStaffBusiness();
                            bool es = esmanage.DelEmploystaff(emp.EmployeeId);
                            ajaxresult.Success = es;
                        }
                        if (dname.Equals("市场部"))
                        {
                            ChannelStaffBusiness csmanage = new ChannelStaffBusiness();
                            bool cs = csmanage.DelChannelStaff(emp.EmployeeId);
                            ajaxresult.Success = cs;
                        }
                        if ((dname.Equals("s1、s2教质部") || dname.Equals("s3教质部")) && !pname.Equals("教官"))
                        {
                            HeadmasterBusiness hmmanage = new HeadmasterBusiness();
                            bool hm = hmmanage.removeHeadmaster(emp.EmployeeId);
                            ajaxresult.Success = hm;
                        }
                        if ((dname.Equals("s1、s2教质部") || dname.Equals("s3教质部")) && pname.Equals("教官"))
                        {
                            InstructorListBusiness itmanage = new InstructorListBusiness();
                            bool hm = itmanage.RemoveInstructorList(emp.EmployeeId);
                            ajaxresult.Success = hm;
                        }
                        if (pname.Equals("咨询师") || pname.Equals("咨询主任"))
                        {
                            ConsultTeacherManeger cmanage = new ConsultTeacherManeger();
                            bool s = cmanage.DeltConsultTeacher(emp.EmployeeId);
                            ajaxresult.Success = s;
                        }
                        if (dname.Equals("s1、s2教学部") || dname.Equals("s3教学部") || dname.Equals("s4教学部"))
                        {
                            TeacherBusiness teamanage = new TeacherBusiness();
                            bool s = teamanage.dimission(emp.EmployeeId);
                            ajaxresult.Success = s;
                        }
                        if (dname.Equals("财务部"))
                        {
                            FinanceModelBusiness fmmanage = new FinanceModelBusiness();
                            bool s = fmmanage.UpdateFinancialstaff(emp.EmployeeId);
                            ajaxresult.Success = s;
                        }
                        StaffAccdationBusiness sdmanae = new StaffAccdationBusiness();
                        bool mysd = sdmanae.DelStaffacc(emp.EmployeeId);
                        ajaxresult.Success = mysd;
                    }

                }
                #endregion
                else if (mtname.Equals("转正"))
                {
                    if (ajaxresult.Success)
                    {
                        var emp = empmanage.GetEntity(etr.EmployeeId);
                        emp.PositiveDate = etr.TransactionTime;
                        empmanage.Update(emp);
                        ajaxresult = empmanage.Success();

                        //员工转正时间修改好之后将该员工的绩效工资及岗位工资修改一下
                        if (ajaxresult.Success)
                        {
                            EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
                            var ese = esemanage.GetEseByEmpid(emp.EmployeeId);
                            //当该员工的岗位是主任或者是副主任绩效额度为1000，普通员工为500
                            if (empmanage.GetPositionByEmpid(emp.EmployeeId).PositionName.Contains("主任"))
                            {
                                ese.PerformancePay = 1000;
                            }
                            else if (empmanage.GetDeptByEmpid(emp.EmployeeId).DeptName == "校办")
                            {
                                ese.PerformancePay = 3000;
                            }
                            else
                            {
                                ese.PerformancePay = 500;
                            }
                            ese.PositionSalary = emp.Salary - ese.BaseSalary - ese.PerformancePay;

                            esemanage.Update(ese);
                            ajaxresult = esemanage.Success();
                        }
                    }
                }
                else if (mtname.Equals("调岗"))
                {
                    if (ajaxresult.Success)
                    {
                        var emp = empmanage.GetEntity(etr.EmployeeId);
                        if (emp.PositiveDate == null)
                        {
                            emp.ProbationSalary = etr.PresentSalary;
                        }
                        else
                        {
                            emp.Salary = etr.PresentSalary;
                        }
                        empmanage.Update(emp);
                        ajaxresult = empmanage.Success();
                        if (ajaxresult.Success)
                        {
                            EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
                            var ese = esemanage.GetEseByEmpid(emp.EmployeeId);
                            if (empmanage.GetPositionByEmpid(emp.EmployeeId).PositionName.Contains("主任"))
                            {
                                ese.PerformancePay = 1000;
                            }
                            else if (empmanage.GetDeptByEmpid(emp.EmployeeId).DeptName == "校办")
                            {
                                ese.PerformancePay = 3000;
                            }
                            else
                            {
                                ese.PerformancePay = 500;
                            }
                            if (emp.PositiveDate == null)
                            {
                                ese.PositionSalary = emp.ProbationSalary - ese.BaseSalary - ese.PerformancePay;
                            }
                            else
                            {
                                ese.PositionSalary = emp.Salary - ese.BaseSalary - ese.PerformancePay;
                            }
                            esemanage.Update(ese);
                            ajaxresult = esemanage.Success();
                        }
                    }
                } else if (mtname.Equals("加薪")) {
                    if(ajaxresult.Success){
                        //异动添加成功后将员工表中的员工工资也改变
                        var emp = empmanage.GetEntity(etr.EmployeeId);
                        if (emp.PositiveDate == null)
                        {
                            emp.ProbationSalary = etr.PresentSalary;
                        }
                        else
                        {
                            emp.Salary = etr.PresentSalary;
                        }
                        empmanage.Update(emp);
                        ajaxresult = empmanage.Success();
                        //员工工资改变之后，将员工岗位工资也改一下
                        if (ajaxresult.Success)
                        {
                            EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
                            var ese = esemanage.GetEseByEmpid(emp.EmployeeId);
                            if (emp.PositiveDate == null)
                            {
                                var mysalary = emp.Salary - emp.ProbationSalary;
                                ese.PositionSalary = emp.ProbationSalary - ese.BaseSalary;
                                emp.Salary = emp.ProbationSalary + mysalary;
                            }
                            else
                            {
                                ese.PositionSalary = emp.Salary - ese.BaseSalary;
                            }
                            if (!string.IsNullOrEmpty(ese.PerformancePay.ToString()))
                            {
                                ese.PositionSalary=ese.PositionSalary- ese.PerformancePay;
                            }

                            esemanage.Update(ese);
                            ajaxresult = esemanage.Success();
                        }
                    }
                }
            }


            return Json(ajaxresult,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除异动信息，即修改异动信息的状态
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteTransactionInfo(string list) {
            EmpTransactionManage brmanage = new EmpTransactionManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                string[] arr = list.Split(',');

                for (int i = 0; i < arr.Length - 1; i++)
                {
                    string id = arr[i];
                    var br = brmanage.GetEntity(int.Parse(id));
                    br.IsDel = true;
                    brmanage.Update(br);
                    AjaxResultxx = brmanage.Success();
                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = brmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
    }
}