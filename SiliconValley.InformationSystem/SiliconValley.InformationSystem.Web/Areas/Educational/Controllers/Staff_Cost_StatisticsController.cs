using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
/// <summary>
/// ////////////////////////
/// </summary>
/// 

using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    public class Staff_Cost_StatisticsController : Controller
    {

        private Staff_Cost_StatisticssBusiness db_staf_Cost;


        public Staff_Cost_StatisticsController()
        {
            db_staf_Cost = new Staff_Cost_StatisticssBusiness();
        }

        // GET: Educational/Staff_Cost_Statistics

        /// <summary>
        /// 员工费用统计页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Staff_Cost_StatisticsIndex()
        {

            var deps = db_staf_Cost.GetDepartments();

            ViewBag.Deps = deps;

            return View();
        }

        /// <summary>
        /// 员工数据
        /// </summary>
        /// <returns></returns>
        public ActionResult EmpData(int limit, int page,string empName = null, string depId = null )
        {
            //获取筛选之后的员工
            List<EmployeesInfo> emplist = db_staf_Cost.ScreenEmp(EmpName: empName, DepId: depId);

            //分页
            List<EmployeesInfo> skiplist = emplist.Skip((page - 1) * limit).Take(limit).ToList();

            //组装返回对象
            var result = new
            {
                code = 0,
                count = emplist.Count,
                msg = "",
                data = skiplist

            };

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 返回员工部门
        /// </summary>
        /// <param name="PositionId">员工编号</param>
        /// <returns></returns>
        public ActionResult empDep(string EmployeeId)
        {
            AjaxResult result = new AjaxResult();
            try
            {
                var dep = db_staf_Cost.GetDeparmentByEmp(EmployeeId);

                result.Data = dep;
                result.ErrorCode = 200;
                result.Msg = "";

            }
            catch (Exception)
            {
                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = "";
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 获取员工岗位
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public ActionResult empPosition(string EmployeeId)
        {
            AjaxResult result = new AjaxResult();
            try
            {
                var poo = db_staf_Cost.GetPositionByEmp(EmployeeId);

                result.Data = poo;
                result.ErrorCode = 200;
                result.Msg = "";

            }
            catch (Exception)
            {
                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = "";
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}