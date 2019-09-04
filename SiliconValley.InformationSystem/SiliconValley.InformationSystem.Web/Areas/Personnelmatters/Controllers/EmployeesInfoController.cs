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
    public class EmployeesInfoController : Controller
    {
        // GET: Personnelmatters/EmployeesInfo
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        ///获取员工信息数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="AppCondition"></param>
        /// <returns></returns>
        public ActionResult GetData(int page,int limit,string AppCondition)
        {
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var list = empinfo.GetList();
            if (!string.IsNullOrEmpty(AppCondition)) {
                string[] str = AppCondition.Split(',');
                string ename = str[0];
                string deptname = str[1];
                string pname = str[2];
                string IsDel = str[3];
                string start_time = str[4];
                string end_time = str[5];
                list = list.Where(e => e.EmpName.Contains(ename)).ToList();
                if (!string.IsNullOrEmpty(deptname)) {
                   
                    list = list.Where(e => GetDept((int)e.PositionId).DeptId== int.Parse(deptname)).ToList();
                }
                if (!string.IsNullOrEmpty(pname)) {
                    list = list.Where(e => e.PositionId==int.Parse(pname)).ToList();
                }
                if (!string.IsNullOrEmpty(IsDel)) {
                    list = list.Where(e => e.IsDel.Equals(bool.Parse(IsDel))).ToList();
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
            return Json(newobj,JsonRequestBehavior.AllowGet);
       }
        //获取所属岗位对象
        public Position GetPosition(int pid) {
            PositionManage pmanage = new PositionManage();
            var str = pmanage.GetEntity(pid);
            return  str;
        }
        //获取所属岗位的所属部门对象
        public Department GetDept(int pid) {
            DepartmentManage deptmanage = new DepartmentManage();
            //ar deptlist = deptmanage.GetList();//获取公司部门数据集
            //ViewBag.DeptList = deptlist;
            var str = deptmanage.GetEntity(GetPosition(pid).DeptId);
            return str;
        }

        //添加员工页面显示
       [HttpGet]
        public ActionResult AddEmp() {
            return View();
        }

        //部门及岗位管理页面显示
        [HttpGet]
        public ActionResult DeptOperation()
        {
            return View();
        }

        /// <summary>
        ///添加员工时获取未禁用的部门信息 
        /// </summary>
        /// <returns></returns>
        public ActionResult BindDeptSelect() {
            DepartmentManage deptmanage = new DepartmentManage();
            var deptlist = deptmanage.GetList().Where(d=>d.IsDel==false);//获取公司部门数据集
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
            var plist = pmanage.GetList().Where(d => d.DeptId == deptid && d.IsDel==false);
            var newstr = new
            {
                code = 0,
                msg = "",
                count = plist.Count(),
                data = plist
            };
            return Json(newstr,JsonRequestBehavior.AllowGet);
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
                if (emp.IdCardNum!=null) {
                    emp.Birthdate =DateTime.Parse(GetBirth(emp.IdCardNum));
                emp.Age = Convert.ToInt32(GetAge((DateTime)emp.Birthdate,DateTime.Now));
                }
                emp.IsDel = false;
                empinfo.Insert(emp);
                if (GetDept(emp.PositionId).DeptName == "就业部")
                {
                    bool s = esmanage.AddEmploystaff(emp.EmployeeId);
                    empinfo.Success().Success = s;
                    AjaxResultxx = empinfo.Success();
                }
                if (GetDept(emp.PositionId).DeptName == "市场部")
                {
                    bool s = csmanage.AddChannelStaff(emp.EmployeeId);
                    empinfo.Success().Success = s;
                    AjaxResultxx = empinfo.Success();
                }
                if (GetDept(emp.PositionId).DeptName == "教质部") {
                    bool s = hm.AddHeadmaster(emp.EmployeeId);
                    empinfo.Success().Success = s;
                    AjaxResultxx = empinfo.Success();
                }
                if (GetPosition(emp.PositionId).PositionName=="咨询师" || GetPosition(emp.PositionId).PositionName=="咨询主任") {
                    bool s= cmanage.AddConsultTeacherData(emp.EmployeeId);
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
            var deptlist = deptmanage.GetList();//获取公司部门数据集
          
            var newlist=from p in deptlist
                        select new {
                            p.Pid,
                            deptname=GetDept(p.Pid).DeptName,
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
                    if (dname==d.DeptName) {
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
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }
        //修改部门表的IsDel属性
        [HttpPost]
        public ActionResult EditDeptIsDel(int id,bool isdel) {
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
            
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }

        //部门伪删除即,将部门禁用
        [HttpPost]
        public ActionResult DelDepts(string list) {
            DepartmentManage deptmanage = new DepartmentManage();
            PositionManage pmanage = new PositionManage();
            var AjaxResultxx = new AjaxResult();
            string [] str = list.Split(',');
            try
            {
                for (int i = 0; i < str.Length-1; i++)
                {
                    int id = int.Parse(str[i]);
                  var dept=  deptmanage.GetEntity(id);
                    dept.IsDel = true;
                    deptmanage.Update(dept);
                    var plist = pmanage.GetList().Where(p => p.DeptId == id);
                    foreach (var p in plist)
                    {
                        p.IsDel = true;
                        pmanage.Update(p);
                    }

                }
                AjaxResultxx= deptmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = deptmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
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
        public ActionResult VerifyPosition(int deptid,string pname)
        {
            PositionManage deptmanage = new PositionManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                foreach (var d in deptmanage.GetList().Where(s=>s.DeptId==deptid))
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
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }
        //修改岗位表的IsDel属性
        [HttpPost]
        public ActionResult EditPositionIsDel(int id, bool isdel)
        {
            PositionManage deptmanage = new PositionManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var dept = deptmanage.GetEntity(id);
                if (isdel==true) {
                    dept.IsDel = isdel;
                    deptmanage.Update(dept);
                    AjaxResultxx = deptmanage.Success();
                }
                else
                {
                    if (GetDept(id).IsDel == true)
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
                        var emp1 = empinfo.GetEntity(id);
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
                        var emp2 = empinfo.GetEntity(id);
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
        public ActionResult EditTableCell(string id,string Attrbute,string endvalue) {
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                switch (Attrbute)
                {
                    case "EmpName":
                        var emp1 = empinfo.GetEntity(id);
                        emp1.EmpName = endvalue;
                      empinfo.Update(emp1);
                        break;
                    case "Phone":
                        var emp2 = empinfo.GetEntity(id);
                        emp2.Phone = endvalue;
                        empinfo.Update(emp2);
                        break;
                    case "Nation":
                        var emp3 = empinfo.GetEntity(id);
                        emp3.Nation = endvalue;
                        empinfo.Update(emp3);
                        break;
                    case "IdCardNum":
                        var emp12 = empinfo.GetEntity(id);
                        emp12.IdCardNum = endvalue;
                        if (emp12.IdCardNum != null)
                        {
                            emp12.Birthdate = DateTime.Parse(GetBirth(emp12.IdCardNum));
                            emp12.Age = Convert.ToInt32(GetAge((DateTime)emp12.Birthdate, DateTime.Now));
                        }
                        empinfo.Update(emp12);
                        break;
                    case "Birthday":
                        var emp4= empinfo.GetEntity(id);
                        emp4.Birthday = endvalue;
                        empinfo.Update(emp4);
                        break;
                    case "UrgentPhone":
                        var emp5 = empinfo.GetEntity(id);
                        emp5.UrgentPhone = endvalue;
                        empinfo.Update(emp5);
                        break;
                    case "DomicileAddress":
                        var emp6 = empinfo.GetEntity(id);
                        emp6.UrgentPhone = endvalue;
                        empinfo.Update(emp6);
                        break;
                    case "Address":
                        var emp7 = empinfo.GetEntity(id);
                        emp7.UrgentPhone = endvalue;
                        empinfo.Update(emp7);
                        break;
                    case "WorkExperience":
                        var emp8 = empinfo.GetEntity(id);
                        emp8.WorkExperience = endvalue;
                        empinfo.Update(emp8);
                        break;
                    case "BCNum":
                        var emp9 = empinfo.GetEntity(id);
                        emp9.BCNum = endvalue;
                        empinfo.Update(emp9);
                        break;
                    case "Material":
                        var emp10 = empinfo.GetEntity(id);
                        emp10.Material = endvalue;
                        empinfo.Update(emp10);
                        break;
                    case "Remark":
                        var emp11 = empinfo.GetEntity(id);
                        emp11.Remark = endvalue;
                        empinfo.Update(emp11);
                        break;
                    case "Education":
                        var emp13 = empinfo.GetEntity(id);
                        emp13.Education = endvalue;
                        empinfo.Update(emp13);
                        break;
                    case "PoliticsStatus":
                        var emp14 = empinfo.GetEntity(id);
                        emp14.PoliticsStatus = endvalue;
                        empinfo.Update(emp14);
                        break;
                }
                AjaxResultxx = empinfo.Success();
            }
            catch (Exception ex)
            {
                empinfo.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }
        //员工信息列表时间单元格编辑
        public ActionResult EditDateCell(string id, string Attrbute, DateTime endvalue) {
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                switch (Attrbute)
                {
                    case "IdCardIndate":
                        var emp1 = empinfo.GetEntity(id);
                        emp1.IdCardIndate = endvalue;
                        empinfo.Update(emp1);
                        AjaxResultxx = empinfo.Success();
                        break;
                    case "SSStartMonth":
                        var emp2 = empinfo.GetEntity(id);
                        emp2.SSStartMonth = endvalue;
                        empinfo.Update(emp2);
                        AjaxResultxx = empinfo.Success();
                        break;
                    case "ContractStartTime":
                        var emp3 = empinfo.GetEntity(id);
                            emp3.ContractStartTime = endvalue;
                            empinfo.Update(emp3);
                        AjaxResultxx = empinfo.Success();
                        break;
                    case "ContractEndTime":
                        var emp4 = empinfo.GetEntity(id);
                            emp4.ContractEndTime = endvalue;
                            empinfo.Update(emp4);
                        AjaxResultxx = empinfo.Success();
                        break;
                }
               
            }
            catch (Exception ex)
            {
                AjaxResultxx = empinfo.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }

    }
}