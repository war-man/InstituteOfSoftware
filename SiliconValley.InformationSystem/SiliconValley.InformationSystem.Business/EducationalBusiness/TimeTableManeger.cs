using SiliconValley.InformationSystem.Entity.Entity;
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
        EvningSelfStudyManeger EvningSelfStudy_Entity;
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
            List<Reconcile> r_list = Reconcile_Entity.GetReconciles(time);//获取这个日期的排课数据
            EvningSelfStudy_Entity = new EvningSelfStudyManeger();
            List<EvningSelfStudy> e_list = EvningSelfStudy_Entity.GetTimeData(time);//获取这个日期的晚自习安排数据
            string timenames = timename.Substring(0, 2);
            if (timename == "上午12节" || timename == "上午34节" || timename == "下午12节" || timename == "下午34节")
            {
                foreach (Classroom c in classrooms)
                {
                     List<Reconcile> finddata=  r_list.Where(r => (r.Curse_Id == timename || r.Curse_Id == timenames) && r.ClassRoom_Id == c.Id).ToList();
                     AnPaiData new_a = new AnPaiData();
                    if (finddata.Count==1)
                    {
                      
                        new_a.R_Id = finddata[0].Id.ToString();
                        new_a.ClassName = Reconcile_Com.ClassSchedule_Entity.GetEntity(finddata[0].ClassSchedule_Id).ClassNumber;
                        new_a.Teacher = finddata[0].EmployeesInfo_Id == null ? "无" : Reconcile_Com.Employees_Entity.GetEntity(finddata[0].EmployeesInfo_Id).EmpName;
                        new_a.NeiRong = finddata[0].Curriculum_Id;
                        a_list.Add(new_a);
                    }else if (finddata.Count>1)
                    {
                        StringBuilder sbRid = new StringBuilder();
                        StringBuilder sbclassname = new StringBuilder();
                        StringBuilder sbTeacher = new StringBuilder();
                        StringBuilder sbNeiRong = new StringBuilder();
                        int index = 0;
                        foreach (Reconcile rr in finddata)
                        {
                            index++;
                            if (index==finddata.Count)
                            {
                                sbRid.Append(rr.Id);
                                sbclassname.Append(Reconcile_Com.ClassSchedule_Entity.GetEntity(rr.ClassSchedule_Id).ClassNumber );
                                sbTeacher.Append(rr.EmployeesInfo_Id == null ? "无" : Reconcile_Com.Employees_Entity.GetEntity(rr.EmployeesInfo_Id).EmpName );
                                sbNeiRong.Append(rr.Curriculum_Id);
                            }
                            else
                            {
                                sbRid.Append(rr.Id + ",");
                                sbclassname.Append(Reconcile_Com.ClassSchedule_Entity.GetEntity(rr.ClassSchedule_Id).ClassNumber + ",");
                                sbTeacher.Append(rr.EmployeesInfo_Id == null ? "无" : Reconcile_Com.Employees_Entity.GetEntity(rr.EmployeesInfo_Id).EmpName + ",");
                                sbNeiRong.Append(rr.Curriculum_Id + ",");
                            }
                            
                        }
                        new_a.R_Id = sbRid.ToString();
                        new_a.ClassName = sbclassname.ToString();
                        new_a.Teacher = sbTeacher.ToString();
                        new_a.NeiRong = sbNeiRong.ToString();
                        a_list.Add(new_a);
                    }
                    else
                    {
                        a_list.Add(new_a);
                    }    
                }        
            }            
             
            else if (timename == "晚一" || timename == "晚二")
            {
                foreach (Classroom c in classrooms)
                {
                    List<EvningSelfStudy> finddata= e_list.Where(e => e.curd_name == timename && e.Classroom_id==c.Id).ToList();
                    AnPaiData a = new AnPaiData();
                    if (finddata.Count==1)
                    {                        
                        a.R_Id = finddata[0].id.ToString();
                        a.ClassName =Reconcile_Com.ClassSchedule_Entity.GetEntity( finddata[0].ClassSchedule_id).ClassNumber;
                        a.Teacher = finddata[0].emp_id == null ? "无" : Reconcile_Com.Employees_Entity.GetEntity(finddata[0].emp_id).EmpName;
                        a_list.Add(a);
                    }
                    else if(finddata.Count >1)
                    {
                        StringBuilder sbrid = new StringBuilder();
                        StringBuilder sbclassname = new StringBuilder();
                        StringBuilder sbteacher = new StringBuilder();
                        int index = 0;
                        foreach (EvningSelfStudy item in finddata)
                        {
                            index++;
                            if (index==finddata.Count)
                            {
                                sbrid.Append(item.id.ToString());
                                sbclassname.Append(Reconcile_Com.ClassSchedule_Entity.GetEntity(item.ClassSchedule_id).ClassNumber);
                                sbteacher.Append(item.emp_id == null ? "无" : Reconcile_Com.Employees_Entity.GetEntity(item.emp_id).EmpName);
                            }
                            else
                            {
                                sbrid.Append(item.id.ToString() + ",");
                                sbclassname.Append(Reconcile_Com.ClassSchedule_Entity.GetEntity(item.ClassSchedule_id).ClassNumber + ",");
                                sbteacher.Append(item.emp_id == null ? "无" : Reconcile_Com.Employees_Entity.GetEntity(item.emp_id).EmpName + ",");
                            }
                        }
                        a.R_Id = sbrid.ToString();
                        a.ClassName = sbclassname.ToString();
                        a.Teacher = sbteacher.ToString();
                        a_list.Add(a);
                    }
                    else
                    {
                        a_list.Add(a);
                    }
                }
            }
            return a_list;
        }

        /// <summary>
        /// 根据排课id获取班级排课信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<AnPaiData> GetClassName(string[] id)
        {
            List<AnPaiData> list = new List<AnPaiData>();
            Reconcile_Entity = new ReconcileManeger();
            foreach (string item in id)
            {
                AnPaiData a = new AnPaiData();
                int number_id=  Convert.ToInt32(item);
               Reconcile finddata= Reconcile_Entity.GetEntity(number_id);
                a.R_Id = item;
                a.ClassName = Reconcile_Com.ClassSchedule_Entity.GetEntity(finddata.ClassSchedule_Id).ClassNumber;
                list.Add(a);
            }

            return list;
        }

        /// <summary>
        /// 根据晚自习id获取班级数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<AnPaiData> GetEvningClassName(string[] id)
        {
            EvningSelfStudy_Entity = new EvningSelfStudyManeger();
            return   EvningSelfStudy_Entity.GetClassname(id);
        }
    }
}
