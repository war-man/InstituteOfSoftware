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
    using SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness;
    using SiliconValley.InformationSystem.Business.DormitoryBusiness;
    using System.Text;
    using System.IO;
    using System.Globalization;

    public class EmployeesInfoController : Controller
    {
        RedisCache rc;
        // GET: Personnelmatters/EmployeesInfo
        public ActionResult Index()
        {
            ViewBag.birth = GetTheGodOfLongevity().Count();
            ViewBag.contractEnd = ContractEndRemind().Count();
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
            var list = empinfo.GetEmpInfoData().Where(e => e.IsDel == false).ToList();
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
            var slist = empinfo.GetEmpInfoData().Where(e => e.IsDel == true).ToList();
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

        #region 添加员工相关action

        //添加员工页面显示
        [HttpGet]
        public ActionResult AddEmp()
        {
            return View();
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
            EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();
            AttendanceInfoManage attinfomanage = new AttendanceInfoManage();
            MeritsCheckManage mcmanage = new MeritsCheckManage();
            InstructorListBusiness itmanage = new InstructorListBusiness();
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
                if (emp.Image != "undefined")
                {
                    emp.Image = ImageUpload();
                }
                else {
                    emp.Image = null;
                }
                if (emp.ProbationSalary == null) {
                    emp.PositiveDate = emp.EntryTime;//当该员工的试用期工资为空时（即没有试用期)，该员工的转正时间即等同于该员工入职时间
                }
                empinfo.Insert(emp);
                rc = new RedisCache();
                rc.RemoveCache("InRedisEmpInfoData");
                AjaxResultxx = empinfo.Success();
                if (AjaxResultxx.Success) {
                    var dname = empinfo.GetDept(emp.PositionId).DeptName;//获取该员工所属部门名称
                    var pname = empinfo.GetPosition(emp.PositionId).PositionName;//获取该员工所属岗位名称
                    if (dname.Equals("就业部"))
                    {
                        bool s = esmanage.AddEmploystaff(emp.EmployeeId);//给就业部员工表添加员工
                        AjaxResultxx.Success = s;
                    }
                    if (dname.Equals("市场部"))
                    {
                        bool s = csmanage.AddChannelStaff(emp.EmployeeId);
                        AjaxResultxx.Success = s;
                    }//给市场部员工表添加员工
                    if ((dname.Equals("s1、s2教质部") || dname.Equals("s3教质部")) && !pname.Equals("教官"))
                    {
                        bool s = hm.AddHeadmaster(emp.EmployeeId);
                        AjaxResultxx.Success = s;
                    }//给两个教质部员工表添加除教官外的员工
                    if ((dname.Equals("s1、s2教质部") || dname.Equals("s3教质部")) && pname.Equals("教官"))
                    {
                        bool s = itmanage.AddInstructorList(emp.EmployeeId);
                        AjaxResultxx.Success = s;
                    }//给教官员工表添加教官
                    if (pname.Equals("咨询师") || pname.Equals("咨询主任"))
                    {
                        bool s = cmanage.AddConsultTeacherData(emp.EmployeeId);
                        AjaxResultxx.Success = s;
                    }//给咨询部员工表添加除咨询助理外的员工
                    if (dname.Equals("s1、s2教学部") || dname.Equals("s3教学部") || dname.Equals("s4教学部"))//给三个教学部员工表添加员工
                    {
                        Teacher tea = new Teacher();
                        tea.EmployeeId = emp.EmployeeId;
                        bool s = teamanage.AddTeacher(tea);
                        AjaxResultxx.Success = s;
                    }
                    if (dname.Equals("财务部"))
                    {
                        bool s = fmmanage.AddFinancialstaff(emp.EmployeeId);
                        AjaxResultxx.Success = s;
                    }//给财务部员工表添加员工
                    bool ss = esemanage.AddEmpToEmpSalary(emp.EmployeeId);//往员工工资体系表添加员工
                    AjaxResultxx.Success = ss;
                    //if (AjaxResultxx.Success) {
                    //    bool monthss = msrmanage.AddEmpToEmpMonthSalary(emp.EmployeeId);//往月度工资表添加员工
                    //    AjaxResultxx.Success = monthss;
                    //}

                    //if (AjaxResultxx.Success) {
                    //    bool mc = mcmanage.AddEmpToMeritsCheck(emp.EmployeeId);//往绩效考核表添加员工
                    //    AjaxResultxx.Success = mc;
                    //    if (AjaxResultxx.Success && !string.IsNullOrEmpty(emp.PositiveDate.ToString())) {
                    //        //并将该员工绩效分默认改为100
                    //        var mcemp = mcmanage.GetmcempByEmpid(emp.EmployeeId);
                    //        AjaxResultxx.Success = mcemp;

                    //    }
                    //}

                }
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
            var lastobj = empinfo.GetEmpInfoData().LastOrDefault();
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
        /// <summary>
        /// 验证钉钉号是否已存在
        /// </summary>
        /// <param name="ddnum"></param>
        /// <returns></returns>
        public ActionResult CheckDDNum(string ddnum) {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var emplist = empmanage.GetEmpInfoData();
                if (emplist.Where(s => s.DDAppId == int.Parse(ddnum)).Count() > 0) {
                    AjaxResultxx.Success = true;
                }
            }
            catch (Exception)
            {
                AjaxResultxx.Success = false;
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 部门及岗位相关业务
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
                foreach (var d in deptmanage.GetList().Where(s => s.DeptId == deptid && s.IsDel == false))
                {
                    if (pname == d.PositionName && d.IsDel == false)
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
        #endregion

        #region 员工信息列表的单元格修改
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
        #endregion

        /// <summary>
        /// 员工信息详情页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EmpDetail(string id)
        {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var emp = empmanage.GetEntity(id);
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
            EmpTransactionManage etm = new EmpTransactionManage();
            var e = emanage.GetEntity(id);
            var etmobj = etm.GetDelEmp(e.EmployeeId);
            var empobj = new
            {
                #region 获取属性值 
                e.EmployeeId,
                e.DDAppId,
                e.EmpName,
                e.PositionId,
                dname = emanage.GetDept((int)e.PositionId).DeptName,
                pname = emanage.GetPosition((int)e.PositionId).PositionName,
                deptid = emanage.GetDept((int)e.PositionId).DeptId,
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
                e.IsDel,
                e.Image,
                deltime = etmobj == null ? null : etmobj.TransactionTime,//离职时间
                delreason = etmobj == null ? null : etmobj.Reason//离职原因
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
            ViewBag.pid = emp.PositionId;
            ViewBag.pname = empmanage.GetPobjById(emp.PositionId).PositionName;
            return View(emp);
        }
        [HttpPost]
        public ActionResult EditEmp(EmployeesInfo emp)
        {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            EmpTransactionManage etmanage = new EmpTransactionManage();
            MoveTypeManage mtmanage = new MoveTypeManage();
            var ajaxresult = new AjaxResult();
            try
            {
                var emp2 = empmanage.GetInfoByEmpID(emp.EmployeeId);
                if (emp.IdCardNum != null) {
                    emp.Birthdate = DateTime.Parse(GetBirth(emp.IdCardNum));
                    emp.Age = Convert.ToInt32(GetAge((DateTime)emp.Birthdate, DateTime.Now));
                }

                emp.IsDel = emp2.IsDel;
                if (emp.Image != "undefined")
                {
                    emp.Image = ImageUpload();
                }
                else {
                    emp.Image = emp2.Image;
                }

                empmanage.Update(emp);
                ajaxresult = empmanage.Success();

                if (ajaxresult.Success)
                {
                    //员工转正，直接由人事修改日期的情况下添加该员工异动
                    if (string.IsNullOrEmpty(emp2.PositiveDate.ToString()))
                    {
                        if (!string.IsNullOrEmpty(emp.PositiveDate.ToString()))
                        {
                            EmpTransaction et = new EmpTransaction();
                            et.EmployeeId = emp.EmployeeId;
                            et.TransactionType = mtmanage.GetmtTypeidByTname("转正");
                            et.TransactionTime = emp.PositiveDate;
                            et.IsDel = false;
                            etmanage.Insert(et);
                            ajaxresult = etmanage.Success();
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
                                //并将该员工绩效分默认改为100
                                if (ajaxresult.Success) {
                                    MeritsCheckManage mcmanage = new MeritsCheckManage();
                                    var mcemp = mcmanage.GetmcempByEmpid(emp.EmployeeId);
                                    ajaxresult.Success = mcemp;
                                }
                            }
                        }
                    }

                    //员工调岗，直接由人事修改日期的情况下添加该员工异动
                    if (emp.PositionId != emp2.PositionId) {
                        EmpTransaction et = new EmpTransaction();
                        et.EmployeeId = emp.EmployeeId;
                        et.TransactionType = mtmanage.GetmtTypeidByTname("调岗");
                        et.TransactionTime = DateTime.Now;
                        et.PreviousDept = empmanage.GetDeptByPid(emp2.PositionId).DeptId;
                        et.PreviousPosition = emp2.PositionId;
                        et.PresentDept = empmanage.GetDeptByPid(emp.PositionId).DeptId;
                        et.PresentPosition = emp.PositionId;
                        if (emp.PositiveDate == null)
                        {
                            et.PresentSalary = emp.ProbationSalary;
                        }
                        else
                        {
                            et.PresentSalary = emp.Salary;
                        }
                        et.IsDel = false;
                        etmanage.Insert(et);
                        ajaxresult = etmanage.Success();

                        if (ajaxresult.Success)
                        {
                            EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
                            var ese = esemanage.GetEseByEmpid(emp.EmployeeId);
                            if (empmanage.GetPositionByEmpid(emp.EmployeeId).PositionName.Contains("主任"))
                            {
                                ese.PerformancePay = 1000;
                            } else if (empmanage.GetDeptByEmpid(emp.EmployeeId).DeptName == "校办") {
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

                    //当员工的社保之前没有值现在添值的情况下则员工绩效额度也要改变
                    if (string.IsNullOrEmpty(emp2.SSStartMonth.ToString()))
                    {
                        if (!string.IsNullOrEmpty(emp.SSStartMonth.ToString()))
                        {
                            EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
                            var ese = esemanage.GetEseByEmpid(emp.EmployeeId);
                            if (string.IsNullOrEmpty(emp.ProbationSalary.ToString()))
                            {//没有实习工资的情况下
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
                            }
                            else {
                                ese.PerformancePay = emp.Salary - emp.ProbationSalary;
                            }
                            ese.PositionSalary = emp.Salary - ese.BaseSalary - ese.PerformancePay;
                            esemanage.Update(ese);
                            ajaxresult = esemanage.Success();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ajaxresult = empmanage.Error(ex.Message);
            }

            return Json(ajaxresult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 离职员工详细信息
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public ActionResult LeftEmpDetail(string id) {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var emp = empmanage.GetEntity(id);
            return View(emp);
        }


        /// <summary>
        /// 生日提醒
        /// </summary>
        /// <returns></returns>
        public ActionResult Birthdayremind() {
            ViewBag.birthemps = GetTheGodOfLongevity();
            return View();
        }
        /// <summary>
        /// 获取所有当天生日的员工
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetTheGodOfLongevity() {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            List<EmployeesInfo> luckdog = new List<EmployeesInfo>();
            var myemplist = empmanage.GetEmpInfoData().Where(s => s.IsDel == false).ToList();

            string lunardate = GetLunarCalendar();
            string[] lunar = lunardate.Split('/');//分割获取到的当前日期的农历日期
            var lunarmonth = lunar[0];//获取当前农历月份
            var lunarday = lunar[1];//获取当前农历日期

            var month = DateTime.Now.Month;//获取当前阳历月份
            var day = DateTime.Now.Day;//获取当前阳历日期

            for (int i = 0; i < myemplist.Count(); i++)
            {
                string birth = myemplist[i].Birthday;//获取该员工的生日属性
                if (!string.IsNullOrEmpty(birth)) {
                    var birthmon = birth.Split('月', '日');
                    var mymonth = birthmon[0];//月份          
                    var myday = birthmon[1];//日期
                    var myyinyang = birthmon[2];//阴阳历

                    if (myyinyang == "农历")
                    {
                        if (mymonth == lunarmonth && myday == lunarday)
                        {
                            luckdog.Add(myemplist[i]);
                        }
                    }
                    if (myyinyang == "阳历")
                    {
                        if (int.Parse(mymonth) == month && int.Parse(myday) == day)
                        {
                            luckdog.Add(myemplist[i]);
                        }
                    }
                }

            }
            return luckdog;
        }
        /// <summary>
        /// 获取当前农历的年月日
        /// </summary>
        /// <returns></returns>
        public string GetLunarCalendar() {
            ChineseLunisolarCalendar ChineseCalendar = new ChineseLunisolarCalendar();
            int year = ChineseCalendar.GetYear(DateTime.Now);
            int day = ChineseCalendar.GetDayOfMonth(DateTime.Now);
            int month = ChineseCalendar.GetMonth(DateTime.Now);
            //int leapMonth = ChineseCalendar.GetLeapMonth(year);

            string date = string.Format("{0}/{1}", month, day);
            return date;
        }

        public ActionResult ContractEnd() {
            ViewBag.contractendData = ContractEndRemind();
            return View();
        }

        /// <summary>
        /// "合同到期提醒",获取一个月内（小于等于30天）合同将过期的员工
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> ContractEndRemind()

        {
            //找到所有一个月内即将过期的员工，返回员工集合
            List<EmployeesInfo> ContractendingEmp = new List<EmployeesInfo>();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var empsum = empmanage.GetEmpInfoData().Where(s => s.IsDel == false).ToList();
            var nowtime = DateTime.Now;
            for (int i = 0; i < empsum.Count(); i++)
            {
                DateTime endtime = Convert.ToDateTime(empsum[i].ContractEndTime);
                TimeSpan ts = endtime - nowtime;
                //相差的天数 
                var diffday = ts.Days;
                if (diffday <= 30 && diffday >= 0) {
                    ContractendingEmp.Add(empsum[i]);
                }
            }
            return ContractendingEmp;
        }


        // 图片上传
        public string ImageUpload()
        {
            StringBuilder ProName = new StringBuilder();
            HttpPostedFileBase file = Request.Files["Image"];
            string fname = file.FileName; //获取上传文件名称（包含扩展名）
            string f = Path.GetFileNameWithoutExtension(fname);//获取文件名称
            string name = Path.GetExtension(fname);//获取扩展名
            string pfilename = AppDomain.CurrentDomain.BaseDirectory + "uploadXLSXfile/EmpImage/";//获取当前程序集下面的uploads文件夹中的文件夹目录
            string completefilePath = DateTime.Now.ToString("yyyyMMddhhmmss") + name;//将上传的文件名称转变为当前项目名称
            ProName.Append(Path.Combine(pfilename, completefilePath));//合并成一个完整的路径;
            file.SaveAs(ProName.ToString());//上传文件   

            return completefilePath;
        }

        /// <summary>
        /// 用于用户修改员工信息的编辑页（电话号码/现住地址/图片）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditEmpAttr(string id) {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var emp = empmanage.GetInfoByEmpID(id);
           
            return View(emp);
        }

        [HttpPost]
        public ActionResult EditEmpAttr(EmployeesInfo emp) {
            string EmployeeId = Request.QueryString["EmployeeId"];
            var ajaxresult = new AjaxResult();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            try
            {
                var emp2 = empmanage.GetInfoByEmpID(emp.EmployeeId);
                if (emp.Image != "undefined")
                {
                    emp.Image = ImageUpload();
                    emp2.Image = emp.Image;
                }
                emp2.Phone = emp.Phone;
                emp2.Address = emp.Address;
               
                empmanage.Update(emp2);
                ajaxresult = empmanage.Success();
            }
            catch (Exception ex)
            {
                ajaxresult = empmanage.Error(ex.Message);
            }
            return Json(ajaxresult,JsonRequestBehavior.AllowGet);
        }

    }
}