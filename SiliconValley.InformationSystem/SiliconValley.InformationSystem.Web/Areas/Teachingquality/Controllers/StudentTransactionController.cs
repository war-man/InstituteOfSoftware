using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{
    /// <summary>
    /// 学员异动处理
    /// </summary>
    public class StudentTransactionController : Controller
    {
        private readonly ClassScheduleBusiness dbtext;
        public StudentTransactionController()
        {
            dbtext = new ClassScheduleBusiness();

        }
        //拆班记录
        BaseBusiness<RemovalRecords> Dismantle = new BaseBusiness<RemovalRecords>();
        //学员班级表
        ScheduleForTraineesBusiness Stuclass = new ScheduleForTraineesBusiness();

        // GET: Teachingquality/StudentTransaction
        [HttpPost]
        public ActionResult ClassEnd(int ClassID)
        {
            return Json(dbtext.ClassEnd(ClassID), JsonRequestBehavior.AllowGet);
        }
       
        /// <summary>
        /// 异动申请处理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult transactionprocessing()
        {
            ViewBag.ClassID = Request.QueryString["ClassID"];
            return View();
        }
        /// <summary>
        /// 通过班级获取该班级所有异动数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult TransactionDate(int page, int limit)
        {
            int ClassID = int.Parse(Request.QueryString["ClassID"]);

            return Json(dbtext.TransactionDate(page, limit, ClassID), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 转班详细数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Transactiondetails()
        {
            int ID = int.Parse(Request.QueryString["ID"]);
            return View(dbtext.Transactiondetails(ID));
        }
        /// <summary>
        /// 转班操作
        /// </summary>
        /// <param name="ID">异动id</param>
        /// <param name="Secss">同意或者拒绝</param>
        /// <returns></returns>
        public ActionResult Shiftworkoperation(int ID,string Secss)
        {
            return Json(dbtext.Shiftworkoperation(ID, Secss), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 重修申请表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Rebuild()
        {
            //学号
            string StudentID = Request.QueryString["StudentID"];
            //班级id
            int ClassID = int.Parse(Request.QueryString["ClassID"]);
            var Dismantl = Dismantle.GetList().Where(a => a.IsDelete == false).ToList();
            var List = dbtext.RebuildStage(dbtext.FintClassSchedule(ClassID).grade_Id);
            foreach (var item in Dismantl)
            {
                List = List.Where(a => a.id != item.FormerClass).ToList();
            }
            ViewBag.List = List.Where(a => a.id != ClassID).Select(a => new SelectListItem { Value = a.id.ToString(), Text = a.ClassNumber }).ToList();
            return View(dbtext.ShiftworkFine(StudentID));
        }
        /// <summary>
        /// 重修表单数据提交
        /// </summary>
        /// <param name="transactionView">数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Rebuild(TransactionView transactionView)
        {
            return Json(dbtext.RebuildAdd(transactionView), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 重修详细数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Rebuilddetailed()
        {
            int ID = int.Parse(Request.QueryString["ID"]);
            return View(dbtext.Transactiondetails(ID));
        }
        /// <summary>
        /// 重修数据操作
        /// </summary>
        /// <param name="ID">异动id</param>
        /// <param name="Secss">同意或者拒绝</param>
        /// <returns></returns>
        public ActionResult Rebuildkoperation(int ID, string Secss)
        {
            return Json(dbtext.Rebuildkoperation(ID, Secss), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 休学
        /// </summary>
        /// <returns></returns>
        public ActionResult Suspensionofschool()
        {
            return View();
        }
    }
}