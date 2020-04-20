using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.Coursewaremaking_Business;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Courseware.Controllers
{
    [CheckLogin]
    public class CoursewaremakingController : Controller
    {
        private readonly CoursewaremakingBusiness dbtext;
        public CoursewaremakingController()
        {
            dbtext = new CoursewaremakingBusiness();

        }
        // GET: Courseware/Coursewaremaking
        public ActionResult Index()
        {
            //阶段
            ViewBag.StageID = Grandcontext.GetList().Select(a => new SelectListItem { Text = a.GrandName, Value = a.Id.ToString() });
            //专业
            ViewBag.MajorID = Techarcontext.GetList().Select(a => new SelectListItem { Text = a.SpecialtyName, Value = a.Id.ToString() });
            //课件类型
            ViewBag.MakingType = dbtext.MakingType().Select(a => new SelectListItem { Text = a, Value = a });
            //部门
           ViewBag.DeptId= Depa.GetList().Where(a=>a.DeptName.Contains("教学")||a.DeptName.Contains("教质")).ToList().Select(a => new SelectListItem { Text = a.DeptName, Value = a.DeptId.ToString() });
            return View();

        }
        /// <summary>
        /// 获取文件夹内容
        /// </summary>
        /// <returns></returns>
        public ActionResult CourDate()
        {
            var strs = Request.QueryString["str"];
            List<string> db_Name = new List<string>();
            var strPath = Server.MapPath("/Areas/Courseware" + strs);
            List<string> str = getDirectory(strPath);

            foreach (var item in str)
            {
                db_Name.Add(Path.GetFileName(item));
            }
           
            return Json(db_Name, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取子文件地址
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns></returns>
        public static List<string> getDirectory(string path)
        {
            List<String> list = new List<string>();
            DirectoryInfo root = new DirectoryInfo(path);
            try
            {
                DirectoryInfo[] di = root.GetDirectories();

                for (int i = 0; i < di.Length; i++)
                {
                    //   Console.WriteLine(di[i].FullName);
                    list.Add(di[i].FullName);
                }
            }
            catch (Exception)
            {

  
            }
            finally {

              
                    for (int i = 0; i < root.GetFiles().Length; i++)
                    {
                        //   Console.WriteLine(di[i].FullName);
                        list.Add(root.GetFiles()[i].FullName);
                    }
            
               
            }
              
            return list;
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="url">文件地址</param>
        /// <param name="fineName">名称</param>
        /// <returns></returns>
        public int fines(string url,string fineName)
        {
            try
            {
                HttpPostedFileBase file = Request.Files[0];
                //  var fi=  getDirectory()
                file.SaveAs(Server.MapPath("/Areas/Courseware/" + url+ fineName + file.FileName));
                return 1;
            }
            catch (Exception ex)
            {

                return 0;
            }
          
           
        }

        //private List<string> GetFullFileName(string path)
        //{
        //    List<string> filePath = new List<string>();
        //    filePath.Add(path);

        //    DirectoryInfo dir = new DirectoryInfo(path);
        //    foreach (var item in dir.GetDirectories())
        //    {
        //        filePath.AddRange(GetFullFileName(item.FullName));
        //    }
        //    foreach (var item in dir.GetFiles())
        //    {
        //        filePath.Add(item.FullName);

        //    }
        //    return filePath;
        //}

        /// <summary>
        /// 上传课件页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddCoursewaremaking()
        {
            //阶段
            ViewBag.StageID = Grandcontext.GetList().Select(a => new SelectListItem { Text = a.GrandName, Value = a.Id.ToString() });
            //专业
            ViewBag.MajorID = Techarcontext.GetList().Select(a => new SelectListItem { Text = a.SpecialtyName, Value = a.Id.ToString() });
            //课件类型
            ViewBag.MakingType = dbtext.MakingType().Select(a => new SelectListItem { Text = a, Value = a });
            return View();
        }
        //人事,部门表
        BaseBusiness<Department> Depa = new BaseBusiness<Department>();
        //专业
        SpecialtyBusiness Techarcontext = new SpecialtyBusiness();
        //阶段
        GrandBusiness Grandcontext = new GrandBusiness();
        /// <summary>
        /// 获取研发人页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ProducerID()
        {
            //部门
            ViewBag.department = Depa.GetList().Where(a => a.DeptName.Contains("教学")|| a.DeptName.Contains("教质")).ToList().Select(a => new SelectListItem { Text = a.DeptName, Value = a.DeptId.ToString() }); ;
           
            return View();
        }
        /// <summary>
        /// 获取所有可研发的人
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="EmployeeId"></param>
        /// <param name="EmpName"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        public ActionResult TraineeDate(int page, int limit, string EmployeeId, string EmpName, string department)
        {
            return Json(dbtext.TraineeDate(page, limit, EmployeeId, EmpName, department), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 课件信息保存到1数据库
        /// </summary>
        /// <param name="coursewaremaking">数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCoursewaremaking(Coursewaremaking coursewaremaking)
        {
            bool bot = false;
           var cou= fines(coursewaremaking.Filepath, coursewaremaking.RampDpersonID.Substring(coursewaremaking.RampDpersonID.Length - 4));
            if (cou>0)
            {
                coursewaremaking.Filename = coursewaremaking.RampDpersonID.Substring(coursewaremaking.RampDpersonID.Length - 4) + coursewaremaking.Filename;
                bot = true;
                bot = dbtext.AddCoursewaremaking(coursewaremaking);
            }
            return Json(bot, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询文件基本信息
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public ActionResult FineCoursewaremaking(string FileName)
        {
            return Json(dbtext.FineCoursewaremaking(FileName), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 下载单个文件
        /// </summary>
        /// <returns></returns>
        public ActionResult Filedownload()
        {
            string URL = Request.QueryString["URL"];
            string filename = Request.QueryString["filename"];
            string[] finmae = filename.Split('.');
            string pathName = "/Areas/Courseware/" + URL+filename;

            //开始下载
            FileStream stream = new FileStream(Server.MapPath(pathName), FileMode.Open, FileAccess.Read);

            return File(stream, finmae[1], filename);
        }
        /// <summary>
        /// 获取课件上传数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="EmpName">名字</param>
        /// <param name="DeptId">部门</param>
        /// <param name="MajorID">专业</param>
        /// <param name="StageID">阶段</param>
        /// <param name="MakingType">类型</param>
        /// <param name="Filename">文件名</param>
        /// <returns></returns>
        public ActionResult CourDates(int page, int limit, string EmpName, string DeptId, string MajorID, string StageID, string MakingType, string Filename)
        {
            return Json(dbtext.CourDate(page, limit, EmpName, DeptId, MajorID, StageID, MakingType, Filename), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Filedeletion(int id)
        {
            bool bit = false;
            try
            {
                string yourPath = Request.QueryString["yourPath"];
                FileInfo FileInfos = new FileInfo(Server.MapPath("/Areas/Courseware/" + yourPath));
                FileInfos.Delete();
                dbtext.RemoveCoursewaremaking(id);
                bit = true;
            }
            catch (Exception ex)
            {

                bit = false;
            }
            return Json(bit, JsonRequestBehavior.AllowGet);
            
            
        }
    }
}