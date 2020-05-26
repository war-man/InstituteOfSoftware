using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    [CheckLogin]
    public class ClassroomController : Controller
    {
        private ClassroomManeger Classroom_Entity;
        private BaseDataEnumManeger BaseData_Entity;

        // GET: /Educational/Classroom/ClassroomIndexView
        public ActionResult ClassroomIndexView()
        {
            return View();
        }

        public ActionResult GetClassRoom(int limit,int page)
        {
            Classroom_Entity = new ClassroomManeger();
            BaseData_Entity = new BaseDataEnumManeger();
            List<Classroom> c_list= Classroom_Entity.GetList();
            var mydata = c_list.OrderByDescending(c => c.Id).Skip((page - 1) * limit).Take(limit).Select(c => new {
                Id = c.Id,
                IsDelete = c.IsDelete,
                ClassroomName=c.ClassroomName,
                OfAddren = BaseData_Entity.GetEntity(c.BaseData_Id).Name,
                Count=c.Count,
                Rmark=c.Rmark
            }).ToList();
            var jsondata = new { code=0,msg="",data=mydata,count=c_list.Count};
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddorEditView(int? id)
        {          
            BaseData_Entity = new BaseDataEnumManeger();
            Classroom_Entity = new ClassroomManeger();
            if (id>0)
            {
                //编辑页面
               Classroom find_c= Classroom_Entity.GetEntity(id);
                return View(find_c);
            }
            else
            {
                //获取校区名称
                List<SelectListItem> SchoolsType = BaseData_Entity.GetsameFartherData("校区地址").Select(t=>new SelectListItem() { Text=t.Name,Value=t.Id.ToString()}).ToList();
                ViewBag.schools = SchoolsType;
                return View();
            }
        }
        [HttpPost]
        public ActionResult AddorEditFunction(Classroom c)
        {
            Classroom_Entity = new ClassroomManeger();
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
                   if(Classroom_Entity.FindSameName(c.ClassroomName,Convert.ToInt32( c.BaseData_Id)))
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
            catch (Exception ex)
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
            Classroom_Entity = new ClassroomManeger();
            if (Classroom_Entity.My_Delete(id))
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