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
    public class StudentBedtimeController : Controller
    {

        private ProStudentInformationViewBusiness dbproStudentInformationViewBusiness;
        private ProClassScheduleViewBusiness dbproClassScheduleViewBusiness;
        private DormInformationBusiness dbdorm;
        private TungFloorBusiness dbtungfloor;
        private RoomdeWithPageXmlHelp dbxml;
        private dbacc_dbroomnumber dbaccroomnumber;
        private DormitoryLeaderBusiness dbleader;
        private ProStudentInformationBusiness dbprostudent;
        private dbacc_dbben_dbroomnumber_dbdorm dbacc_Dbben_Dbroomnumber_Dbdorm;
        private AccdationinformationBusiness dbacc;
        private ProDormInfoViewBusiness dbproDormInfoViewBusiness;
        private dbprosutdent_dbproheadmaster dbprosutdent_Dbproheadmaster;
        // GET: Dormitory/c
        public ActionResult StudentBedtimeIndex()
        {
            return View();
        }

        /// <summary>
        /// 加载#table00
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="classno"></param>
        /// <returns></returns>
        public ActionResult table00(int page, int limit, string classno)
        {
            dbproClassScheduleViewBusiness = new ProClassScheduleViewBusiness();
            dbprosutdent_Dbproheadmaster = new dbprosutdent_dbproheadmaster();
            dbacc = new AccdationinformationBusiness();
            var list0 = dbacc.GetAccdationinformations();
            var param0 = new List<ClassSchedule>();
            foreach (var item in list0)
            {
                param0.Add(dbprosutdent_Dbproheadmaster.GetClassScheduleByStudentNumber(item.Studentnumber));
            }
            if (!string.IsNullOrEmpty(classno))
            {
                param0 = param0.Where(a => a.ClassNumber == classno).ToList();
            }
            var query0 = dbproClassScheduleViewBusiness.Conversion(param0);
            var data = query0.OrderByDescending(a => a.ClassNumber).Skip((page - 1) * limit).Take(limit).ToList();
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
        /// 加载#table01
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="tungid"></param>
        /// <param name="floorid"></param>
        /// <param name="dormname"></param>
        /// <returns></returns>
        public ActionResult table01(int page, int limit, int tungid,int floorid,string dormname) {

            dbdorm = new DormInformationBusiness();
            dbtungfloor = new TungFloorBusiness();
            dbxml = new RoomdeWithPageXmlHelp();
            dbacc = new AccdationinformationBusiness();
            var obj0= dbtungfloor.GetTungFloorByTungIDAndFloorID(tungid, floorid);
            int roomtype = dbxml.GetRoomType(Entity.ViewEntity.RoomTypeEnum.RoomType.StudentRoom);

            var list0= dbdorm.GetDormsByTungFloorIDing(obj0.Id).Where(a=>a.RoomStayTypeId== roomtype).ToList();
            if (!string.IsNullOrEmpty(dormname))
            {
                list0 = list0.Where(a => a.DormInfoName == dormname).ToList();
            }
            for (int i = list0.Count-1; i >=0; i--)
            {
                var querylist0 = dbacc.GetAccdationinformationByDormId(list0[i].ID);
                if (querylist0.Count<1)
                {
                    list0.Remove(list0[i]);
                }
            }
            var resutl0 = list0.Select(a => new
            {
                ID = a.ID,
                SexType = a.SexType,
                DormInfoName=a.DormInfoName
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
        /// 加载#table02
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="tungid"></param>
        /// <param name="floorid"></param>
        /// <param name="name1"></param>
        /// <returns></returns>
        public ActionResult table02(int page, int limit, int tungid, int floorid, string name1) {

            dbproStudentInformationViewBusiness = new ProStudentInformationViewBusiness();
            dbdorm = new DormInformationBusiness();
            dbtungfloor = new TungFloorBusiness();
            dbxml = new RoomdeWithPageXmlHelp();
            var obj0 = dbtungfloor.GetTungFloorByTungIDAndFloorID(tungid, floorid);
            int roomtype = dbxml.GetRoomType(Entity.ViewEntity.RoomTypeEnum.RoomType.StudentRoom);
            var list0 = dbdorm.GetDormsByTungFloorIDing(obj0.Id).Where(a => a.RoomStayTypeId == roomtype).ToList();
            var resutl0=dbproStudentInformationViewBusiness.GetProBedtimeStudentsViews(list0,name1);
            var data = resutl0.OrderByDescending(a => a.StudentNmber).Skip((page - 1) * limit).Take(limit).ToList();
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
        /// 楼层全部学生数据
        /// </summary>
        /// <param name="tungid"></param>
        /// <param name="floorid"></param>
        /// <returns></returns>
        public ActionResult loadall(int tungid, int floorid) {
            dbproStudentInformationViewBusiness = new ProStudentInformationViewBusiness();
            dbdorm = new DormInformationBusiness();
            dbtungfloor = new TungFloorBusiness();
            dbxml = new RoomdeWithPageXmlHelp();
            var obj0 = dbtungfloor.GetTungFloorByTungIDAndFloorID(tungid, floorid);
            int roomtype = dbxml.GetRoomType(Entity.ViewEntity.RoomTypeEnum.RoomType.StudentRoom);
            var list0 = dbdorm.GetDormsByTungFloorIDing(obj0.Id).Where(a => a.RoomStayTypeId == roomtype).ToList();
            var resutl0 = dbproStudentInformationViewBusiness.GetProBedtimeStudentsViews(list0, string.Empty);
            return Json(resutl0,JsonRequestBehavior.AllowGet);
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


                var xmlroomtype = dbxml.GetRoomType(RoomTypeEnum.RoomType.StudentRoom);
                //学生宿舍
                data = data.Where(a => a.RoomStayTypeId == xmlroomtype).ToList();

                var dormInfoViews = dbproDormInfoViewBusiness.dormInfoViewsByStudent(data);
                ajaxResult.Data = dormInfoViews;
                ajaxResult.Success = true;
                BusHelper.WriteSysLog("查询学生寝室数据Dormitory/DormitoryInfo/ChoiceInfo", Entity.Base_SysManage.EnumType.LogType.查询数据success);
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
            dbacc = new AccdationinformationBusiness();

            //这个房间的床位号是否有人居住，如果有人居住 需要删除这个原来的居住人员，之后才能进行添加
           var obj=  dbacc.GetAccdationinformationByDormId(DormId).Where(a => a.BedId == BedId).FirstOrDefault();
            if (obj!=null)
            {
                obj.IsDel = true;
                obj.EndDate = DateTime.Now;
                dbacc.Update(obj);
                //如果是寝室长。将职位去除掉
                dbleader = new DormitoryLeaderBusiness();
                var obj1 = dbleader.GetLeaderByStudentNumber(obj.Studentnumber);
                if (obj1 != null)
                {
                    dbleader.Cancellation(obj1);
                }
            }
            var obj0 = dbacc.GetAccdationByStudentNumber(resultdata);
            if (obj0 != null)
            {
                dbacc = new AccdationinformationBusiness();
                //首先将改为居住信息改为false
                obj0.IsDel = true;
                obj0.EndDate = DateTime.Now;
                dbacc.Update(obj0);
                dbleader = new DormitoryLeaderBusiness();
                var obj1 = dbleader.GetLeaderByStudentNumber(resultdata);
                if (obj1 != null)
                {
                    dbleader.Cancellation(obj1);
                }
                //如果是寝室长。将职位去除掉
            }
            dbacc = new AccdationinformationBusiness();
            Accdationinformation accdationinformation = new Accdationinformation();
            accdationinformation.CreationTime = DateTime.Now;
            accdationinformation.IsDel = false;
            accdationinformation.Remark = string.Empty;
            accdationinformation.StayDate = DateTime.Now;
            accdationinformation.BedId = BedId;
            accdationinformation.DormId = DormId;
            accdationinformation.Studentnumber = resultdata;
            ajaxResult.Success = dbacc.AddAcc(accdationinformation);


            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
    }
}