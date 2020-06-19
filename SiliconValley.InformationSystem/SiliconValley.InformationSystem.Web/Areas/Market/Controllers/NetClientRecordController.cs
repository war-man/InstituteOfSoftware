using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    using SiliconValley.InformationSystem.Business.NetClientRecordBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Business.PositionBusiness;
    using SiliconValley.InformationSystem.Business.DepartmentBusiness;
    using SiliconValley.InformationSystem.Business.Channel;
    using SiliconValley.InformationSystem.Util;
    using SiliconValley.InformationSystem.Entity.Base_SysManage;
    using SiliconValley.InformationSystem.Business.Common;
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Business.MarketChair_Business;
    public class NetClientRecordController : Controller
    {
        // GET: Market/NetClientRecord
        public ActionResult NetIndex()
        {
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var elist = emanage.GetList();
            ViewBag.recorder = new SelectList(elist, "EmployeeId", "EmpName");

            return View();
        }
        /// <summary>
        /// 获取网咨学生所有信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="AppCondition"></param>
        /// <returns></returns>
        public ActionResult GetData(int page, int limit, string AppCondition)
        {
            NetClientRecordManage ncrmanage = new NetClientRecordManage();
            List<NetClientRecordView> ncrlist = new List<NetClientRecordView>();
            var list = ncrmanage.GetList().Where(s=>string.IsNullOrEmpty(Convert.ToString(s.NetClientDate))).ToList();
            if (!string.IsNullOrEmpty(AppCondition))
            {
                string[] str = AppCondition.Split(',');
                string name = str[0];
                string InformationSource = str[1];
                string registrant = str[2];
                string IsFaceConsult = str[3];
                string IsDel = str[4];
                string start_time = str[5];
                string end_time = str[6];
                //list = list.Where(e => e.StuName.Contains(name)).ToList();
                //list = list.Where(e => e.InformationSource.Contains(InformationSource)).ToList();
                //if (!string.IsNullOrEmpty(registrant))
                //{
                //    list = list.Where(e =>e.EmployeeId==registrant).ToList();
                //}
                //if (!string.IsNullOrEmpty(IsFaceConsult))
                //{
                //    list = list.Where(e =>e.IsFaceConsult==bool.Parse(IsFaceConsult)).ToList();
                //}
                if (!string.IsNullOrEmpty(IsDel))
                {
                    list = list.Where(e => e.IsDel == bool.Parse(IsDel)).ToList();
                }
                if (!string.IsNullOrEmpty(start_time))
                {
                    DateTime stime = Convert.ToDateTime(start_time + " 00:00:00.000");
                    list = list.Where(a => a.NetClientDate >= stime).ToList();
                }
                if (!string.IsNullOrEmpty(end_time))
                {
                    DateTime etime = Convert.ToDateTime(end_time + " 23:59:59.999");
                    list = list.Where(a => a.NetClientDate <= etime).ToList();
                }
            }
            var mylist = list.OrderBy(n => n.Id).Skip((page - 1) * limit).Take(limit).ToList();
            foreach (var item in mylist)
            {
                #region 获取属性值 
                var ncrview = ncrmanage.GetNcrviewById(item.Id);
                ncrlist.Add(ncrview);
                #endregion
            }
            var newobj = new
            {
                code = 0,
                msg = "",
                count = list.Count(),
                data = ncrlist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }

        //添加网咨学员信息
        public ActionResult AddNetConsultStuinfo(int id)
        {
            NetClientRecordManage nmanage = new NetClientRecordManage();
            var ncr = nmanage.GetEntity(id);
            ViewBag.Number = nmanage.GetList().Where(s => s.SPRId == ncr.SPRId).ToList().Count() - 1;
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public ActionResult AddNetConsultStuinfo(NetClientRecord ncr)
        {
            NetClientRecordManage nmanage = new NetClientRecordManage();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();
            var UserName = Base_UserBusiness.GetCurrentUser();//获取当前登录人
            string eid = UserName.EmpNumber;
            try
            {
                var oldncr = nmanage.GetEntity(ncr.Id);
                NetClientRecord ncrnew = new NetClientRecord();
                ncrnew.SPRId = oldncr.SPRId;
                ncrnew.EmpId = eid;
                ncrnew.NetClientDate = DateTime.Now;
                ncrnew.IsDel = oldncr.IsDel;
                ncrnew.Grade = ncr.Grade;
                ncrnew.CallBackCase = ncr.CallBackCase;
                nmanage.Insert(ncrnew);
                AjaxResultxx = nmanage.Success();
            //    BusHelper.WriteSysLog(empmanage.GetInfoByEmpID(eid).EmpName + "添加了一条回访学生信息", Entity.Base_SysManage.EnumType.LogType.添加数据);
               
            }
            catch (Exception ex)
            {
                AjaxResultxx = nmanage.Error(ex.Message);
              //  BusHelper.WriteSysLog(empmanage.GetInfoByEmpID(eid).EmpName + "添加回访数据出错:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);

            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 网咨学员详细信息查看
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CallbackDetailInfo(int Id)
        {
            NetClientRecordManage ncr = new NetClientRecordManage();
            var n = ncr.GetNcrviewById(Id);
            ViewBag.Id = Id;
            //BindSelect();
            return View(n);
        }
        public ActionResult GetCallbackInfoById(int id)
        {
            NetClientRecordManage ncrmanage = new NetClientRecordManage();
            var ncrview = ncrmanage.GetNcrviewById(id);
            return Json(ncrview, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectChannelemp()
        {
            return View();
        }
        /// <summary>
        /// 获取所有的渠道员工(包含离职的)
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMarketTea(int page, int limit, string ename)
        {
            ChannelStaffBusiness channelmanage = new ChannelStaffBusiness();
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var cslist = channelmanage.GetAll();
            if (!string.IsNullOrEmpty(ename))
            {
                cslist = cslist.Where(e => empinfo.GetInfoByEmpID(e.EmployeesInfomation_Id).EmpName.Contains(ename)).ToList();
            }
            var mylist = cslist.OrderBy(e => e.ID).Skip((page - 1) * limit).Take(limit).ToList();
            var newlist = from e in mylist
                          select new
                          {
                              #region 获取属性值 
                              e.ID,
                              empid=e.EmployeesInfomation_Id,
                              empname = empinfo.GetInfoByEmpID(e.EmployeesInfomation_Id).EmpName,
                              Position = empinfo.GetPositionByEmpid(e.EmployeesInfomation_Id).PositionName,
                              Sex = empinfo.GetInfoByEmpID(e.EmployeesInfomation_Id).Sex,
                              empstate = empinfo.GetInfoByEmpID(e.EmployeesInfomation_Id).IsDel,
                              #endregion
                          };
            var newobj = new
            {
                code = 0,
                msg = "",
                count = cslist.Count(),
                data = newlist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }
     
        /// <summary>
        /// 获取回访信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCallBackInfoByNid(int id) {
            NetClientRecordManage ncrmanage = new NetClientRecordManage();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            ChannelStaffBusiness channel = new ChannelStaffBusiness();
            var AjaxResultxx = new AjaxResult();
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                var ncr = ncrmanage.GetNcrviewById(id);
                AjaxResultxx.Data = ncr;
                AjaxResultxx.Success = true;
            }
            else
            {
                AjaxResultxx.Success = false;
                AjaxResultxx.Data = "";
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑网咨学员信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult EditCallbackInfo(int Id)
        {
            NetClientRecordManage ncr = new NetClientRecordManage();
            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            List<NetClientRecordView> ncrviewlist = new List<NetClientRecordView>();
            var n = ncr.GetEntity(Id);
            ViewBag.Id = Id;
            var trackdata = ncr.GetList().Where(s =>s.SPRId==n.SPRId&& s.NetClientDate != null).ToList();//获取跟踪的数据（没有跟踪时间的是初始数据即不属于跟踪数据）
            foreach (var item in trackdata)
            {
                var ncrview = ncr.GetNcrviewById(item.Id);
                ncrviewlist.Add(ncrview);
            }
            var nview = ncr.GetNcrviewById(Id);
            ViewBag.Number = ncrviewlist.Count();
            var newlist = from e in trackdata
                          select new
                          {
                              #region 获取属性值 
                              e.Id,
                              e.EmpId,
                              empname = empinfo.GetInfoByEmpID(e.EmpId).EmpName,
                              e.NetClientDate,
                              e.MarketTeaId,
                              e.Grade,
                              e.IsDel
                              #endregion
                          };

            ViewBag.ncrlist = ncrviewlist;
            return View(nview);
        }
        [HttpPost]
        public ActionResult EditCallbackInfo()
        {
            NetClientRecordManage ncrmanage = new NetClientRecordManage();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();
            int fid=Convert.ToInt32(Request.Form["F_Id"]);
            var UserName = Base_UserBusiness.GetCurrentUser();//获取当前登录人
            string eid = UserName.EmpNumber;
            NetClientRecord n = new NetClientRecord();
            try
            {
                n.EmpId = eid;
                n.NetClientDate = DateTime.Now;
                ncrmanage.Update(n);
                AjaxResultxx = ncrmanage.Success();
            
            }
            catch (Exception ex)
            {
                AjaxResultxx = ncrmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditNetStu(int id, string name, bool ismarry)
        {
            NetClientRecordManage ncrinfo = new NetClientRecordManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var emp = ncrinfo.GetEntity(id);

                ncrinfo.Update(emp);
                AjaxResultxx = ncrinfo.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = ncrinfo.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
    }
}