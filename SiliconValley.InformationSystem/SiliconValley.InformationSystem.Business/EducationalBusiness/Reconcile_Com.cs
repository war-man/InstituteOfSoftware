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
    }
}
