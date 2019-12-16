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
    public class EmpApprovalController : Controller
    {
        // GET: Personnelmatters/EmpApproval
        public ActionResult EmpApprovalIndex()
        {
            return View();
        }
       
        /// <summary>
        /// 获取所有转正申请数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult GetPositiveData(int page, int limit)
        {
            ApplyForFullMemberManage affmmanage = new ApplyForFullMemberManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var list = affmmanage.GetList();
            var newlist = list.OrderByDescending(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var empobj = from e in newlist
                         select new
                         {
                             #region 获取属性值 
                             e.Id,
                             empName = emanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             esex = emanage.GetInfoByEmpID(e.EmployeeId).Sex,
                             dname = emanage.GetDept(emanage.GetInfoByEmpID(e.EmployeeId).PositionId).DeptName,
                             pname = emanage.GetPosition(emanage.GetInfoByEmpID(e.EmployeeId).PositionId).PositionName,
                             e.IsBuySS,
                             e.ProbationEndDate,
                             e.ProbationPersonalSummary,
                             e.ApplicationDate,
                             e.IsApproval,
                             e.IsPass
                             #endregion
                         };
            var newobj = new
            {
                code = 0,
                msg = "",
                count = list.Count(),
                data = empobj
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改员工的转正申请的审批状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PositiveIsPassed(int id, bool state)
        {
            ApplyForFullMemberManage affmmanage = new ApplyForFullMemberManage();
            EmpTransactionManage etmanage = new EmpTransactionManage();
            MoveTypeManage m = new MoveTypeManage();
            var positive = affmmanage.GetEntity(id);
            EmpTransaction et = new EmpTransaction();
            var ajaxresult = new AjaxResult();
            try
            {
                if (state == true)
                {
                    positive.IsApproval = true;
                    positive.IsPass = true;//表示转正申请通过
                    affmmanage.Update(positive);
                    ajaxresult = affmmanage.Success();
                    try
                    {
                        if (ajaxresult.Success)//转正申请通过之后，将该条员工异动情况添加到员工异动表中
                        {
                            et.EmployeeId = positive.EmployeeId;
                            et.IsDel = false;
                            et.TransactionType = m.GetList().Where(s => s.MoveTypeName == "转正").FirstOrDefault().ID;
                            et.TransactionTime = positive.ProbationEndDate;
                            etmanage.Insert(et);
                            ajaxresult = etmanage.Success();
                            if (ajaxresult.Success)
                            {
                                //将员工表中转正时间修改好
                                EmployeesInfoManage empmanage = new EmployeesInfoManage();
                                var emp = empmanage.GetEntity(et.EmployeeId);
                                emp.PositiveDate = et.TransactionTime;
                                empmanage.Update(emp);
                                ajaxresult = empmanage.Success();

                                //员工转正时间修改好之后将该员工的绩效工资及岗位工资修改一下
                                if (ajaxresult.Success) {
                                    EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
                                    var ese = esemanage.GetEseByEmpid(emp.EmployeeId);
                                    if (empmanage.GetPositionByEmpid(emp.EmployeeId).PositionName.Contains("主任"))
                                    {
                                        ese.PerformancePay = 1000;
                                    }
                                    else
                                    {
                                        ese.PerformancePay = 500;
                                    }
                                    ese.PositionSalary = emp.Salary - ese.BaseSalary - ese.PerformancePay;
                                    if (positive.IsBuySS==false) {//代表该员工不需要购买社保,则该员工的社保补贴为500
                                        ese.SocialSecuritySubsidy = 500;
                                    }
                                    esemanage.Update(ese);
                                   ajaxresult= esemanage.Success();
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        ajaxresult = etmanage.Error(ex.Message);
                    }
                }
                else
                {
                    positive.IsApproval = true;//表示该员工申请已审批
                    positive.IsPass = false;//表示转正申请未通过
                    affmmanage.Update(positive);
                    ajaxresult = affmmanage.Success();
                }
            }
            catch (Exception ex)
            {
                ajaxresult = affmmanage.Error(ex.Message);
            }

            return Json(ajaxresult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有离职申请数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult GetDimissionData(int page, int limit)
        {
            DimissionApplyManage damanage = new DimissionApplyManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var list = damanage.GetList();
            var newlist = list.OrderByDescending(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var etlist = from e in newlist
                         select new
                         {
                             #region 获取属性值 
                             e.Id,
                             empName = emanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             esex = emanage.GetInfoByEmpID(e.EmployeeId).Sex,
                             dname = emanage.GetDept(emanage.GetInfoByEmpID(e.EmployeeId).PositionId).DeptName,
                             pname = emanage.GetPosition(emanage.GetInfoByEmpID(e.EmployeeId).PositionId).PositionName,
                             e.DimissionDate,
                             e.DimissionReason,
                             e.OpinionOrAdvice,
                             e.IsApproval,
                             e.IsPass
                             #endregion
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
        /// <summary>
        /// 修改员工离职申请的审批状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DimissionIsPassed(int id, bool state)
        {
            DimissionApplyManage dammanage = new DimissionApplyManage();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            EmpTransactionManage etmanage = new EmpTransactionManage();
            MoveTypeManage m = new MoveTypeManage();
            var positive = dammanage.GetEntity(id);
            EmpTransaction et = new EmpTransaction();
            var ajaxresult = new AjaxResult();
            try
            {
                if (state == true)
                {
                    positive.IsApproval = true;//表示该员工申请已审批
                    positive.IsPass = true;//表示离职申请通过
                    dammanage.Update(positive);
                    ajaxresult = dammanage.Success();
                    if (ajaxresult.Success)//离职申请通过修改成功之后，将该条员工异动情况添加到员工异动表中,且将员工表（及相关表）的状态改为已离职
                    {
                        try
                        {
                            #region 异动表添加
                            et.EmployeeId = positive.EmployeeId;
                            et.IsDel = false;
                            et.TransactionType = m.GetList().Where(s => s.MoveTypeName == "离职").FirstOrDefault().ID;
                            et.Reason = positive.DimissionReason;
                            etmanage.Insert(et);
                            ajaxresult = etmanage.Success();
                            #endregion

                            #region 员工表（及相关子表）修改
                            var emp = empmanage.GetEntity(et.EmployeeId);
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
                                    bool hm = hmmanage.QuitEntity(emp.EmployeeId);
                                    ajaxresult.Success = hm;
                                }

                                if ((dname.Equals("s1、s2教质部")|| dname.Equals("s3教质部")) && pname.Equals("教官"))
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
                               
                              
                                //EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();//员工工资体系中员工离职改变
                                //bool s;
                                //s = esemanage.EditEmpSalaryState(emp.EmployeeId);
                                //MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();//员工月度工资表中员工离职改变
                                //s = msrmanage.EditEmpMS(emp.EmployeeId);
                                //ajaxresult.Success = s;

                            }
                            #endregion

                        }


                        catch (Exception ex)
                        {
                            ajaxresult = etmanage.Error(ex.Message);
                        }
                    }
                }
                else
                {
                    positive.IsApproval = true;//表示该员工申请已审批
                    positive.IsPass = false;//表示离职申请未通过
                    dammanage.Update(positive);
                    ajaxresult = dammanage.Success();
                }

            }
            catch (Exception ex)
            {
                ajaxresult = dammanage.Error(ex.Message);
            }

            return Json(ajaxresult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有调岗申请数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult GetTransferPositionData(int page, int limit)
        {
            JobTransferApplyManage jtfamanage = new JobTransferApplyManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var list = jtfamanage.GetList();
            var newlist = list.OrderByDescending(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var etlist = from e in newlist
                         select new
                         {
                             #region 获取属性值 
                             e.Id,
                             empName = emanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             esex = emanage.GetInfoByEmpID(e.EmployeeId).Sex,
                             dname = emanage.GetDept(emanage.GetInfoByEmpID(e.EmployeeId).PositionId).DeptName,
                             pname = emanage.GetPosition(emanage.GetInfoByEmpID(e.EmployeeId).PositionId).PositionName,
                             presalary = emanage.GetInfoByEmpID(e.EmployeeId).PositiveDate == null ? emanage.GetInfoByEmpID(e.EmployeeId).ProbationSalary : emanage.GetInfoByEmpID(e.EmployeeId).Salary,//未转正的情况下员工工资指的是实习工资
                             nowdname = emanage.GetDeptById((int)e.PlanTurnDeptId).DeptName,
                             nowpname = emanage.GetPobjById((int)e.PlanTurnPositionId).PositionName,
                             e.TurnAfterSalary,
                             e.Reason,
                             e.IsApproval,
                             e.IsPass
                             #endregion
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
        /// <summary>
        /// 修改员工调岗申请的审批状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TransferPositionIsPassed(int id, bool state)
        {
            JobTransferApplyManage jammanage = new JobTransferApplyManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            EmpTransactionManage etmanage = new EmpTransactionManage();
            MoveTypeManage m = new MoveTypeManage();
            var positive = jammanage.GetEntity(id);
            EmpTransaction et = new EmpTransaction();
            var ajaxresult = new AjaxResult();
            try
            {
                if (state == true)
                {
                    positive.IsApproval = true;//表示该员工申请已审批
                    positive.IsPass = true;//表示调岗申请通过
                    jammanage.Update(positive);
                    ajaxresult = jammanage.Success();
                    try
                    {
                        if (ajaxresult.Success)//调岗申请通过修改成功之后，将该条员工异动情况添加到员工异动表中
                        {
                            et.EmployeeId = positive.EmployeeId;
                            et.IsDel = false;
                            et.TransactionType = m.GetList().Where(s => s.MoveTypeName == "调岗").FirstOrDefault().ID;
                            et.Reason = positive.Reason;
                            et.PreviousDept = emanage.GetDept(emanage.GetInfoByEmpID(et.EmployeeId).PositionId).DeptId;
                            et.PreviousPosition = emanage.GetPosition(emanage.GetInfoByEmpID(et.EmployeeId).PositionId).Pid;
                            et.PreviousSalary = emanage.GetInfoByEmpID(et.EmployeeId).PositiveDate == null ? emanage.GetInfoByEmpID(et.EmployeeId).ProbationSalary : emanage.GetInfoByEmpID(et.EmployeeId).Salary;
                            et.PresentDept = positive.PlanTurnDeptId;
                            et.PresentPosition = positive.PlanTurnPositionId;
                            et.PresentSalary = positive.TurnAfterSalary;
                            et.Reason = positive.Reason;
                            etmanage.Insert(et);
                            ajaxresult = etmanage.Success();
                            if (ajaxresult.Success)
                            {
                                var emp = emanage.GetEntity(et.EmployeeId);
                                if (emp.PositiveDate == null)
                                {
                                    emp.ProbationSalary = et.PresentSalary;
                                }
                                else
                                {
                                    emp.Salary = et.PresentSalary;
                                }
                                emanage.Update(emp);
                                ajaxresult = emanage.Success();
                                if (ajaxresult.Success) {
                                    EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
                                    var ese = esemanage.GetEseByEmpid(emp.EmployeeId);
                                    if (emanage.GetPositionByEmpid(emp.EmployeeId).PositionName.Contains("主任"))
                                    {
                                        ese.PerformancePay = 1000;
                                    }
                                    else
                                    {
                                        ese.PerformancePay = 500;
                                    } if (emp.PositiveDate == null)
                                    {
                                        ese.PositionSalary = emp.ProbationSalary - ese.BaseSalary - ese.PerformancePay;
                                    }
                                    else {
                                        ese.PositionSalary = emp.Salary - ese.BaseSalary - ese.PerformancePay;
                                    }
                                    esemanage.Update(ese);
                                    ajaxresult = esemanage.Success();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ajaxresult = etmanage.Error(ex.Message);
                    }
                }
                else
                {
                    positive.IsApproval = true;//表示该员工申请已审批
                    positive.IsPass = false;//表示离职申请未通过
                    jammanage.Update(positive);
                    ajaxresult = jammanage.Success();
                }

            }
            catch (Exception ex)
            {
                ajaxresult = jammanage.Error(ex.Message);
            }
            return Json(ajaxresult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有加薪申请数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult GetIncreaseSalaryData(int page, int limit)
        {
            SalaryRaiseApplyManage sramanage = new SalaryRaiseApplyManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var list = sramanage.GetList();
            var newlist = list.OrderByDescending(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var etlist = from e in newlist
                         select new
                         {
                             #region 获取属性值 
                             e.Id,
                             empName = emanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             esex = emanage.GetInfoByEmpID(e.EmployeeId).Sex,
                             dname = emanage.GetDept(emanage.GetInfoByEmpID(e.EmployeeId).PositionId).DeptName,
                             pname = emanage.GetPosition(emanage.GetInfoByEmpID(e.EmployeeId).PositionId).PositionName,
                             EntryTime = emanage.GetInfoByEmpID(e.EmployeeId).EntryTime,
                             Education = emanage.GetInfoByEmpID(e.EmployeeId).Education,
                             PositiveDate = emanage.GetInfoByEmpID(e.EmployeeId).PositiveDate,
                             presalary = emanage.GetInfoByEmpID(e.EmployeeId).PositiveDate == null ? emanage.GetInfoByEmpID(e.EmployeeId).ProbationSalary : emanage.GetInfoByEmpID(e.EmployeeId).Salary,//未转正的情况下员工工资指的是实习工资
                             e.RaisesLimit,
                             e.RaisesReason,
                             e.IsApproval,
                             e.IsPass
                             #endregion
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
        /// <summary>
        /// 修改员工加薪申请的审批状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IncreaseSalaryIsPassed(int id, bool state)
        {
            SalaryRaiseApplyManage sramanage = new SalaryRaiseApplyManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            EmpTransactionManage etmanage = new EmpTransactionManage();
            MoveTypeManage m = new MoveTypeManage();
            var positive = sramanage.GetEntity(id);
            EmpTransaction et = new EmpTransaction();
            var ajaxresult = new AjaxResult();
            try
            {
                if (state == true)
                {
                    positive.IsApproval = true;//表示该员工申请已审批
                    positive.IsPass = true;//表示加薪申请通过
                    sramanage.Update(positive);
                    ajaxresult = sramanage.Success();
                    try
                    {
                        if (ajaxresult.Success)//加薪申请通过之后，将该条员工异动情况添加到员工异动表中
                        {
                            et.EmployeeId = positive.EmployeeId;
                            et.IsDel = false;
                            et.TransactionType = m.GetList().Where(s => s.MoveTypeName == "加薪").FirstOrDefault().ID;
                            et.PreviousSalary = emanage.GetInfoByEmpID(et.EmployeeId).PositiveDate == null ? emanage.GetInfoByEmpID(et.EmployeeId).ProbationSalary : emanage.GetInfoByEmpID(et.EmployeeId).Salary;
                            et.PresentSalary = (decimal)et.PreviousSalary + (decimal)positive.RaisesLimit;
                            et.Reason = positive.RaisesReason;
                            etmanage.Insert(et);
                            ajaxresult = etmanage.Success();
                            if (ajaxresult.Success)
                            {//异动添加成功后将员工表中的员工工资也改变
                                var emp = emanage.GetEntity(et.EmployeeId);
                                if (emp.PositiveDate == null)
                                {
                                    emp.ProbationSalary = et.PresentSalary;
                                }
                                else
                                {
                                    emp.Salary = et.PresentSalary;
                                }
                                emanage.Update(emp);
                                ajaxresult = emanage.Success();
                                //员工工资改变之后，将员工岗位工资也改一下
                                if (ajaxresult.Success) {
                                    EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
                                    var ese = esemanage.GetEseByEmpid(emp.EmployeeId);
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
                        }
                    }
                    catch (Exception ex)
                    {
                        ajaxresult = etmanage.Error(ex.Message);
                    }
                }
                else
                {
                    positive.IsApproval = true;//表示该员工申请已审批
                    positive.IsPass = false;//表示离职申请未通过
                    sramanage.Update(positive);
                    ajaxresult = sramanage.Success();
                }

            }
            catch (Exception ex)
            {
                ajaxresult = sramanage.Error(ex.Message);
            }
            return Json(ajaxresult, JsonRequestBehavior.AllowGet);
        }

       
    }
}