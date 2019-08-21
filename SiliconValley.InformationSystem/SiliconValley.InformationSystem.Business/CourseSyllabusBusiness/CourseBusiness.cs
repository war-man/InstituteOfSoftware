using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.CourseSyllabusBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;


    /// <summary>
    /// 课程业务类
    /// </summary>
    public class CourseBusiness:BaseBusiness<Curriculum>
    {


        /// <summary>
        /// 获取所有课程
        /// </summary>
        /// <returns></returns>
        public List<Curriculum> GetCurriculas()
        {

            return this.GetList().Where(d => d.IsDelete == false).ToList().OrderByDescending(d=>d.CurriculumID).ToList();

        }

        /// <summary>
        /// 根据专业获取课程
        /// </summary>
        /// <param name="majorid">专业id</param>
        /// <returns></returns>
        public List<Curriculum> GetCurriculaByMajor(int majorid)
        {
           return this.GetCurriculas().Where(d=>d.MajorID==majorid).ToList();
        }


        /// <summary>
        /// 将EF模型转换为View模型
        /// </summary>
        /// <param name="curriculum">EF模型 课程</param>
        /// <returns></returns>
        public CourseView ToCourseView(Curriculum curriculum)
        {

            BaseBusiness<CourseType> db_coursetype = new BaseBusiness<CourseType>();
            BaseBusiness<Grand> db_Grand = new BaseBusiness<Grand>();
            BaseBusiness<Specialty> db_Specialty = new BaseBusiness<Specialty>();

            CourseView courseView = new CourseView();

            courseView.CourseCount = curriculum.CourseCount;
            courseView.CourseName = curriculum.CourseName;
            courseView.CourseType = db_coursetype.GetList().Where(d => d.IsDelete == false && d.Id == curriculum.CourseType_Id).FirstOrDefault();
            courseView.CurriculumID = curriculum.CurriculumID;
            courseView.Grand = db_Grand.GetList().Where(d => d.IsDelete == false && d.Id == curriculum.Grand_Id).FirstOrDefault();
            courseView.Major = db_Specialty.GetList().Where(d => d.IsDelete == false && d.Id == curriculum.MajorID).FirstOrDefault();
            courseView.PeriodMoney = curriculum.PeriodMoney;
            courseView.Rmark = curriculum.Rmark;


            return courseView;
        }

        /// <summary>
        /// 将视图模型转换为EF模型
        /// </summary>
        /// <param name="courseView">视图模型 课程</param>
        /// <returns></returns>
        public Curriculum ToCurriculum(CourseView courseView)
        {

            Curriculum curriculum = new Curriculum();


            curriculum.CurriculumID = courseView.CurriculumID;
            curriculum.CourseCount = courseView.CourseCount;
            curriculum.CourseName = courseView.CourseName;
            curriculum.CourseType_Id = courseView.CourseType.Id;
            curriculum.CurriculumID = courseView.CurriculumID;
            curriculum.Grand_Id = courseView.Grand.Id;
            curriculum.MajorID = courseView.Major.Id;
            curriculum.PeriodMoney = courseView.PeriodMoney;
            curriculum.Rmark = courseView.Rmark;
            curriculum.IsDelete = courseView.IsDelete ;


            return curriculum;

        }

        public void DoAdd(Curriculum curriculum)
        {
            this.Insert(curriculum);

        }


        public void DoEdit(Curriculum curriculum)
        {
            this.Update(curriculum);

        }
    }
}
