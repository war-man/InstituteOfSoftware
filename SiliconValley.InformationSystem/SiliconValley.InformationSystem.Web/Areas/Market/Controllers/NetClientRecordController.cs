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
            var list = ncrmanage.GetList();
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

        /// <summary>
        /// 通过员工编号获取员工
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        //public EmployeesInfo GetNetConsultTea(string empid) {
        //    EmployeesInfoManage empmanage = new EmployeesInfoManage();
        //   var NetConsultTeaobj = empmanage.GetEntity(empid);
        //    return NetConsultTeaobj;
        //    }


        //添加网咨学员信息
        public ActionResult AddNetConsultStuinfo(int id)
        {
            NetClientRecordManage nmanage = new NetClientRecordManage();
            var ncr = nmanage.GetEntity(id);
            ViewBag.Number = nmanage.GetList().Where(s => s.SPRId == ncr.SPRId).ToList().Count()-1;
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public ActionResult AddNetConsultStuinfo(NetClientRecord ncr)
        {
            NetClientRecordManage nmanage = new NetClientRecordManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var UserName = Base_UserBusiness.GetCurrentUser();//获取当前登录人

                string eid = UserName.EmpNumber;
                var oldncr = nmanage.GetEntity(ncr.Id);
                NetClientRecord ncrnew = new NetClientRecord();
                ncrnew.SPRId = oldncr.SPRId;
                ncrnew.EmpId = eid;
                ncrnew.NetClientDate = DateTime.Now;
                nmanage.Insert(ncr);
                AjaxResultxx = nmanage.Success();
                if (AjaxResultxx.Success){
                    var ncrlist = nmanage.GetList().Where(s => s.SPRId == oldncr.SPRId).ToList();
                    foreach (var item in ncrlist)
                    {  
                        item.EmpId = eid;
                        nmanage.Update(item);
                    }
                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = nmanage.Error(ex.Message);
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
            var ncrview= ncrmanage.GetNcrviewById(id);
            return Json(ncrview, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编辑网咨学员信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult EditCallbackInfo(int Id)
        {
            NetClientRecordManage ncr = new NetClientRecordManage();
            var n = ncr.GetEntity(Id);
            ViewBag.Id = Id;
            ViewBag.mydate = n.NetClientDate;

            return View(n);
        }
        [HttpPost]
        public ActionResult EditCallbackInfo(NetClientRecord ncr)
        {
            NetClientRecordManage ncrmanage = new NetClientRecordManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                ncrmanage.Update(ncr);
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