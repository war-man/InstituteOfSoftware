using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.StuSatae_Maneger;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    [CheckLogin]
    public class StatusController : Controller
    {

        private StuStateManeger Stustate_Entity;
        // GET: /Market/Status/AddStatesFunction
        public ActionResult StatusIndexView()
        {
            return View();
        }

        //获取学生状态的所有数据
        public ActionResult GetStatusData(int page, int limit)
        {
            Stustate_Entity = new StuStateManeger();
            List<StuStatus> state_list = Stustate_Entity.GetList();
            List<StuStatus> newstate = state_list.OrderByDescending(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var Jsondata = new
            {
                code = 0, //解析接口状态,
                msg = "", //解析提示文本,
                count = state_list.Count, //解析数据长度
                data = newstate //解析数据列表
            };
            return Json(Jsondata, JsonRequestBehavior.AllowGet);
        }

        //修改状态
        public ActionResult EditStates(string id, string name, string state,string shuxing)
        {
            Stustate_Entity = new StuStateManeger();
            //获取当前上传的操作人
            string UserName = Base_UserBusiness.GetCurrentUser().UserName;
            try
            {
                int Id = Convert.ToInt32(id);
                StuStatus findstate = Stustate_Entity.GetEntity(Id);
                StuStatus IsRepart = Stustate_Entity.GetList().Where(s => s.StatusName == name).FirstOrDefault();
                if (findstate != null && IsRepart == null && shuxing!=null)
                {
                    switch (shuxing)
                    {
                        case "StatusName":
                            findstate.StatusName = name;
                            break;
                        case "Rmark":
                            findstate.Rmark = name;
                            break;
                    }
                   
                    Stustate_Entity.Update(findstate);
                }
                else if (IsRepart != null)
                {
                    return Json("数据重复!!!", JsonRequestBehavior.AllowGet);
                }
                else if (findstate == null)
                {
                    return Json("数据错误，请重试!", JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(state))
                {
                    bool states = Convert.ToBoolean(state);
                    findstate.IsDelete = states == true ? false : true;
                    Stustate_Entity.Update(findstate);
                }
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog("操作人:" + UserName + "出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                return Json("系统异常，请联系管理员", JsonRequestBehavior.AllowGet);
            }
            BusHelper.WriteSysLog("操作人:" + UserName + "编辑成功", Entity.Base_SysManage.EnumType.LogType.编辑数据);
            return Json("ok", JsonRequestBehavior.AllowGet);
        }


        //添加数据方法
        public ActionResult AddStatesFunction(StuStatus new_s)
        {
            Stustate_Entity = new StuStateManeger();
            new_s.IsDelete = false;
            AjaxResult a= Stustate_Entity.Add_Data(new_s);
            return Json(a,JsonRequestBehavior.AllowGet);
        }

        //添加页面
        public ActionResult AddView()
        {
            return View();
        }
    }
}