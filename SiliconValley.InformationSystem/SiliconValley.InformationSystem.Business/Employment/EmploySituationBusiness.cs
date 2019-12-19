using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    public class EmploySituationBusiness : BaseBusiness<EmploySituation>
    {
        private QuarterBusiness dbquarter;
        private EmploymentJurisdictionBusiness dbemploymentJurisdiction;
        private EmploymentStaffBusiness dbemploymentStaff;
        private EnterpriseInfoBusiness dbenterpriseInfo;
        private StudentIntentionBusiness dbstudentIntention;
        private EmploymentAreasBusiness dbemploymentAreas;
        private ProStudentInformationBusiness dbproStudentInformation;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private EmpStaffAndStuBusiness dbempStaffAndStu;
        /// <summary>
        /// 加载右侧的树
        /// </summary>
        /// <returns></returns>
        public resultdtree loadtree() {
            dbquarter = new QuarterBusiness();
            dbemploymentJurisdiction = new EmploymentJurisdictionBusiness();
            dbemploymentStaff = new EmploymentStaffBusiness();
            dbempStaffAndStu = new EmpStaffAndStuBusiness();
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            var queryempstaff = dbemploymentStaff.GetEmploymentByEmpInfoID(user.EmpNumber);
            //第一层
            var querydata = new List<EmploymentYearView>();
            bool isJurisdiction = dbemploymentJurisdiction.isstaffJurisdiction(user);
            var data = new List<Quarter>();
            if (!isJurisdiction)
            {
                data= dbempStaffAndStu.Quarters(queryempstaff.ID);

                querydata = dbquarter.yearplan(data);
            }
            else
            {
               data = dbempStaffAndStu.Quarters();
                querydata = dbquarter.yearplan(data);
            }

            //返回的结果
            resultdtree result = new resultdtree();

            //状态
            dtreestatus dtreestatus = new dtreestatus();

            //最外层的儿子数据
            List<dtreeview> childrendtreedata = new List<dtreeview>();

            for (int i = 0; i < querydata.Count; i++)
            {
                //第一层
                dtreeview seconddtree = new dtreeview();
                try
                {
                    if (i == 0)
                    {
                        seconddtree.spread = true;
                    }
                    seconddtree.nodeId = querydata[i].Year.ToString();
                    seconddtree.context = querydata[i].YearTitle;
                    seconddtree.last = false;
                    seconddtree.parentId = "0";
                    seconddtree.level = 0;

                    if (data.Count > 0)
                    {

                        //第二层的tree数据
                        List<dtreeview> Quarterlist = new List<dtreeview>();
                        for (int j = 0; j < data.Count; j++)
                        {
                            dtreeview Quarters = new dtreeview();
                            Quarters.nodeId = data[j].ID.ToString();
                            Quarters.context = data[j].QuaTitle;
                            Quarters.last = true;
                            Quarters.parentId = querydata[i].Year.ToString();
                            Quarters.level = 1;
                            Quarterlist.Add(Quarters);


                        }
                        seconddtree.children = Quarterlist;

                    }
                    else
                    {
                        seconddtree.last = true;
                    }
                    dtreestatus.code = "200";
                    dtreestatus.message = "操作成功";
                }
                catch (Exception ex)
                {
                    dtreestatus.code = "1";
                    dtreestatus.code = "操作失败";
                    throw;
                }
                childrendtreedata.Add(seconddtree);
            }

            result.status = dtreestatus;
            result.data = childrendtreedata;
            return result;
        }

        /// <summary>
        /// 获取可用的数据
        /// </summary>
        /// <returns></returns>
        public List<EmploySituation> GetEmploySituations() {
           return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }


        /// <summary>
        /// 根据计划id 返回出这个计划的记录
        /// </summary>
        /// <param name="Quarterid"></param>
        /// <returns></returns>
        public List<EmploySituation> GetSituationsByQuarterid(int Quarterid) {
            dbstudentIntention = new StudentIntentionBusiness();
           var data=  this.GetEmploySituations();
            for (int i = data.Count - 1; i >= 0; i--)
            {
                if (dbstudentIntention.GetInformationBystudentno(data[i].StudentNO).QuarterID != Quarterid)
                {
                    data.RemoveAt(i);
                }
            }
            return data;
        }

        /// <summary>
        /// 年 记录
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<EmploySituation> GetSituationsByear(int year) {
            dbquarter = new QuarterBusiness();
           var quaters=  dbquarter.GetQuartersByYear(year);
            List<EmploySituation> result = new List<EmploySituation>();
            foreach (var item in quaters)
            {
                result.AddRange(this.GetSituationsByQuarterid(item.ID));
            }
            return result;
        }

        /// <summary>
        /// 年 员工 记录
        /// </summary>
        /// <param name="year"></param>
        /// <param name="empid"></param>
        /// <returns></returns>
        public List<EmploySituation> GetSituationsByear(int year,int empid)
        {
           return this.GetSituationsByear(year).Where(a => a.empid == empid).ToList();
        }
        /// <summary>
        /// 根据员工id 返回出登记的记录
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public List<EmploySituation> GetEmploySituationsByempid(int Quarterid, int empid) {
           return this.GetSituationsByQuarterid(Quarterid).Where(a => a.empid == empid).ToList();
        }
        /// <summary>
        /// 获取毕业的学生
        /// </summary>
        /// <returns></returns>
        public List<EmploySituation> Getgraduation() {
           return this.GetEmploySituations().Where(a => a.EntinfoID != null).ToList();
        }

        /// <summary>
        /// 获取未毕业的学生
        /// </summary>
        /// <returns></returns>
        public List<EmploySituation> Getungraduation()
        {
            return this.GetEmploySituations().Where(a => a.EntinfoID == null).ToList();
        }

        /// <summary>
        /// 根据学生编号返回出这个学生就业情况
        /// </summary>
        /// <param name="Studentno"></param>
        /// <returns></returns>
        public List<EmploySituation> GetSituationByStudentno(string Studentno)
        {
            return this.GetEmploySituations().Where(a => a.StudentNO == Studentno).ToList();
        }

        /// <summary>
        /// 根据这个原型转化为view 视图对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public EmploySituationView objconversiontoview(EmploySituation obj) {
            dbenterpriseInfo = new EnterpriseInfoBusiness();
            dbstudentIntention = new StudentIntentionBusiness();
            dbemploymentAreas = new EmploymentAreasBusiness();
            dbproStudentInformation = new ProStudentInformationBusiness();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbemploymentStaff = new EmploymentStaffBusiness();
            EmploySituationView view = new EmploySituationView();
            view.ID = obj.ID;
            view.Date = obj.Date;
            view.EntinfoID = obj.EntinfoID;
            if (obj.EntinfoID==null)
            {
                view.EntinfoName = "未找到工作！暂无公司！";
            }
            else
            {
                view.EntinfoName = dbenterpriseInfo.GetEntity(obj.EntinfoID).EntName;
            }
            
            view.NoReasons = obj.NoReasons;
            view.RealWages = obj.RealWages;
            view.Remark = obj.Remark;
            StudnetIntention intention= dbstudentIntention.GetInformationBystudentno(obj.StudentNO);
            view.Salary = intention.Salary;
            EmploymentAreas areas= dbemploymentAreas.GetEntity(intention.AreaID);
            view.City = areas.AreaName;
            StudentInformation  student = dbproStudentInformation.GetEntity(obj.StudentNO);
            view.StudentName = student.Name;
            view.Telephone = student.Telephone;
            view.classno = dbproScheduleForTrainees.GetTraineesByStudentNumber(obj.StudentNO).ClassID;
            view.empname = dbemploymentStaff.GetEmpInfoByEmpID(obj.empid).EmpName;
            return view;
        }

        /// <summary>
        /// 集合转化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<EmploySituationView> objconversiontoview(List<EmploySituation> obj) {
            List<EmploySituationView> view = new List<EmploySituationView>();
            foreach (var item in obj)
            {
                view.Add(this.objconversiontoview(item));
            }
            return view;
        }

        /// <summary>
        /// 将意向数据转化为就业数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<EmploySituation> intentionconversiontosituation(List<StudnetIntention>data) {
            List<EmploySituation> reuslt = new List<EmploySituation>();
            foreach (var item in data)
            {
                reuslt.AddRange(this.GetSituationByStudentno(item.StudentNO));
            }
            return reuslt;
        }

        /// <summary>
        /// 如果是年，再者是主任，或者这年全部数据如果是员工，那就返回这个年员工的数据,不是年就是季度 同理
        /// </summary>
        /// <param name="isdirector"></param>
        /// <param name="year"></param>
        /// <param name="param0"></param>
        /// <param name="empid"></param>
        /// <returns></returns>
        public List<EmploySituation> GetSituationsByQuarterid(bool isdirector,bool year,int param0,int empid) {

            List<EmploySituation> result1 = new List<EmploySituation>();
            dbstudentIntention = new StudentIntentionBusiness();
            if (year)
            {
                if (isdirector)
                {
                    result1 = this.GetSituationsByear(param0);
                }
                else
                {
                    result1 = this.GetSituationsByear(param0,empid);
                }
            }
            else
            {
                if (isdirector)
                {
                    result1 = this.GetSituationsByQuarterid(param0);
                }
                else
                {
                    result1 = this.GetEmploySituationsByempid(param0,empid);
                }
            }
            return result1;

        }

        /// <summary>
        /// 返回视图model
        /// </summary>
        /// <param name="isdirector"></param>
        /// <param name="year"></param>
        /// <param name="param0"></param>
        /// <param name="empid"></param>
        /// <returns></returns>
        public List<EmploySituationView> GetSituationsViewByQuarterid(bool isdirector, bool year, int param0, int empid)
        {
            return this.objconversiontoview(this.GetSituationsByQuarterid(isdirector,year,param0,empid));
        }

    }
}
