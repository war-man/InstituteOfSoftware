using SiliconValley.InformationSystem.Business.DepartmentBusiness;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Dormitory.Controllers
{
    public class StaffRoomDetailsController : Controller
    {
        private DormInformationBusiness dbdorm;
        private RoomdeWithPageXmlHelp dbroomxml;
        private EmployeesInfoManage dbempinfo;
        private ConversionToViewBusiness dbconversion;
        private DepartmentManage dbdepartmentManages;
        private DepartmentManage dbdpte;
        private ProEmpinfoBusiness dbproempinfo;
        // GET: Dormitory/StaffRoomDetails
        public ActionResult StaffRoomDetailsIndex()
        {
            return View();
        }

        [HttpGet]
        /// <summary>
        /// 房间详细页面
        /// </summary>
        /// <returns></returns>
        public ActionResult StaffRoomdeWithPage(int DorminfoID)
        {
            dbdorm = new DormInformationBusiness();
            dbroomxml = new RoomdeWithPageXmlHelp();
            DormInformation dorm = dbdorm.GetDormByDorminfoID(DorminfoID);
            int female = dbroomxml.Getmale(RoomTypeEnum.SexType.Female);
            string title = string.Empty;

            //女
            if (dorm.SexType == female)
            {
                title = "-女寝";
            }
            else
            {
                title = "-男寝";
            }
            ViewBag.Title = dorm.DormInfoName + title;
            ViewBag.SexType = dorm.SexType;
            ViewBag.DormInformation = DorminfoID;
            return View();
        }


        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="param0">下拉框的选项</param>
        /// <param name="param1">后面的文本框的值</param>
        /// <returns></returns>
        public ActionResult StaffSeachoption(string param0, string param1)
        {
            dbconversion = new ConversionToViewBusiness();
           
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                switch (param0)
                {
                    case "name0":
                        dbempinfo = new EmployeesInfoManage();
                        List<EmployeesInfo> empinfodata = dbempinfo.GetAll().Where(a => a.EmpName == param1).ToList();
                        List<RoomArrangeEmpinfoView> resultdata = dbconversion.EmpinfoToRoomArrangeEmpinfoView(empinfodata, true);
                        ajaxResult.Data = resultdata;
                        if (resultdata.Count != 0)
                        {
                            ajaxResult.Success = true;
                        }
                        else
                        {
                            ajaxResult.Success = false;
                            ajaxResult.Msg = "没有该员工的居住信息。";
                        }
                        break;
                    case "name1":
                        dbdorm = new DormInformationBusiness();
                        ajaxResult.Data = dbdorm.GetDorms().Where(a => a.DormInfoName == param1).FirstOrDefault();

                        if (ajaxResult.Data != null)
                        {
                            ajaxResult.Success = true;
                        }
                        else
                        {
                            ajaxResult.Success = false;
                            ajaxResult.Msg = "没有该寝室。";
                        }
                        break;
                    case "name2":
                        dbdpte = new DepartmentManage();
                        dbproempinfo = new  ProEmpinfoBusiness();
                        var obj0= dbdpte.GetDepartmentByName(param1);
                        
                        if (obj0!=null)
                        {
                            var list0 = dbproempinfo.GetEmployeesInfosByDepteID(obj0.DeptId);
                            List<RoomArrangeEmpinfoView> result0 = dbconversion.EmpinfoToRoomArrangeEmpinfoView(list0, true);
                            ajaxResult.Data = result0;
                            ajaxResult.Success = true;
                        }
                        else
                        {
                            ajaxResult.Success = false;
                            ajaxResult.Msg = "没有该部门名称。";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ajaxResult.Data = "";
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！为你及时处理。";
                throw;
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 搜索名字出现重复的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Staffloadlistwith()
        {
            return View();
        }

    }
}