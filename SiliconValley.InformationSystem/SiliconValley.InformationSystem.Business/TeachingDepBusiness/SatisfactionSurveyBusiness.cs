using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;



    /// <summary>
    /// 满意度调查业务类
    /// </summary>
  public  class SatisfactionSurveyBusiness:BaseBusiness<SatisficingItem>
    {
        CourseBusiness db_course = new CourseBusiness();

        BaseBusiness<EmployeesInfo> db_emp = new BaseBusiness<EmployeesInfo>();

        BaseBusiness<StudentInformation> db_student = new BaseBusiness<StudentInformation>();

        BaseBusiness<ClassSchedule> db_class = new BaseBusiness<ClassSchedule>();

        BaseBusiness<SatisficingResult> db_satisresult = new BaseBusiness<SatisficingResult>();

        BaseBusiness<SatisficingConfig> db_satisconfig = new BaseBusiness<SatisficingConfig>();

        BaseBusiness<SatisficingResultDetail> db_satisresultdetail = new BaseBusiness<SatisficingResultDetail>();


        private readonly BaseBusiness<SatisficingType> db_saitemtype;
        public SatisfactionSurveyBusiness()
        {

            db_saitemtype = new BaseBusiness<SatisficingType>();

        }

        public List<SatisficingItem> GetAllSatisfactionItems()
        {

            return this.GetList().Where(d=>d.IsDel==false).ToList();

        }

        /// <summary>
        /// 对调查具体项进行筛选
        /// </summary>
        /// <param name="DepID">部门ID</param>
        /// <param name="satisfactionTypeID">调查类型ID 比如:学术能力、教学态度、教学能力</param>
        /// <returns></returns>
        public List<SatisficingItem> Screen(int DepID, int satisfactionTypeID)
        {

            List<SatisficingItem> resultlist = new List<SatisficingItem>();


            if (DepID == 0 && satisfactionTypeID == 0)
            {
                resultlist = this.GetAllSatisfactionItems();
            }
            else if (satisfactionTypeID == 0)
            {
                var list = this.GetAllSatisfactionItems();

                foreach (var item in list)
                {
                    //获取项的类型
                    var objtype = db_saitemtype.GetList().Where(d => d.ID == item.ItemType).FirstOrDefault();

                    if (objtype.DepartmentID == DepID)
                    {
                        resultlist.Add(item);
                    }


                }
            }
            else
            {
                var list = this.GetAllSatisfactionItems();

                foreach (var item in list)
                {
                    //获取项的类型
                    var objtype = db_saitemtype.GetList().Where(d => d.ID == satisfactionTypeID && d.DepartmentID ==DepID).FirstOrDefault();

                    if (item.ItemType == objtype.ID)
                    {
                       
                            resultlist.Add(item);
                       
                    }

                }

            }

           

          

            return resultlist;
            

        }


        /// <summary>
        /// 转为视图模型
        /// </summary>
        /// <param name="satisficingItem">数据实体</param>
        /// <returns></returns>
        public SatisfactionSurveyView ConvertModelView(SatisficingItem satisficingItem)
        {
            SatisfactionSurveyView surveyView = new SatisfactionSurveyView();

            surveyView.IsDel = satisficingItem.IsDel;
            surveyView.ItemContent = satisficingItem.ItemContent;
            surveyView.ItemID = satisficingItem.ItemID;

           var type = db_saitemtype.GetList().Where(d=>d.ID==satisficingItem.ItemType).FirstOrDefault();

            if (type != null)
            {
                surveyView.ItemType = type;
            }
            else
            {
                surveyView.ItemType = null;
            }

            surveyView.Remark = satisficingItem.Remark;
            
            return surveyView;
        }

        /// <summary>
        /// 获取所有调查类型
        /// </summary>
        /// <returns></returns>
        public List<SatisficingType> GetSatisficingTypes()
        {
           return db_saitemtype.GetList();
        }

        /// <summary>
        /// 筛选调查项类型
        /// </summary>
        /// <param name="typename">类型名称</param>
        /// <returns></returns>
        public List<SatisficingType> Screen(string  typename, int depid)
        {

            if (string.IsNullOrEmpty(typename))

            {
                return this.GetSatisficingTypes().Where(d => depid == d.DepartmentID).ToList();

            }
            else
            {
                return this.GetSatisficingTypes().Where(d => d.TypeName.Trim() == typename.Trim() && depid == d.DepartmentID).ToList();
            }

        }

        /// <summary>
        /// 添加调查项类型
        /// </summary>
        /// <param name="typename">类型名称</param>
        public void AddSurveyItemType(string typename, int depid)
        {
            SatisficingType satisficingType = new SatisficingType();

            satisficingType.DepartmentID = depid;
            satisficingType.TypeName = typename.Trim();

            db_saitemtype.Insert(satisficingType);

        }


        /// <summary>
        /// 删除调查项类型
        /// </summary>
        /// <param name="typeid">类型ID</param>
        public void RemoveItemType(int typeid)
        {

            //首先删除这个类型下面的具体项

           var list = this.GetAllSatisfactionItems().Where(d => d.ItemType == typeid).ToList();

       
           this.Delete(list);

           var obj = db_saitemtype.GetList().Where(d => d.ID == typeid).FirstOrDefault();

            db_saitemtype.Delete(obj);

        }



        public SatisfactionSurveyDetailView ConvertToViewModel(SatisficingResult satisficingResult)
        {
            SatisfactionSurveyDetailView detailView = new SatisfactionSurveyDetailView();

            var satisficingConfig = db_satisconfig.GetList().Where(d=>d.IsDel==false && d.ID== satisficingResult.SatisficingConfig).FirstOrDefault();


            detailView.Curriculum = db_course.GetCurriculas().Where(d => d.CurriculumID == satisficingConfig.CurriculumID).FirstOrDefault();

            detailView.Emp = db_emp.GetList().Where(d => d.EmployeeId == satisficingConfig.EmployeeId && d.IsDel == false).FirstOrDefault();

            detailView.investigationClass = db_class.GetList().Where(d => d.ClassNumber == satisficingConfig.ClassNumber).FirstOrDefault();

            detailView.FillInPerson = db_student.GetList().Where(d => d.StudentNumber == satisficingResult.Answerer).FirstOrDefault();

            detailView.investigationDate =(DateTime)satisficingConfig.CreateTime;

            detailView.Proposal = satisficingResult.Suggest;

            detailView.SurveyResultID = satisficingResult.ID;

          var templist =  db_satisresultdetail.GetList().Where(d => d.SatisficingBill == satisficingResult.ID).ToList();

            foreach (var item in templist)
            {

               detailView.TotalScore += (int)item.Scores;

            }

            List<SatisficingResultDetailView> templist1 = new List<SatisficingResultDetailView>();

           var ss = db_satisresultdetail.GetList().Where(d => d.SatisficingBill == satisficingResult.ID).ToList();

            foreach (var item in ss)
            {

               var obj1= this.ConvertToSatisfactionSurveyDetailView(item);

                templist1.Add(obj1);

            }


            detailView.detailitem = templist1;
            return detailView;

        }


        public SatisficingResultDetailView ConvertToSatisfactionSurveyDetailView(SatisficingResultDetail detail)
        {



            SatisficingResultDetailView detailview = new SatisficingResultDetailView();

            detailview.ID = detail.ID;
            detailview.SatisficingBill = detail.SatisficingBill;
            detailview.SatisficingItem = this.GetList().Where(d => d.ItemID == detail.SatisficingItem).FirstOrDefault();
            detailview.Scores = detail.Scores;

            return detailview;



        }
        /// <summary>
        /// 获取满意度调查详细数据
        /// </summary>
        /// <param name="empid"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<SatisfactionSurveyDetailView> SurveyHistoryData(string empid, string date , int? Curriculum, string classnumber)
        {


            List<SatisfactionSurveyDetailView> resultlist = new List<SatisfactionSurveyDetailView>();

           //判断部门

           var emp = db_emp.GetList().Where(d => d.EmployeeId == empid).FirstOrDefault();

            EmployeesInfoManage empmanage = new EmployeesInfoManage();

            var dep = empmanage.GetDept(emp.PositionId);

            if (dep.DeptId == 1)
            {
                //教质部
                resultlist = SurveyHistoryData(empid, classnumber, date);


            }

            if (dep.DeptId == 2)
            {
                //教学部
                resultlist = SurveyHistoryData(empid, (int)Curriculum, classnumber);

            }
           


            return resultlist;

        }


        /// <summary>
        /// 获取满意度调查详细数据 --教员
        /// </summary>
        /// <returns></returns>
        public List<SatisfactionSurveyDetailView> SurveyHistoryData(string empid , int Curriculum, string classnumber)
        {
            List<SatisfactionSurveyDetailView> resultlist = new List<SatisfactionSurveyDetailView>();
            List<SatisficingConfig> templist = new List<SatisficingConfig>();

            if (string.IsNullOrEmpty(classnumber) ||Curriculum == 0)
            {
                templist = db_satisconfig.GetList().Where(d => d.EmployeeId == empid ).ToList();
            }
            else
            {
                templist = db_satisconfig.GetList().Where(d => d.EmployeeId == empid && d.CurriculumID == Curriculum && d.ClassNumber == classnumber).ToList();
            }

            foreach (var item in templist)
            {

                var temp1list = db_satisresult.GetList().Where(d => d.SatisficingConfig == item.ID).ToList();

                foreach (var item1 in temp1list)
                {

                    var obj = this.ConvertToViewModel(item1);

                    if (obj != null)
                    {

                        resultlist.Add(obj);
                    }

                }

            }

            return resultlist;

        }

        public List<SatisfactionSurveyDetailView> SurveyHistoryData(string empid, string classnumber,string date)
        {
            List<SatisfactionSurveyDetailView> resultlist = new List<SatisfactionSurveyDetailView>();
            List<SatisficingConfig> templist = new List<SatisficingConfig>();

            if (classnumber == null)
            {
                templist = db_satisconfig.GetList().Where(d => d.EmployeeId == empid && d.CreateTime >=DateTime.Parse(date) && d.CreateTime< DateTime.Parse(date).AddMonths(1)).ToList();
            }
            else
            {
                templist = db_satisconfig.GetList().Where(d => d.EmployeeId == empid && d.CreateTime >= DateTime.Parse(date) && d.CreateTime < DateTime.Parse(date).AddMonths(1) && d.ClassNumber == classnumber).ToList();
            }

            foreach (var item in templist)
            {

                var temp1list = db_satisresult.GetList().Where(d => d.SatisficingConfig == item.ID).ToList();

                foreach (var item1 in temp1list)
                {

                    var obj = this.ConvertToViewModel(item1);

                    if (obj != null)
                    {

                        resultlist.Add(obj);
                    }

                }

            }

            return resultlist;
        }


        /// <summary>
        /// 返回满意度调查结果
        /// </summary>
        /// <param name="id">iD</param>
        /// <returns></returns>
        public SatisficingResult GetSatisficingResultByID(int id)
        {


          return  db_satisresult.GetList().Where(d => d.IsDel == false && d.ID == id).FirstOrDefault();


        }

        public List<SatisficingResult> SatisficingResults()
        {

            return db_satisresult.GetList();
        }

        public SatisfactionSurveyDetailView GetSatisficingBy(SatisficingResult satisficingResult)
        {

            return this.ConvertToViewModel(satisficingResult);

        }

        public List<SatisficingConfig> satisficingConfigs()
        {
            return db_satisconfig.GetList().Where(d => d.IsDel == false).ToList();
        }
    }
}
