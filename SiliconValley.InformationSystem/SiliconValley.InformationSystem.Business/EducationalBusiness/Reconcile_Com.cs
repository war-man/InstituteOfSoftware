using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class Reconcile_Com
    {
        //教室业务类
        public static readonly ClassroomManeger Classroom_Entity = new ClassroomManeger();
        //课程业务类
        public static readonly CourseBusiness Curriculum_Entity = new CourseBusiness();
        //课程类型业务类
        public static readonly CourseTypeBusiness CourseType_Entity = new CourseTypeBusiness();
        //班级业务类
        public static readonly ClassScheduleBusiness ClassSchedule_Entity = new ClassScheduleBusiness();
        //阶段业务类
        public static readonly GrandBusiness Grand_Entity = new GrandBusiness();
        //教学老师业务类
        public static readonly TeacherBusiness Teacher_Entity = new TeacherBusiness();

        //专业业务类
        public static readonly SpecialtyBusiness Specialty_Entity = new SpecialtyBusiness();

        //老师擅长课程业务类
        public static readonly GoodSkillManeger GoodSkill_Entity = new GoodSkillManeger();

        public static readonly HeadmasterBusiness Headmaster_Etity = new HeadmasterBusiness();

        public static readonly TeacherClassBusiness TeacherClass_Entity = new TeacherClassBusiness();

        public static readonly BaseBusiness<HeadClass> Hoadclass_Entity = new BaseBusiness<HeadClass>();

        public static readonly  RedisCache redisCache = new RedisCache();


        /// <summary>
        /// 根据阶段名称获取阶段Id
        /// </summary>
        /// <param name="name">阶段名称</param>
        /// <returns></returns>
        public static int GetGrand_Id(string name)
        {
           return Grand_Entity.GetList().Where(g => g.GrandName.Equals(name,StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().Id;
        }
        /// <summary>
        /// 获取XX父级下的XX子集
        /// </summary>
        /// <param name="father">父级名称</param>
        /// <param name="name">子集名称</param>
        /// <returns></returns>
        public static int GetBase_Id(string father,string name)
        {
            List<BaseDataEnum> b_list= ClassSchedule_Entity.BaseDataEnum_Entity.GetsameFartherData(father);
            return b_list.Where(b=>b.Name==name).FirstOrDefault().Id;
        }
        /// <summary>
        /// 获取带班的班主任
        /// </summary>
        /// <param name="class_id"></param>
        /// <returns></returns>
        public static AjaxResult GetZhisuTeacher(int class_id)
        {
            AjaxResult result = new AjaxResult();
            result.Success = false;
            BaseBusiness<EmployeesInfo> emp_Entity = new BaseBusiness<EmployeesInfo>();
            //学员班级
            ClassScheduleBusiness classScheduleBusiness = new ClassScheduleBusiness();
            HeadClass mysex = Hoadclass_Entity.GetList().Where(a => a.ClassID ==class_id && a.EndingTime==null).FirstOrDefault();
            if (mysex!=null)
            {
                Headmaster heasmaster=  Headmaster_Etity.GetEntity(mysex.LeaderID);
                if (heasmaster!=null)
                {
                    EmployeesInfo find_e= emp_Entity.GetEntity(heasmaster.informatiees_Id);
                    if (find_e!=null)
                    {
                        result.Data = find_e.EmployeeId;
                        result.Success = true;
                    }
                }
                else
                {
                    result.Msg = "没有找到数据";
                }
            }
            else
            {
                result.Msg = "没有找到数据";
            }
            return result;
        }
        /// <summary>
        /// 获取班主任带领的班级
        /// </summary>
        /// <param name="emp_id"></param>
        /// <returns></returns>
        public static AjaxResult GetHadMasterClass(string emp_id)
        {
            AjaxResult result = new AjaxResult();
            result.Success = false;
            //根据员工Id找班主任编号
            Headmaster find_h= Headmaster_Etity.GetList().Where(h => h.informatiees_Id == emp_id).FirstOrDefault();
            //根据编号获取班主任带的班级
            List<HeadClass> hc_list= Hoadclass_Entity.GetList().Where(h => h.EndingTime == null && h.LeaderID == find_h.ID).ToList();
            if (hc_list.Count>0)
            {
                result.Success = true;
                result.Data = hc_list;
            }

            return result;
                 
        }
             
    }
}
