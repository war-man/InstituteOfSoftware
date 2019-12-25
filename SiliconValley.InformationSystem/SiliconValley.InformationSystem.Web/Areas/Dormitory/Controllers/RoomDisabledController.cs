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
    //[CheckLogin]
    public class RoomDisabledController : Controller
    {
        private DormInformationBusiness dbdorm;
        private TungFloorBusiness dbtungfloor;
        private TungBusiness dbtung;
        private DormitoryfloorBusiness dbfloor;
        // GET: Dormitory/RoomDisabled
        public ActionResult RoomDisabledIndex()
        {
            return View();
        }

        /// <summary>
        /// 数据
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="limit">limint</param>
        /// <returns></returns>
        public ActionResult SeachData(int page, int limit) {
            dbdorm = new DormInformationBusiness();
            dbtungfloor = new TungFloorBusiness();
            dbtung = new TungBusiness();
            dbfloor = new DormitoryfloorBusiness();

            List<Tung> tunglist= dbtung.GetTungs();
            List<TungFloor> tungfoolrlist = new List<TungFloor>();
            List<DormInformation> dormlist = new List<DormInformation>();
            foreach (var item in tunglist)
            {
                tungfoolrlist.AddRange(dbtungfloor.GetTungFloorByTungID(item.Id));
            }
            tungfoolrlist = tungfoolrlist.Where(a => a.IsDel == false).ToList();

            foreach (var item in tungfoolrlist)
            {
                dormlist.AddRange(dbdorm.GetIQueryable().Where(a => a.TungFloorId == item.Id && a.IsDelete == true).ToList()) ;
            }
          
            var resultdata = dormlist.OrderByDescending(a => a.ID).Skip((page - 1) * limit).Take(limit).ToList();

            var tian1 = new List<ProRoomDisabledView>();    
            foreach (var item in resultdata)
            {
                ProRoomDisabledView proRoomDisabledView = new ProRoomDisabledView();
                proRoomDisabledView.ID = item.ID;
                proRoomDisabledView.DormInfoName = item.DormInfoName;
                proRoomDisabledView.ProhibitRemark = item.ProhibitRemark;
                proRoomDisabledView.RoomStayTypeId = item.RoomStayTypeId;
                proRoomDisabledView.SexType = item.SexType;

                var doinb = dbtungfloor.GetEntity(item.TungFloorId);
                var doinb1 = dbtung.GetEntity(doinb.TungId);
                var doinb2 = dbfloor.GetEntity(doinb.FloorId);
                proRoomDisabledView.Address = doinb1.TungName + doinb2.FloorName;
                tian1.Add(proRoomDisabledView);
            }

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = dormlist.Count(),
                data = tian1
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 解禁
        /// </summary>
        /// <param name="param0">房间id</param>
        /// <returns></returns>
        public ActionResult LiftABan(int param0) {
            AjaxResult result = new AjaxResult();
            dbdorm = new DormInformationBusiness();
            try
            {
                var obj = dbdorm.GetEntity(param0);
                obj.IsDelete = false;
                obj.ProhibitRemark = string.Empty;
                dbdorm.Update(obj);
                result.Success = true;
            }
            catch (Exception)
            {

                result.Success = false;
                result.Msg = "请联系信息部成员!";
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }
    }
}