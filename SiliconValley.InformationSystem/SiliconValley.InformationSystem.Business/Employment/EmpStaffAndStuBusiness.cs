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
        /// 获取员工带学生带ing的记录
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStu> Getising()
        {
            return this.GetEmpStaffAndStus().Where(a => a.Ising == true).ToList();
        }

        /// <summary>
        /// 获取阶段为1的记录
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetEmploymentStage1() {
            return this.GetEmpStaffAndStus().Where(a => a.EmploymentStage == 1).ToList();
        }
        /// <summary>
        /// 获取阶段为2的记录
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetEmploymentStage2()
        {
            return this.GetEmpStaffAndStus().Where(a => a.EmploymentStage == 2).ToList();
        }

        /// <summary>
        /// 返回当前学生正在带的记录
        /// </summary>
        /// <param name="studentno"></param>
        /// <returns></returns>
        public EmpStaffAndStu GetIsingBystudentno(string studentno)
        {
            return this.Getising().Where(a => a.Studentno == studentno).FirstOrDefault();
        }
        /// <summary>
        /// 返回学生阶段1的记录
        /// </summary>
        /// <param name="studentno"></param>
        /// <returns></returns>
        public EmpStaffAndStu GetStage1Bystudentno(string studentno)
        {
            return this.GetEmploymentStage1().Where(a => a.Studentno == studentno).FirstOrDefault();
        }
        /// <summary>
        /// 返回当前学生阶段2的记录
        /// </summary>
        /// <param name="studentno"></param>
        /// <returns></returns>
        public EmpStaffAndStu GetStage2Bystudentno(string studentno)
        {
            return this.GetEmploymentStage2().Where(a => a.Studentno == studentno).FirstOrDefault();
        }
        /// <summary>
        /// 分配好的数据，如果他是二次分配就返回第二次的数据，而不是第一次的数据
        /// </summary>
        /// <returns></returns>
        public EmpStaffAndStu GetEmpStaffAndStuingBystudentno(string studentno)
        {
            List<EmpStaffAndStu> data = this.GetEmpStaffAndStus().Where(a => a.Studentno == studentno).ToList();

            if (data.Count > 1)
            {
                for (int i = data.Count - 1; i >= 0; i--)
                {

                    if (data[i].EmploymentStage != 2)
                    {
                        data.RemoveAt(i);
                    }
                }
                
            }
            return data.FirstOrDefault();
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
        /// 根据学生编号返回这个学生未就业就业分配
        /// </summary>
        /// <returns></returns>
        public EmpStaffAndStu GetEmploymentState3ByStudentno(string studentno) {
           return this.GetEmploymentState3().Where(a=> a.Studentno == studentno).FirstOrDefault();
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
           var data= this.GetEmploymentState3();

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
                view.AreaID = item.AreaID;
                EmpStaffAndStu queryobj = this.GetEmpStaffAndStuingBystudentno(item.StudentNO);
                if (queryobj!=null)
                {
                    view.EmploymentStage = "第二次就业";
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


        /// <summary>
        /// 根据员工id返回当前这个员工目前带的学生
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetEmploymentState1ByEmpid(int empid) {
           return this.Getising().Where(a => a.EmpStaffID == empid).ToList();
        }

        /// <summary>
        /// 返回正在就业的记录
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetEmploymentState1() {
           return this.GetEmpStaffAndStus().Where(a => a.EmploymentState == 1).ToList();
        }
        /// <summary>
        /// 返回已经就业的记录
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetEmploymentState2()
        {
            return this.GetEmpStaffAndStus().Where(a => a.EmploymentState == 2).ToList();
        }
        /// <summary>
        /// 返回未就业的记录
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetEmploymentState3()
        {
            return this.GetEmpStaffAndStus().Where(a => a.EmploymentState == 3).ToList();
        }

        /// <summary>
        /// 传入过来的学生编号转化为对象的分配信息简介
        /// </summary>
        /// <param name="studentno"></param>
        /// <returns></returns>
        public EmpStaffAndStuView studentnoconversionempstaffandstubiew(string studentno)
        {
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbemploymentAreas = new EmploymentAreasBusiness();
            dbstudentIntention = new StudentIntentionBusiness();
            dbquarter = new QuarterBusiness();
            dbproStudentInformation = new ProStudentInformationBusiness();
            StudnetIntention intention = dbstudentIntention.GetStudnetIntentionByStudentNO(studentno);
            EmpStaffAndStuView view = new EmpStaffAndStuView();
            ScheduleForTrainees Trainees = dbproScheduleForTrainees.GetTraineesByStudentNumber(studentno);
            view.classno = Trainees.ClassID;
            view.Areaname = dbemploymentAreas.GetEntity(intention.AreaID).AreaName;
            view.AreaID = intention.AreaID;
            EmpStaffAndStu queryobj = this.GetEmpStaffAndStuingBystudentno(studentno);

            view.EmploymentStage = queryobj.EmploymentStage == 1 ? "第一次就业" : "第二次就业";
            view.ID = queryobj.ID;

            view.QuarterID = intention.QuarterID;
            view.Quartertitle = dbquarter.GetEntity(intention.QuarterID).QuaTitle;
            view.Salary = intention.Salary;
            view.StudentName = dbproStudentInformation.GetEntity(studentno).Name;
            view.StudentNO = studentno;
            return view;
        }

    }
}
