using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    public class BeOnDutyController : Controller
    {
        BeOnDutyManeger BD_Entity = new BeOnDutyManeger();
        
        // GET: /Educational/BeOnDuty/FindSingleData
        public ActionResult BeOnDutyIndexView()
        {
            var UserName = Base_UserBusiness.GetCurrentUser();//获取当前登录人
            return View();
        }
        //数据加载
        public ActionResult GetBeOnDutyAllData(int limit,int page)
        {
            try
            {
                List<BeOnDuty> All = BD_Entity.GetList();//获取所有数据
                                                         //分页
                List<BeOnDuty> Page_All = All.Skip((page - 1) * limit).Take(limit).ToList();
                var datajson = new
                {
                    code = 0,  
                    msg = "",  
                    count = All.Count,  
                    data = Page_All  
                };
                return Json(datajson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //BusHelper.WriteSysLog(UserName+ "加载值班，加班费数据时出现：" + ex.Message, EnumType.LogType.加载数据);
                var datajson = new
                {
                    code = 0,  
                    msg = "",  
                    count = 0,  
                    data =0  
                };
                return Json(datajson, JsonRequestBehavior.AllowGet);
            }
           
             
        }
        //添加或编辑数据
        public ActionResult AddDataFunction()
        {
            try
            {
                string TypeName = Request.Form["MyTypeName"];
                string Cost = Request.Form["MyCost"];
                string Reamk = Request.Form["MyReak"];
                string BeId = Request.Form["BeId"];
                string State = Request.Form["IsDelete"];    
                if (!string.IsNullOrEmpty(BeId) && !string.IsNullOrEmpty(State))
                {
                   int Id_My= Convert.ToInt32(BeId);
                   BeOnDuty find_be= BD_Entity.GetEntity(Id_My);
                    if (find_be!=null)
                    {
                        find_be.IsDelete =Convert.ToBoolean( State);
                        find_be.Reak = Reamk;
                        find_be.TypeName = TypeName;
                        find_be.Cost = Convert.ToDecimal(Cost);
                        BD_Entity.Update(find_be);
                        //BusHelper.WriteSysLog(UserName + "值班，加班费用成功编辑数据", EnumType.LogType.编辑数据);
                    }
                }
                else
                {
                    BeOnDuty newBeonduty = new BeOnDuty();
                    newBeonduty.TypeName = TypeName;
                    newBeonduty.Cost = Convert.ToDecimal(Cost);
                    newBeonduty.Reak = Reamk;
                    newBeonduty.IsDelete = false;
                    newBeonduty.AddDate = DateTime.Now;
                    BD_Entity.Insert(newBeonduty);
                    //BusHelper.WriteSysLog(UserName + "值班，加班费用成功添加数据", EnumType.LogType.添加数据);
                }                                  
                return Json("ok",JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //BusHelper.WriteSysLog(UserName + "值班，加班费用添加数据时出现：" + ex.Message, EnumType.LogType.添加数据);
                return Json("系统错误，请重试！！！", JsonRequestBehavior.AllowGet);
            }             
        }
        //添加或编辑页面
        public ActionResult AddBeOnDutyView(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                //添加页面
                ViewBag.Id = 0;
            }
            else
            {
                //编辑页面
                ViewBag.Id = id;
            }
            return View();
        }
        //查询单个数据
        public ActionResult FindSingleData(int id)
        {
           BeOnDuty find_result= BD_Entity.GetSingleBeOnButy(id.ToString(), true);
            return Json(find_result,JsonRequestBehavior.AllowGet);
        }
    }
}