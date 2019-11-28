using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.FinaceBusines;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Business.EnrollmentBusiness;
namespace SiliconValley.InformationSystem.Business.StudentmanagementBusinsess
{

   public class StudentFeeStandardBusinsess : BaseBusiness<StudentFeeStandard>
    {
       
        //专业表
        BaseBusiness<Specialty> special = new BaseBusiness<Specialty>();
        //明目类型
        BaseBusiness<CostitemsX> costitemssX = new BaseBusiness<CostitemsX>();
        //班主任表
        HeadmasterBusiness headmasters = new HeadmasterBusiness();
        //学员班级
        ScheduleForTraineesBusiness scheduleForTraineesBusiness = new ScheduleForTraineesBusiness();
        //阶段专业表
        BaseBusiness<StageGrade> stagegrade = new BaseBusiness<StageGrade>();
        //阶段表
        BaseBusiness<Grand> geand = new BaseBusiness<Grand>();
        //班级表
        ClassScheduleBusiness classschedu = new ClassScheduleBusiness();
        //学费详情
        BaseBusiness<Studenttuitionfeestandard> Feestandard = new BaseBusiness<Studenttuitionfeestandard>();
        //学员信息
        StudentInformationBusiness studentInformationBusiness = new StudentInformationBusiness();
        //费用明目
        CostitemsBusiness costitemsBusiness = new CostitemsBusiness();
        //自考本科
        EnrollmentBusinesse enrollmentBusiness = new EnrollmentBusinesse();
        //升学阶段
        BaseBusiness<GotoschoolStage> GotoschoolStageBusiness = new BaseBusiness<GotoschoolStage>();
        /// <summary>
        /// 获取所有学员数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="Name">姓名</param>
        /// <param name="Sex">性别</param>
        /// <param name="StudentNumber">学号</param>
        /// <param name="identitydocument">身份证号码</param>
        /// <returns></returns>
        public object GetDate(int page, int limit, string Name, string Sex, string StudentNumber, string identitydocument)
        {
            //班主任带班
            BaseBusiness<HeadClass> Hoadclass = new BaseBusiness<HeadClass>();
            //    List<StudentInformation>list=  dbtext.GetPagination(dbtext.GetIQueryable(),page,limit, dbtext)
           // List<StudentInformation> list = studentInformationBusiness.Mylist("StudentInformation").Where(a => a.IsDelete ==false).ToList();
            List<StudentInformation> list = studentInformationBusiness.GetList().Where(a => a.IsDelete == false).ToList();

            if (!string.IsNullOrEmpty(Name))
                {
                    list = list.Where(a => a.Name.Contains(Name)).ToList();
                }
                if (!string.IsNullOrEmpty(Sex))
                {
                    bool sex = Convert.ToBoolean(Sex);
                    list = list.Where(a => a.Sex == sex).ToList();
                }
                if (!string.IsNullOrEmpty(StudentNumber))
                {
                    list = list.Where(a => a.StudentNumber.Contains(StudentNumber)).ToList();
                }
                if (!string.IsNullOrEmpty(identitydocument))
                {
                    list = list.Where(a => a.identitydocument.Contains(identitydocument)).ToList();
                }


           var xz= list.Select(a => new
            {
                a.StudentNumber,
                a.Name,
                a.Sex,
                a.BirthDate,
                a.identitydocument,
                ClassName= scheduleForTraineesBusiness.SutdentCLassName(a.StudentNumber).ClassID,
                Headmasters =headmasters.Listheadmasters(a.StudentNumber)==null?"暂无": headmasters.Listheadmasters(a.StudentNumber).EmpName

           }).ToList();
            var dataList = xz.OrderBy(a => a.StudentNumber).Skip((page - 1) * limit).Take(limit).ToList();
            //  var x = dbtext.GetList();
            var data = new
            {
                code = "",
                msg = "",
                count = xz.Count,
                data = dataList
            };
            return data;

        }
        /// <summary>
        /// 下拉框获取价格
        /// </summary>
        /// <param name="Grand_id">阶段</param>
        /// <param name="Rategory">明目类型</param>
        /// <returns></returns>
        public Costitems Singlecostitems(int? Grand_id, int Rategory)
        {
            var list = costitemsBusiness.costitemslist().Where(a => a.Rategory == Rategory);
            if (Grand_id!=null)
            {
                list = list.Where(a => a.Grand_id == Grand_id);
            }
           return  list.FirstOrDefault();
        }
        //学员费用
        BaseBusiness<StudentFeeRecord> studentfee = new BaseBusiness<StudentFeeRecord>();
        //财务人员
        BaseBusiness<FinanceModel> finacemo = new BaseBusiness<FinanceModel>();
        /// <summary>
        /// 添加学员费用
        /// </summary>
        /// <param name="studentFeeRecord">费用对象数据</param>
        /// <returns></returns>
        public AjaxResult AddstudentFeeRecord(StudentFeeRecord studentFeeRecord)
        {
            //当前登陆人
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            AjaxResult retus = null;
            try
            {
               var fine= finacemo.GetList().Where(a => a.Financialstaff == user.EmpNumber).FirstOrDefault();
                studentFeeRecord.FinanceModelid = fine.id;
                studentFeeRecord.IsDelete = false;
                studentFeeRecord.AddDate = DateTime.Now;

                studentfee.Insert(studentFeeRecord);
                retus = new SuccessResult();
                retus.Success = true;
                retus.Msg = "录入费用成功";
                BusHelper.WriteSysLog("添加费用费用数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {

                retus = new ErrorResult();
                retus.Msg = "服务器错误";
                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return retus;
        }
        /// <summary>
        /// 学杂费添加
        /// </summary>
        /// <param name="studentFeeRecord">数据对象</param>
        /// <param name="Rategorys">多个费用明目id</param>
        /// <returns></returns>
        public AjaxResult Tuitionandfees(StudentFeeRecord studentFee)
        {
            List<StudentFeeRecord> listFeeRecord = new List<StudentFeeRecord>();
           
            //当前登陆人
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            var fine = finacemo.GetList().Where(a => a.Financialstaff == user.EmpNumber).FirstOrDefault();

         
                 AjaxResult retus = null;
            try
            {
                studentFee.FinanceModelid = fine.id;
                studentFee.IsDelete = false;
                studentFee.AddDate = DateTime.Now;
                listFeeRecord.Add(studentFee);
                SessionHelper.Session["person"] = listFeeRecord;
                Enrollment enrollment = new Enrollment();
                enrollment.StudentNumber = studentFee.StudenID;
                enrollment.IsDelete = false;
                enrollmentBusiness.AddEnro(enrollment, studentFee.Costitemsid);
                studentfee.Insert(listFeeRecord);
                retus = new SuccessResult();
                retus.Success = true;
                retus.Msg = "录入费用成功";
                BusHelper.WriteSysLog("添加费用费用数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {

                retus = new ErrorResult();
                retus.Msg = "服务器错误";
                retus.Success = false;
                retus.ErrorCode = 500;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return retus;

        }
        /// <summary>
        /// 没有阶段的费用明目在这获取费用
        /// </summary>
        /// <param name="id">明目id</param>
        /// <returns></returns>
        public int Otherexpenses(string id)
        {
        
            string[] mingmu = id.Split(',');
            //价格
            int price = 0;
            foreach (var item in mingmu)
            {
                int cid = int.Parse(item);
                price= price +Convert.ToInt32( costitemsBusiness.GetEntity(cid).Amountofmoney);
            }
            return price;
          
        }

        /// <summary>
        /// 根据名目类型获取明目
        /// </summary>
        /// <param name="Typeid">类型id</param>
        /// <returns></returns>
        public List<Costitems> ListTuitionandfees(int Typeid)
        {
          return  costitemsBusiness.costitemslist().Where(a => a.Rategory == Typeid).ToList();
        }
        /// <summary>
        /// 根据学号获取学杂费获取当前没有缴费的名目
        /// </summary>
        /// <param name="StudentID">学号</param>
        /// <param name="Typeid"><明目类型/param>
        /// <returns></returns>
        public List<Costitems> boolTuitionandfees(string StudentID,int Typeid)
        {
            var list = this.ListTuitionandfees(Typeid);
            var FeeRecordsList = this.SinglestudentFeeRecords(StudentID);
            foreach (var item in FeeRecordsList)
            {
              list=  list.Where(a => a.id != item.Costitemsid).ToList();
            }
            return list;
           
        }
        /// <summary>
        /// 获取所有缴费记录
        /// </summary>
        /// <returns></returns>
        public List<StudentFeeRecord> ListstudentFeeRecords()
        {
            return studentfee.GetList().Where(a => a.IsDelete == false).ToList();
        }
        /// <summary>
        /// 获取单个学员所有缴费记录
        /// </summary>
        /// <param name="Studentid">学号</param>
        /// <returns></returns>
        public List<StudentFeeRecord> SinglestudentFeeRecords(string Studentid)
        {
           return this.ListstudentFeeRecords().Where(a => a.StudenID == Studentid).ToList();
        }

        /// <summary>
        /// 学员费用查询复杂
        /// </summary>
        /// <param name="Grand_id">阶段</param>
        /// <returns></returns>
        public object Studentfeepayment(int Grand_id,string studentid)
        {
            int countfee = 0;
            var idcost = costitemssX.GetList().Where(a => a.IsDelete == false && a.Name == "学杂费").FirstOrDefault().id;
            var costitemslist = costitemsBusiness.costitemslist().Where(a => a.Rategory==idcost).ToList();
            foreach (var item in costitemslist)
            {
              if(studentfee.GetList().Where(a => a.IsDelete == false && a.StudenID == studentid && a.Costitemsid == item.id).FirstOrDefault() != null)
                {
                    countfee++;
                }
            }
           
         return  costitemsBusiness.GetList().Where(a => a.Grand_id == Grand_id).Select(a => new {a.id, a.Name, a.Amountofmoney, Rategory = costitemssX.GetEntity(a.Rategory).Name,countfee }).ToList();
        }
        /// <summary>
        /// 根据学号获取姓名，性别，班级，身份证，学号
        /// </summary>
        /// <param name="studentid">学号</param>
        /// <returns></returns>
        public object StudentFind(string studentid)
        {
      
          var a=  studentInformationBusiness.GetEntity(studentid);
          var ClassID=  scheduleForTraineesBusiness.GetList().Where(c => c.StudentID == a.StudentNumber && c.CurrentClass == true).First().ID_ClassName;
            var x = new
            {
                StudentNumber = a.StudentNumber,//学号
                Name = a.Name,//姓名
                identitydocument = a.identitydocument,//身份证号码
                Sex = a.Sex,//性别,
            
                GrandName = classschedu.GetClassGrand(ClassID, 22),//阶段
                classa = classschedu.GetList().Where(q => q.IsDelete == false && q.ClassStatus == false && q.id == ClassID).FirstOrDefault().ClassNumber//班级号                                                                                                                                              //a => a.IsDelete == false && a.ClassStatus == false
            };
            return x;
        }
        /// <summary>
        /// 学员阶段费用录入
        /// </summary>
        /// <param name="studentFeeRecords">集合数据</param>
        /// <returns></returns>
        public AjaxResult StudentPrices(List<StudentFeeRecord> studentFeeRecords,  string Remarks)
        {
            AjaxResult retus = null;
            List<StudentFeeRecord> listFeeRecord = new List<StudentFeeRecord>();
            //当前登陆人
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            var fine = finacemo.GetList().Where(a => a.Financialstaff == user.EmpNumber).FirstOrDefault();
            if (fine == null)
            {
                retus = new ErrorResult();
                retus = new ErrorResult();
                retus.Msg = "对不起,当前登陆账号不能进行缴费！";
                retus.Success = false;
                retus.ErrorCode = 500;
            }
            else
            {
                foreach (var item in studentFeeRecords)
                {
                    StudentFeeRecord studentFeeRecord = new StudentFeeRecord();
                    studentFeeRecord.IsDelete = false;
                    studentFeeRecord.AddDate = DateTime.Now;
                    studentFeeRecord.FinanceModelid = fine.id;
                    studentFeeRecord.Costitemsid = item.Costitemsid;
                    studentFeeRecord.Amountofmoney = item.Amountofmoney;
                    studentFeeRecord.StudenID = item.StudenID;
                    studentFeeRecord.Remarks = Remarks;
                    listFeeRecord.Add(studentFeeRecord);
                }

                try
                {
                    SessionHelper.Session["person"] = listFeeRecord;
                    studentfee.Insert(listFeeRecord);
                    retus = new SuccessResult();
                    retus.Success = true;
                    retus.Msg = "录入费用成功";
                    BusHelper.WriteSysLog("录入费用数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
                }
                catch (Exception ex)
                {

                    retus = new ErrorResult();
                    retus.Msg = "服务器错误";
                    retus.Success = false;
                    retus.ErrorCode = 500;
                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                }
            }
            return retus;
        }

        /// <summary>
        /// 收据项目及费用详情
        /// </summary>
        /// <param name="studentFeeRecords">集合数据</param>
        /// <returns></returns>
        public object Receiptdata(List<StudentFeeRecord> studentFeeRecords)
        {
            int Stage = Convert.ToInt32(SessionHelper.Session["Stage"]);
            return studentFeeRecords.Select(a => new
            {
                a.Amountofmoney,
                ostitemsName = costitemsBusiness.GetEntity(a.Costitemsid).Name,
                GrandName = Stage > 0 ? geand.GetEntity(Stage).GrandName : "",
                Rategory = costitemssX.GetEntity( costitemsBusiness.GetEntity(a.Costitemsid).Rategory).Name,
                Remarks=a.Remarks,
                a.AddDate
               
            }).ToList();
        }

        /// <summary>
        /// 根据学号查询出所有缴费记录
        /// </summary>
        /// <param name="student">学号</param>
        /// <returns></returns>
        public List<vierprice> FienPrice(string student)
        {
            List<object> students = new List<object>();
            var st = studentfee.GetList().Where(a => a.IsDelete == false && a.StudenID == student).ToList();
            foreach (var item in st)
            {
                StudentFeeRecord record = new StudentFeeRecord();

                students.Add(Convert.ToDateTime(item.AddDate).ToLongDateString().ToString());
            }
              var x=  students.Distinct().ToList();
            List<vierprice> listvier = new List<vierprice>();
               // string date = Convert.ToDateTime(item.AddDate).ToLongDateString().ToString();
                foreach (var item1 in x)
                {
                vierprice vierprices = new vierprice();
                vierprices.Date = item1.ToString();
                List<vierprice> view = new List<vierprice>();
                foreach (var item in st)
                    {
                   
                    string date = Convert.ToDateTime(item.AddDate).ToLongDateString().ToString();
                        if (item1.ToString()==date)
                        {
                     var costit=   costitemsBusiness.GetEntity(item.Costitemsid);
                        vierprice stivier = new vierprice();
                        stivier.Amountofmoney =(decimal) item.Amountofmoney;
                        stivier.CostitemName = costit.Name;
                        stivier.GrandName = costit.Grand_id!=null? geand.GetEntity(costit.Grand_id).GrandName:"";
                        stivier.Rategory = costitemssX.GetEntity(costit.Rategory).Name;
                        view.Add(stivier);
                        }
                    }
                vierprices.Chicked = view;
                listvier.Add(vierprices);
            }
            return listvier;
            
        }
       

    }
    
}
