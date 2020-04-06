using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Courseware.Controllers
{
    public class CoursewaremakingController : Controller
    {
        // GET: Courseware/Coursewaremaking
        public ActionResult Index()
        {
         

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
        
        public ActionResult fines(string url)
        {
            try
            {
                HttpPostedFileBase file = Request.Files[0];
                //  var fi=  getDirectory()
                file.SaveAs(Server.MapPath("/Areas/Courseware/" + url + file.FileName));
                return Json(2, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(0, JsonRequestBehavior.AllowGet);
            }
          
           
        }

        private List<string> GetFullFileName(string path)
        {
            List<string> filePath = new List<string>();
            filePath.Add(path);

            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (var item in dir.GetDirectories())
            {
                filePath.AddRange(GetFullFileName(item.FullName));
            }
            foreach (var item in dir.GetFiles())
            {
                filePath.Add(item.FullName);

            }
            return filePath;
        }
    }
}