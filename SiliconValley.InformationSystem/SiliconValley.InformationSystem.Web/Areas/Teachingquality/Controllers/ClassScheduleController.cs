using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.StageGrade_Business;
using SiliconValley.InformationSystem.Business.BaseDataEnum_Business;

using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{
    public class ClassScheduleController : Controller
    {
        private readonly ClassScheduleBusiness dbtext;
        public ClassScheduleController()
        {
            dbtext = new ClassScheduleBusiness();

        }
        // GET: Teachingquality/ClassSchedule
        //主页面
        public ActionResult Index()
        {
            return View();
        }
        //获取数据
        public ActionResult GetDate(int page ,int limit)
        {
       //     //阶段数据
       //     StageGradeBusiness StageContext = new StageGradeBusiness();
       //     //基础数据枚举数据
       //     BaseDataEnumBusiness BanseDatea = new BaseDataEnumBusiness();
       //     //阶段
       //     GrandBusiness Grandcontext = new GrandBusiness();

          List<ClassSchedule> list = dbtext.GetList();
       //var listx=  list.Select(a => new {
       //       //  a.BaseDataEnum_Id,
       //         ClassNumber=  a.ClassNumber,
       //         ClassRemarks=  a.ClassRemarks,
       //         ClassStatus=  a.ClassStatus,
       //         IsDelete=  a.IsDelete,

       //    grade_Id = Grandcontext.GetEntity(a.grade_Id).GrandName, //阶段id
       //         BaseDataEnum_Id = BanseDatea.GetEntity(a.BaseDataEnum_Id).Name//专业课时间
       //         Major_Id//专业
       //}).ToList();



            var dataList = list.OrderBy(a => a.ClassNumber).Skip((page - 1) * limit).Take(limit).ToList();
            //  var x = dbtext.GetList();
            var data = new
            {
                code = "",
                msg = "",
                count = list.Count,
                data = list
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

         //开设班级页面
        public ActionResult AddClassSchedule()
        {
            return View();
        }
    }
}