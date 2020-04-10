using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
    /// <summary>
    /// 显示课表数据的业务类
    /// </summary>
   public class TimeTableManeger 
    {
        ReconcileManeger Reconcile_Entity;
        /// <summary>
        /// 获取排课数据(显示课表形式)
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="timename">上课时间段</param>
        /// <param name="classrooms">教室集合</param>
        /// <returns></returns>
        public List<AnPaiData> GetPaiDatas(DateTime time, string timename, List<Classroom> classrooms)
        {
            List<AnPaiData> a_list = new List<AnPaiData>();
            Reconcile_Entity = new ReconcileManeger();
            List<Reconcile> r_list = Reconcile_Entity.GetReconciles(time);
            BaseBusiness<EmployeesInfo> entity = new BaseBusiness<EmployeesInfo>();
            if (timename == "上午12节" || timename == "上午34节")
            {
                foreach (Classroom c1 in classrooms)
                {
                    AnPaiData new_a = new AnPaiData();
                    foreach (Reconcile r1 in r_list)
                    {
                        if (r1.ClassRoom_Id == c1.Id && (r1.Curse_Id == timename || r1.Curse_Id == "上午"))
                        {
                            new_a.class_Id = r1.Id;
                            new_a.ClassName = Reconcile_Com.ClassSchedule_Entity.GetEntity(r1.ClassSchedule_Id).ClassNumber;
                            new_a.Teacher = r1.EmployeesInfo_Id == null ? "无" : entity.GetEntity(r1.EmployeesInfo_Id).EmpName;
                            new_a.NeiRong = r1.Curriculum_Id;
                        }
                    }
                    a_list.Add(new_a);
                }
            }
            else if (timename == "下午12节" || timename == "下午34节")
            {
                foreach (Classroom c1 in classrooms)
                {
                    AnPaiData new_a = new AnPaiData();
                    foreach (Reconcile r1 in r_list)
                    {
                        if (r1.ClassRoom_Id == c1.Id && (r1.Curse_Id == timename || r1.Curse_Id == "下午"))
                        {
                            new_a.class_Id = r1.Id;
                            new_a.ClassName = Reconcile_Com.ClassSchedule_Entity.GetEntity(r1.ClassSchedule_Id).ClassNumber;
                            new_a.Teacher = r1.EmployeesInfo_Id == null ? "无" : entity.GetEntity(r1.EmployeesInfo_Id).EmpName;
                            new_a.NeiRong = r1.Curriculum_Id;
                        }
                    }
                    a_list.Add(new_a);
                }
            }
            else if (timename == "晚一" || timename == "晚二")
            {
                foreach (Classroom c1 in classrooms)
                {
                    AnPaiData new_a = new AnPaiData();
                    foreach (Reconcile r1 in r_list)
                    {
                        if (r1.ClassRoom_Id == c1.Id && r1.Curse_Id == timename)
                        {
                            new_a.class_Id = r1.Id;
                            new_a.ClassName = Reconcile_Com.ClassSchedule_Entity.GetEntity(r1.ClassSchedule_Id).ClassNumber;
                            new_a.Teacher = r1.EmployeesInfo_Id == null ? "无" : entity.GetEntity(r1.EmployeesInfo_Id).EmpName;
                            new_a.NeiRong = r1.Curriculum_Id;
                        }
                    }
                    a_list.Add(new_a);
                }
            }
            return a_list;
        }
    }
}
