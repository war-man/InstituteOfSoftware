using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.EmpTransactionBusiness;
using SiliconValley.InformationSystem.Business.SchoolAttendanceManagementBusiness;
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
            var list = etmanage.GetList().ToList();
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
                        if (emp.Salary == null)
                        {
                            emp.ProbationSalary = e.PresentSalary;
                            empmanage.Update(emp);
                            ajaxresult = empmanage.Success();
                        }
                        else
                        {
                            emp.Salary = e.PresentSalary;
                            empmanage.Update(emp);
                            ajaxresult = empmanage.Success();
                        }
                    }
                    else if (ajaxresult.Success && e.TransactionType == myype4)
                    {
                        if (ajaxresult.Success)
                        {//异动添加成功后将员工表中的员工工资也改变    
                            if (emp.Salary == null)
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
            ViewBag.etrType = new SelectList(mtlist, "ID","MoveTypeName");
            return View();
        }

    }
}