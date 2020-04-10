using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiliconValley.InformationSystem.Business.Employment
{
    public class EmpClassBusiness : BaseBusiness<EmpClass>
    {

        private EmploymentStaffBusiness dbemploymentStaffBusiness;
        private ProClassSchedule dbproClassSchedule;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private SurveyRecordsBusiness dbsurveyRecords;
        private CDInterviewBusiness dbCDInterview;
        private EmploymentStaffBusiness dbemploymentStaff;
        private SimulatInterviewBusiness dbsimulatInterview;
        /// <summary>
        /// 获取所有的专员带班记录
        /// </summary>
        /// <returns>专员带班集合</returns>
        public List<EmpClass> GetEmpClassFormServer()
        {

            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        public EmpClass GetEmpclassByclassid(int classid) {
           return this.GetEmpClassFormServer().Where(a => a.ClassId == classid).FirstOrDefault();
        }



        // <summary>
        /// 根据就业专员id获取就业专员带班记录
        /// </summary>
        /// <param name="EmplotStaffID"></param>
        /// <returns></returns>
        public List<EmpClass> GetEmpsByEmpID(int EmplotStaffID)
        {
            return this.GetEmpClassFormServer().Where(a => a.EmpStaffID == EmplotStaffID).ToList();
        }


        /// <summary>
        /// 获取所有的班级对象
        /// </summary>
        /// <returns></returns>
        public List<ClassSchedule> GetClassFormServer()
        {

            ClassScheduleBusiness dbclass = new ClassScheduleBusiness();
            return dbclass.GetIQueryable().Where(a => a.IsDelete == false && a.ClassstatusID == null).ToList();


        }

        /// <summary>
        /// 根据班级id获取已毕业班级
        /// </summary>
        /// <param name="classiD"></param>
        /// <returns></returns>
        public ClassSchedule GetClassedByID(int classiD)
        {
            return this.GetClassFormServer().Where(a => a.id == classiD && a.ClassStatus == true).FirstOrDefault();
        }
        /// <summary>
        /// 根据班级id获取正在学习的班级
        /// </summary>
        /// <param name="classiD"></param>
        /// <returns></returns>
        public ClassSchedule GetClassingByID(int classiD)
        {
            return this.GetClassFormServer().Where(a => a.id == classiD  && a.ClassStatus == false).FirstOrDefault();
        }
        /// <summary>
        /// 获取带班已毕业的
        /// </summary>
        /// <param name="emps"></param>
        /// <returns></returns>
        public List<ClassSchedule> GetClassedList(List<EmpClass> emps)
        {
            List<ClassSchedule> classedList = new List<ClassSchedule>();
            foreach (var item in emps)
            {
                ClassSchedule classed = this.GetClassedByID(item.ClassId);
                classedList.Add(classed);
            }
            return classedList;
        }

        /// <summary>
        /// 获取带班未毕业的
        /// </summary>
        /// <param name="emps"></param>
        /// <returns></returns>
        public List<ClassSchedule> GetClassingList(List<EmpClass> emps)
        {
            List<ClassSchedule> classedList = new List<ClassSchedule>();
            foreach (var item in emps)
            {
                ClassSchedule classed = this.GetClassingByID(item.ClassId);
                classedList.Add(classed);
            }
            return classedList;
        }

        /// <summary>
        /// 获取s3跟s4的班级以及没有毕业的
        /// </summary>
        /// <returns></returns>
        public List<ClassSchedule> GetS3Class()
        {
            var resultdata = this.GetClassFormServer().Where(a => a.grade_Id == 3 || a.grade_Id == 4).ToList();
            return resultdata.Where(a => a.ClassStatus == false & a.IsDelete == false).ToList();
        }
        /// <summary>
        /// 获取所有的毕业班
        /// </summary>
        /// <returns></returns>
        public List<ClassSchedule> GetGraduations()
        {
            var resultdata = this.GetClassFormServer().Where(a => a.grade_Id == 4).ToList();
            return resultdata.Where(a => a.ClassStatus == true & a.IsDelete == false).ToList();
        }


        /// <summary>
        /// 判断班级是否是毕业班
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool IsGraduation(ClassSchedule schedule)
        {
            if (schedule.IsDelete == false && schedule.ClassStatus == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 根据员工id 返回出这个员工现在带班对象 （没毕业的）
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public List<EmpClass> GetEmpClassesindByempid(int empid) {
            dbproClassSchedule = new ProClassSchedule();
            var data = this.GetEmpsByEmpID(empid);
            for (int i = data.Count - 1; i >= 0; i--)
            {
                ClassSchedule classSchedule= dbproClassSchedule.GetEntity(data[i].ClassId);
                if (this.IsGraduation(classSchedule))
                {
                    data.RemoveAt(i);
                }
            }
            return data;
        }


        /// <summary>
        /// 删除传入过来的员工id正在带班的记录
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool delempforclass(int empid) {
            bool result = false;
            try
            {
                var data = this.GetEmpClassesindByempid(empid);
                foreach (var item in data)
                {
                    item.IsDel = true;
                    this.Update(item);
                }
                result = true;
            }
            catch (Exception ex)
            {

                result = false;
            }
            return result;
            
        }
        /// <summary>
        /// 获取s3或者时s4没有毕业也还没分配专员的对象集合
        /// </summary>
        /// <returns></returns>
        public List<ClassSchedule> NoDistribution()
        {
            // 获取s3跟s4的班级以及没有毕业的对象
            var alldata = this.GetS3Class();
            var resultdata = this.GetS3Class();
            //带班记录
            var empclasslist = this.GetEmpClassFormServer();
            //分配的班级它的班级编号就会出现在这个带班记录中

            for (int i = alldata.Count - 1; i >= 0; i--)
            {
                foreach (var empclass in empclasslist)
                {
                    if (alldata[i].id == empclass.ClassId)
                    {
                        alldata.Remove(alldata[i]);
                        break;
                    }
                }
            }
            return alldata;
        }

        /// <summary>
        /// 获取所有的阶段对象
        /// </summary>
        /// <returns></returns>
        public List<Grand> GetGrandAll()
        {
            BaseBusiness<Grand> dbgrand = new BaseBusiness<Grand>();
            return dbgrand.GetIQueryable().Where(a => a.IsDelete == false).ToList();
        }

        /// <summary>
        /// 根据阶段id 获取阶段对象
        /// </summary>
        /// <returns></returns>
        public Grand GetGrandByID(int? GrandID)
        {
            return this.GetGrandAll().Where(a => a.Id == GrandID).FirstOrDefault();

        }
        /// <summary>
        /// 根据班级编号返回阶段对象
        /// </summary>
        /// <param name="ClassNo"></param>
        /// <returns></returns>
        public Grand GetGrandByClassid(int classid)
        {
            var classdata = this.GetClassFormServer().Where(a => a.id == classid).FirstOrDefault();
            return this.GetGrandByID(classdata.grade_Id);
        }
        /// <summary>
        /// 添加专员带班
        /// </summary>
        /// <param name="empClass"></param>
        /// <returns></returns>

        public bool AddEmpClass(EmpClass empClass)
        {

            bool result = false;
            try
            {
                this.Insert(empClass);
                result = true;

                //BusHelper.WriteSysLog("Obtainemployment区域EmpClass控制器ClassToEmpstaff方法成功", EnumType.LogType.上传文件异常);
            }
            catch (Exception ex)
            {
                result = false;
                //BusHelper.WriteSysLog("Obtainemployment区域EmpClass控制器ClassToEmpstaff方法", EnumType.LogType.上传文件异常);
            }
            return result;
        }

        /// <summary>
        /// 根据员工编号返回员工带班记录
        /// </summary>
        /// <param name="empinfoid"></param>
        /// <returns></returns>
        public List<EmpClass> GetEmpClassesByempinfoid(string empinfoid)
        {
            dbemploymentStaffBusiness = new EmploymentStaffBusiness();
            var a = dbemploymentStaffBusiness.GetEmploymentByEmpInfoID(empinfoid);
            return this.GetEmpsByEmpID(a.ID);
        }

        /// <summary>
        /// 留下未毕业的班级
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public List<EmpClass> Leavebehinding(List<EmpClass> result)
        {
            dbproClassSchedule = new ProClassSchedule();
            for (int i = result.Count - 1; i >= 0; i--)
            {
                if (dbproClassSchedule.isgraduationclass(result[i].ClassId))
                {
                    result.Remove(result[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// 留下未毕业的班级
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public List<EmpClass> Leavebehinded(List<EmpClass> result)
        {
            dbproClassSchedule = new ProClassSchedule();
            for (int i = result.Count - 1; i >= 0; i--)
            {
                if (!dbproClassSchedule.isgraduationclass(result[i].ClassId))
                {
                    result.Remove(result[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// 根据班级编号返回对应的数据
        /// </summary>
        /// <param name="ClassNumber"></param>
        /// <returns></returns>
        public List<EmpClass> CorrespondingByClassNumber(List<EmpClass> result, string ClassNumber) {
            dbproClassSchedule = new ProClassSchedule();
            for (int i = result.Count - 1; i >= 0; i--)
            {
                var query = dbproClassSchedule.GetEntity(result[i].ClassId);
                if (query.ClassNumber != ClassNumber)
                {
                    result.Remove(result[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// 转化 将就业部带班记录转为EmpClassView model
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public List<SResearchEmpClassView> Conversion(List<EmpClass> result) {
            dbproClassSchedule = new ProClassSchedule();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbsurveyRecords = new SurveyRecordsBusiness();
            List<SResearchEmpClassView> views = new List<SResearchEmpClassView>();
            foreach (var item in result)
            {
                var query = dbproClassSchedule.GetEntity(item.ClassId);
                //该班级的访谈记录
                var surveylist = dbsurveyRecords.GetSurveyRecordsByclassno(item.ClassId);
                //班级学生记录
                var trainesslist = dbproScheduleForTrainees.GetTraineesByClassid(item.ClassId);

                SResearchEmpClassView empClassView = new SResearchEmpClassView();

                empClassView.classid = query.id;
                empClassView.classnumber = query.ClassNumber;
                empClassView.interviewcount = surveylist.Count;
                empClassView.totalnumber = trainesslist.Count;
                empClassView.repeatedinterviews = 0;
                for (int i = 0; i < surveylist.Count; i++)
                {
                    for (int j = surveylist.Count - 1; j > i; j--)  //内循环是 外循环一次比较的次数
                    {
                        if (surveylist[i].StudentNO == surveylist[j].StudentNO)
                        {
                            surveylist.RemoveAt(j);
                            empClassView.repeatedinterviews = empClassView.repeatedinterviews + 1;
                        }
                    }
                }
                empClassView.peoplecount = surveylist.Count;

                if (dbproClassSchedule.isgraduationclass(query.id))
                    empClassView.isgraduation = true;
                else
                    empClassView.isgraduation = false;
                views.Add(empClassView);
            }
            return views;
        }

        /// <summary>
        /// 转化 将就业部带班记录转为SimulatInterviewView 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public List<SimulatInterviewView> Conversion1(List<EmpClass> result)
        {
            dbproClassSchedule = new ProClassSchedule();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbsimulatInterview = new SimulatInterviewBusiness();
            List<SimulatInterviewView> views = new List<SimulatInterviewView>();
        
            foreach (var item in result)
            {
                var query = dbproClassSchedule.GetEntity(item.ClassId);

                //该班级的面试记录
                var surveylist = dbsimulatInterview.GetSimulatInterviewsByclassid(item.ClassId);

                //班级学生记录
                var trainesslist = dbproScheduleForTrainees.GetTraineesByClassid(item.ClassId);

                SimulatInterviewView simulatInterviewView = new SimulatInterviewView();

                simulatInterviewView.classid = query.id;
                simulatInterviewView.classnumber = query.ClassNumber;
                simulatInterviewView.interviewcount = surveylist.Count;
                simulatInterviewView.totalnumber = trainesslist.Count;
                simulatInterviewView.repeatedinterviews = 0;
                for (int i = 0; i < surveylist.Count; i++)
                {
                    for (int j = surveylist.Count - 1; j > i; j--)  //内循环是 外循环一次比较的次数
                    {
                        if (surveylist[i].StudentNo == surveylist[j].StudentNo)
                        {
                            surveylist.RemoveAt(j);
                            simulatInterviewView.repeatedinterviews = simulatInterviewView.repeatedinterviews + 1;
                        }
                    }
                }
                simulatInterviewView.peoplecount = surveylist.Count;

                if (dbproClassSchedule.isgraduationclass(query.id))
                    simulatInterviewView.isgraduation = true;
                else
                    simulatInterviewView.isgraduation = false;
                views.Add(simulatInterviewView);

            }
            return views;
        }

        /// <summary>
        /// 转化 数据返回右侧数据 cd
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public List<SResearchEmpClassView> ConversiontoCD(List<EmpClass> result) {
            dbproClassSchedule = new ProClassSchedule();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbCDInterview = new CDInterviewBusiness();
            dbsurveyRecords = new SurveyRecordsBusiness();
            List<SResearchEmpClassView> views = new List<SResearchEmpClassView>();
            foreach (var item in result)
            {
                SResearchEmpClassView classView = new SResearchEmpClassView();
                var query = dbproClassSchedule.GetEntity(item.ClassId);
                classView.classid = query.id;
                classView.classnumber = query.ClassNumber;
                if (query.ClassStatus == true)
                    classView.isgraduation = true;
                else
                    classView.isgraduation = false;
                List<CDInterview> querycdlist = dbCDInterview.GetCDInterviewsByClassid(item.ClassId);
                classView.interviewcount = querycdlist.Count;
                classView.totalnumber = dbsurveyRecords.GetCDSurveyRecordsByclassid(item.ClassId).Count;
                for (int i = 0; i < querycdlist.Count; i++)
                {
                    for (int j = querycdlist.Count - 1; j > i; j--)  //内循环是 外循环一次比较的次数
                    {
                        if (querycdlist[i].StudentNO == querycdlist[j].StudentNO)
                        {
                            querycdlist.RemoveAt(j);
                            classView.repeatedinterviews = classView.repeatedinterviews + 1;
                        }
                    }
                }
                classView.peoplecount = querycdlist.Count;
                views.Add(classView);
            }
            return views;
        }

        /// <summary>
        /// 跟班级id返回员工带班记录
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public EmpClass GetEmpClassByclassid(int classid) {
            return this.GetIQueryable().Where(a => a.IsDel == false).Where(a => a.ClassId == classid).FirstOrDefault();
        }

        /// <summary>
        /// 根据班级编号返回就业员工
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public EmploymentStaff GetStaffByClassid(int classid) {
            EmpClass empClass = this.GetEmpClassByclassid(classid);
            dbemploymentStaff = new EmploymentStaffBusiness();
            return dbemploymentStaff.GetEntity(empClass.EmpStaffID);
        }
        /// <summary>
        /// 根据班级编号返回就业员工2.0
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public EmploymentStaff GetStaffByClassids(int classid)
        {
            EmpClass empClass = this.GetEmpClassByclassid(classid);
            if (empClass!=null)
            {
                dbemploymentStaff = new EmploymentStaffBusiness();
                return dbemploymentStaff.GetEntity(empClass.EmpStaffID);
            }return new EmploymentStaff();
            
        }

        /// <summary>
        /// 升学改班级
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public bool entrance(int oldclassid,int newclassid) {
            bool result = false;
            try
            {
                var data = this.GetEmpclassByclassid(oldclassid);
                if (data == null)
                {
                    result = true;
                }
                else
                {
                    data.ClassId = newclassid;
                    this.Update(data);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
    }
}
