using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{

    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    public class EmployeesInfoController : Controller
    {
        // GET: Personnelmatters/EmployeesInfo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(int page,int limit) {
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var list = empinfo.GetList();
            var mylist = list.OrderBy(e => e.EmployeeId).Skip((page - 1) * limit).Take(limit).ToList();
            var newlist=from e in mylist select new
                        { empid= e.EmployeeId,
                            e.DDAppId,
                            e.EmpName,
                            Position=e.PositionId,
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
                            e.WorkingState,
                            e.SSStartMonth,
                            e.BCNum,
                            e.Material,
                            e.Remark,
                            e.IsDel
                           };
            var newobj = new
            {
                code = 0,
                msg = "",
                count = list.Count(),
                data = newlist
            };
            return Json(newobj,JsonRequestBehavior.AllowGet);
        }
    }
}