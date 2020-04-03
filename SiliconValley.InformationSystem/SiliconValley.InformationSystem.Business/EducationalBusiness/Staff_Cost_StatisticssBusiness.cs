using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
////////////////////////////////////////
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using Newtonsoft.Json.Linq;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{

    /// <summary>
    ///员工费用统计--教务处
    /// </summary>
    public class Staff_Cost_StatisticssBusiness
    {
        /// <summary>
        /// 员工业务类实例
        /// </summary>
        private EmployeesInfoManage db_emp;

        public Staff_Cost_StatisticssBusiness()
        {
            db_emp = new EmployeesInfoManage();
        }

        /// <summary>
        /// 返回所有员工
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> Emplist()
        {
            return db_emp.GetAll(); //没有离职的员工

        }


        /// <summary>
        /// 筛选员工
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> ScreenEmp(string EmpName = null, string DepId = null)
        {
            List<EmployeesInfo> result = new List<EmployeesInfo>();

            if (!string.IsNullOrEmpty(EmpName))
            {
                //全局所搜名字

                var templist = this.Emplist();

                result = templist.Where(d => d.EmpName.Contains(EmpName)).ToList();
            }

            if (!string.IsNullOrEmpty(DepId))
            {
                var templist = this.Emplist();

                if (DepId == "0")
                {
                    result = templist;
                }
                else
                {
                    result = db_emp.GetEmpsByDeptid(int.Parse(DepId));
                }

            }


            return result;

        }


        /// <summary>
        /// 获取员工部门
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public Department GetDeparmentByEmp(string empid)
        {
            //获取岗位对象
            var po = db_emp.GetPositionByEmpid(empid);

            //获取部门对象

            return db_emp.GetDeptByPid(po.Pid);

        }

        /// <summary>
        /// 获取员工岗位对象
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public Position GetPositionByEmp(string empid)
        {
            var po = db_emp.GetPositionByEmpid(empid);

            return po = db_emp.GetPosition(po.Pid);
        }


        /// <summary>
        /// 获取部门集合
        /// </summary>
        /// <returns></returns>
        public List<Department> GetDepartments()
        {
            DepartmentBusiness.DepartmentManage tempdb_dep = new DepartmentBusiness.DepartmentManage();

            return tempdb_dep.GetDepartments();
        }


        /// <summary>
        /// 获取统计需要的数据  
        /// </summary>
        /// <returns></returns>

        public Staff_Cost_StatisticesDetailView Staff_CostData(string empid, DateTime date)
        {
            //获取这位员工

            var empObj = db_emp.GetInfoByEmpID(empid);

            var dep = this.GetDeparmentByEmp(empObj.EmployeeId);

            Staff_Cost_StatisticesDetailView resultObj = new Staff_Cost_StatisticesDetailView(); //返回值

            if (dep.DeptName.Contains("教学"))
            {
                //获取到专业课时
                resultObj.teachingitems = teachingitems(empObj);
            }
            else
            {
                //职业素养课，语数外 体育 课时
                resultObj.otherTeaccher_count = otherTeaccher_count();
            }

            //内训课时
            resultObj.InternalTraining_Count = InternalTraining_Count(empObj);

            //获取底课时
            resultObj.BottomClassHour = CalculationsBottomClassHour(empObj, resultObj.teachingitems);

            //获取满意度调查分数
            resultObj.SatisfactionScore = CalculationSeurev(emp:empObj,tempdate:date);
            #region 获取上专业课的课时

            List<TeachingItem> teachingitems(EmployeesInfo emp)
            {

                List<TeachingItem> result = new List<TeachingItem>();
               var templist = ScreenReconcile(empid, date, type:"skill");

                //获取所有阶段
                GrandBusiness tempdb_grand = new GrandBusiness();
                var grandlist = tempdb_grand.AllGrand();

                foreach (var item in grandlist)
                {
                    TeachingItem teachitem = new TeachingItem();

                    ///获取到课时
                    int count = ReconcileGroupByGrand(templist, item);

                    teachitem.grand = item;

                    teachitem.NodeNumber = count;

                    result.Add(teachitem);
                }

                return result;

            }

            #endregion


            #region 将排课数据按照阶段分组 获取课时
            int ReconcileGroupByGrand(List<Reconcile> data, Grand grand)
            {

                int result = 0;

                foreach (var item in data)
                {
                    CourseBusiness tempdb_course = new CourseBusiness();

                    //获取课程的阶段

                    var course = tempdb_course.GetCurriculas().Where(d=>d.CourseName == item.Curriculum_Id).FirstOrDefault();

                    if (course.Grand_Id == grand.Id)
                    {
                        result += 4;
                    }

                }

                return result;
            }

            #endregion


            #region 获取内训课时

            int InternalTraining_Count(EmployeesInfo emp)
            {
                int result = 0;

                if (dep.DeptName.Contains("教质"))
                {
                    BaseBusiness<Professionala> ProfessionalaBusiness = new BaseBusiness<Professionala>();

                    //按照时间筛选出培训记录
                    var templist = ProfessionalaBusiness.GetList().Where(d=>((DateTime)d.AddTime).Year == date.Year && ((DateTime)d.AddTime).Month == date.Month).ToList();

                    //获取员工班主任ID

                    HeadmasterBusiness temphead_db = new HeadmasterBusiness();
                    var headmaster = temphead_db.GetList().Where(d => d.informatiees_Id == emp.EmployeeId).FirstOrDefault();
                    //根据员工筛选数据

                    var resultlist = templist.Where(d => d.Trainee == headmaster.ID).ToList();

                    result = resultlist.Count;
                }

                if (dep.DeptName.Contains("教学"))
                {
                    BaseBusiness<Teachingtraining> temptedb_achtran = new BaseBusiness<Teachingtraining>();

                    //按照时间筛选出培训记录
                    var templist = temptedb_achtran.GetList().Where(d => ((DateTime)d.AddTime).Year == date.Year && ((DateTime)d.AddTime).Month == date.Month).ToList();


                    //获取员工 教员ID
                    TeacherBusiness tempdb_teacher = new TeacherBusiness();

                    var teacher = tempdb_teacher.GetTeachers(isContains_Jiaowu: false, IsNeedDimission: true).Where(d=>d.EmployeeId == emp.EmployeeId).FirstOrDefault();

                    var resultlist = templist.Where(d => d.Trainee == teacher.TeacherID).ToList();

                    result = resultlist.Count;

                }

                return result;


            }

            #endregion

            #region 获取职业素养课，语数外 体育 课时
            int otherTeaccher_count()
            {
               return ScreenReconcile(empObj.EmployeeId, date, type: "other").Count;
            }
            #endregion

            #region 计算底课时 
            /* S1S2教学经理，S3教学经理: 如果带1个班底课时为0，如果带2个班则按照标准底课时计算(需要把
            底课时分摊到每一个工作日，然后在看上两个班级的工作日是多少，得出底课时，如果有小数 向上取整)
            */
            float CalculationsBottomClassHour(EmployeesInfo emp, List<TeachingItem> teachingItems)
            {
                float result = 0;
                //获取员工岗位
                var poo = db_emp.GetPositionByEmpid(emp.EmployeeId);

                //判断这个月所带班情况

                TeacherClassBusiness tempdb_teacherclass = new TeacherClassBusiness();

                //获取员工 教员ID
                TeacherBusiness tempdb_teacher = new TeacherBusiness();

                var teacher = tempdb_teacher.GetTeachers(IsNeedDimission: true).Where(d => d.EmployeeId == emp.EmployeeId).FirstOrDefault();

                ///获取到这个月教员所带班级个数
                ///
                List< Reconcile>  tempcount = new List<Reconcile>(); //记录带班个数
                var reconlielist = this.ScreenReconcile(emp.EmployeeId, date, type: "skill");

                //去掉重复项
                foreach (var item in reconlielist)
                {
                    if (!IsContains(tempcount, item))
                    {
                        tempcount.Add(item);
                    }
                }

                //*************************S1S2教学经理，S3教学经理 的底课时计算方式*********************************
                if ((dep.DeptName == "s1、s2教学部" && poo.PositionName == "教学主任") || (dep.DeptName == "s3教学部" && poo.PositionName == "教学主任"))
                {
                    

                    if (tempcount.Count > 1)
                    {
                        //如果带2个班则按照标准底课时计算(需要把底课时分摊到每一个工作日，然后在看上两个班级的工作日是多少，得出底课时，如果有小数 向上取整)

                        //获取到工作日
                        int workingdateCount = this.WorkingDate(date);

                        //获取标准底课时
                        var MinimumCourseHours = (float)teacher.MinimumCourseHours;

                        //计算结果
                        result = MinimumCourseHours / workingdateCount;


                    }
                    else
                    {
                        //底课时为0
                        result = 0;
                    }


                }


                else //****************************教员的底课时计算方式******************************
                {
                    //当老师有上2种阶段的课程时的计算方式：底课时优先从底阶段减掉，若优先阶段课时不够，则优先级上升

                    var tempnumList = new List<TeachingItem>() ;  //教员这个月的 阶段课程课时

                    foreach (var item in teachingItems)
                    {
                        if (item.NodeNumber >= 1)
                        {
                            tempnumList.Add(item);
                        }
                    }


                    float MinimumCourseHours = (float)teacher.MinimumCourseHours;//标准底课时

                    if (tempnumList.Count > 1) //上多个班的情况
                    {
                        //底课时优先从底阶段减掉，若优先阶段课时不够，则优先级上升

                        foreach (var item in tempnumList)
                        {
                            if (item.grand.GrandName == "S1")
                            {
                                if (MinimumCourseHours > 0)
                                {
                                    MinimumCourseHours -= item.NodeNumber;
                                }
                                else
                                    break;
                            }

                            if (item.grand.GrandName == "S2")
                            {
                                if (MinimumCourseHours > 0)
                                {
                                    MinimumCourseHours -= item.NodeNumber;
                                }
                                else
                                    break;
                            }

                            if (item.grand.GrandName == "S3")
                            {
                                if (MinimumCourseHours > 0)
                                {
                                    MinimumCourseHours -= item.NodeNumber;
                                }
                                else
                                    break;
                            }
                        }

                    }
                    else
                    {
                        //获取标准底课时

                        result = (float)teacher.MinimumCourseHours;
                    }


                }
                return result;
            }
            #endregion

            #region 计算满意度分数
            float CalculationSeurev(EmployeesInfo emp, DateTime tempdate)
            {

                float result = 0;

                SatisfactionSurveyBusiness tempdb_datis = new SatisfactionSurveyBusiness();//满意度业务类实例

                List< SatisfactionSurveyDetailView> datalist = tempdb_datis.SurveyResult_Cost(emp.EmployeeId, tempdate);

                //开始计算 

                float totalScore = 0; //获取达到总分
                foreach (var item in datalist)
                {
                    totalScore += item.TotalScore;
                }

                result = totalScore / datalist.Count;


                return result;

            }

            #endregion


            return resultObj;
        }


        /// <summary>
        /// 排课筛选
        /// </summary>
        /// <param name="empid"></param>
        /// <param name="date"></param>
        /// <param name="type">type:0 = 所有；type: skill = 专业；type: other = 其他</param>
        /// <returns></returns>
        public List<Reconcile> ScreenReconcile(string empid, DateTime date,string type = "0")
        {
            ReconcileManeger tempdb_reconcile = new ReconcileManeger();
            CourseBusiness db_course = new CourseBusiness();

            //获取当期日期
            var currentData = DateTime.Now;

            var list = tempdb_reconcile.AllReconcile().Where(d=>d.AnPaiDate.Month==date.Month && d.AnPaiDate.Year == date.Year &&d.EmployeesInfo_Id == empid).ToList();

            //定义返回值
            List<Reconcile> result = new List<Reconcile>();

            switch (type)
            {
                case "0":
                    result = list;
                    break;

                case "skill":
                    //获取专业课

                    foreach (var item in list)
                    {
                        
                       var coustype = db_course.CurseType(item.Curriculum_Id);

                        if (coustype.Id == 1)
                        {
                            result.Add(item);
                        }
                    }


                    break;
                case "other":

                    foreach (var item in list)
                    {

                        var coustype = db_course.CurseType(item.Curriculum_Id);

                        if (coustype.Id != 1)
                        {
                            result.Add(item);
                        }
                    }

                    break;
            }

            return result;
            
        }

        public bool IsContains(List<Reconcile> sourcs, Reconcile r)
        {
            foreach (var item in sourcs)
            {
                if (item.ClassSchedule_Id == r.ClassSchedule_Id)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 判断一个日期是否为节假日
        /// </summary>
        /// <returns></returns>
        public bool iSHoliday(string date)
        {
          

            bool isHoliday = false;
            System.Net.WebClient WebClientObj = new System.Net.WebClient();
            System.Collections.Specialized.NameValueCollection PostVars = new System.Collections.Specialized.NameValueCollection();
            PostVars.Add("d", date);//参数
            try
            {
                byte[] byRemoteInfo = WebClientObj.UploadValues(@"http://www.easybots.cn/api/holiday.php", "POST", PostVars);//请求地址,传参方式,参数集合
                string sRemoteInfo = System.Text.Encoding.UTF8.GetString(byRemoteInfo);//获取返回值
                string result = JObject.Parse(sRemoteInfo)[date].ToString();
                if (result == "0")
                {
                    isHoliday = false;
                }
                else if (result == "1" || result == "2")
                {
                    isHoliday = true;
                }
            }
            catch
            {
                isHoliday = false;
            }
            
            return isHoliday;
        }


        
        /// <summary>
        /// 获取工作日的天数
        /// </summary>
        /// <returns></returns>q
        public int WorkingDate(DateTime date)
        {

            int result = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(date.Year, date.Month); //获取这个月的总天数

            ReconcileManeger tempdbR = new ReconcileManeger();

            GetYear find_g = tempdbR.MyGetYear(date.Year.ToString(),System.Web.HttpContext.Current.Server.MapPath("~/Xmlconfigure/Reconcile_XML.xml"));

            //判断是否为休息日

            if (date.Month >= find_g.StartmonthName && date.Month <= find_g.EndmonthName)
            {
                //单休
                DateTime dt = new DateTime(date.Year, date.Month, 1); //初始化时间
                while (dt.Month == date.Month)
                {
                    //判读是否为周末

                    if (dt.DayOfWeek == DayOfWeek.Sunday)
                    {
                        //总天数减一天
                        result -= 1;
                    }

                    dt = dt.AddDays(1);
                }
            }
            else
            {

                //双休

                DateTime dt = new DateTime(date.Year, date.Month, 1); //初始化时间
                while (dt.Month == date.Month)
                {
                    //判读是否为周末

                    if (dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday)
                    {
                        //总天数减一天
                        result -= 1;
                    }
                    dt = dt.AddDays(1);
                    
                }
            }


            DateTime dtt = new DateTime(date.Year, date.Month, 1); //初始化时间
            //是否法定节假日
            var isHoliday = this.iSHoliday(dtt.ToString());


            while (dtt.Month == date.Month)
            {
                //判读是否为周末

                if (isHoliday)
                {
                    result -= 1;
                }
                dtt = dtt.AddDays(1);
            }
           


            return result;
        }

    }
}
