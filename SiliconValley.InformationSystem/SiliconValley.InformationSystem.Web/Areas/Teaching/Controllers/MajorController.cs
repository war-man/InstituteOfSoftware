using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Util;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using System.IO;
    public class MajorController : Controller
    {

        public readonly SpecialtyBusiness db_major;

        public MajorController()
        {
            db_major = new SpecialtyBusiness();

        }

        // GET: Teaching/Major
        public ActionResult MajorIndex()
        {

           ViewBag.Majors = db_major.GetSpecialties();

            return View();
        }


      /// <summary>
      /// 添加专业
      /// </summary>
      /// <param name="majorName">专业名称</param>
      /// <returns>ajaxResult</returns>
        [HttpPost]
        public ActionResult AddMajor(string majorName)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                Specialty specialty = db_major.AddMajor(majorName);

                result.Data = specialty;
                result.ErrorCode = 200;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = ex.Message;

            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 获取和名称相似的专业
        /// </summary>
        /// <param name="majorName">专业名称</param>
        /// <returns></returns>
        public ActionResult ContainsMajorName(string majorName)
        {
            AjaxResult result = new AjaxResult();

            var list = new List<Specialty>();


            try
            {
                list = db_major.ContainsMajorName(majorName);
                result.Data = list;
                result.ErrorCode = 200;
                result.Msg = "成功";

            }
            catch (Exception ex)
            {
                result.Data = list;
                result.ErrorCode = 500;
                result.Msg = ex.Message;

            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }



        /// <summary>
        /// 图片上传
        /// </summary>
        /// <returns></returns>
        public ActionResult upload(HttpPostedFileBase postedFileBase,string name)
        {

            string fielpath = postedFileBase.FileName;

            //获取文件扩展名称

            FileInfo fileInfo = new FileInfo(fielpath);
            string fileExtension = fileInfo.Extension;

            string filename = name + fileExtension;

            postedFileBase.SaveAs(Server.MapPath("/Teaching/Images/MajorLogoImages/" + filename));

            return null;
        }

        [HttpGet]
        public ActionResult operationView(int ? id)
        {
            if (id == null)
            {
                return View();

            }
            else
            {
                var major = db_major.GetSpecialties().Where(d => d.Id == id).FirstOrDefault();
                return View(major);
            }

        }
    }
}