using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    public class ClassroomController : Controller
    {
        public static readonly ClassroomManeger Classroom_Entity = new ClassroomManeger();

        // GET: /Educational/Classroom/FindSameName
        public ActionResult ClassroomIndexView()
        {
            return View();
        }

        public ActionResult GetClassRoom(int limit,int page)
        {
           List<Classroom> c_list= Classroom_Entity.GetList();
            var mydata = c_list.OrderByDescending(c => c.Id).Skip((page - 1) * limit).Take(limit);
            var jsondata = new { code=0,msg="",data=mydata,count=c_list.Count};
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddorEditView(int? id)
        {
            if (id>0)
            {
                //编辑页面
               Classroom find_c= Classroom_Entity.GetEntity(id);
                return View(find_c);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult AddorEditFunction(Classroom c)
        {
            try
            {                 
                if (c.Id > 0)
                {
                    //编辑                    
                    Classroom find_c= Classroom_Entity.GetSingData(c.ClassroomName, false);
                    if (find_c.Id==c.Id)
                    {
                        Classroom_Entity.My_update(c);
                    }
                    else
                    {
                        return Json("已有该教室!!!", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                   if(Classroom_Entity.FindSameName(c.ClassroomName))
                    {
                        return Json("已有该教室!!!",JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //添加              
                        c.IsDelete = false;
                        Classroom_Entity.My_add(c);
                    }                                        
                }
                return Json("ok",JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("系统错误，请重试！！！", JsonRequestBehavior.AllowGet);
            }
             

            
        }
        /// <summary>
        /// 禁用或激活
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
           if(Classroom_Entity.My_Delete(id))
            {
                return Json("ok",JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("系统错误，请重试！！！",JsonRequestBehavior.AllowGet);
            }
        }
    }
}