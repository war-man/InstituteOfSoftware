using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    public class EmpStaffAndStuBusiness:BaseBusiness<EmpStaffAndStu>
    {
        private StudentIntentionBusiness dbstudentIntention;
        private EmploymentAreasBusiness dbemploymentAreas;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private ProStudentInformationBusiness dbproStudentInformation;
        private ProClassSchedule dbproClassSchedule;
        private QuarterBusiness dbquarter;
        /// <summary>
        /// 返回全部可用的专员带学生就业信息
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetEmpStaffAndStus() {
           return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 判断这个学生编号是否存在分配
        /// </summary>
        /// <param name="studentno"></param>
        /// <returns></returns>
        public bool isdistribution(string studentno) {
            bool result = false;
            if (this.GetEmpStaffAndStus().Where(a => a.Studentno == studentno).FirstOrDefault()!=null)
            {
                result = true;
            }
            return result;
        }

        

        /// <summary>
        /// 根据学生编号返回这个学生就业分配
        /// </summary>
        /// <returns></returns>
        public EmpStaffAndStu GetEmpStaffAndStuByStudentno(string studentno) {
           return this.GetEmpStaffAndStus().Where(a => a.EmploymentState == 3 && a.Studentno == studentno).FirstOrDefault();
        }

        /// <summary>
        /// 获取未分配的学生以及分配中EmploymentState状态为3：未就业
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStuView> Getnodistribution() {
            dbstudentIntention = new StudentIntentionBusiness();
            dbemploymentAreas = new EmploymentAreasBusiness();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbproClassSchedule = new ProClassSchedule();
            dbemploymentAreas = new EmploymentAreasBusiness();
            dbquarter = new QuarterBusiness();
            dbproStudentInformation = new ProStudentInformationBusiness();
            List<EmpStaffAndStuView> stuViews = new List<EmpStaffAndStuView>();
           var data= this.GetEmpStaffAndStus().Where(a => a.EmploymentState == 3).ToList();
            var list1 = dbstudentIntention.GetnodistributionIntentions();
            foreach (var item in data)
            {
                list1.Add(dbstudentIntention.GetStudnetIntentionByStudentNO(item.Studentno));
            }
            foreach (var item in list1)
            {
                EmpStaffAndStuView view = new EmpStaffAndStuView();
               ScheduleForTrainees Trainees= dbproScheduleForTrainees.GetTraineesByStudentNumber(item.StudentNO);
                view.classno = Trainees.ClassID;
                view.Areaname = dbemploymentAreas.GetEntity(item.AreaID).AreaName;
                EmpStaffAndStu queryobj=  this.GetEmpStaffAndStuByStudentno(item.StudentNO);
                if (queryobj!=null)
                {
                    view.EmploymentStage = queryobj.EmploymentStage==1?"第一次就业":"第二次就业";
                    view.ID = queryobj.ID;
                }
                else
                {
                    view.EmploymentStage = "第一次就业";
                    view.ID = -1;
                }
                view.QuarterID = item.QuarterID;
                view.Quartertitle = dbquarter.GetEntity(item.QuarterID).QuaTitle;
                view.Salary = item.Salary;
                view.StudentName = dbproStudentInformation.GetEntity(item.StudentNO).Name;
                view.StudentNO = item.StudentNO;
                stuViews.Add(view);
            }
            
            return stuViews;
        }
    }
}
