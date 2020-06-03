using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
using SiliconValley.InformationSystem.Business.ExaminationSystemBusiness;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    public class ExamController : BaseController
    {
        // GET: Teaching/Exam
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 考题讲解
        /// </summary>
        /// <returns></returns>
        public ActionResult ExamquestionExplanation()
        {

            GrandBusiness grandBusiness = new GrandBusiness();
            var grandlist = grandBusiness.AllGrand();
            ViewBag.grandlist = grandlist;

            ExaminationBusiness db_exam = new ExaminationBusiness();
            List<ExamType> list = db_exam.allExamType();

            List<ExamTypeView> viewlist = new List<ExamTypeView>();
            //转换类型
            foreach (var item in list)
            {
                var tempobj = db_exam.ConvertToExamTypeView(item);

                if (tempobj != null)
                    viewlist.Add(tempobj);
            }

            ViewBag.examtypelist = viewlist;

            //提供课程数据
            CourseBusiness db_course = new CourseBusiness();

            ViewBag.Courselist = db_course.GetCurriculas();

            var levellist = db_exam.AllQuestionLevel();
            ViewBag.levellist = levellist;
            return View();
        }
    }
}