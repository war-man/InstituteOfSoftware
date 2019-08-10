using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{

    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Business.PositionBusiness;
    using SiliconValley.InformationSystem.Business.DepartmentBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using System.Net;
    using SiliconValley.InformationSystem.Util;
    using System.ComponentModel.DataAnnotations;

    public class EmployeesInfoController : Controller
    {
        // GET: Personnelmatters/EmployeesInfo
        public ActionResult Index()
        {
            return View();
        }
        //给员工编号最后几位常数的生成定义一个全局变量
        int num;

        //获取员工信息数据
        public ActionResult GetData(int page,int limit){
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var list = empinfo.GetList();
                var mylist = list.OrderBy(e => e.EmployeeId).Skip((page - 1) * limit).Take(limit).ToList();
            // var mylist = empinfo.GetPagination(list,page,limit);
            var newlist = from e in mylist
                          select new
                          {
                              e.EmployeeId,
                              e.DDAppId,
                              e.EmpName,
                              Position = GetPosition((int)e.PositionId).PositionName,
                              Depart = GetDept((int)e.PositionId).DeptName,
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

        //获取所属岗位对象
        public Position GetPosition(int pid) {
            PositionManage pmanage = new PositionManage();
            var plist = pmanage.GetList();//获取公司所有岗位的数据集合
            var str = plist.Where(p => p.Pid == pid).FirstOrDefault();
            return  str;
        }
        //获取所属岗位的所属部门对象
        public Department GetDept(int pid) {
            DepartmentManage deptmanage = new DepartmentManage();
            var deptlist = deptmanage.GetList();//获取公司部门数据集
            ViewBag.DeptList = deptlist;
            var str = deptlist.Where(d => d.DeptId == GetPosition(pid).DeptId).FirstOrDefault();
            return str;
        }

        //添加员工页面显示
       [HttpGet]
        public ActionResult AddEmp() {
           
            return View();
        }

        //员工添加的所属部门下拉框绑定
        public ActionResult BindDeptSelect() {
            DepartmentManage deptmanage = new DepartmentManage();
            var deptlist = deptmanage.GetList();//获取公司部门数据集
            var newstr = new
            {
                code = 0,
                msg = "",
                count = deptlist.Count(),
                data = deptlist
            };
            return Json(newstr,JsonRequestBehavior.AllowGet);
        }
        //员工添加的所属部门下拉框绑定
        //[HttpPost]
        public ActionResult BindPositionSelect(int deptid) {
            PositionManage pmanage = new PositionManage();
            var plist = pmanage.GetList().Where(d => d.DeptId == deptid);
            var newstr = new
            {
                code = 0,
                msg = "",
                count = plist.Count(),
                data = plist
            };
            return Json(newstr,JsonRequestBehavior.AllowGet);
        }

        //添加员工
        [HttpPost]
        public ActionResult AddEmpInfo(EmployeesInfo emp) {
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                emp.EmployeeId = EmpId();
                emp.Age =int.Parse(GetAge((DateTime)emp.Birthdate,DateTime.Now));
                empinfo.Insert(emp);
                AjaxResultxx= empinfo.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = empinfo.Success(ex.Message);
            }
          
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }


       
        //获取网络时间
        public string Date()
        {
            WebRequest request = null;
            WebResponse response = null;
            WebHeaderCollection headerCollection = null;

            string datetime = string.Empty;
            try
            {
                request = WebRequest.Create("https://www.baidu.com");
                request.Timeout = 3000;
                request.Credentials = CredentialCache.DefaultCredentials;
                response = (WebResponse)request.GetResponse();
                headerCollection = response.Headers;
                foreach (var h in headerCollection.AllKeys)
                { if (h == "Date") { datetime = headerCollection[h]; } }
                return datetime;
            }

            catch (Exception) { return datetime; }
            finally
            {
                if (request != null)
                { request.Abort(); }
                if (response != null)
                { response.Close(); }
                if (headerCollection != null)
                { headerCollection.Clear(); }
            }
        }
        //月份及日期前面加个零
        public string MonthAndDay(int a)
        {

            if (a < 10)
            {
                return "0" + a;
            }
            string c = a.ToString();
            return c;
        }
        //生成员工编号
        public string EmpId()
        {
            num++;
            string mingci = string.Empty;
            DateTime date = Convert.ToDateTime(Date());
            //string sfz = IDnumber.Substring(6, 8);
            string n = date.Year.ToString();//获取年份
            string y = MonthAndDay(Convert.ToInt32(date.Month)).ToString();//获取月份
            string d = MonthAndDay(Convert.ToInt32(date.Date)).ToString();//互殴年份

            // string count = Count().ToString();
            string count = num.ToString();
            if (count.Length < 2)
                mingci = "0000" + count;
            string EmpidResult = n + y + d + mingci;
            return EmpidResult;
        }


        //计算员工年龄
        public static string GetAge(DateTime dtBirthday, DateTime dtNow)
        {
            string strAge = string.Empty; // 年龄的字符串表示
            int intYear = 0; // 岁
            int intMonth = 0; // 月
            int intDay = 0; // 天

            // 如果没有设定出生日期, 返回空
            if (dtBirthday==null)
            {
                return string.Empty;
            }

            // 计算天数
            intDay = dtNow.Day - dtBirthday.Day;
            if (intDay < 0)
            {
                dtNow = dtNow.AddMonths(-1);
                intDay += DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
            }

            // 计算月数
            intMonth = dtNow.Month - dtBirthday.Month;
            if (intMonth < 0)
            {
                intMonth += 12;
                dtNow = dtNow.AddYears(-1);
            }

            // 计算年数
            intYear = dtNow.Year - dtBirthday.Year;

            // 格式化年龄输出
            if (intYear >= 1) // 年份输出
            {
                strAge = intYear.ToString() + "岁";
            }

            if (intMonth > 0 && intYear <= 5) // 五岁以下可以输出月数
            {
                strAge += intMonth.ToString() + "月";
            }

            if (intDay >= 0 && intYear < 1) // 一岁以下可以输出天数
            {
                if (strAge.Length == 0 || intDay > 0)
                {
                    strAge += intDay.ToString() + "日";
                }
            }

            return strAge;
        }
    }
}