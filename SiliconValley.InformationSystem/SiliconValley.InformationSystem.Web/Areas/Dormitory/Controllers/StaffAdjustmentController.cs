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
    public class StaffAdjustmentController : Controller
    {

        private ConversionToViewBusiness dbconversion;
        private dbstaffacc_dbempinfo dbstaffacc_dbempinfo;
        private DormInformationBusiness dbdorm;
        private RoomdeWithPageXmlHelp dbxml;
        private dbacc_dbroomnumber dbaccroomnumber;
        private ProDormInfoViewBusiness dbproDormInfoViewBusiness;
        private TungFloorBusiness dbtungfloor;
        private dbacc_dbben_dbroomnumber_dbdorm dbacc_Dbben_Dbroomnumber_Dbdorm;
        private StaffAccdationBusiness dbstaffacc;
        // GET: Dormitory/StaffAdjustment
        public ActionResult StaffAdjustmentIndex()
        {
            return View();
        }

       /// <summary>
       /// 加载数据表格#table00
       /// </summary>
       /// <param name="page"></param>
       /// <param name="limit"></param>
       /// <param name="string1"></param>
       /// <returns></returns>
        public ActionResult table00(int page, int limit, string string1)
        {
            dbconversion = new ConversionToViewBusiness();

            dbstaffacc_dbempinfo = new dbstaffacc_dbempinfo();

            List<EmployeesInfo> employeesInfos = dbstaffacc_dbempinfo.GetinhabitedData();

            List<RoomArrangeEmpinfoView> resultdata = dbconversion.EmpinfoToRoomArrangeEmpinfoView(employeesInfos, true);

            if (!string.IsNullOrEmpty(string1))
            {
                resultdata= resultdata.Where(a => a.EmpName == string1).ToList();

            }

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
        /// 全部数据
        /// </summary>
        /// <returns></returns>
        public ActionResult loadall() {
            dbconversion = new ConversionToViewBusiness();

            dbstaffacc_dbempinfo = new dbstaffacc_dbempinfo();

            List<EmployeesInfo> employeesInfos = dbstaffacc_dbempinfo.GetinhabitedData();

            List<RoomArrangeEmpinfoView> resultdata = dbconversion.EmpinfoToRoomArrangeEmpinfoView(employeesInfos, true);
            return Json(resultdata,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 加载#table01
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="tungid"></param>
        /// <param name="floorid"></param>
        /// <param name="dormname"></param>
        /// <returns></returns>
        public ActionResult table01(int page, int limit,string dormname)
        {

            dbdorm = new DormInformationBusiness();
           
            dbxml = new RoomdeWithPageXmlHelp();
           

            var list0 = dbdorm.GetStaffJuzhuing();
            if (!string.IsNullOrEmpty(dormname))
            {
                list0 = list0.Where(a => a.DormInfoName == dormname).ToList();
            }
            var resutl0 = list0.Select(a => new
            {
                ID = a.ID,
                SexType = a.SexType,
                DormInfoName = a.DormInfoName
            }).ToList();

            var data = resutl0.OrderByDescending(a => a.ID).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = list0.Count(),
                data = data
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 寝室查询
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="TungID"></param>
        /// <param name="FloorID"></param>
        /// <returns></returns>
        public ActionResult ChoiceInfo(bool sex, int TungID, int FloorID)
        {
            dbdorm = new DormInformationBusiness();
            dbaccroomnumber = new dbacc_dbroomnumber();
            dbxml = new RoomdeWithPageXmlHelp();
            dbtungfloor = new TungFloorBusiness();
            dbproDormInfoViewBusiness = new ProDormInfoViewBusiness();
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                TungFloor querytungfloor = dbtungfloor.GetTungFloorByTungIDAndFloorID(TungID, FloorID);

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
                    maleid = dbxml.Getmale(RoomTypeEnum.SexType.Female);

                    //女寝寝数据
                    data = data.Where(a => a.SexType == maleid).ToList();
                }


                var xmlroomtype = dbxml.GetRoomType(RoomTypeEnum.RoomType.StaffRoom);
                //员工宿舍
                data = data.Where(a => a.RoomStayTypeId == xmlroomtype).ToList();

                var dormInfoViews = dbproDormInfoViewBusiness.dormInfoViewsByStaff(data);
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
        /// 返回房间床位数
        /// </summary>
        /// <param name="DorminfoID"></param>
        /// <returns></returns>
        public ActionResult BedInfo(int DorminfoID, string datatype)
        {
            AjaxResult ajaxResult = new AjaxResult();

            dbacc_Dbben_Dbroomnumber_Dbdorm = new dbacc_dbben_dbroomnumber_dbdorm();
            try
            {
                var querydata = dbacc_Dbben_Dbroomnumber_Dbdorm.GetBensByDorminfoID(DorminfoID);
                BusHelper.WriteSysLog("位于Dormitory/DormitoryInfo/BedInfo", Entity.Base_SysManage.EnumType.LogType.查询数据success);
                ajaxResult.Data = querydata;
                ajaxResult.Success = true;
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message + "位于Dormitory/DormitoryInfo/BedInfo", Entity.Base_SysManage.EnumType.LogType.查询数据error);
                ajaxResult.Data = "";
                ajaxResult.Success = false;
            }

            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
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

             //这个房间的床位号是否有人居住，如果有人居住 需要删除这个原来的居住人员，之后才能进行添加
             var obj = dbstaffacc.GetStaffAccdationsByDorminfoID(DormId).Where(a => a.BedId == BedId).FirstOrDefault();

            if (obj != null)
            {
                obj.IsDel = true;
                obj.EndDate = DateTime.Now;
                dbstaffacc.Update(obj);
            }
            var obj0 = dbstaffacc.GetStaffAccdationsByEmpinfoID(resultdata);

            if (obj0 != null)
            {
                dbstaffacc = new StaffAccdationBusiness();
                //首先将改为居住信息改为false
                obj0.IsDel = true;
                obj0.EndDate = DateTime.Now;
                dbstaffacc.Update(obj0);
                
            }
         
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

    }
}