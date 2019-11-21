using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
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
    }
}