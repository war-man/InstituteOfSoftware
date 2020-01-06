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
    public class CourseBusiness : BaseBusiness<Curriculum>
    {

        private BaseBusiness<ClassTeacher> db_classteacher;

        public CourseBusiness()
        {
            db_classteacher = new BaseBusiness<ClassTeacher>();
        }


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

        public bool isContain(List<Curriculum> sources, Curriculum curriculum)
        {
            foreach (var item in sources)
            {
                if (item.CurriculumID == curriculum.CurriculumID)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 获取某个阶段某个专业的课程
        /// </summary>
        /// <param name="grand_id">阶段Id</param>
        /// <param name="marjon_id">专业Id</param>
        ///  <param name="ishavaEnglish">是否需要显示英语课程(fale--否，true-是)</param>
        /// <returns></returns>
        public List<Curriculum> GetRelevantCurricul(int grand_id, int? marjon_id,bool ishavaEnglish)
        {
            if (ishavaEnglish)
            {
                //有英语
                //获取属于这个阶段的课程
                List<Curriculum> find_list = this.GetCurriculas().OrderBy(d => d.Sort).Where(d => d.Grand_Id == grand_id && d.IsDelete == false).ToList();
                //获取不属于这个专业的课程
                List<Curriculum> notof= this.GetCurriculas().OrderBy(d => d.Sort).Where(d => d.Grand_Id == grand_id && d.MajorID!=marjon_id && d.IsDelete == false).ToList();
                find_list.RemoveAll(d => d.MajorID != marjon_id && d.Grand_Id==grand_id && d.MajorID!=null);


                return find_list;
            }
            else
            {
                //没有英语 
                List<Curriculum> find_list= this.GetCurriculas().OrderBy(d => d.Sort).Where(d => d.Grand_Id == grand_id && d.IsDelete == false && d.CourseName != "英语").ToList();
                find_list.RemoveAll(d => d.MajorID != marjon_id && d.Grand_Id == grand_id && d.MajorID != null);
                return find_list;
            }
           

        }


        public ClassCourseView ConvertToView(ClassTeacher classTeacher)
        {

            if (classTeacher == null)
                return null;

            ClassCourseView view = new ClassCourseView();

            view.BeginDate = classTeacher.BeginDate;

            BaseBusiness<ClassSchedule> dbclass = new BaseBusiness<ClassSchedule>();

            view.ClassNumber = dbclass.GetList().Where(d => d.id == classTeacher.ClassNumber).FirstOrDefault();
            view.EndDate = classTeacher.EndDate;
            view.ID = classTeacher.ID;
            view.IsDel = classTeacher.IsDel;
            view.Skill = this.GetCurriculas().Where(d => d.CurriculumID == classTeacher.Skill).FirstOrDefault();

            TeacherBusiness dbteacher = new TeacherBusiness();
            view.Teacher = dbteacher.GetEmpByEmpNo(dbteacher.GetTeacherByID(classTeacher.TeacherID).EmployeeId);

            return view;
        }



        /// <summary>
        /// 班级课程
        /// </summary>
        /// <returns></returns>
        public ClassCourseView CurrentClassCourse(int classSchedule)
        {

           var obj = db_classteacher.GetIQueryable().Where(d => d.ClassNumber == classSchedule && d.IsDel==false).FirstOrDefault();

            return this.ConvertToView(obj);
            

        }

        /// <summary>
        /// 获取班级下一门课程
        /// </summary>
        /// <returns></returns>
        public Curriculum GetClassNextCourse(int classid)
        {
            TeacherClassBusiness dbteacherclass = new TeacherClassBusiness();

            var courseid = dbteacherclass.GetClassTeachers().Where(d => d.ClassNumber == classid && d.IsDel == false).FirstOrDefault().Skill;
            var course = this.GetCurriculas().Where(d => d.CurriculumID == courseid).FirstOrDefault();

             return  this.GetCurriculas().Where(d => d.Grand_Id == course.Grand_Id && d.MajorID == course.MajorID && d.Sort == course.Sort+1).FirstOrDefault();

        }


        /// <summary>
        /// 给班级安排课程和教学老师
        /// </summary>
        /// <param name="classTeacher"></param>
        public void EditClassCourseArrangment(ClassTeacher classTeacher)
        {
           var classteacherlist = db_classteacher.GetIQueryable().Where(d => d.ClassNumber == classTeacher.ClassNumber && d.IsDel == false).ToList();

            if (classteacherlist != null)
            {
                foreach (var item in classteacherlist)
                {
                    item.IsDel = true;
                    item.EndDate = DateTime.Now;

                    db_classteacher.Update(item);
                }
            }

            
            classTeacher.EndDate = null;
            classTeacher.IsDel = false;

            db_classteacher.Insert(classTeacher);


        }

        public void UsingOrProhibit(string status, int classteacherid)
        {
           var classteacher =  db_classteacher.GetIQueryable().Where(d => d.ID == classteacherid).FirstOrDefault();

            //如果为启用  则要禁用掉其他的 

            if (status == "true")
            {
                var templist = db_classteacher.GetIQueryable().Where(d => d.ClassNumber == classteacher.ClassNumber && d.IsDel == false).ToList();

                if (templist != null)
                {
                    foreach (var item in templist)
                    {
                        item.IsDel = true;
                        db_classteacher.Update(item);
                    }
                }


                classteacher.IsDel = false;

                db_classteacher.Update(classteacher);
            }

            else
            {
                //禁用
                classteacher.IsDel = true;

                db_classteacher.Update(classteacher);

            }

        }
    }
}
