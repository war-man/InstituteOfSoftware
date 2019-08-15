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

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{
    public class ClassScheduleController : Controller
    {
        private readonly ClassScheduleBusiness dbtext;
        public ClassScheduleController()
        {
            dbtext = new ClassScheduleBusiness();

        }
        //学员班级表
        ScheduleForTraineesBusiness Stuclass = new ScheduleForTraineesBusiness();
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
        //专业
        SpecialtyBusiness Techarcontext = new SpecialtyBusiness();
        //阶段
        GrandBusiness Grandcontext = new GrandBusiness();
        //获取数据
        public ActionResult GetDate(int page ,int limit,string ClassNumber,string Major_Id,string grade_Id,string BaseDataEnum_Id)
        {

          List<ClassSchedule> list = dbtext.GetList().Where(a=>a.ClassStatus==false).ToList();
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

            var listx = list.Select(a => new
            {
                //  a.BaseDataEnum_Id,
                ClassNumber = a.ClassNumber,
                ClassRemarks = a.ClassRemarks,
                ClassStatus = a.ClassStatus,
                IsDelete = a.IsDelete,
                grade_Id = Grandcontext.GetEntity(a.grade_Id).GrandName, //阶段id
                BaseDataEnum_Id = BanseDatea.GetEntity(a.BaseDataEnum_Id).Name,//专业课时间
                Major_Id = Techarcontext.GetEntity(a.Major_Id).SpecialtyName,//专业
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
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

         //开设班级页面
         [HttpGet]
        public ActionResult AddClassSchedule()
        {
          
            //专业课时段
            ViewBag.BaseDataEnum_Id = BanseDatea.GetList().Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() });
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
           var cou= dbtext.GetList().Where(a => a.ClassNumber == classSchedule.ClassNumber).Count();
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
                    dbtext.Insert(classSchedule);
                    retus = new SuccessResult();
                    retus.Success = true;
                    retus.Msg = "开设成功";
                }
                catch (Exception)
                {
                    retus = new ErrorResult();
                    retus.Msg = "服务器错误";

                    retus.Success = false;
                    retus.ErrorCode = 500;

                }
            }
            
            return Json(retus, JsonRequestBehavior.AllowGet);
        }
    }
}