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





            # region 获取上专业课的课时

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

                    var course = tempdb_course.GetEntity(item.Curriculum_Id);

                    if (course.Grand_Id == grand.Id)
                    {
                        result++;
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

                    //获取当期日期
                    var currentData = DateTime.Now;

                    //按照时间筛选出培训记录
                    var templist = ProfessionalaBusiness.GetList().Where(d=>((DateTime)d.AddTime).Year == date.Year && ((DateTime)d.AddTime).Month == date.Month).ToList();

                    //获取员工班主任ID

                    HeadmasterBusiness temphead_db = new HeadmasterBusiness();
                    var headmaster = temphead_db.GetList().Where(d => d.informatiees_Id == emp.EmployeeId).FirstOrDefault();
                    //根据员工筛选数据

                    //var resultlist = templist.Where(d=>d.tra)

                    var resultlist = templist;

                   
                }

                return result;
            }

            #endregion

            return resultObj;
        }


        //筛选课程
        public List<Reconcile> ScreenReconcile(string empid, DateTime date,string type = "0")
        {
            ReconcileManeger tempdb_reconcile = new ReconcileManeger();
            CourseBusiness db_course = new CourseBusiness();

            //获取当期日期
            var currentData = DateTime.Now;

            var list = tempdb_reconcile.AllReconcile().Where(d=>d.AnPaiDate.Month==date.Month && d.AnPaiDate.Year == date.Year && d.AnPaiDate.Date <= currentData.Date).ToList();

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

                       var coustype = db_course.CurseType(int.Parse(item.Curriculum_Id));

                        if (coustype.Id == 1)
                        {
                            result.Add(item);
                        }
                    }


                    break;
                case "other":

                    foreach (var item in list)
                    {

                        var coustype = db_course.CurseType(int.Parse(item.Curriculum_Id));

                        if (coustype.Id != 1)
                        {
                            result.Add(item);
                        }
                    }

                    break;
            }

            return result;
            
        }



    }
}
