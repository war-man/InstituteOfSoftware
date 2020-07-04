using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Business.EducationalBusiness;
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Util;
 
    public static class MyEntity{
        public static readonly BreakManeger Break_Entity = new BreakManeger();
     }
    [CheckLogin]
    public class BreakController : Controller
    {
        // GET: /Educational/Break/BreakIndexView
        public ActionResult BreakIndexView()
        {
            //加载阶段
            List<SelectListItem> g_list = MyEntity.Break_Entity.GetEffectiveData().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString(),Selected=false }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.s_grandlist = g_list;

            //获取巡班时间段
            BaseDataEnum find_data = BreakManeger.BaseDataEnum_Entity.GetSingData("巡班时间段", false);
            ViewBag.s_dataenum = BreakManeger.BaseDataEnum_Entity.GetChildData(find_data.Id).Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() }).ToList();

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
                    //break_list = break_list.Where(b => b.ClassSchedule_Id == Name).ToList();
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
                   
                    BaseDataEnum_Id = b.BaseDataEnum_Id,
                    RecodeDate = b.RecodeDate,
                    Rmark=b.Rmark,
                    IsDelete =b.IsDelete,
                     
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
            //加载阶段
            List<SelectListItem> g_list = MyEntity.Break_Entity.GetEffectiveData().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString(), Selected = false }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.Add_grandlist = g_list;

            //获取巡班时间段
            BaseDataEnum find_data= BreakManeger.BaseDataEnum_Entity.GetSingData("巡班时间段", false);
            ViewBag.Add_dataenum = BreakManeger.BaseDataEnum_Entity.GetChildData(find_data.Id).Select(b=>new SelectListItem { Text = b.Name, Value = b.Id.ToString() }).ToList();

            //获取上课违纪类型
            BaseDataEnum find_dataV = BreakManeger.BaseDataEnum_Entity.GetSingData("上课违纪类型", false);
            ViewBag.Add_V = BreakManeger.BaseDataEnum_Entity.GetChildData(find_dataV.Id).Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() }).ToList();


            

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
        public ActionResult AddorEditFunction()
        {
            Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();//获取登录人信息

            int ClassSchedule_Id=Convert.ToInt32(Request.Form["ClassSchedule_Id"]);

            int BaseDataEnum_Id = Convert.ToInt32(Request.Form["BaseDataEnum_Id"]);//时间段

            string Rmark=  Request.Form["Rmark"];

            string Count = Request.Form["Count"];//违纪类型与人数

            List<Break> blist = new List<Break>();

            AjaxResult a = new AjaxResult();
            try
            {
                string[] Counts= Count.Split(',');
                foreach (string item in Counts)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        Break bre = new Break();
                        bre.Count = Convert.ToInt32(item.Split(',')[1]);
                        bre.Violationofdiscipline_Id = Convert.ToInt32(item.Split(',')[0]);
                        bre.Emp_Id = UserName.EmpNumber;
                        bre.ClassSchedule_Id = ClassSchedule_Id;
                        bre.RecodeDate = DateTime.Now;
                        bre.BaseDataEnum_Id = BaseDataEnum_Id;
                        bre.IsDelete = false;
                        bre.Rmark = Rmark;

                        blist.Add(bre);
                    }                                       
                }

               a= MyEntity.Break_Entity.Addlist(blist);


                //if (b.Id > 0)
                //{
                //    //Break fin_b = MyEntity.Break_Entity.GetEntity(b.Id);                    
                //    ////编辑

                //    //fin_b.ClassSchedule_Id = b.ClassSchedule_Id;

                //    //fin_b.BaseDataEnum_Id = b.BaseDataEnum_Id;
                //    //fin_b.Rmark = b.Rmark;
                //    //MyEntity.Break_Entity.Update(fin_b);
                //}
                //else
                //{

                //    //添加                   
                //    MyEntity.Break_Entity.Insert(b);
                //}

                return Json(a, JsonRequestBehavior.AllowGet);
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