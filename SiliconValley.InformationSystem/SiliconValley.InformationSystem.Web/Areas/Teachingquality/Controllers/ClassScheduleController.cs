using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.StageGrade_Business;
using SiliconValley.InformationSystem.Business.BaseDataEnum_Business;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.StudentmanagementBusinsess;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using System.Web.Script.Serialization;
using SiliconValley.InformationSystem.Business.EnroExamination_Business;
using SiliconValley.InformationSystem.Depository.CellPhoneSMS;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Business.Shortmessage_Business;
using SiliconValley.InformationSystem.Entity.Entity;
//班级管理
namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{
    
    public class ClassScheduleController : Controller
    {
        private static int? classNumberss = null;
        private readonly ClassScheduleBusiness dbtext;
        public ClassScheduleController()
        {
            dbtext = new ClassScheduleBusiness();

        }
        //拆班记录
        BaseBusiness<RemovalRecords> Dismantle = new BaseBusiness<RemovalRecords>();
        //学员信息
        StudentInformationBusiness student = new StudentInformationBusiness();

        //学员班级表
        ScheduleForTraineesBusiness Stuclass = new ScheduleForTraineesBusiness();
        //班主任带班
        BaseBusiness<HeadClass> HeadClassEnti = new BaseBusiness<HeadClass>();
        //班主任
        HeadmasterBusiness Hadmst = new HeadmasterBusiness();
        //成考管理
        EnroExaminationBusiness enroExaminationBusiness = new EnroExaminationBusiness();
        //短信模板
        ShortmessageBusiness shortmessageBusiness = new ShortmessageBusiness();
        // GET: Teachingquality/ClassSchedule
        //主页面
        public ActionResult Index()
        {
            //专业课时段
            ViewBag.BaseDataEnum_Id = BanseDatea.GetList().Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() });
            //专业
            ViewBag.Major_Id = Techarcontext.GetList().Select(a => new SelectListItem { Text = a.SpecialtyName, Value = a.Id.ToString() });
            //阶段
            ViewBag.grade_Id = Grandcontext.GetList().Select(a => new SelectListItem { Text = a.GrandName, Value = a.Id.ToString() });
            return View();
        }
        //基础数据枚举数据
        BaseDataEnumBusiness BanseDatea = new BaseDataEnumBusiness();
        //当前登陆人
        Base_UserModel user = Base_UserBusiness.GetCurrentUser();
        //专业
        SpecialtyBusiness Techarcontext = new SpecialtyBusiness();
        //阶段
        GrandBusiness Grandcontext = new GrandBusiness();
        //获取数据
        public ActionResult GetDate(int page ,int limit,string ClassNumber,string Major_Id,string grade_Id,string BaseDataEnum_Id)
        {
            try
            {
                List<ClassSchedule> list = new List<ClassSchedule>();
                if (user.UserName=="Admin")
                {
                    list = dbtext.GetList().Where(a => a.ClassStatus == false && a.IsDelete == false).ToList();
                }
                else
                {
                    var HadnID = Hadmst.GetList().Where(c => c.informatiees_Id == user.EmpNumber && c.IsDelete == false).FirstOrDefault();
                    if (HadnID != null)
                    {
                        var x = HeadClassEnti.GetList().Where(a => a.IsDelete == false && a.LeaderID == HadnID.ID).ToList();
                        foreach (var item in x)
                        {
                            list.Add(dbtext.GetList().Where(a => a.ClassStatus == false && a.IsDelete == false && a.id == item.ClassID).FirstOrDefault());
                        }
                    }


                }
            if (!string.IsNullOrEmpty(ClassNumber))
            {
                list = list.Where(a => a.ClassNumber.Contains(ClassNumber)).ToList();
            }
            if (!string.IsNullOrEmpty(Major_Id))
            {
                int maid = int.Parse(Major_Id);
                list = list.Where(a => a.Major_Id== maid).ToList();
            }
            if (!string.IsNullOrEmpty(grade_Id))
            {
                int maid = int.Parse(grade_Id);
                list = list.Where(a => a.grade_Id == maid).ToList();
            }
            if (!string.IsNullOrEmpty(BaseDataEnum_Id))
            {
                int maid = int.Parse(BaseDataEnum_Id);
                list = list.Where(a => a.BaseDataEnum_Id == maid).ToList();
            }
            //班级状态表
                BaseBusiness<Classstatus> classtatus = new BaseBusiness<Classstatus>();
              var listx = list.Select(a => new
            {
                //  a.BaseDataEnum_Id,
                a.id,
                ClassNumber = a.ClassNumber,
                ClassRemarks = a.ClassRemarks,
                ClassStatus = a.ClassStatus,
                IsDelete = a.IsDelete,
                grade_Id = Grandcontext.GetEntity(a.grade_Id).GrandName, //阶段id
                BaseDataEnum_Id = BanseDatea.GetEntity(a.BaseDataEnum_Id).Name,//专业课时间
                Major_Id = a.Major_Id==null?"暂无专业": Techarcontext.GetEntity(a.Major_Id).SpecialtyName,//专业
                HeadmasterName= Hadmst.ClassHeadmaster(a.id)==null?"未设置班主任": Hadmst.ClassHeadmaster(a.id).EmpName,
                IsBool = classtatus.GetList().Where(c=>c.IsDelete==false&&c.id==a.ClassstatusID).FirstOrDefault()==null?"正常":classtatus.GetList().Where(c => c.IsDelete == false && c.id == a.ClassstatusID).FirstOrDefault().TypeName,
                 stuclasss = Stuclass.GetList().Where(c=>c.ClassID==a.ClassNumber&&c.CurrentClass==true).Count()//专业
            }).ToList();
            var dataList = listx.OrderBy(a => a.ClassNumber).Skip((page - 1) * limit).Take(limit).ToList();
            //  var x = dbtext.GetList();
            var data = new
            {
                code = "",
                msg = "",
                count = dataList.Count,
                data = dataList
            }; return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.加载数据);
                return Json("数据有误", JsonRequestBehavior.AllowGet);
            }
        }

         //开设班级页面
         [HttpGet]
        public ActionResult AddClassSchedule()
        {
            //公共类库
            BaseDataEnumManeger baseDataEnumManeger = new BaseDataEnumManeger();
            //GetsameFartherData
            //专业课时段
            ViewBag.BaseDataEnum_Id = baseDataEnumManeger.GetsameFartherData("上课时间类型").Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() });
            //专业
            ViewBag.Major_Id = Techarcontext.GetList().Select(a => new SelectListItem { Text = a.SpecialtyName, Value = a.Id.ToString() });
                //阶段
          ViewBag.grade_Id = Grandcontext.GetList().Select(a => new SelectListItem { Text = a.GrandName, Value = a.Id.ToString() });
            return View();
        }
        //添加班级数据
        [HttpPost]
        public ActionResult AddClassSchedule(ClassSchedule classSchedule)
        {
          AjaxResult  retus = null;
           var cou= dbtext.GetList().Where(a => a.ClassNumber == classSchedule.ClassNumber && a.IsDelete == false &&a.ClassStatus==false).Count();
            if (cou>0)
            {
                retus = new ErrorResult();
                retus.Msg = "班级名称重复";

                retus.Success = false;
                retus.ErrorCode = 300;
            }
            else
            {
                try
                {
                    classSchedule.ClassStatus = false;
                    classSchedule.IsDelete = false;
                    dbtext.Insert(classSchedule);
                    retus = new SuccessResult();
                    retus.Success = true;
                    retus.Msg = "开设成功";
                    BusHelper.WriteSysLog("班级开设成功", Entity.Base_SysManage.EnumType.LogType.添加数据);
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
            
            return Json(retus, JsonRequestBehavior.AllowGet);
        }
        //查看班级的学员
        public ActionResult ClassStudent()
        {

            classNumberss =int.Parse( Request.QueryString["ClassNumber"]);
            var x = dbtext.ClassStudentneViewList((int)classNumberss);
            ViewBag.ClassName =dbtext.GetEntity( classNumberss).ClassNumber;
            ViewBag.ClassdetailsView = dbtext.Listdatails((int)classNumberss);
            ViewBag.ClassID = classNumberss;
            ViewBag.Members = dbtext.MembersList();
            ViewBag.Stage = dbtext.GetClassGrand((int)classNumberss, 234);
            return View(x);
        }

        //群号操作页面
        public ActionResult Groupnumber()
        {
            int ClassNumber =int.Parse( Request.QueryString["ClassNumber"]);
            ViewBag.ClassNumber = ClassNumber;
            ViewBag.ClassName = dbtext.GetEntity(ClassNumber).ClassNumber;
            return View(dbtext.Grouselect(ClassNumber));
        }
        //群号数据操作
        public ActionResult GroupEntity(GroupManagement groupManagement)
        {
            return Json(dbtext.GroupAdd(groupManagement), JsonRequestBehavior.AllowGet);
        }
        public ActionResult text()
        {
            return View();
        }
        //获取班委
        public ActionResult DateMembers()
        {
            return Json(dbtext.ClassStudentneList((int)classNumberss), JsonRequestBehavior.AllowGet);
        }
        //班委数据操作
        public ActionResult AddMembers()
        {
            //学员学号
            string Stuid = Request.QueryString["Stuid"];
            //班委名称
            string MenName = Request.QueryString["MenName"];
            //数据操作
            string Entity = Request.QueryString["Entity"];


         return Json(dbtext.Entityembers(Stuid, MenName, (int)classNumberss, Entity),JsonRequestBehavior.AllowGet);



        }
        //班会表单页面
        [HttpGet]
        public ActionResult AssmeetingsEntity()
        {
            //undefined
            //string ClassName = Request.QueryString["ClassName"];

            string uid = Request.QueryString["id"];
            Assmeetings assmeetings = new Assmeetings();
            if (uid!= "undefined")
            {
                ViewBag.Name = "添加班会记录";
                assmeetings =  dbtext.AssmeetingsSelect(int.Parse(uid));
                ViewBag.ClassName = dbtext.GetEntity(assmeetings.ClassNumber).ClassNumber;
                return View(assmeetings);
            }
            else
            {
                ViewBag.Name = "编辑班会记录";
                assmeetings.ClassNumber =(int) classNumberss;
                ViewBag.ClassName = dbtext.GetEntity(assmeetings.ClassNumber).ClassNumber;
                return View(assmeetings);
            }
         
        }
        //班会数据操作方法
        [HttpPost]
        public ActionResult AssmeetingsEntity(Assmeetings assmeetings)
        {
            return Json(dbtext.EntityAssmeetings(assmeetings), JsonRequestBehavior.AllowGet);
        }

        //查看是否有相同的班委
        public ActionResult AssmeetingsBool()
        {
            //班委名称
            string MenName = Request.QueryString["MenName"];
            //学号
            string Stuid = Request.QueryString["Stuid"];
            return Json(dbtext.AssmeetingsBool(MenName,(int)classNumberss, Stuid), JsonRequestBehavior.AllowGet);
        }
        //班级班会数据
        public ActionResult AssmeetingsGetDate(int page, int limit,string Title,string qBeginTime,string qEndTime)
        {
            var dataList = dbtext.AssmeetingsList((int)classNumberss) ;
            if (!string.IsNullOrEmpty(Title))
            {
                dataList = dataList.Where(a => a.Title.Contains(Title)).ToList();
            }
       
            if (!string.IsNullOrEmpty(qBeginTime))
            {
                dataList = dataList.Where(a => a.Classmeetingdate >= Convert.ToDateTime(qBeginTime)).ToList();
            }
            if (!string.IsNullOrEmpty(qEndTime))
            {
                dataList = dataList.Where(a => a.Classmeetingdate <= Convert.ToDateTime(qEndTime)).ToList();
            }
            dataList= dataList.OrderBy(a => a.ID).Skip((page - 1) * limit).Take(limit).ToList();
            var data = new
            {
                code = "",
                msg = "",
                count = dataList.Count,
                data = dataList
            }; return Json(data, JsonRequestBehavior.AllowGet);
        
          
       
        }

        //转班业务
        public ActionResult Transferoftraunees()
        {
            StudentUnionBusiness SS = new StudentUnionBusiness();
            var x= SS.StudentList("aa");
            return View();
        }
        //拆班页面
        [HttpGet]
        public ActionResult Dismantleclasses()
        {
            string studentID = Request.QueryString["StudentID"];
          
          var Dismantl=  Dismantle.GetList().Where(a => a.IsDelete == false).ToList();
            var List = dbtext.GetList().Where(a => a.IsDelete == false && a.ClassStatus == false).ToList();
            foreach (var item in Dismantl)
            {
                List = List.Where(a => a.ClassNumber != item.FormerClass).ToList();
            }
            ViewBag.List= List.Where(a=>a.id!= classNumberss).Select(a => new SelectListItem { Value = a.ClassNumber, Text = a.ClassNumber }).ToList();
            studentID = studentID.Substring(0, studentID.Length - 1);
            string[] stu = studentID.Split(',');
            List<StudentInformation> list = new List<StudentInformation>();
            foreach (var item in stu)
            {
                list.Add(student.GetEntity(item));
            }

            ViewBag.StudentID = studentID;
            ViewBag.ClassName = classNumberss;
            ViewBag.Mylist = list;


            return View();
        }
        //拆班数据操作
        [HttpPost]
        public ActionResult Dismantleclasses(string Addtime,string FormerClass,string List,string Reasong,string Remarks,string StudentID)
        {
           return Json(dbtext. Dismantleclasses(Addtime, FormerClass, List, Reasong, Remarks, StudentID),JsonRequestBehavior.AllowGet);

        }
 
        /// <summary>
        /// 验证成考学员是否重复
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public ActionResult BoollistEnroExamination(string student)
        {
            //引入序列化
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //序列化
            var list = serializer.Deserialize<List<StudentInformation>>(student);

            return Json(enroExaminationBusiness.BoollistEnroExamination(list), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 验证班级名称是否重复
        /// </summary>
        /// <param name="id">班级名称</param>
        /// <returns></returns>
        public ActionResult ClassNameCount(string id)
        {
            return Json(dbtext.ClassNameCount(id), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 升学缴费情况
        /// </summary>
        /// <returns></returns>
        public ActionResult FindTuition(string id)
        {
            ViewBag.ClassName = id;
            return View();
        }
        /// <summary>
        /// 根据班级查询班上学生升学缴费情况
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="id">班级号</param>
        /// <returns></returns>
        public ActionResult listTuiton(int page, int limit,int id)
        {
           var x= dbtext.listTuiton(page, limit, id);
            var data = new
            {
                code = "",
                msg = "",
              
                data = x
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SMScharging()
        {
            var x = shortmessageBusiness.FineShortmessage("学费催费");
            ViewBag.horetem = x == null ? "" : x.content;
            return View();
        }
        /// <summary>
        /// 短信模板
        /// </summary>
        /// <param name="Datailedcost">内容</param>
        /// <returns></returns>
       // [HttpPost]
       //[ValidateInput(false)]
       // public ActionResult SMScharging(string Datailedcost)
       // {
       //     // 引入序列化
       //     JavaScriptSerializer serializer = new JavaScriptSerializer();
       //  //   string str = Datailedcost.Substring(3, Datailedcost.Length - 7);
       //     //Replace("Name", "替换").Replace("NextStageID","S2");
       //     // 序列化
       //     // var  personlist = serializer.Deserialize<List<DetailedcostView>>(Datailedcost);
       //     //return dbtext.SMScharging(personlist);
       //     return Json(dbtext.EntiShortmessage(Datailedcost), JsonRequestBehavior.AllowGet);

       // }
        /// <summary>
        /// 短信催费
        /// </summary>
        /// <param name="Datailedcost">字符串集合数据</param>
        /// <returns></returns>
        public ActionResult SendoutSMScharging(string Datailedcost)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var personlist = serializer.Deserialize<List<DetailedcostView>>(Datailedcost);
            return Json(dbtext.SMScharging(personlist), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EntitAddClassSchedule()
        {
            string classname = Request.QueryString["ClassName"];
            return Json(dbtext.EntitAddClassSchedule(classname), JsonRequestBehavior.AllowGet);
        }
        public ActionResult PhoneSMS()
        {
            string str = "<p>湖南硅谷高科软件学员逾期缴费学员通知：</p><p>{Name}家长：您好！经财务核查，您孩子{NextStageID}阶段升学费用逾期未缴，应交{ShouldJiao}元，欠费{Surplus}元。请您于本周内经财务办理缴费手续，逾期不缴教务处将根据学员管理规定予以听课处理。感谢您的配合与理解！</p><p>班主任：{HeadmasterName}&nbsp; 电话：{Phone}</p><p><br/></p>";
             dbtext.PhoneSMS("15073315702",str);
            return null;
        }
    }
}