using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.FinaceBusines;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
using SiliconValley.InformationSystem.Business.StudentmanagementBusinsess;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Business;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Web.Areas.Finance.Controllers
{
    public class PricedetailsController : Controller
    {
 
        private readonly StudentFeeStandardBusinsess dbtext;
        public PricedetailsController()
        {
            dbtext = new StudentFeeStandardBusinsess();
        }
        // GET: Teachingquality/Pricedetails
        //专业
        SpecialtyBusiness Techarcontext = new SpecialtyBusiness();
        //阶段
        GrandBusiness Grandcontext = new GrandBusiness();
        //明目类型
        BaseBusiness<CostitemsX> costitemssX = new BaseBusiness<CostitemsX>();

        //学员费用明目
    CostitemsBusiness costitemsBusiness = new CostitemsBusiness();
        //学员费用明目页面
        public ActionResult Studentfees()
        {
          
            return View();
        }
    
        //所有学生费用录入明目
        [HttpGet]
        public ActionResult Costitems()
        {
            //阶段
            ViewBag.Grand_id = Grandcontext.GetList().Select(a => new SelectListItem { Text = a.GrandName, Value = a.Id.ToString() });
            ViewBag.TypeDate = costitemsBusiness.TypeDate();
            return View();
        }
        [HttpPost]
        //学生费用名目录入数据操作
        public ActionResult Costitems(Costitems costitems)
        {
            return Json(costitemsBusiness.AddCostitems(costitems), JsonRequestBehavior.AllowGet);
        }
            //查询名目名称是否重复
        public int costiBoolName(int id)
        {
            string Name = Request.QueryString["Name"];
          return  costitemsBusiness.BoolName(id, Name);
        }
        //查询所有名目数据
        public ActionResult DateCostitems(int page,int limit)
        {
            return Json(costitemsBusiness.DateCostitems(page, limit), JsonRequestBehavior.AllowGet);
        }

        //学费明目类型
        [HttpGet]
        public ActionResult Typeeyesight()
        {
            return View();
        }
        //查看类别名称是否重复
        [HttpPost]
        public int TypeName(string id)
        {
            return costitemsBusiness.TypeName(id);
        }
        //添加明目类型数据操作
        public ActionResult Typeeyesight(CostitemsX costitemsX)
        {
            return Json(costitemsBusiness.AddType(costitemsX), JsonRequestBehavior.AllowGet);
        }
        //验证明目表是否有重复的数据
        public int BoolCostitems(Costitems costitems)
        {
          return  costitemsBusiness.BoolCostitems(costitems);
        }
        //学员缴费操作页面
        public ActionResult Studentpayment()
        {
         
            return View();
        }
        //获取学员信息
        public ActionResult GetDate(int page, int limit, string Name, string Sex, string StudentNumber, string identitydocument)
        {
            return Json(dbtext.GetDate(page, limit, Name, Sex, StudentNumber, identitydocument),JsonRequestBehavior.AllowGet);
        }
       
        //学员学费数据加载
        public ActionResult Singlecostitems(int? Grand_id, int TypeID)
        {
            var data = dbtext.Singlecostitems(Grand_id, TypeID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
      

        //自考本科费用表单录入
        public ActionResult Undergraduatefee(string id)
        {
            //学号
            ViewBag.Stuid = id;
            // ViewBag.Costitemsid
            int Typeid = int.Parse(Request.QueryString["Typeid"]);
            //明目类型id
            ViewBag.Typeid = Typeid;
            //名目名称
            ViewBag.Costitemsid= costitemsBusiness.costitemslist().Where(a => a.Rategory == Typeid).Select(a => new SelectListItem { Text = a.Name, Value = a.id.ToString() }).ToList();
            return View();
        }
        //获取没有阶段的明目费用
        public ActionResult Otherexpenses(string Costitemsid)
        {
            return Json( dbtext.Otherexpenses(Costitemsid),JsonRequestBehavior.AllowGet);
        }
        //自考本科表单
        [HttpGet]
        public ActionResult Tuitionandfees(string id)
        {
           int Typeid = int.Parse(Request.QueryString["Typeid"]);
            int price = 0;
            var list = dbtext.boolTuitionandfees(id, Typeid);
            foreach (var item in list)
            {
            price=price+Convert.ToInt32( item.Amountofmoney);
            }
            ViewBag.price = price;
            //学号
            ViewBag.Stuid = id;
            // ViewBag.Costitemsid
            
            //明目类型id  
            ViewBag.Typeid = Typeid;
            ViewBag.Rategory = dbtext.boolTuitionandfees(id, Typeid);


            return View();
        }

        //接收自考本科表单数据
        [HttpPost]
        public ActionResult Tuitionandfees(StudentFeeRecord studentFeeRecord)
        {
           
            return Json(dbtext.Tuitionandfees(studentFeeRecord),JsonRequestBehavior.AllowGet);
        }
        //学员缴费页面
        [HttpGet]
        public ActionResult StudentPrice(string id)
        {
           
            //阶段
            ViewBag.Stage = Grandcontext.GetList().Where(a => a.IsDelete == false).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.GrandName }).ToList();
            ViewBag.student = JsonConvert.SerializeObject(dbtext.StudentFind(id));
            return View();
        }
        [HttpPost]
        //学员缴费数据操作
        public ActionResult StudentPrices(string person, string Remarks,int Stage)
        {
            SessionHelper.Session["Stage"] = Stage;
            //引入序列化
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //序列化
            var list = serializer.Deserialize<List<StudentFeeRecord>>(person);
            return Json(dbtext.StudentPrices(list, Remarks), JsonRequestBehavior.AllowGet);
        }
            //树形菜单明目类别
            public ActionResult Tree()
        {     
            var list = costitemssX.GetList().Where(a => a.IsDelete == false).ToList();
            List<TreeClass> listtree = new List<TreeClass>();
            foreach (var item in list)
            {
                TreeClass seclass = new TreeClass();
                seclass.title = item.Name;
                seclass.id = item.id.ToString();
                listtree.Add(seclass);
            }
             
            return Json(listtree, JsonRequestBehavior.AllowGet);
        }

        //学员费用缴纳查询阶段费用
        public ActionResult Studentfeepayment(int Grand_id,string studentid)
        {
            return Json(dbtext.Studentfeepayment(Grand_id, studentid), JsonRequestBehavior.AllowGet);
         
        }
       


        //费用收据发票数据
        public ActionResult Receipt()
        {
          
        
                var personlist = SessionHelper.Session["person"] as List<StudentFeeRecord>;
          
            // 引入序列化
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            // string person = Request.QueryString["person"];
            //序列化
            // var  personlist = serializer.Deserialize<List<StudentFeeRecord>>(person);
       
            ViewBag.student = JsonConvert.SerializeObject(dbtext.StudentFind(personlist.FirstOrDefault().StudenID));
            ViewBag.Receiptdata = JsonConvert.SerializeObject(dbtext.Receiptdata(personlist));
            //ViewBag.Remarks = personlist.FirstOrDefault().Remarks;
            return View();
        }
        //查看缴费记录
        public ActionResult Printrecord()
        {
            string student = Request.QueryString["student"];
            ViewBag.vier = dbtext.FienPrice(student);
            return View();
        }
    }
}