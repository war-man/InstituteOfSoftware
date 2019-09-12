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
    using SiliconValley.InformationSystem.Business.ClassesBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using System.Net;
    using SiliconValley.InformationSystem.Util;
    using System.ComponentModel.DataAnnotations;
    using SiliconValley.InformationSystem.Business.Employment;
    using SiliconValley.InformationSystem.Business.Consult_Business;
    using SiliconValley.InformationSystem.Business.Channel;
    using SiliconValley.InformationSystem.Business.EmpTransactionBusiness;
    using SiliconValley.InformationSystem.Business;
    public class EmployeesInfoController : Controller
    {
        // GET: Personnelmatters/EmployeesInfo
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///获取在职员工员工信息数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="AppCondition"></param>
        /// <returns></returns>
        public ActionResult GetData(int page, int limit, string AppCondition)
        {
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var list = empinfo.GetList().Where(e => e.IsDel == false).ToList();
            if (!string.IsNullOrEmpty(AppCondition)) {
                string[] str = AppCondition.Split(',');
                string ename = str[0];
                string deptname = str[1];
                string pname = str[2];
                string Education = str[3];
                string sex = str[4];
                string PoliticsStatus = str[5];
                string start_time = str[6];
                string end_time = str[7];
                list = list.Where(e => e.EmpName.Contains(ename)).ToList();
                if (!string.IsNullOrEmpty(deptname)) {

                    list = list.Where(e => empinfo.GetDept((int)e.PositionId).DeptId == int.Parse(deptname)).ToList();
                }
                if (!string.IsNullOrEmpty(pname)) {
                    list = list.Where(e => e.PositionId == int.Parse(pname)).ToList();
                }
                if (!string.IsNullOrEmpty(Education)) {
                    list = list.Where(e => e.Education==Education).ToList();
                }
                if (!string.IsNullOrEmpty(sex))
                {
                    list = list.Where(e => e.Sex==sex).ToList();
                }
                if (!string.IsNullOrEmpty(PoliticsStatus))
                {
                    list = list.Where(e => e.PoliticsStatus==PoliticsStatus).ToList();
                }

                if (!string.IsNullOrEmpty(start_time))
                {
                    DateTime stime = Convert.ToDateTime(start_time + " 00:00:00.000");
                    list = list.Where(a => a.EntryTime >= stime).ToList();
                }
                if (!string.IsNullOrEmpty(end_time))
                {
                    DateTime etime = Convert.ToDateTime(end_time + " 23:59:59.999");
                    list = list.Where(a => a.EntryTime <= etime).ToList();
                }
            }
            var mylist = list.OrderBy(e => e.EmployeeId).Skip((page - 1) * limit).Take(limit).ToList();
            var newlist = from e in mylist
                          select new
                          {
                              #region 获取属性值 
                              e.EmployeeId,
                              e.DDAppId,
                              e.EmpName,
                              Position =empinfo.GetPosition((int)e.PositionId).PositionName,
                              Depart =empinfo.GetDept((int)e.PositionId).DeptName,
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
        /// 获取离职员工信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult GetDelEmpData(int page, int limit) {
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            EmpTransactionManage etm = new EmpTransactionManage();
            var slist = empinfo.GetList().Where(e => e.IsDel == true).ToList();
            var list = slist.Select(e1 => new
            {
                #region 两表查询的数据
                e1.EmployeeId,
                e1.DDAppId,
                e1.EmpName,
                Position = empinfo.GetPosition((int)e1.PositionId).PositionName,
                Depart =empinfo.GetDept((int)e1.PositionId).DeptName,
                e1.Sex,
                e1.Age,
                e1.Nation,
                e1.Phone,
                e1.IdCardNum,
                e1.EntryTime,
                e1.ContractStartTime,
                e1.ContractEndTime,
                e1.Birthdate,
                e1.Birthday,
                e1.PositiveDate,
                e1.UrgentPhone,
                e1.DomicileAddress,
                e1.Address,
                e1.Education,
                e1.MaritalStatus,
                e1.IdCardIndate,
                e1.PoliticsStatus,
                e1.WorkExperience,
                e1.ProbationSalary,
                e1.Salary,
                e1.SSStartMonth,
                e1.BCNum,
                e1.Material,
                e1.Remark,
                e1.IsDel,
                deltime = etm.GetDelEmp(e1.EmployeeId).TransactionTime,//离职时间
                delreason = etm.GetDelEmp(e1.EmployeeId).Reason//离职原因
                #endregion

            }).ToList();
            var mylist = list.OrderBy(e => e.EmployeeId).Skip((page - 1) * limit).Take(limit).ToList();

            var newobj = new
            {
                code = 0,
                msg = "",
                count = list.Count(),
                data = mylist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }


  
        //添加员工页面显示
        [HttpGet]
        public ActionResult AddEmp() {
            return View();
        }
        /// <summary>
        ///添加员工时获取未禁用的部门信息 
        /// </summary>
        /// <returns></returns>
        public ActionResult BindDeptSelect() {
            DepartmentManage deptmanage = new DepartmentManage();
            var deptlist = deptmanage.GetList().Where(d => d.IsDel == false);//获取公司部门数据集
            var newstr = new
            {
                code = 0,
                msg = "",
                count = deptlist.Count(),
                data = deptlist
            };
            return Json(newstr, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 员工添加的所属岗位下拉框绑定且是未禁用的
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult BindPositionSelect(int deptid) {
            PositionManage pmanage = new PositionManage();
            var plist = pmanage.GetList().Where(d => d.DeptId == deptid && d.IsDel == false);
            var newstr = new
            {
                code = 0,
                msg = "",
                count = plist.Count(),
                data = plist
            };
            return Json(newstr, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddEmpInfo(EmployeesInfo emp) {
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();
            EmploymentStaffBusiness esmanage = new EmploymentStaffBusiness();
            HeadmasterBusiness hm = new HeadmasterBusiness();
            ConsultTeacherManeger cmanage = new ConsultTeacherManeger();
            ChannelStaffBusiness csmanage = new ChannelStaffBusiness();
            try
            {
                emp.EmployeeId = EmpId();
                if (emp.IdCardNum != null) {
                    emp.Birthdate = DateTime.Parse(GetBirth(emp.IdCardNum));
                   
                }
                if (emp.Birthdate!=null) {
                    emp.Age = Convert.ToInt32(GetAge((DateTime)emp.Birthdate, DateTime.Now));
                }
                emp.IsDel = false;
                empinfo.Insert(emp);
                if (empinfo.GetDept(emp.PositionId).DeptName == "就业部")
                {
                    bool s = esmanage.AddEmploystaff(emp.EmployeeId);
                    empinfo.Success().Success = s;
                    AjaxResultxx = empinfo.Success();
                }
                if (empinfo.GetDept(emp.PositionId).DeptName == "市场部")
                {
                    bool s = csmanage.AddChannelStaff(emp.EmployeeId);
                    empinfo.Success().Success = s;
                    AjaxResultxx = empinfo.Success();
                }
                if (empinfo.GetDept(emp.PositionId).DeptName == "教质部") {
                    bool s = hm.AddHeadmaster(emp.EmployeeId);
                    empinfo.Success().Success = s;
                    AjaxResultxx = empinfo.Success();
                }
                if (empinfo.GetPosition(emp.PositionId).PositionName == "咨询师" || empinfo.GetPosition(emp.PositionId).PositionName == "咨询主任") {
                    bool s = cmanage.AddConsultTeacherData(emp.EmployeeId);
                    empinfo.Success().Success = s;
                    AjaxResultxx = empinfo.Success();
                }
                AjaxResultxx = empinfo.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = empinfo.Error(ex.Message);
            }

            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //获取时间（网络时间/当前计算机时间）
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
        /// <summary>
        ///生成员工编号
        /// </summary>
        /// <returns></returns>
        public string EmpId()
        {
            string mingci = string.Empty;
            // DateTime date = Convert.ToDateTime(Date());
            DateTime date = DateTime.Now;
            string n = date.Year.ToString();//获取年份
            string y = MonthAndDay(Convert.ToInt32(date.Month)).ToString();//获取月份
            string d = MonthAndDay(Convert.ToInt32(date.Day)).ToString();//获取日期

            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var lastobj = empinfo.GetList().LastOrDefault();
            if (lastobj == null)
            {
                mingci = "0001";
            }
            else {
                string laststr = lastobj.EmployeeId;
                string startfournum = laststr.Substring(0, 4);
                string endfournum = laststr.Substring(laststr.Length - 4, 4);
                if (int.Parse(n) > int.Parse(startfournum))
                {
                    mingci = "0001";
                }
                else
                {
                    string newstr = (int.Parse(endfournum) + 1).ToString();
                    if (int.Parse(newstr) < 10)
                    {
                        mingci = "000" + newstr;
                    }
                    else if (int.Parse(newstr) >= 10 && int.Parse(newstr) < 100)
                    {
                        mingci = "00" + newstr;
                    }
                    else if (int.Parse(newstr) >= 100 && int.Parse(newstr) < 1000)
                    {
                        mingci = "0" + newstr;
                    }
                    else
                    {
                        mingci = newstr;
                    }
                }
            }
            string EmpidResult = n + y + d + mingci;
            return EmpidResult;
        }
        /// <summary>
        ///计算员工年龄
        /// </summary>
        /// <param name="dtBirthday"></param>
        /// <param name="dtNow"></param>
        /// <returns></returns>
        public static string GetAge(DateTime dtBirthday, DateTime dtNow)
        {
            string strAge = string.Empty; // 年龄的字符串表示
            int intYear = 0; // 岁
            int intMonth = 0; // 月
            int intDay = 0; // 天

            // 如果没有设定出生日期, 返回空
            if (dtBirthday == null)
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
                strAge = intYear.ToString();
            }

            //if (intMonth > 0 && intYear <= 5) // 五岁以下可以输出月数
            //{
            //    strAge += intMonth.ToString() + "月";
            //}

            //if (intDay >= 0 && intYear < 1) // 一岁以下可以输出天数
            //{
            //    if (strAge.Length == 0 || intDay > 0)
            //    {
            //        strAge += intDay.ToString() + "日";
            //    }
            //}

            return strAge;
        }
        /// <summary>
        /// 根据身份证号码获取出生日期
        /// </summary>
        /// <param name="idnum"></param>
        /// <returns></returns>
        public static string GetBirth(string idnum) {
            string year = idnum.Substring(6, 4);
            string month = idnum.Substring(10, 2);
            string date = idnum.Substring(12, 2);
            string result = year + "-" + month + "-" + date;
            return result;
        }



        //部门及岗位管理页面显示
        [HttpGet]
        public ActionResult DeptOperation()
        {
            return View();
        }
        //部门信息获取
        public ActionResult GetDepts()
        {
            DepartmentManage deptmanage = new DepartmentManage();
            var deptlist = deptmanage.GetList();//获取公司部门数据集
            var newstr = new
            {
                code = 0,
                msg = "",
                count = deptlist.Count(),
                data = deptlist
            };
            return Json(newstr, JsonRequestBehavior.AllowGet);
        }
        //岗位信息获取
        public ActionResult GetPositions() {
            PositionManage deptmanage = new PositionManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var deptlist = deptmanage.GetList();//获取公司部门数据集

            var newlist = from p in deptlist
                          select new {
                              p.Pid,
                              deptname = emanage.GetDept(p.Pid).DeptName,
                              p.PositionName,
                              p.IsDel
                          };
            var newstr = new
            {
                code = 0,
                msg = "",
                count = deptlist.Count(),
                data = newlist
            };
            return Json(newstr, JsonRequestBehavior.AllowGet);
        }

        //部门增加显示页
        public ActionResult DeptUpdate() {
            return View();
        }
        //验证添加的部门是否已存在
        [HttpPost]
        public ActionResult VerifyDname(string dname) {
            DepartmentManage deptmanage = new DepartmentManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                foreach (var d in deptmanage.GetList())
                {
                    if (dname == d.DeptName) {
                        AjaxResultxx = deptmanage.Success();
                    }
                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = deptmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddDept(Department dept) {
            DepartmentManage deptmanage = new DepartmentManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                dept.IsDel = false;
                deptmanage.Insert(dept);
                AjaxResultxx = deptmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = deptmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //修改部门表的IsDel属性
        [HttpPost]
        public ActionResult EditDeptIsDel(int id, bool isdel) {
            DepartmentManage deptmanage = new DepartmentManage();
            PositionManage pmanage = new PositionManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var dept = deptmanage.GetEntity(id);
                dept.IsDel = isdel;
                deptmanage.Update(dept);
                var plist = pmanage.GetList().Where(p => p.DeptId == id);
                foreach (var p in plist)
                {
                    p.IsDel = true;
                    pmanage.Update(p);
                }
                AjaxResultxx = deptmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = deptmanage.Error(ex.Message);
            }

            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //部门伪删除即,将部门禁用
        [HttpPost]
        public ActionResult DelDepts(string list) {
            DepartmentManage deptmanage = new DepartmentManage();
            PositionManage pmanage = new PositionManage();
            var AjaxResultxx = new AjaxResult();
            string[] str = list.Split(',');
            try
            {
                for (int i = 0; i < str.Length - 1; i++)
                {
                    int id = int.Parse(str[i]);
                    var dept = deptmanage.GetEntity(id);
                    dept.IsDel = true;
                    deptmanage.Update(dept);
                    var plist = pmanage.GetList().Where(p => p.DeptId == id);
                    foreach (var p in plist)
                    {
                        p.IsDel = true;
                        pmanage.Update(p);
                    }

                }
                AjaxResultxx = deptmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = deptmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        //岗位表添加显示页
        public ActionResult AddPosition() {
            DepartmentManage deptmanage = new DepartmentManage();
            var deptlist = deptmanage.GetList();//获取公司部门数据集
            ViewBag.mydept = new SelectList(deptlist, "DeptId", "DeptName");
            return View();
        }
        //验证输入的岗位是否已存在
        [HttpPost]
        public ActionResult VerifyPosition(int deptid, string pname)
        {
            PositionManage deptmanage = new PositionManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                foreach (var d in deptmanage.GetList().Where(s => s.DeptId == deptid))
                {
                    if (pname == d.PositionName)
                    {
                        AjaxResultxx = deptmanage.Success();
                    }
                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = deptmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //岗位添加
        [HttpPost]
        public ActionResult AddPosition(Position p) {
            PositionManage deptmanage = new PositionManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                p.IsDel = false;
                deptmanage.Insert(p);
                AjaxResultxx = deptmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = deptmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //修改岗位表的IsDel属性
        [HttpPost]
        public ActionResult EditPositionIsDel(int id, bool isdel)
        {
            PositionManage deptmanage = new PositionManage();
            EmployeesInfoManage emanag = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var dept = deptmanage.GetEntity(id);
                if (isdel == true) {
                    dept.IsDel = isdel;
                    deptmanage.Update(dept);
                    AjaxResultxx = deptmanage.Success();
                }
                else
                {
                    if (emanag.GetDept(id).IsDel == true)
                    {
                        AjaxResultxx = deptmanage.Success();
                        AjaxResultxx.Msg = "不能启用";
                    }
                    else {
                        dept.IsDel = isdel;
                        deptmanage.Update(dept);
                        AjaxResultxx = deptmanage.Success();
                    }
                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = deptmanage.Error(ex.Message);
            }

            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //岗位伪删除即 将岗位禁用
        [HttpPost]
        public ActionResult DelPositions(string list)
        {
            PositionManage deptmanage = new PositionManage();
            var AjaxResultxx = new AjaxResult();
            string[] str = list.Split(',');
            try
            {
                for (int i = 0; i < str.Length; i++)
                {
                    var dept = deptmanage.GetEntity(str[i]);
                    dept.IsDel = true;
                    deptmanage.Update(dept);

                }
                AjaxResultxx = deptmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = deptmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }


        //员工婚姻状态和性别的修改 
        public ActionResult EditEmphunyin(string id, string name, bool ismarry)
        {
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                switch (name)
                {
                    case "IdCardIndate":
                        var emp1 = empinfo.GetInfoByEmpID(id);
                        if (ismarry == false)
                        {
                            emp1.MaritalStatus = true;
                        }
                        else
                        {
                            emp1.MaritalStatus = false;
                        }
                        empinfo.Update(emp1);
                        break;
                    case "Sex":
                        var emp2 = empinfo.GetInfoByEmpID(id);
                        if (ismarry == false)
                        {
                            emp2.Sex = "女";
                        }
                        else
                        {
                            emp2.Sex = "男";
                        }
                        empinfo.Update(emp2);
                        break;
                }
                AjaxResultxx = empinfo.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = empinfo.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //员工信息列表单元格编辑（文本框形式）
        public ActionResult EditTableCell(string id, string Attrbute, string endvalue) {
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                switch (Attrbute)
                {
                    case "EmpName"://员工姓名
                        var emp1 = empinfo.GetInfoByEmpID(id);
                        emp1.EmpName = endvalue;
                        empinfo.Update(emp1);
                        break;
                    case "Phone"://员工电话号码
                        var emp2 = empinfo.GetInfoByEmpID(id);
                        emp2.Phone = endvalue;
                        empinfo.Update(emp2);
                        break;
                    case "IdCardNum"://员工身份证号码
                        var emp12 = empinfo.GetInfoByEmpID(id);
                        emp12.IdCardNum = endvalue;
                        if (emp12.IdCardNum != null)
                        {
                            emp12.Birthdate = DateTime.Parse(GetBirth(emp12.IdCardNum));
                            emp12.Age = Convert.ToInt32(GetAge((DateTime)emp12.Birthdate, DateTime.Now));
                        }
                        empinfo.Update(emp12);
                        break;
                    case "Remark"://备注
                        var emp11 = empinfo.GetInfoByEmpID(id);
                        emp11.Remark = endvalue;
                        empinfo.Update(emp11);
                        break;
                }
                AjaxResultxx = empinfo.Success();
            }
            catch (Exception ex)
            {
                empinfo.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //员工信息列表时间单元格编辑
        public ActionResult EditDateCell(string id, DateTime endvalue) {
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();
            try
            {

                        var emp2 = empinfo.GetInfoByEmpID(id);
                        emp2.SSStartMonth = endvalue;
                        empinfo.Update(emp2);
                        AjaxResultxx = empinfo.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = empinfo.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 员工信息详情页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EmpDetail(string id) {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
             var emp= empmanage.GetInfoByEmpID(id);
            return View(emp);
        }

        /// <summary>
        /// 根据员工编号获取对应员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetempById(string id) {
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var e = emanage.GetInfoByEmpID(id);
            var empobj = new {
                #region 获取属性值 
                e.EmployeeId,
                e.DDAppId,
                e.EmpName,
                e.PositionId,
                pname = emanage.GetPosition((int)e.PositionId).PositionName,
                dname = emanage.GetDept((int)e.PositionId).DeptName,
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
            return Json(empobj,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编辑员工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditEmp(string id) {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
             var emp= empmanage.GetInfoByEmpID(id);
            return View(emp);
        }
        [HttpPost]
        public ActionResult EditEmp(EmployeesInfo emp) {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var ajaxresult= new AjaxResult();
            try
            {
                var emp2 = empmanage.GetInfoByEmpID(emp.EmployeeId);
                emp.Birthdate = DateTime.Parse(GetBirth(emp.IdCardNum));
                emp.Age = Convert.ToInt32(GetAge((DateTime)emp.Birthdate, DateTime.Now));
                emp.IsDel = emp2.IsDel;
                empmanage.Update(emp);
                ajaxresult = empmanage.Success();
            }
            catch (Exception ex)
            {
                ajaxresult = empmanage.Error(ex.Message);
            }
            return Json(ajaxresult,JsonRequestBehavior.AllowGet);
        }


        //员工异动表显示页
        public ActionResult EmpTransactionRecord() {
            return View();
        }
        //获取员工异动表数据
        public ActionResult GetEtrData(int page,int limit) {
            EmpTransactionManage etmanage = new EmpTransactionManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var list = etmanage.GetList().ToList();
            var mylist = list.OrderBy(e => e.TransactionId).Skip((page - 1) * limit).Take(limit).ToList();
            var etlist = from e in mylist
                         select new
                         {
                             e.TransactionId,
                             empName = emanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             type = emanage.GetETById(e.TransactionType).MoveTypeName,
                             e.TransactionTime,
                             predname= e.PreviousDept==null?null:emanage.GetDeptById((int)e.PreviousDept).DeptName,
                             prepname=e.PreviousPosition==null?null:emanage.GetPobjById((int)e.PreviousPosition).PositionName,
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
        public ActionResult EditETR(int id) {
            return View();
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
                dname=emanage.GetDept(emanage.GetInfoByEmpID(e.EmployeeId).PositionId).DeptName,
                pname= emanage.GetPosition(emanage.GetInfoByEmpID(e.EmployeeId).PositionId).PositionName,
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
        //员工异动详情信息
        public object EmpETRDetail(int id) {
            EmpTransactionManage etmanage = new EmpTransactionManage();
            MoveTypeManage mt = new MoveTypeManage();
            object viewobj = new object();
             var etobj=etmanage.GetEntity(id);

            var mtobj1 = mt.GetList().Where(s => s.MoveTypeName == "转正").FirstOrDefault();//找到转正申请的异动
            if (etobj.TransactionType==mtobj1.ID) {
                viewobj = PositiveDetail(id);
            }
            var mtobj2 = mt.GetList().Where(s => s.MoveTypeName == "离职").FirstOrDefault();//找到离职申请的异动
            if (etobj.TransactionType == mtobj2.ID) {
                viewobj = DimissionDetail(id);
            }
            return viewobj;
        }
        //转正异动的详情页
        public ActionResult PositiveDetail(int id)
        {
            EmpTransactionManage etmanage = new EmpTransactionManage();
            var etobj = etmanage.GetEntity(id);
            return View(etobj);
        }
        //离职异动的详情页
        public ActionResult DimissionDetail(int id) {
            EmpTransactionManage etmanage = new EmpTransactionManage();
            var etobj = etmanage.GetEntity(id);
            return View(etobj);
        }


        //员工审批状态管理
        public ActionResult EmpApproval() {
            return View();
        }
    }
}