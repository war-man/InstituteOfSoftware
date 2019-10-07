using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Dormitory.Controllers
{

    /// <summary>
    /// 宿舍管理控制器
    /// </summary>
    public class DormitoryInfoController : Controller
    {
        /// <summary>
        /// 栋楼业务类
        /// </summary>
        private TungFloorBusiness dbtungfloor;
        /// <summary>
        /// 栋业务类
        /// </summary>
        private TungBusiness dbtung;

        /// <summary>
        /// 楼层业务类
        /// </summary>
        private DormitoryfloorBusiness dbfloor;

        /// <summary>
        ///宿舍房间业务类
        /// </summary>
        private DormInformationBusiness dbdorm;

        // GET: Dormitory/DormitoryInfo

        /// <summary>
        /// 主页面。
        /// </summary>
        /// <returns></returns>
        public ActionResult DormitoryIndex()
        {
            return View();
        }

        /// <summary>
        /// 创建左侧的树形菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult EstablishTree()
        {
            dbtung = new TungBusiness();
            dbfloor = new DormitoryfloorBusiness();
            dbtungfloor = new TungFloorBusiness();
            //获取父级对象楼
            List<Tung> tunglist = dbtung.GetTungs();

            List<TreeClass> resulttree = new List<TreeClass>();

            foreach (var item in tunglist)
            {
                TreeClass tungtree = new TreeClass();
                Tung fortung = dbtung.GetTungByTungID(item.Id);
                tungtree.title = fortung.TungName;
                tungtree.id = fortung.Id.ToString();
                tungtree.spread = true;
                List<TungFloor> floorlist = dbtungfloor.GetTungFloorByTungID(item.Id);

                List<TreeClass> floortreelist = new List<TreeClass>();
                foreach (var floor in floorlist)
                {
                    var floorobj = dbfloor.GetDormitoryfloorByFloorID(floor.FloorId);
                    TreeClass floortree = new TreeClass();
                    floortree.title = floorobj.FloorName;
                    floortree.id = floorobj.ID.ToString();
                    floortree.spread = true;
                    floortreelist.Add(floortree);
                }
                tungtree.children = floortreelist;
                resulttree.Add(tungtree);
            }

            return Json(resulttree, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///根据 点击那个树形菜单产生房间数据 
        /// </summary>
        /// <param name="TungID"></param>
        /// <param name="FloorID"></param>
        /// <returns></returns>
        public ActionResult EstablishRoom(int? TungID, int? FloorID)
        {
            dbdorm = new DormInformationBusiness();
            dbtungfloor = new TungFloorBusiness();
            TungFloor resulttungfloorobj = new TungFloor();
            if (string.IsNullOrEmpty(TungID.ToString())&&string.IsNullOrEmpty(FloorID.ToString()))
            {
                resulttungfloorobj= dbtungfloor.GetTungFloors().FirstOrDefault();
            }
            else
            {
                resulttungfloorobj= dbtungfloor.GetTungFloorByTungIDAndFloorID(TungID, FloorID);
            }
            
            List<DormInformation> dormlist = dbdorm.GetDormsByTungFloorID(resulttungfloorobj.Id);
            List<DormitoryView> result = new List<DormitoryView>();
            foreach (var item in dormlist)
            {
                DormitoryView view = new DormitoryView();
                view.Direction = item.Direction;
                view.DormInfoName = item.DormInfoName;
                view.ID = item.ID;
                view.RoomStayNumberId = item.RoomStayNumberId;
                view.RoomStayTypeId = item.RoomStayTypeId;
                view.SexType = item.SexType;
                result.Add(view);
            }
            Thread.Sleep(1000);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult test() {
            return View();
        }
    }
}