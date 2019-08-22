using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;

    public class TeacherClassBusiness:BaseBusiness<ClassTeacher>
    {


        BaseBusiness<StudentInformation> db_student = new BaseBusiness<StudentInformation>();

        BaseBusiness<ClassSchedule> db_class = new BaseBusiness<ClassSchedule>();

        BaseBusiness<ClassMembers> db_stuposi = new BaseBusiness<ClassMembers>();

        BaseBusiness<Members> db_members = new BaseBusiness<Members>();

        SpecialtyBusiness db_major = new SpecialtyBusiness();
        GrandBusiness db_grand = new GrandBusiness();


        /// <summary>
        /// 学员所在班级
        /// </summary>
        BaseBusiness<ScheduleForTrainees> db_studentclass = new BaseBusiness<ScheduleForTrainees>();

        public List<ClassTeacher> GetClassTeachers()
        {
            return this.GetList().Where(d => d.IsDel == false).ToList() ;
        }



        /// <summary>
        /// 获取我的班级
        /// </summary>
        /// <param name="teacherid">老师ID</param>
        /// <returns>班级集合</returns>

        public List<ClassSchedule> GetCrrentMyClass(int teacherid)
        {

           var templist =  this.GetClassTeachers().Where(d=>d.TeacherID==teacherid).ToList();

            BaseBusiness<ClassSchedule> classdb = new BaseBusiness<ClassSchedule>();

            var classlisttemp = classdb.GetList().Where(d => d.IsDelete == false && d.ClassStatus==false).ToList();

            List<ClassSchedule> resultlist = new List<ClassSchedule>();

            foreach (var item in templist)
            {

               var obj = classlisttemp.Where(d => d.ClassNumber == item.ClassNumber).FirstOrDefault();

                if (obj != null)
                {
                    resultlist.Add(obj);
                }

            }

            return resultlist;

        }


        /// <summary>
        /// 获取学员
        /// </summary>
        /// <param name="studentnumber">学员编号</param>
        /// <returns></returns>
        public StudentInformation GetStudentByNumber(string studentnumber)
        {

           var student = db_student.GetList().Where(d=>d.IsDelete==false && d.StudentNumber==studentnumber).FirstOrDefault();

            return student;
        }

        public StudentDetailView GetStudetentDetailView(StudentInformation student)
        {
            StudentDetailView detailView = new StudentDetailView();
            detailView.BirthDate = student.BirthDate;
            detailView.Education = student.Education;
            detailView.Familyaddress = student.Familyaddress;
            detailView.Name = student.Name;
            detailView.Nation = student.Nation;
            detailView.Picture = student.Picture;
            detailView.qq = student.qq;
            detailView.Sex = (bool)student.Sex ? "男" : "女";
            detailView.State = student.State;
            detailView.StudentNumber = student.StudentNumber;
            detailView.Telephone = student.Telephone;
            detailView.WeChat = student.WeChat;
           

            //获取这个学员的当前班级

           var temp = db_studentclass.GetList().Where(d => d.CurrentClass == true && d.StudentID == student.StudentNumber).FirstOrDefault();
            ClassSchedule myclass = db_class.GetList().Where(d=>d.IsDelete==false && d.ClassNumber==temp.ClassID).FirstOrDefault();
            var classmenber = db_stuposi.GetList().Where(d => d.IsDelete == false && d.ClassNumber == myclass.ClassNumber && d.Student_number == student.StudentNumber).FirstOrDefault();


            if (classmenber != null)
            {
               var posti =  db_members.GetList().Where(d => d.IsDelete == false && d.ID == classmenber.Typeofposition).FirstOrDefault();
                detailView.PositionName = posti.Nameofmembers;

            }

            

           detailView.ClassName = myclass.ClassNumber;

            detailView.GrandName = db_grand.GetGrandByID((int)myclass.grade_Id).GrandName;
            detailView.MajorName = db_major.GetSpecialtyByID((int)myclass.Major_Id).SpecialtyName;


            return detailView;
        }

        /// <summary>
        /// 获取班级的表格类型数据
        /// </summary>
        /// <returns></returns>
        public ClassTableView GetClassTableView(ClassSchedule classSchedule)
        {

            ClassTableView classTableView = new ClassTableView();

            classTableView.ClassNumber = classSchedule.ClassNumber;
            classTableView.ClassRemarks = classSchedule.ClassRemarks;
            classTableView.ClassStatus = classSchedule.ClassStatus;
            classTableView.GradeName = db_grand.GetGrandByID((int)classSchedule.grade_Id).GrandName;
            classTableView.IsDelete = classSchedule.IsDelete;
            classTableView.MajorName = db_major.GetSpecialtyByID((int)classSchedule.Major_Id).SpecialtyName;

            return classTableView;

        }

    }
}
