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

    public class NetClientRecordController : Controller
    {
        // GET: Market/NetClientRecord
        public ActionResult NetIndex()
        {
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var elist = emanage.GetList();
            ViewBag.recorder = new SelectList(elist, "EmployeeId", "EmpName");
            BindSelect();
            return View();
        }
        /// <summary>
        /// 获取网咨学生所有信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="AppCondition"></param>
        /// <returns></returns>
        public ActionResult GetData(int page,int limit,string AppCondition){
            NetClientRecordManage ncrmanage = new NetClientRecordManage();
            ChannelStaffBusiness channel = new ChannelStaffBusiness();
            var list= ncrmanage.GetList();
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
                    list = list.Where(e =>e.IsDel==bool.Parse(IsDel)).ToList();
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
            var newlist = from e in mylist
                          select new
                          {
                              #region 获取属性值 
                              e.Id,
                              //netemp= GetNetConsultTea(e.EmployeeId).EmpName,
                              e.NetClientDate,
                              //channeltea = e.MarketTeaId==null ? null: GetNetConsultTea(e.MarketTeaId).EmpName,
                              //e.StuName,
                              //e.StuSex,
                              //e.StuAge,
                              //e.InformationSource,
                              //e.Education,
                              //e.Region,
                              //e.ContactInformation,
                              //e.QQNum,
                              //e.WeChatNum,
                              //e.IsFaceConsult,
                                e.CallBackCase,
                              //e.SecondCallBack,
                              //e.ThirdCallBack,
                              e.IsDel,
                              #endregion
                          };
            var newobj = new
               {
                  code = 0,
                   msg = "",
                 count = list.Count(),
                  data = newlist
                };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过员工编号获取员工
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public EmployeesInfo GetNetConsultTea(string empid) {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
           var NetConsultTeaobj = empmanage.GetEntity(empid);
            return NetConsultTeaobj;
            }
    
        /// 绑定登记人和市场对接老师两个下拉框的方法
        /// </summary>
        /// <returns></returns>
        public bool BindSelect()
        {
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            DepartmentManage dmanage = new DepartmentManage();
            PositionManage pmanage = new PositionManage();
            bool result = true;
            try
            {
                //获取网络部，绑定网络咨询师下拉框
                var deptid = dmanage.GetList().Where(d => d.DeptName == "网络部").FirstOrDefault().DeptId;//获取找到网络部部门编号
                var pobj = pmanage.GetList().Where(s => s.DeptId == deptid).ToList();
                var elist = emanage.GetList();
                List<EmployeesInfo> elist2 = new List<EmployeesInfo>();
                foreach (var e in elist)
                {
                    foreach (var p in pobj)
                    {
                        if (p.Pid == e.PositionId)
                        {
                            elist2.Add(e);
                        }
                    }
                }
                ViewBag.nettea = new SelectList(elist2, "EmployeeId", "EmpName");

                //获取市场部，绑定市场对接老师下拉框
                var mid = dmanage.GetList().Where(d => d.DeptName == "市场部").FirstOrDefault().DeptId;//获取找到网络部部门编号
                var markettobj = pmanage.GetList().Where(s => s.DeptId == mid).ToList();
                List<EmployeesInfo> elist3 = new List<EmployeesInfo>();
                foreach (var e in elist)
                {
                    foreach (var m in markettobj)
                    {
                        if ( m.Pid==e.PositionId)
                        {
                            elist3.Add(e);
                        }
                    }
                }
                ViewBag.markettea = new SelectList(elist3, "EmployeeId", "EmpName");
                result = true;
                BusHelper.WriteSysLog("网咨学生信息添加、修改及详情页的登记人和市场对接老师下拉框的绑定，绑定成功。", EnumType.LogType.添加数据);
            }
            catch (Exception)
            {
                result = false;
                BusHelper.WriteSysLog("网咨学生信息添加、修改及详情页的登记人和市场对接老师下拉框的绑定，绑定失败。", EnumType.LogType.添加数据);
            }

            return result;
        }

        //添加网咨学员信息
        public ActionResult AddNetConsultStuinfo() {
            BindSelect();
            return View();
        }
        [HttpPost]
        public ActionResult AddNetConsultStuinfo(NetClientRecord ncr) {
            NetClientRecordManage nmanage = new NetClientRecordManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var UserName = Base_UserBusiness.GetCurrentUser();//获取当前登录人
                                                               
                string eid = UserName.EmpNumber;//为测试，暂时设置的死数据
                // ncr.EmployeeId=session['网络咨询师'];网咨信息登记者即为正在登录该页面的员工
              //  ncr.EmployeeId = eid;//防止测试的报错暂时设置一个死值
                ncr.NetClientDate = DateTime.Now;
                nmanage.Insert(ncr);
               AjaxResultxx= nmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = nmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 网咨学员详细信息查看
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DetailInfo(int Id) {
            NetClientRecordManage ncr = new NetClientRecordManage();
            var n= ncr.GetEntity(Id);
            ViewBag.Id = Id;
            BindSelect();
            return View(n);
        }
        public ActionResult GetNetById(int id) {
            NetClientRecordManage ncr = new NetClientRecordManage();
            var n= ncr.GetEntity(id);
            return Json(n,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编辑网咨学员信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult EditView(int Id) {
            NetClientRecordManage ncr = new NetClientRecordManage();
            var n = ncr.GetEntity(Id);
            ViewBag.Id = Id;
            ViewBag.mydate = n.NetClientDate;
            BindSelect();
            return View(n);
        }
        [HttpPost]
        public ActionResult EditNetConsultStuinfo(NetClientRecord ncr) {
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
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }

        //public ActionResult EditNetStu(int id, string name, bool ismarry) {
        //    NetClientRecordManage ncrinfo = new NetClientRecordManage();
        //    var AjaxResultxx = new AjaxResult();
        //    try
        //    {
        //        var emp= ncrinfo.GetEntity(id);
        //        switch (name)
        //        {
        //            case "IsFaceConsult":
                       
        //                    emp.IsFaceConsult = ismarry;
        //                break;
        //            case "IsDel":
        //                    emp.IsDel =ismarry;

        //                break;
        //        }
        //        ncrinfo.Update(emp);
        //        AjaxResultxx = ncrinfo.Success();
        //    }
        //    catch (Exception ex)
        //    {
        //        AjaxResultxx = ncrinfo.Error(ex.Message);
        //    }
        //    return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        //}
    }
}