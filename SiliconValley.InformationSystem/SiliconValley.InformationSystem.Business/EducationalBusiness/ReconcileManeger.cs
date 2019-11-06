using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class ReconcileManeger:BaseBusiness<Reconcile>
    {
        //教室处理类
        public static readonly ClassroomManeger Classroom_Entity = new ClassroomManeger();
        //课程处理类
        public static readonly CourseBusiness Curriculum_Entity = new CourseBusiness();
        //课程时间处理类
        public static readonly CourseTimeManeger CourseTime_Entity = new CourseTimeManeger();
        //班级处理类
        public static readonly ClassScheduleBusiness ClassSchedule_Entity = new ClassScheduleBusiness();
        //阶段处理类
        public static readonly GrandBusiness Grand_Entity = new GrandBusiness();
        //教学老师处理类
        public static readonly TeacherClassBusiness Teacher_Entity = new TeacherClassBusiness();
        /// <summary>
        /// 向Xml文件读取配置
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="XmlFile_url">xml的url</param>
        /// <returns></returns>
        public GetYear MyGetYear(string year,string XmlFile_url)
        {
            GetYear g = new GetYear();
            XElement  xx=XElement.Load(XmlFile_url);
            XElement s = xx.Elements("Year").Where(e=>e.Attribute("name").Value== year.ToString()).First();//筛选
            g.YearName = year;
            g.StartmonthName = s.Element("startmonth").Value;
            g.EndmonthName = s.Element("endmonth").Value;
            return g;
            
        }
        /// <summary>
        /// 判断某年是否是闰年
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public bool IsRunYear(int year)
        {
           //true--是闰年2月有29天，false--不是闰年   
            if((year%4==0 && year % 100 != 0) || year% 400 == 0)//是闰年
            {
               return true;
            }
            else
            {
                return false;
            }           
        }
        /// <summary>
        /// 判断该日期是否是周六或周末
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsSaturday(DateTime date)
        {
            var day = date.DayOfWeek;
            //判断是否为周末

            if (day == DayOfWeek.Sunday || day == DayOfWeek.Saturday)
            {
                return true;
            }
            else
            {
                return false;
            }              
        }
        /// <summary>
        /// 获取有效的阶段集合
        /// </summary>
        /// <returns></returns>
        public List<Grand> GetEffectiveData()
        {
            return Grand_Entity.GetList().Where(g => g.IsDelete == false).ToList();
        }
        /// <summary>
        /// 获取属于某个阶段的有效班级集合
        /// </summary>
        /// <param name="grand_id"></param>
        /// <returns></returns>
        public List<ClassSchedule> GetGrandClass(int grand_id)
        {
            //获取有效的班级数据//获取属于某个阶段的班级
             List<ClassSchedule> c_list= ClassSchedule_Entity.GetList().Where(c => c.ClassStatus == false && c.IsDelete == false && c.grade_Id== grand_id).ToList();
            return c_list;
        }
        /// <summary>
        /// 获取某个阶段某个专业的课程
        /// </summary>
        /// <param name="grand_id"></param>
        /// <param name="marjon_id"></param>
        /// <returns></returns>
        //public List<Curriculum> GetRelevantCurricul(int grand_id,int marjon_id)
        //{

        //}
    }
}
