using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.StuSatae_Maneger;
using SiliconValley.InformationSystem.Entity.MyEntity;
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
        // GET: /Market/Status/StatusIndexView
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
        public ActionResult EditStates(string id, string name, string state)
        {
            Stustate_Entity = new StuStateManeger();
            //获取当前上传的操作人
            string UserName = Base_UserBusiness.GetCurrentUser().UserName;
            try
            {
                int Id = Convert.ToInt32(id);
                StuStatus findstate = Stustate_Entity.GetEntity(Id);
                StuStatus IsRepart = Stustate_Entity.GetList().Where(s => s.StatusName == name).FirstOrDefault();
                if (!string.IsNullOrEmpty(name) && findstate != null && IsRepart == null)
                {
                    findstate.StatusName = name;
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


        //添加
        public ActionResult AddStates()
        {
            Stustate_Entity = new StuStateManeger();
            //获取当前上传的操作人
            string UserName = Base_UserBusiness.GetCurrentUser().UserName;
            List<StuStatus> Isrepert = Stustate_Entity.GetList().Where(s => s.StatusName == null).ToList();
            try
            {
                if (Isrepert.Count > 0)
                {
                    return Json("有信息未填写，请填写之后在添加", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Stustate_Entity.Insert(new StuStatus() { IsDelete = false });
                }
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog("操作人:" + UserName + "出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                return Json("系统异常，请联系管理员", JsonRequestBehavior.AllowGet);
            }
            BusHelper.WriteSysLog("操作人:" + UserName + "触发了添加按钮", Entity.Base_SysManage.EnumType.LogType.添加数据);
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

    }
}