using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Business.EducationalBusiness;

    public static class MyEntity{
        public static readonly BreakManeger Break_Entity = new BreakManeger();
     }
    public class BreakController : Controller
    {
        // GET: /Educational/Break/BreakIndexView
        public ActionResult BreakIndexView()
        {
            ViewBag.classshche = MyEntity.Break_Entity.GetClassSchedules();
            return View();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult GetBrakData(int limit,int page)
        {
            try
            {
                string Name = Request.QueryString["classname"];
                List<Break> break_list = MyEntity.Break_Entity.GetList().Where(b=>b.IsDelete==false).ToList();
                if (!string.IsNullOrEmpty(Name) && Name!="选择班级" && Name!="无")
                {                    
                    break_list = break_list.Where(b => b.ClassSchedule_Id == Name).ToList();
                }
                string starDate = Request.QueryString["starDate"];
                if (!string.IsNullOrEmpty(starDate))
                {

                    break_list = break_list.Where(b => b.RecodeDate >= Convert.ToDateTime(starDate)).ToList();
                }

                string endDate = Request.QueryString["endDate"];
                if (!string.IsNullOrEmpty(endDate))
                {
                    break_list = break_list.Where(b => b.RecodeDate <= Convert.ToDateTime(endDate)).ToList();
                }
                var mydata = break_list.OrderByDescending(b=>b.Id).Skip((page - 1) * limit).Take(limit).Select(b=>new {
                    Id=b.Id,
                    ClassSchedule_Id=b.ClassSchedule_Id,
                    ClassRoomID= b.ClassRoomID,
                    ReadNovels=b.ReadNovels,
                    PlayPhone=b.PlayPhone,
                    PlayGame=b.PlayGame,
                    SeelpCount=b.SeelpCount,
                    EmployeesInfo_Id=b.EmployeesInfo_Id,
                    BaseDataEnum_Id = b.BaseDataEnum_Id,
                    RecodeDate = b.RecodeDate,
                    Rmark=b.Rmark,
                    IsDelete =b.IsDelete,
                    ClassName= BreakManeger.Classroom_Entity.GetSingData(b.ClassRoomID.ToString(),true).ClassroomName,
                    E_Name= MyEntity.Break_Entity.GetEmploySingData(b.EmployeesInfo_Id,true).EmpName,
                    DataEnumName=BreakManeger.BaseDataEnum_Entity.GetSingData(b.BaseDataEnum_Id.ToString(),true).Name
                }).ToList();
                var datajson = new
                {
                    code = 0,
                    msg = "",
                    count = break_list.Count,
                    data = mydata
                };
                return Json(datajson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public ActionResult AddorEditView(int? Id)
        {
            //获取阶段
            ViewBag.classSchedules= MyEntity.Break_Entity.GetClassSchedules().Select(c => new SelectListItem { Text = c.ClassNumber, Value = c.ClassNumber }).ToList();
            ViewBag.classRooms = MyEntity.Break_Entity.GetClassRoomData().Select(c => new SelectListItem { Text = c.ClassroomName, Value = c.Id.ToString() }).ToList();
            BaseDataEnum find_data= BreakManeger.BaseDataEnum_Entity.GetSingData("巡班时间段", false);
            ViewBag.dataenum = BreakManeger.BaseDataEnum_Entity.GetChildData(find_data.Id).Select(b=>new SelectListItem { Text = b.Name, Value = b.Id.ToString() }).ToList();
            if (Id==null)
            {
                //添加
                return View();
            }
            else
            {
                //编辑
               Break find_b= MyEntity.Break_Entity.GetEntity(Id);                 
                    return View(find_b);                            
            }
            
        }
        [HttpPost]
        public ActionResult AddorEditFunction(Break b)
        {
            try
            {
                if (b.Id > 0)
                {
                    Break fin_b = MyEntity.Break_Entity.GetEntity(b.Id);                    
                    //编辑
                    
                    fin_b.ClassSchedule_Id = b.ClassSchedule_Id;
                    fin_b.ClassRoomID = b.ClassRoomID;
                    fin_b.ReadNovels = b.ReadNovels;
                    fin_b.PlayPhone = b.PlayPhone;
                    fin_b.SeelpCount = b.SeelpCount;
                    fin_b.PlayGame = b.PlayGame;
                    fin_b.BaseDataEnum_Id = b.BaseDataEnum_Id;
                    fin_b.Rmark = b.Rmark;
                    MyEntity.Break_Entity.Update(fin_b);
                }
                else
                {
                    //添加
                    b.RecodeDate = DateTime.Now;
                    b.IsDelete = false;
                    b.EmployeesInfo_Id = "201908160008";//判断登陆的人员
                    MyEntity.Break_Entity.Insert(b);
                }

                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }
             
             
        }
        
        [HttpPost]

        public ActionResult Deletefunction(int Id)
        {
            Break fin_b = MyEntity.Break_Entity.GetEntity(Id);
             //删除
                fin_b.IsDelete = true;
                MyEntity.Break_Entity.Update(fin_b);
                return Json("ok", JsonRequestBehavior.AllowGet);            
        }
    }
}