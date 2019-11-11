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
    /// 房间基础信息
    /// </summary>
    public class RoomBasisController : Controller
    {
        private RoomStayTypeBusiness dbroomtype;
        private RoomStayNumberBusiness dbroomnumber;
        private DormInformationBusiness dbdorm;
        private RoomdeWithPageXmlHelp dbroomxml;
        private TungBusiness dbtung;
        private AccdationinformationBusiness dbacc;
        private TungFloorBusiness dbtungfloor;
        private StaffAccdationBusiness dbstaffacc;
        // GET: Dormitory/RoomBasis
        public ActionResult RoomBasisIndex()
        {
            return View();
        }
        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="Direction"></param>
        /// <param name="Floorid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddRoomPage()
        {
            dbroomtype = new RoomStayTypeBusiness();
            dbroomnumber = new RoomStayNumberBusiness();
            var data = dbroomtype.GetRoomStayTypes();
            var SelectListItemlist = data.Select(a => new SelectListItem()
            {
                Text = a.RoomStayTypeName,
                Value = a.Id.ToString()
            });
            ViewBag.SelectListItemlist = SelectListItemlist;

            var RoomNumber = dbroomnumber.GetRoomStayNumbers().Select(a => new SelectListItem()
            {
                Text = a.StayNumber + "人制",
                Value = a.Id.ToString()
            });
            ViewBag.RoomNumber = RoomNumber;
            return View();
        }
        [HttpPost]
        /// <summary>
        /// post 添加
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public ActionResult AddRoomPage(DormInformation jsonStr, int FloorId, int TungId)
        {
            AjaxResult result = new AjaxResult();
            dbdorm = new DormInformationBusiness();
            dbroomxml = new RoomdeWithPageXmlHelp();
            dbtung = new TungBusiness();
            dbtungfloor = new TungFloorBusiness();
            try
            {
                TungFloor querytungfloor = dbtungfloor.GetTungFloorByTungIDAndFloorID(TungId, FloorId);

                //根据这个楼层来说房间号是不能重复的。
                List<DormInformation> querydormlist = dbdorm.GetDormsByTungFloorID(querytungfloor.Id).ToList();
                DormInformation querydorm = querydormlist.Where(a => a.DormInfoName == jsonStr.DormInfoName).FirstOrDefault();
                if (querydormlist.Count != 0 && querydorm != null)
                {
                    result.Msg = "不可添加重复房间号";
                    result.Success = false;
                }
                else
                {
                    jsonStr.TungFloorId = querytungfloor.Id;
                    //仓库
                    if (jsonStr.RoomStayTypeId == dbroomxml.GetRoomType(RoomTypeEnum.RoomType.Warehouse))
                    {
                        jsonStr.RoomStayNumberId = 0;
                    }
                    if (jsonStr.RoomStayTypeId == dbroomxml.GetRoomType(RoomTypeEnum.RoomType.StudentRoom) || jsonStr.RoomStayTypeId == dbroomxml.GetRoomType(RoomTypeEnum.RoomType.StaffRoom))
                    {

                    }
                    else
                    {
                        jsonStr.SexType = 0;
                    }
                    jsonStr.CreationTime = DateTime.Now;

                    dbdorm.Insert(jsonStr);
                    BusHelper.WriteSysLog("添加数据位置于Dormitory/DormitoryInfo/AddTungPage", Entity.Base_SysManage.EnumType.LogType.添加数据);
                    result.Msg = "添加成功";
                    result.Success = true;


                }

            }
            catch (Exception ex)
            {

                result.Msg = "请及时联系信息部，为你即时解决问题。";
                result.Success = false;
            }




            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 禁用该房间
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult DelRoom(int param0, string param1) {
            AjaxResult ajaxResult = new AjaxResult();
            dbacc = new AccdationinformationBusiness();
            dbdorm = new DormInformationBusiness();
            dbroomxml = new RoomdeWithPageXmlHelp();
            var fpxnb = dbroomxml.GetRoomType(RoomTypeEnum.RoomType.StaffRoom);
            var fpxnb1 = dbroomxml.GetRoomType(RoomTypeEnum.RoomType.StudentRoom);

            var entity0 = dbdorm.GetEntity(param0);
            bool Candelete = true;
            if (entity0.RoomStayTypeId==fpxnb)
            {
                dbstaffacc = new StaffAccdationBusiness();
               var fpxnb3= dbstaffacc.GetStaffAccdationsByDorminfoID(param0);
                if (fpxnb3.Count!=0)
                {
                    Candelete = false;
                }
            }
            if (entity0.RoomStayTypeId==fpxnb1)
            {
                var fpx4 = dbacc.GetAccdationinformationByDormId(param0);
                if (fpx4.Count != 0)
                {
                    Candelete = false;
                }
            }

            if (!Candelete)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "该房间还有人员居住，</br>请将居住人员撤离再执行！";
            }
            else
            {
                try
                {
                    
                    entity0.IsDelete = true;
                    entity0.ProhibitRemark = param1;
                    dbdorm.Update(entity0);
                    ajaxResult.Success = true;
                    ajaxResult.Msg = "操作成功！";
                }
                catch (Exception)
                {

                    ajaxResult.Success = false;
                    ajaxResult.Msg = "请及时联系信息部成员！";
                }
            }
            return Json(ajaxResult,JsonRequestBehavior.AllowGet);
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult HttpGetUpdRoom(int param0) {
            return View();
        }

     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult HttpPostUpdRoom(DormInformation param0)
        {
            return View();
        }
    }
}