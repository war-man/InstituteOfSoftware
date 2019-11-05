using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
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
    /// <summary>
    /// 员工入住
    /// </summary>
    public class EmployeeCheckInController : Controller
    {

        private ConversionToViewBusiness dbconversion;

        private dbstaffacc_dbempinfo dbstaffacc_dbempinfo;

        private dbstaffacc_dbroomnumber dbstaffacc_dbroomnumber;

        private DormInformationBusiness dbdorm;

        private dbacc_dbroomnumber dbaccroomnumber;

        private RoomdeWithPageXmlHelp dbroomxml;

        private StaffAccdationBusiness dbstaffacc;

        private TungFloorBusiness dbtungfloor;
        // GET: Dormitory/EmployeeCheckIn
        public ActionResult EmployeeCheckInIndex()
        {
            return View();
        }

        /// <summary>
        /// 用于加载未居住的员工数据
        /// </summary>
        /// <returns></returns>
        public ActionResult UninhabitedList(int page, int limit)
        {
            dbconversion = new ConversionToViewBusiness();

            List<RoomArrangeEmpinfoView> resultdata = this.GetUninhabitedListforstaff();

            var data = resultdata.OrderByDescending(a => a.EmployeeId).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = resultdata.Count(),
                data = data
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }
       
        /// <summary>
        /// 获取未居住员工
        /// </summary>
        /// <returns></returns>
        public List<RoomArrangeEmpinfoView> GetUninhabitedListforstaff()
        {
            dbstaffacc_dbempinfo = new dbstaffacc_dbempinfo();
            dbconversion = new ConversionToViewBusiness();
            List<EmployeesInfo> employeesInfos = dbstaffacc_dbempinfo.GetUninhabitedData();
            return dbconversion.EmpinfoToRoomArrangeEmpinfoView(employeesInfos, false);
        }
        /// <summary>
        /// 添加居住信息
        /// </summary>
        /// <param name="BedId"></param>
        /// <param name="DormId"></param>
        /// <param name="Studentnumber"></param>
        /// <returns></returns>
        public ActionResult ArrangeDorm(int BedId, int DormId, string resultdata)
        {
            AjaxResult ajaxResult = new AjaxResult();

            dbstaffacc = new StaffAccdationBusiness();
            StaffAccdation staffAccdation = new StaffAccdation();
            staffAccdation.BedId = BedId;
            staffAccdation.CreationTime = DateTime.Now;
            staffAccdation.DormId = DormId;
            staffAccdation.EmployeeId = resultdata;
            staffAccdation.IsDel = false;
            staffAccdation.Remark = string.Empty;
            staffAccdation.StayDate = DateTime.Now;
            ajaxResult.Success = dbstaffacc.AddStaffacc(staffAccdation);

            return Json(ajaxResult, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 查询寝室
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public ActionResult ChoiceInfo(bool sex, int TungID, int FloorID)
        {
            dbdorm = new DormInformationBusiness();
            dbaccroomnumber = new dbacc_dbroomnumber();
            dbroomxml = new RoomdeWithPageXmlHelp();
            dbtungfloor = new TungFloorBusiness();
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                TungFloor querytungfloor = dbtungfloor.GetTungFloorByTungIDAndFloorID(TungID, FloorID);
                List<DormInformation> dormlist = new List<DormInformation>();
                var data = dbdorm.GetDormsByTungFloorIDing(querytungfloor.Id);
                //默认男寝
                int maleid = 1;
                //男寝
                if (sex)
                {
                    //男寝数据
                    data = data.Where(a => a.SexType == maleid).ToList();

                }
                else
                {
                    maleid = dbroomxml.Getmale(RoomTypeEnum.SexType.Female);

                    //女寝寝数据
                    data = data.Where(a => a.SexType == maleid).ToList();
                }



                dbstaffacc_dbroomnumber = new dbstaffacc_dbroomnumber();
                var xmlroomtype = dbroomxml.GetRoomType(RoomTypeEnum.RoomType.StaffRoom);

                data = data.Where(a => a.RoomStayTypeId == xmlroomtype).ToList();


                foreach (var item in data)
                {
                    if (!dbstaffacc_dbroomnumber.IsFull(item.ID, item.RoomStayNumberId))
                    {
                        dormlist.Add(item);
                    }
                }
                var dormInfoViews = this.dormInfoViews(dormlist);
                ajaxResult.Data = dormInfoViews;
                ajaxResult.Success = true;
                BusHelper.WriteSysLog("查询员工寝室数据Dormitory/DormitoryInfo/ChoiceInfo", Entity.Base_SysManage.EnumType.LogType.查询数据success);


            }
            catch (Exception ex)
            {

                ajaxResult.Msg = "请及时的联系信息部";
                ajaxResult.Success = false;
            }


            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 将房间实体对象转化为页面model
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<DormInfoView> dormInfoViews(List<DormInformation> data)
        {
            List<DormInfoView> dormInfoViews = new List<DormInfoView>();
            foreach (var item in data)
            {
                DormInfoView myview = new DormInfoView();
                myview.ID = item.ID;
                myview.DormInfoName = item.DormInfoName;
                dormInfoViews.Add(myview);
            }
            return dormInfoViews;
        }
    }
}