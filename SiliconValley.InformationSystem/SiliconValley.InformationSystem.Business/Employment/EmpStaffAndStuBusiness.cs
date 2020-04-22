using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
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
    public class EmpStaffAndStuBusiness : BaseBusiness<EmpStaffAndStu>
    {
        private StudentIntentionBusiness dbstudentIntention;
        private EmploymentAreasBusiness dbemploymentAreas;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private ProStudentInformationBusiness dbproStudentInformation;
        private ProClassSchedule dbproClassSchedule;
        private QuarterBusiness dbquarter;
        private EmploySituationBusiness dbemploySituation;
        private EmploymentStaffBusiness dbemploymentStaff;
        /// <summary>
        /// 返回全部可用的专员带学生就业信息
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetEmpStaffAndStus() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 返回正在就业的记录
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetEmploymentState1()
        {
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
        /// 获取正在带的学生带ing的记录
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStu> Getising()
        {

            return this.GetEmploymentState1().Where(a => a.Ising == true).ToList();
        }
        /// <summary>
        /// 获取正在带的学生带ing的记录
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetisingByempid(int empid)
        {

            return this.Getising().Where(a => a.EmpStaffID == empid).ToList();
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
            if (this.GetEmpStaffAndStus().Where(a => a.Studentno == studentno).FirstOrDefault() != null)
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
            return this.GetEmploymentState3().Where(a => a.Studentno == studentno).FirstOrDefault();
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
            var data = this.GetEmploymentState3();

            var list1 = dbstudentIntention.GetnodistributionIntentions();
            foreach (var item in data)
            {
                list1.Add(dbstudentIntention.GetStudnetIntentionByStudentNO(item.Studentno));
            }
            foreach (var item in list1)
            {
                EmpStaffAndStuView view = new EmpStaffAndStuView();
                ScheduleForTrainees Trainees = dbproScheduleForTrainees.GetTraineesByStudentNumber(item.StudentNO);
                view.classno = Trainees.ClassID;
                view.Areaname = dbemploymentAreas.GetEntity(item.AreaID).AreaName;
                view.AreaID = item.AreaID;
                EmpStaffAndStu queryobj = this.GetEmpStaffAndStuingBystudentno(item.StudentNO);
                if (queryobj != null)
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
        /// 传入过来的学生编号转化为对象的分配信息简介
        /// </summary>
        /// <param name="studentno"></param>
        /// <returns></returns>
        public EmpStaffAndStuView studentnoconversionempstaffandstubiew(string studentno)
        {
            dbemploymentStaff = new EmploymentStaffBusiness();
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
            EmployeesInfo employeesInfo = dbproStudentInformation.GetEEmpinfoByStudentNumber(studentno);
            view.empname = employeesInfo.EmpName;

            switch (queryobj.EmploymentState)
            {
                case 1:
                    view.EmploymentState = "就业中";
                    break;
                case 2:
                    view.EmploymentState = "已就业";
                    break;
                case 3:
                    view.EmploymentState = "未就业";
                    break;
            }
            return view;
        }

        /// <summary>
        /// 根据就业专员id 跟计划id 返回出这个带学生数据
        /// </summary>
        /// <param name="QuarterID"></param>
        /// <param name="empid"></param>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetAllByQuarter(int QuarterID, int empid)
        {
            List<EmpStaffAndStu> data = this.GetEmploymentState1ByEmpid(empid);
            for (int i = data.Count - 1; i >= 0; i--)
            {
                if (data[i].QuarterID != QuarterID)
                {
                    data.RemoveAt(i);
                }
            }
            return data;
        }

        /// <summary>
        /// 根据计划id 返回全部的数据
        /// </summary>
        /// <param name="QuarterID"></param>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetAllByQuarter(int QuarterID) {
            dbemploymentStaff = new EmploymentStaffBusiness();
            List<EmploymentStaff> emplist = dbemploymentStaff.GetIQueryable().ToList();
            List<EmpStaffAndStu> result = new List<EmpStaffAndStu>();
            foreach (var item in emplist)
            {
                result.AddRange(this.GetAllByQuarter(QuarterID, item.ID));
            }
            return result;
        }
        /// <summary>
        /// 根据年度获取员工数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetAllByYear(int year, int empid)
        {
            dbquarter = new QuarterBusiness();
            List<Quarter> data = dbquarter.GetQuartersByYear(year);
            List<EmpStaffAndStu> result = new List<EmpStaffAndStu>();
            foreach (var item in data)
            {
                result.AddRange(this.GetAllByQuarter(item.ID, empid));
            }
            return result;
        }
        /// <summary>
        /// 根据年度获取全部数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetAllByYear(int year) {
            dbquarter = new QuarterBusiness();
            List<Quarter> data = dbquarter.GetQuartersByYear(year);
            List<EmpStaffAndStu> result = new List<EmpStaffAndStu>();
            foreach (var item in data)
            {
                result.AddRange(this.GetAllByQuarter(item.ID));
            }
            return result;
        }

        /// <summary>
        /// 将集合EmpStaffAndStu 转化为结合EmpStaffAndStuVIew
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<EmpStaffAndStuView> EmpStaffAndStuConversionEmpStaffAndStuView(List<EmpStaffAndStu> data) {
            List<EmpStaffAndStuView> result = new List<EmpStaffAndStuView>();
            foreach (var item in data)
            {
                result.Add(this.studentnoconversionempstaffandstubiew(item.Studentno));
            }
            return result;
        }


        /// <summary>
        /// 获取该专员该季度带学生转化过后i的数据
        /// </summary>
        public List<EmpStaffAndStuView> Conversioned(int QuarterID, int empid)
        {
            List<EmpStaffAndStu> data = this.GetAllByQuarter(QuarterID, empid);
            return this.EmpStaffAndStuConversionEmpStaffAndStuView(data);
        }
        /// <summary>
        /// 获取该季度带学生转化过后i的数据
        /// </summary>
        public List<EmpStaffAndStuView> Conversioned(int QuarterID)
        {
            List<EmpStaffAndStu> data = this.GetAllByQuarter(QuarterID);
            return this.EmpStaffAndStuConversionEmpStaffAndStuView(data);
        }
        /// <summary>
        /// 获取该专员该年度带学生转化过后i的数据
        /// </summary>
        public List<EmpStaffAndStuView> Conversioned(string year, int empid)
        {
            int paramyear = int.Parse(year);
            List<EmpStaffAndStu> data = this.GetAllByYear(paramyear, empid);
            return this.EmpStaffAndStuConversionEmpStaffAndStuView(data);
        }
        /// <summary>
        /// 获取年度带学生转化过后i的数据
        /// </summary>
        public List<EmpStaffAndStuView> Conversioned(string year)
        {
            int paramyear = int.Parse(year);
            List<EmpStaffAndStu> data = this.GetAllByYear(paramyear);
            return this.EmpStaffAndStuConversionEmpStaffAndStuView(data);
        }

        /// <summary>
        /// 获取正在带带学生转化过后i的数据
        /// </summary>
        public List<EmpStaffAndStuView> Conversioned(bool isJurisdiction, int empid)
        {
            List<EmpStaffAndStu> data = new List<EmpStaffAndStu>();
            if (isJurisdiction)
            {
                data = this.Getising();
            }
            else
            {
                data = this.GetisingByempid(empid);
            }

            return this.EmpStaffAndStuConversionEmpStaffAndStuView(data);
        }

        /// <summary>
        ///  全部 计划
        /// </summary>
        /// <returns></returns>
        public List<Quarter> Quarters() {
            return this.infoconversiontoquart(this.GetEmpStaffAndStus());
        }


        /// <summary>
        /// 全部数据中的这个员工带的东西
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetEmpStaffAndStusByempid(int empid) {
            return this.GetEmpStaffAndStus().Where(a => a.EmpStaffID == empid).ToList();
        }

        /// <summary>
        ///  员工 计划
        /// </summary>
        /// <returns></returns>
        public List<Quarter> Quarters(int empid)
        {
            return this.infoconversiontoquart(this.GetEmpStaffAndStusByempid(empid));
        }

        /// <summary>
        ///  分配表转化为计划   
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Quarter> infoconversiontoquart(List<EmpStaffAndStu> data) {
            dbquarter = new QuarterBusiness();
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = data.Count - 1; j > i; j--)  //内循环是 外循环一次比较的次数
                {
                    if (data[i].QuarterID == data[j].QuarterID)
                    {
                        data.RemoveAt(j);
                    }
                }
            }
            List<Quarter> result = new List<Quarter>();
            foreach (var item in data)
            {
                result.Add(dbquarter.GetEntity(item.QuarterID));
            }
            return result;
        }

        /// <summary>
        /// 删除这个员工现在带的学生记录而且是正在就业的记录
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool delempstaffandstuByempid(int empid) {
            bool result = false;
            try
            {
                var data = this.GetEmploymentState1ByEmpid(empid);
                foreach (var item in data)
                {
                    ///如果是第一阶段 删除之后再添加一个二次就业记录
                    ///如是是第二阶段，删除当前ising=false,再将empid 修改为空
                    if (item.EmploymentStage == 1)
                    {
                        item.Ising = false;
                        this.Update(item);
                        EmpStaffAndStu newobj = new EmpStaffAndStu();
                        newobj.Date = DateTime.Now;
                        newobj.EmploymentStage = 2;
                        newobj.EmploymentState = 1;
                        newobj.EmpStaffID = null;
                        newobj.IsDel = false;
                        newobj.Ising = false;
                        newobj.QuarterID = item.QuarterID;
                        newobj.Remark = string.Empty;
                        newobj.Studentno = item.Studentno;
                        this.Insert(newobj);
                    }
                    else
                    {
                        item.Ising = false;
                        item.EmpStaffID = null;
                        this.Update(item);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {

                result = false;
            }
            return result;
        }


        #region 就业统计涉及方法
        /// <summary>
        /// 获取全部得数据中被带得记录 根据计划id 进行筛选
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetEmpstaffAndStuinfodataByQuarterid(int param0) {
            return this.GetEmpStaffAndStus().Where(a => a.Ising == true & a.QuarterID == param0).ToList();
        }
        /// <summary>
        /// 获取全部得数据中被带得记录 根据计划id 进行筛选
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetEmpstaffAndStuinfodataByyear(int param0)
        {
            dbquarter = new QuarterBusiness();
            var list = dbquarter.GetQuartersByYear(param0);
            List<EmpStaffAndStu> result = new List<EmpStaffAndStu>();
            foreach (var item in list)
            {
                result.AddRange(this.GetEmpstaffAndStuinfodataByQuarterid(item.ID));

            }
            return result;
        }
        public List<EmpStaffAndStu> GetEmploymentSummaryData(bool isyear, int param0, int? empid) {

            List<EmpStaffAndStu> result = new List<EmpStaffAndStu>();
            if (isyear)
            {
                result = this.GetEmpstaffAndStuinfodataByyear(param0);
            }
            else
            {
                result = this.GetEmpstaffAndStuinfodataByQuarterid(param0);
            }
            if (empid != null)
            {
                result = result.Where(a => a.EmpStaffID == empid).ToList();
            }

            return result;

        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="majorid"></param>
        /// <param name="Year"></param>
        /// <param name="type">自主就业，安排就业</param>
        /// <returns></returns>
        public List<EmpStaffAndStu> EmploymentRatio(int majorid, string type ,string Year = null)
        {
            
            List<EmpStaffAndStu> result = new List<EmpStaffAndStu>();

            //List<EmpStaffAndStu> allist = new List<EmpStaffAndStu>();

            //dbemploySituation = new EmploySituationBusiness();
            //if (date == null)
            //    allist = this.GetEmpStaffAndStus();
            //else
            //    allist = this.GetEmpStaffAndStus().Where(d=> dbemploySituation.GetSituationByStudentno(d.Studentno).FirstOrDefault().employedDate.Year);


            //var dbproScheduleForTrainees = new ProScheduleForTrainees();
            //var class_db = new ClassScheduleBusiness();
            //foreach (var item in allist)
            //{
            //    //获取到班级
            //    ScheduleForTrainees Trainees = dbproScheduleForTrainees.GetTraineesByStudentNumber(item.Studentno);

            //    var classObj = class_db.GetList().Where(d => d.id == Trainees.ID_ClassName).FirstOrDefault();

            //    if (majorid == classObj.Major_Id)

            //        result.Add(item);

            //}

            return result;
        }

    }
}
