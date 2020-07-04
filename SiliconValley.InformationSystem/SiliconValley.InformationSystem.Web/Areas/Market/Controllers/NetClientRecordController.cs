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
            //获取区域
            SelectListItem newselectitem = new SelectListItem() ;
            var r_list = EmployandCounTeacherCoom.Studentrecond.GetEffectiveRegionAll(true).Select(r => new SelectListItem { Text = r.RegionName, Value = r.RegionName }).ToList();
            r_list.Add(newselectitem);
            ViewBag.are = r_list;

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
            var list = ncrmanage.GetList().Where(s=>s.IsDel==false).OrderByDescending(n=>n.Id).ToList();
            foreach (var item in list)
            {
                #region 获取属性值 
                var ncrview = ncrmanage.GetNcrviewById(item.Id);
                ncrlist.Add(ncrview);
                #endregion
            }
            if (!string.IsNullOrEmpty(AppCondition))
            {
                string[] str = AppCondition.Split(',');
                string name = str[0];
                string stusex = str[1];
                string recordemp = str[2];
                string IsFaceConsult = str[3];
                string findAreavalue = str[4];
                string graduschool = str[5];
                string status = str[6];
                ncrlist = ncrlist.Where(e => e.StuName.Contains(name)).ToList();
                if (!string.IsNullOrEmpty(stusex))
                {
                    ncrlist = ncrlist.Where(e => e.StuSex == stusex).ToList();
                }
                if (!string.IsNullOrEmpty(recordemp)) {
                    ncrlist = ncrlist.Where(e => e.SprEmp.Contains(recordemp)).ToList();
                }
                if (!string.IsNullOrEmpty(IsFaceConsult))
                {
                    ncrlist = ncrlist.Where(e => e.IsFaceConsult ==IsFaceConsult).ToList();
                }
                if (!string.IsNullOrEmpty(findAreavalue))
                {
                    ncrlist = ncrlist.Where(e => e.RegionName == findAreavalue).ToList();
                }
                if (!string.IsNullOrEmpty(graduschool)) {
                    ncrlist = ncrlist.Where(e => e.StuName.Contains(graduschool)).ToList();
                }
                if (!string.IsNullOrEmpty(status))
                {
                    ncrlist = ncrlist.Where(e => e.StuStatus == status).ToList();
                }
            }
            var newncrlist= ncrlist.OrderBy(n => n.Id).Skip((page - 1) * limit).Take(limit).ToList();
           
            var newobj = new
            {
                code = 0,
                msg = "",
                count = ncrlist.Count(),
                data = newncrlist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }

        //添加网咨学员信息
        public ActionResult AddNetConsultStuinfo(int id)
        {
            NetClientRecordManage nmanage = new NetClientRecordManage();
            var ncr = nmanage.GetEntity(id);
            var nlist = nmanage.GetList().Where(s => s.SPRId == ncr.SPRId).ToList();
            ViewBag.Number = nlist.Count() - 1;
            ViewBag.Id = id;
            ViewBag.grade = nlist.LastOrDefault().Grade;
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
                ncrnew.IsDel = true;
                ncrnew.Grade = ncr.Grade;
                ncrnew.CallBackCase = ncr.CallBackCase;
                var ncrlist = nmanage.GetList().Where(s => s.SPRId == ncrnew.SPRId).ToList();
                if (ncrlist.Count>0) {
                    ncrnew.MarketTeaId = ncrlist.LastOrDefault().MarketTeaId;
                }
                nmanage.Insert(ncrnew);
                AjaxResultxx = nmanage.Success();
                //    BusHelper.WriteSysLog(empmanage.GetInfoByEmpID(eid).EmpName + "添加了一条回访学生信息", Entity.Base_SysManage.EnumType.LogType.添加数据);
                if (AjaxResultxx.Success) {
                    AjaxResultxx.Success= nmanage.UpdateNetClientDate(ncrnew.SPRId,ncrnew.NetClientDate);
                }
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
            List<NetClientRecordView> ncrviewlist = new List<NetClientRecordView>();
             ncrviewlist = ncr.GetNcrviewlist(Id);//获取回访记录集合
            var n = ncr.GetNcrviewById(Id);
            ViewBag.ncrlist = ncrviewlist;
            ViewBag.Number = ncrviewlist.Count();
            ViewBag.Id = Id;
            //BindSelect();
            return View();
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
           
            ViewBag.Id = Id;
            ncrviewlist = ncr.GetNcrviewlist(Id);//获取回访记录集合
            ViewBag.ncrlist = ncrviewlist;
            ViewBag.Number = ncrviewlist.Count();

            var nview = ncr.GetNcrviewById(Id);
            return View(nview);
        }
        [HttpPost]
        public ActionResult EditCallbackInfo(NetClientRecord ncr)
        {

            NetClientRecordManage ncrmanage = new NetClientRecordManage();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();

            try
            {
                var n = ncrmanage.GetEntity(ncr.Id);
                n.CallBackCase = ncr.CallBackCase;
                n.MarketTeaId = ncr.MarketTeaId;
                ncrmanage.Update(n);
                AjaxResultxx = ncrmanage.Success();
                if (AjaxResultxx.Success) {
                    var ncrlist = ncrmanage.GetList().Where(s => s.SPRId == n.SPRId).ToList();
                    foreach (var item in ncrlist)
                    {
                        item.MarketTeaId = n.MarketTeaId;
                        ncrmanage.Update(item);
                    }
                    AjaxResultxx = ncrmanage.Success();
                }
            
            }
            catch (Exception ex)
            {
                AjaxResultxx = ncrmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditCallbackInfos(int id,string markettea)
        {

            NetClientRecordManage ncrmanage = new NetClientRecordManage();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var ncrlist = ncrmanage.GetList().Where(s => s.SPRId == ncrmanage.GetEntity(id).SPRId).ToList();
                foreach (var item in ncrlist)
                {
                    item.MarketTeaId = int.Parse(markettea);
                    ncrmanage.Update(item);
                }
                AjaxResultxx = ncrmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = ncrmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

    }
}