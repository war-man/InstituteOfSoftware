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
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Business.SchoolAttendanceManagementBusiness;
    using SiliconValley.InformationSystem.Business.FinanceBusiness;
    using SiliconValley.InformationSystem.Entity.Entity;
    using SiliconValley.InformationSystem.Business.EducationalBusiness;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
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
            if (!string.IsNullOrEmpty(AppCondition))
            {
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
                if (!string.IsNullOrEmpty(deptname))
                {

                    list = list.Where(e => empinfo.GetDept((int)e.PositionId).DeptId == int.Parse(deptname)).ToList();
                }
                if (!string.IsNullOrEmpty(pname))
                {
                    list = list.Where(e => e.PositionId == int.Parse(pname)).ToList();
                }
                if (!string.IsNullOrEmpty(Education))
                {
                    list = list.Where(e => e.Education == Education).ToList();
                }
                if (!string.IsNullOrEmpty(sex))
                {
                    list = list.Where(e => e.Sex == sex).ToList();
                }
                if (!string.IsNullOrEmpty(PoliticsStatus))
                {
                    list = list.Where(e => e.PoliticsStatus == PoliticsStatus).ToList();
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
                              Position = empinfo.GetPosition((int)e.PositionId).PositionName,
                              Depart = empinfo.GetDept((int)e.PositionId).DeptName,
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
        public ActionResult GetDelEmpData(int page, int limit)
        {
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
                Depart = empinfo.GetDept((int)e1.PositionId).DeptName,
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
        public ActionResult AddEmp()
        {
            return View();
        }
        /// <summary>
        ///添加员工时获取未禁用的部门信息 
        /// </summary>
        /// <returns></returns>
        public ActionResult BindDeptSelect()
        {
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
        public ActionResult BindPositionSelect(int deptid)
        {
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
        public ActionResult AddEmpInfo(EmployeesInfo emp)
        {
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();
            EmploymentStaffBusiness esmanage = new EmploymentStaffBusiness();
            HeadmasterBusiness hm = new HeadmasterBusiness();
            ConsultTeacherManeger cmanage = new ConsultTeacherManeger();
            ChannelStaffBusiness csmanage = new ChannelStaffBusiness();
            TeacherBusiness teamanage = new TeacherBusiness();
            FinanceModelBusiness fmmanage = new FinanceModelBusiness();
            try
            {
                emp.EmployeeId = EmpId();
                if (emp.IdCardNum != null)
                {
                    emp.Birthdate = DateTime.Parse(GetBirth(emp.IdCardNum));

                }
                if (emp.Birthdate != null)
                {
                    emp.Age = Convert.ToInt32(GetAge((DateTime)emp.Birthdate, DateTime.Now));
                }
                emp.IsDel = false;
                empinfo.Insert(emp);
                if (empinfo.GetDept(emp.PositionId).DeptName == "就业部")
                {
                    bool s = esmanage.AddEmploystaff(emp.EmployeeId);
                    AjaxResultxx = empinfo.Success();
                    AjaxResultxx.Success = s;
                }
                if (empinfo.GetDept(emp.PositionId).DeptName == "市场部")
                {
                    bool s = csmanage.AddChannelStaff(emp.EmployeeId);
                    AjaxResultxx = empinfo.Success();
                    AjaxResultxx.Success = s;
                }
                if (empinfo.GetDept(emp.PositionId).DeptName == "教质部")
                {
                    bool s = hm.AddHeadmaster(emp.EmployeeId);
                    AjaxResultxx = empinfo.Success();
                    AjaxResultxx.Success = s;
                }
                if (empinfo.GetPosition(emp.PositionId).PositionName == "咨询师" || empinfo.GetPosition(emp.PositionId).PositionName == "咨询主任")
                {
                    bool s = cmanage.AddConsultTeacherData(emp.EmployeeId);
                    AjaxResultxx = empinfo.Success();
                    AjaxResultxx.Success = s;
                }
                if (empinfo.GetDept(emp.PositionId).DeptName == "教学部")
                {
                    Teacher tea = new Teacher();
                    tea.EmployeeId = emp.EmployeeId;
                    bool s = teamanage.AddTeacher(tea);
                    AjaxResultxx = empinfo.Success();
                    AjaxResultxx.Success = s;
                }
                if (empinfo.GetDept(emp.PositionId).DeptName == "财务部")
                {
                    bool s = fmmanage.AddFinancialstaff(emp.EmployeeId);
                    AjaxResultxx = empinfo.Success();
                    AjaxResultxx.Success = s;
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
            else
            {
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
        public static string GetBirth(string idnum)
        {
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
        //将所有部门及岗位用树形格式显示
        public ActionResult GetDeptAndPosition()
        {
            DepartmentManage deptmanage = new DepartmentManage();
            PositionManage pmanage = new PositionManage();
            int mygrade = 1;
            List<TreeClass> list_Tree = deptmanage.GetList().Where(s => s.IsDel == false).Select(d => new TreeClass() { id = d.DeptId.ToString(), title = d.DeptName, children = new List<TreeClass>(), disable = false, @checked = false, spread = false, grade = mygrade }).ToList();
            List<Position> list_Position = pmanage.GetList().Where(s => s.IsDel == false).ToList();//获取所有岗位有用的数据
            foreach (TreeClass item1 in list_Tree)
            {
                List<TreeClass> bigTree = new List<TreeClass>();
                foreach (Position item2 in list_Position)
                {
                    if (item1.id == item2.DeptId.ToString())
                    {
                        TreeClass tcc2 = new TreeClass();
                        tcc2.id = item2.Pid.ToString();
                        tcc2.title = item2.PositionName;
                        tcc2.grade = 2;
                        bigTree.Add(tcc2);
                        item1.children = bigTree;
                    }
                }
            }
            return Json(list_Tree, JsonRequestBehavior.AllowGet);
        }

        //部门增加显示页
        public ActionResult DeptUpdate()
        {
            return View();
        }
        //验证添加的部门是否已存在
        [HttpPost]
        public ActionResult VerifyDname(string dname)
        {
            DepartmentManage deptmanage = new DepartmentManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                foreach (var d in deptmanage.GetList())
                {
                    if (dname == d.DeptName && d.IsDel == false)
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
        [HttpPost]
        public ActionResult AddDept(Department dept)
        {
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
        //修改部门表属性
        [HttpPost]
        public ActionResult EditDept(int id, string dname)
        {
            DepartmentManage deptmanage = new DepartmentManage();
            PositionManage pmanage = new PositionManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var dept = deptmanage.GetEntity(id);
                dept.DeptName = dname;
                deptmanage.Update(dept);
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
        public ActionResult DelDepts(int id)
        {
            DepartmentManage deptmanage = new DepartmentManage();
            PositionManage pmanage = new PositionManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var dept = deptmanage.GetEntity(id);
                dept.IsDel = true;
                deptmanage.Update(dept);
                AjaxResultxx = deptmanage.Success();
                if (AjaxResultxx.Success)
                {
                    var plist = pmanage.GetList().Where(p => p.DeptId == id).ToList();
                    foreach (var p in plist)
                    {
                        p.IsDel = true;
                        pmanage.Update(p);
                        AjaxResultxx = pmanage.Success();
                    }
                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = deptmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        //岗位表添加显示页
        public ActionResult AddPosition()
        {
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
        public ActionResult AddPosition(Position p)
        {
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
        //修改岗位表属性
        [HttpPost]
        public ActionResult EditPosition(int id, string pname)
        {
            PositionManage pmanage = new PositionManage();
            EmployeesInfoManage emanag = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var dept = pmanage.GetEntity(id);
                dept.PositionName = pname;
                pmanage.Update(dept);
                AjaxResultxx = pmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = pmanage.Error(ex.Message);
            }

            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //岗位伪删除即 将岗位禁用
        [HttpPost]
        public ActionResult DelPositions(int id)
        {
            PositionManage deptmanage = new PositionManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var dept = deptmanage.GetEntity(id);
                dept.IsDel = true;
                deptmanage.Update(dept);
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
        public ActionResult EditTableCell(string id, string Attrbute, string endvalue)
        {
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
        public ActionResult EditDateCell(string id, DateTime endvalue)
        {
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
        public ActionResult EmpDetail(string id)
        {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var emp = empmanage.GetInfoByEmpID(id);
            return View(emp);
        }

        /// <summary>
        /// 根据员工编号获取对应员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetempById(string id)
        {
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var e = emanage.GetInfoByEmpID(id);
            var empobj = new
            {
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
            return Json(empobj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编辑员工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditEmp(string id)
        {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var emp = empmanage.GetInfoByEmpID(id);
            ViewBag.dname = empmanage.GetDept(emp.PositionId).DeptName;
            ViewBag.deptid = empmanage.GetDept(emp.PositionId).DeptId;
            ViewBag.pname = empmanage.GetPosition(emp.PositionId).PositionName;
            ViewBag.pid = empmanage.GetPosition(emp.PositionId).Pid;
            return View(emp);
        }
        [HttpPost]
        public ActionResult EditEmp(EmployeesInfo emp)
        {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var ajaxresult = new AjaxResult();
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
            return Json(ajaxresult, JsonRequestBehavior.AllowGet);
        }




        //员工异动表显示页
        public ActionResult EmpTransactionRecord()
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
                    var mtype2 = mt.GetList().Where(s => s.MoveTypeName == "离职").FirstOrDefault().ID;
                    var mtype3 = mt.GetList().Where(s => s.MoveTypeName == "调岗").FirstOrDefault().ID;
                    var emp = empmanage.GetEntity(e.EmployeeId);
                    if (ajaxresult.Success && e.TransactionType == mtype1)//当异动时间修改好之后且是转正异动的情况下将该员工的转正日期修改
                    {
                        emp.PositiveDate = e.TransactionTime;
                        empmanage.Update(emp);
                        ajaxresult = empmanage.Success();
                    }
                    else if (ajaxresult.Success && e.TransactionType == mtype2)//当异动时间修改好之后且是离职异动的情况下将该员工的在职状态改为离职状态
                    {
                        emp.IsDel = true;
                        empmanage.Update(emp);
                        ajaxresult = empmanage.Success();
                        if (ajaxresult.Success)
                        {
                            switch (empmanage.GetDept(emp.PositionId).DeptName)
                            {
                                case "财务部":
                                    FinanceModelBusiness fmmanage = new FinanceModelBusiness();
                                    bool fm = fmmanage.UpdateFinancialstaff(emp.EmployeeId);
                                    ajaxresult = empmanage.Success();
                                    ajaxresult.Success = fm;
                                    break;
                                case "市场部":
                                    ChannelStaffBusiness csmanage = new ChannelStaffBusiness();
                                    bool cs = csmanage.DelChannelStaff(emp.EmployeeId);
                                    ajaxresult = empmanage.Success();
                                    ajaxresult.Success = cs;
                                    break;
                                case "就业部":
                                    EmploymentStaffBusiness esmanage = new EmploymentStaffBusiness();
                                    bool es = esmanage.DelEmploystaff(emp.EmployeeId);
                                    ajaxresult = empmanage.Success();
                                    ajaxresult.Success = es;
                                    break;
                                case "咨询部":
                                    ConsultTeacherManeger ctmanage = new ConsultTeacherManeger();
                                    bool ct = ctmanage.DeltConsultTeacher(emp.EmployeeId);
                                    ajaxresult = empmanage.Success();
                                    ajaxresult.Success = ct;
                                    break;
                                case "教质部":
                                    HeadmasterBusiness hmmanage = new HeadmasterBusiness();
                                    bool hm = hmmanage.QuitEntity(emp.EmployeeId);
                                    ajaxresult = empmanage.Success();
                                    ajaxresult.Success = hm;
                                    break;
                                case "教学部":
                                    TeacherBusiness tmanage = new TeacherBusiness();
                                    bool t = tmanage.dimission(emp.EmployeeId);
                                    ajaxresult = empmanage.Success();
                                    ajaxresult.Success = t;
                                    break;
                            }

                        }
                    }
                    else if (ajaxresult.Success && e.TransactionType == mtype3)//当异动时间修改好之后且是调岗异动的情况下将该员工的在职状态改为离职状态
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

        //员工审批状态管理
        public ActionResult EmpApproval()
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
                        if (ajaxresult.Success)//转正申请通过修改成功之后，将该条员工异动情况添加到员工异动表中
                        {
                            et.EmployeeId = positive.EmployeeId;
                            et.IsDel = false;
                            et.TransactionType = m.GetList().Where(s => s.MoveTypeName == "转正").FirstOrDefault().ID;
                            etmanage.Insert(et);
                            ajaxresult = etmanage.Success();
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
                    try
                    {
                        if (ajaxresult.Success)//离职申请通过修改成功之后，将该条员工异动情况添加到员工异动表中
                        {
                            et.EmployeeId = positive.EmployeeId;
                            et.IsDel = false;
                            et.TransactionType = m.GetList().Where(s => s.MoveTypeName == "离职").FirstOrDefault().ID;
                            et.Reason = positive.DimissionReason;
                            etmanage.Insert(et);
                            ajaxresult = etmanage.Success();
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
                             presalary = emanage.GetInfoByEmpID(e.EmployeeId).Salary == null ? emanage.GetInfoByEmpID(e.EmployeeId).ProbationSalary : emanage.GetInfoByEmpID(e.EmployeeId).Salary,//未转正的情况下员工工资指的是实习工资
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
                            et.PreviousSalary = emanage.GetInfoByEmpID(et.EmployeeId).Salary == null ? emanage.GetInfoByEmpID(et.EmployeeId).ProbationSalary : emanage.GetInfoByEmpID(et.EmployeeId).Salary;
                            et.PresentDept = positive.PlanTurnDeptId;
                            et.PresentPosition = positive.PlanTurnPositionId;
                            et.PresentSalary = positive.TurnAfterSalary;
                            et.Reason = positive.Reason;
                            etmanage.Insert(et);
                            ajaxresult = etmanage.Success();
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
                             presalary = emanage.GetInfoByEmpID(e.EmployeeId).Salary == null ? emanage.GetInfoByEmpID(e.EmployeeId).ProbationSalary : emanage.GetInfoByEmpID(e.EmployeeId).Salary,//未转正的情况下员工工资指的是实习工资
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
                        if (ajaxresult.Success)//加薪申请通过修改成功之后，将该条员工异动情况添加到员工异动表中
                        {
                            et.EmployeeId = positive.EmployeeId;
                            et.IsDel = false;
                            et.TransactionType = m.GetList().Where(s => s.MoveTypeName == "加薪").FirstOrDefault().ID;
                            et.PreviousSalary = emanage.GetInfoByEmpID(et.EmployeeId).Salary == null ? emanage.GetInfoByEmpID(et.EmployeeId).ProbationSalary : emanage.GetInfoByEmpID(et.EmployeeId).Salary;
                            et.PresentSalary = (decimal)et.PreviousSalary + (decimal)positive.RaisesLimit;
                            et.Reason = positive.RaisesReason;
                            etmanage.Insert(et);
                            ajaxresult = etmanage.Success();
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



        //加班记录及管理
        public ActionResult EmpOvertimeRecord()
        {
            return View();
        }
        /// <summary>
        /// 获取未审批的加班记录
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult GetOverTimeData(int page, int limit)
        {
            OvertimeRecordManage otrmanage = new OvertimeRecordManage();
            BeOnDutyManeger bodmanage = new BeOnDutyManeger();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var list = otrmanage.GetList();

            var newlist = list.Where(s => s.IsApproval == false).OrderByDescending(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var etlist = from e in newlist
                         select new
                         {
                             #region 获取属性值 
                             e.Id,
                             e.EmployeeId,
                             empName = emanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             e.StartTime,
                             e.EndTime,
                             e.Duration,
                             e.OvertimeReason,
                             typename = bodmanage.GetSingleBeOnButy(e.OvertimeTypeId.ToString(), true).TypeName,
                             e.Remark,
                             e.IsNoDaysOff,
                             e.IsPassYear,
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
        /// 获取已审批且未过年限（及当下年份）的加班记录
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult GetOverTimeApprovedData(int page, int limit, string AppCondition)
        {
            OvertimeRecordManage otrmanage = new OvertimeRecordManage();
            BeOnDutyManeger bodmanage = new BeOnDutyManeger();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var list = otrmanage.GetList().Where(s => s.IsApproval == true && s.IsPassYear == false).ToList();
            if (!string.IsNullOrEmpty(AppCondition))
            {
                string[] str = AppCondition.Split(',');
                string ename = str[0];
                string IsNoDaysOff = str[1];
                string YearSelect = str[2];
                string MonthSelect = str[3];
                list = list.Where(e => emanage.GetEntity(e.EmployeeId).EmpName.Contains(ename)).ToList();
                if (!string.IsNullOrEmpty(IsNoDaysOff))
                {
                    list = list.Where(e => e.IsNoDaysOff == bool.Parse(IsNoDaysOff)).ToList();
                }
                if (!string.IsNullOrEmpty(YearSelect))
                {
                    list = list.Where(e => DateTime.Parse(e.StartTime.ToString()).Year == int.Parse(YearSelect)).ToList();
                }
                if (!string.IsNullOrEmpty(MonthSelect))
                {
                    var year = DateTime.Parse(MonthSelect).Year;
                    var month = DateTime.Parse(MonthSelect).Month;
                    list = list.Where(e => DateTime.Parse(e.StartTime.ToString()).Year == year && DateTime.Parse(e.StartTime.ToString()).Month == month).ToList();
                }
            }
            var newlist = list.OrderByDescending(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var etlist = from e in newlist
                         select new
                         {
                             #region 获取属性值 
                             e.Id,
                             e.EmployeeId,
                             empName = emanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             e.StartTime,
                             e.EndTime,
                             e.Duration,
                             e.OvertimeReason,
                             typename = bodmanage.GetSingleBeOnButy(e.OvertimeTypeId.ToString(), true).TypeName,
                             e.Remark,
                             e.IsNoDaysOff,
                             e.IsPassYear,
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
        //审批加班申请
        [HttpPost]
        public ActionResult overtimeIsPassed(int id, bool state)
        {
            OvertimeRecordManage otrmanage = new OvertimeRecordManage();
            var ajaxresult = new AjaxResult();
            try
            {
                var otr = otrmanage.GetEntity(id);
                otr.IsApproval = true;
                otr.IsPass = state;
                otrmanage.Update(otr);
                ajaxresult = otrmanage.Success();
            }
            catch (Exception ex)
            {
                ajaxresult = otrmanage.Error(ex.Message);
            }
            return Json(ajaxresult, JsonRequestBehavior.AllowGet);
        }
        //根据编号获取加班记录对象
        public ActionResult GetOvertimeById(int id)
        {
            OvertimeRecordManage otrmanage = new OvertimeRecordManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            BeOnDutyManeger bodmanage = new BeOnDutyManeger();
            var otobj = otrmanage.GetEntity(id);
            var empobj = new
            {
                #region 获取属性值 
                otobj.Id,
                otobj.EmployeeId,
                empName = emanage.GetInfoByEmpID(otobj.EmployeeId).EmpName,
                otobj.StartTime,
                otobj.EndTime,
                otobj.Duration,
                otobj.OvertimeReason,
                otobj.OvertimeTypeId,
                otobj.Remark,
                otobj.IsNoDaysOff,
                otobj.IsPassYear,
                otobj.IsApproval,
                otobj.IsPass
                #endregion
            };
            return Json(empobj, JsonRequestBehavior.AllowGet);
        }
        //加班编辑页面
        public ActionResult OvertimeEdit(int id)
        {
            OvertimeRecordManage otrmanage = new OvertimeRecordManage();
            var otobj = otrmanage.GetEntity(id);
            ViewBag.Id = id;
            return View(otobj);
        }
        //加班编辑的提交
        [HttpPost]
        public ActionResult OvertimeEdit(OvertimeRecord otr)
        {
            OvertimeRecordManage otrmanage = new OvertimeRecordManage();
            var ajaxresult = new AjaxResult();
            try
            {
                var myotr = otrmanage.GetEntity(otr.Id);
                otr.IsPass = myotr.IsPass;
                otr.IsApproval = myotr.IsApproval;
                otr.IsPassYear = myotr.IsPassYear;
                otrmanage.Update(otr);
                ajaxresult = otrmanage.Success();
            }
            catch (Exception ex)
            {
                ajaxresult = otrmanage.Error(ex.Message);
            }

            return Json(ajaxresult, JsonRequestBehavior.AllowGet);
        }
        //修改加班的过了年限的数据，将 是否过了年限 属性进行修改
        [HttpPost]
        public ActionResult EditIsPassYear(string list)
        {
            OvertimeRecordManage otrmanage = new OvertimeRecordManage();
            var ajaxresult = new AjaxResult();
            string[] arr = list.Split(',');
            for (int i = 0; i < arr.Length - 1; i++)
            {
                try
                {
                    int id = int.Parse(arr[i]);
                    var otr = otrmanage.GetEntity(id);
                    otr.IsPassYear = true;
                    otrmanage.Update(otr);
                    ajaxresult = otrmanage.Success();

                }
                catch (Exception ex)
                {
                    ajaxresult = otrmanage.Error(ex.Message);
                }
            }

            return Json(ajaxresult, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 获取未审批的调休记录
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult GetDaysOffData(int page, int limit)
        {
            DaysOffManage dfmanage = new DaysOffManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var list = dfmanage.GetList();

            var newlist = list.Where(s => s.IsApproval == false).OrderByDescending(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var etlist = from e in newlist
                         select new
                         {
                             #region 获取属性值 
                             e.Id,
                             e.EmployeeId,
                             empName = emanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             e.StartTime,
                             e.EndTime,
                             e.Duration,
                             e.LeaveReason,
                             e.IsPassYear,
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
        /// 获取已审批且未过年限（及当下年份）的调休记录
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult GetDaysOffApprovedData(int page, int limit, string AppCondition)
        {
            DaysOffManage dfmanage = new DaysOffManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var list = dfmanage.GetList().Where(s => s.IsApproval == true && s.IsPassYear == false).ToList();
            if (!string.IsNullOrEmpty(AppCondition))
            {
                string[] str = AppCondition.Split(',');
                string ename = str[0];
                string IsPass = str[1];
                string YearSelect = str[2];
                string MonthSelect = str[3];
                list = list.Where(e => emanage.GetEntity(e.EmployeeId).EmpName.Contains(ename)).ToList();
                if (!string.IsNullOrEmpty(IsPass))
                {
                    list = list.Where(e => e.IsPass == bool.Parse(IsPass)).ToList();
                }
                if (!string.IsNullOrEmpty(YearSelect))
                {
                    list = list.Where(e => DateTime.Parse(e.StartTime.ToString()).Year == int.Parse(YearSelect)).ToList();
                }
                if (!string.IsNullOrEmpty(MonthSelect))
                {
                    var year = DateTime.Parse(MonthSelect).Year;
                    var month = DateTime.Parse(MonthSelect).Month;
                    list = list.Where(e => DateTime.Parse(e.StartTime.ToString()).Year == year && DateTime.Parse(e.StartTime.ToString()).Month == month).ToList();
                }
            }
            var newlist = list.OrderByDescending(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var etlist = from e in newlist
                         select new
                         {
                             #region 获取属性值 
                             e.Id,
                             e.EmployeeId,
                             empName = emanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             e.StartTime,
                             e.EndTime,
                             e.Duration,
                             e.LeaveReason,
                             e.IsPassYear,
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
        //根据编号获取调休申请对象
        public ActionResult GetDaysOffById(int id)
        {
            DaysOffManage dfmanage = new DaysOffManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var mydf = dfmanage.GetEntity(id);
            var empobj = new
            {
                #region 获取属性值 
                mydf.Id,
                mydf.EmployeeId,
                empName = emanage.GetInfoByEmpID(mydf.EmployeeId).EmpName,
                mydf.StartTime,
                mydf.EndTime,
                mydf.Duration,
                mydf.LeaveReason,
                Image = mydf.Image,
                mydf.IsPassYear,
                mydf.IsApproval,
                mydf.IsPass
                #endregion
            };
            return Json(empobj, JsonRequestBehavior.AllowGet);
        }

        //审批调休申请
        [HttpPost]
        public ActionResult DaysoffIsPassed(int id, bool state)
        {
            DaysOffManage dfmanage = new DaysOffManage();
            var ajaxresult = new AjaxResult();
            try
            {
                var otr = dfmanage.GetEntity(id);
                otr.IsApproval = true;
                otr.IsPass = state;
                dfmanage.Update(otr);
                ajaxresult = dfmanage.Success();
            }
            catch (Exception ex)
            {
                ajaxresult = dfmanage.Error(ex.Message);
            }
            return Json(ajaxresult, JsonRequestBehavior.AllowGet);
        }
        //调休申请的详情页
        public ActionResult DaysOffDetail(int id)
        {
            DaysOffManage dfmanage = new DaysOffManage();
            var mydf = dfmanage.GetEntity(id);
            ViewBag.Id = id;
            return View(mydf);
        }
        //已审批的调休申请的编辑
        public ActionResult DaysOffEdit(int id)
        {
            DaysOffManage dfmanage = new DaysOffManage();
            var mydf = dfmanage.GetEntity(id);
            ViewBag.Id = id;
            return View(mydf);
        }
        [HttpPost]
        public ActionResult DaysOffEdit(DaysOff df)
        {
            DaysOffManage dfmanage = new DaysOffManage();
            var ajaxresult = new AjaxResult();
            try
            {
                var myotr = dfmanage.GetEntity(df.Id);
                myotr.Duration = df.Duration;
                dfmanage.Update(myotr);
                ajaxresult = dfmanage.Success();
            }
            catch (Exception ex)
            {
                ajaxresult = dfmanage.Error(ex.Message);
            }
            return Json(ajaxresult, JsonRequestBehavior.AllowGet);
        }
        //修改调休的过了年限的数据，将 是否过了年限 属性进行修改
        public ActionResult EditDaysOffIsPassYear(string list)
        {
            DaysOffManage dfmanage = new DaysOffManage();
            var ajaxresult = new AjaxResult();
            string[] arr = list.Split(',');
            for (int i = 0; i < arr.Length - 1; i++)
            {
                try
                {
                    int id = int.Parse(arr[i]);
                    var otr = dfmanage.GetEntity(id);
                    otr.IsPassYear = true;
                    dfmanage.Update(otr);
                    ajaxresult = dfmanage.Success();
                }
                catch (Exception ex)
                {
                    ajaxresult = dfmanage.Error(ex.Message);
                }
            }

            return Json(ajaxresult, JsonRequestBehavior.AllowGet);
        }


        public bool Check(List<MyStaticsData> mydata, OvertimeRecord otr) {
            foreach (var item in mydata)
            {
                if (item.EmployeeId == otr.EmployeeId)
                {
                    item.OvertimeTotaltime += otr.Duration;//可调休总时间
                    item.ResidueDaysoffTime = item.OvertimeTotaltime - item.DaysoffTotaltime;
                      return true;   
                }
               
            }
             return false;
          
        }
        public bool Check2(List<MyStaticsData> mydata, DaysOff dff)
        {
            foreach (var item in mydata)
            {
                if (item.EmployeeId == dff.EmployeeId)
                {
                    item.DaysoffTotaltime += dff.Duration;//已调休总时间
                    item.ResidueDaysoffTime = item.OvertimeTotaltime - item.DaysoffTotaltime;
                    return true;
                }

            }

            return false;
        }

         

        /// <summary>
        ///获取加班及调休的统计数据 (首先按年份分别找到每个人的加班总时长和调休总时长)
        /// </summary>
        /// <returns></returns>
        public ActionResult GetStatisticsTimeData(int page, int limit)
        {
            OvertimeRecordManage otrmanage = new OvertimeRecordManage();
            DaysOffManage dfmanage = new DaysOffManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            List<MyStaticsData> Statisticslist = new List<MyStaticsData>();
            var otrlist = otrmanage.GetList().Where(s => s.IsNoDaysOff == false && s.IsPassYear==false && s.IsPass==true).ToList();
            var dflist = dfmanage.GetList().Where(s => s.IsPassYear == false && s.IsPass==true).ToList();
            foreach (var item in otrlist)
            {
            if (!Check(Statisticslist,item)) {
                    MyStaticsData msd = new MyStaticsData();
                    msd.EmployeeId = item.EmployeeId;
                    msd.YearTime = DateTime.Parse(item.EndTime.ToString()).Year;
                    msd.OvertimeTotaltime = item.Duration;
                    msd.DaysoffTotaltime = 0;
                    msd.ResidueDaysoffTime = msd.OvertimeTotaltime - msd.DaysoffTotaltime;
                    Statisticslist.Add(msd);
                }

            }
         
            foreach (var item in dflist)
            {
                if (!Check2(Statisticslist,item)) {
                    MyStaticsData msd = new MyStaticsData();
                    msd.EmployeeId = item.EmployeeId;
                    msd.YearTime = DateTime.Parse(item.EndTime.ToString()).Year;
                    msd.OvertimeTotaltime = 0;
                    msd.DaysoffTotaltime = item.Duration;
                    msd.ResidueDaysoffTime = msd.OvertimeTotaltime - msd.DaysoffTotaltime;
                    Statisticslist.Add(msd);
                }
            }
            var newlist = Statisticslist.OrderByDescending(s => s.YearTime).Skip((page - 1) * limit).Take(limit).ToList();

            var newobj = from ss in newlist
                         select new
                         {
                             empName = emanage.GetEntity(ss.EmployeeId).EmpName,
                             ss.EmployeeId,
                             ss.OvertimeTotaltime,
                             ss.DaysoffTotaltime,
                             ss.ResidueDaysoffTime,
                             ss.YearTime
                         };

            var obj = new
            {
                code = 0,
                msg = "",
                count = Statisticslist.Count(),
                data = newobj
            };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}