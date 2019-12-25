using SiliconValley.InformationSystem.Business.Base_SysManage;
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
    [CheckLogin]
    /// <summary>
    /// 寝室卫生登记
    /// </summary>
    public class HealthRegistrationController : Controller
    {

        private HealthRegistrationtableviewBusiness dbhealthRegistrationtableview;

        private DormInformationBusiness dbdorm;

        private dbacc_dbroomnumber dbaccroomnumber;
        private TungFloorBusiness dbtungfloor;
        private RoomdeWithPageXmlHelp dbroomxml;
        private AccdationinformationBusiness dbacc;

        private dbprosutdent_dbproheadmaster dbprosutdent_Dbproheadmaster;

        private DormitoryhygieneBusiness dbdormhygiene;

        private HygienicDeductionBusiness dbhygieneduction;
        private InstructorListBusiness dbinstructorList;
        
        // GET: Dormitory/HealthRegistration
        public ActionResult HealthRegistrationIndex()
        {
            return View();
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="limit">显示个数</param>
        /// <param name="startime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="tungid">栋id</param>
        /// <param name="floorid">楼层id</param>
        /// <returns></returns>
        public ActionResult ListOfHealthViolations(int page, int limit,DateTime startime,DateTime endtime,int tungid,int floorid) {
            dbtungfloor = new TungFloorBusiness();
            TungFloor querytungfloor= dbtungfloor.GetTungFloorByTungIDAndFloorID(tungid, floorid);
            dbhealthRegistrationtableview = new HealthRegistrationtableviewBusiness();
            List<HealthRegistrationtableview> querylist = dbhealthRegistrationtableview.GetHealthRegistrationtableviews(querytungfloor.Id, startime, endtime);

            var data = querylist.OrderByDescending(a => a.RecordTime).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = querylist.Count(),
                data = data
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 房间列表
        /// </summary>
        /// <param name="TungID"></param>
        /// <param name="FloorID"></param>
        /// <returns></returns>
        public ActionResult getdormdata(int TungID, int FloorID) {
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
                var xmlroomtype = dbroomxml.GetRoomType(RoomTypeEnum.RoomType.StudentRoom);
                //学生宿舍
                data = data.Where(a => a.RoomStayTypeId == xmlroomtype).ToList();


                foreach (var item in data)
                {
                    if (dbaccroomnumber.Somepeoplelivein(item.ID))
                    {
                        dormlist.Add(item);
                    }
                }
                var dormInfoViews = this.dormInfoViews(dormlist);
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

        public ActionResult Healthregistration(Dormitoryhygiene dormitoryhygiene)
        {
            AjaxResult ajaxResult = new AjaxResult();
            dbacc = new AccdationinformationBusiness();
            dbdorm = new DormInformationBusiness();
            dbprosutdent_Dbproheadmaster = new dbprosutdent_dbproheadmaster();
            dbdormhygiene = new DormitoryhygieneBusiness();
            dbhygieneduction = new HygienicDeductionBusiness();
            dbinstructorList = new InstructorListBusiness();
            int DorminfoID = dormitoryhygiene.DorminfoID;
            List<Accdationinformation> queryacclist = dbacc.GetAccdationinformationByDormId(DorminfoID);
            List<Headmaster> backlist = new List<Headmaster>();
            try
            {
                //现在就使用1 号 教官 王涛
                dormitoryhygiene.IsDel = false;
                dormitoryhygiene.Addtime = DateTime.Now;

                Base_UserModel user = Base_UserBusiness.GetCurrentUser();
                var query= dbinstructorList.GetInstructorByempid(user.EmpNumber);
                dormitoryhygiene.Inspector = query.ID;

                string now = dormitoryhygiene.Addtime.ToString();

                if (dbdormhygiene.AddDormitoryhygiene(dormitoryhygiene))
                {
                    Dormitoryhygiene Theprevious = dbdormhygiene.GetDormitoryhygienes().Where(a => a.Addtime.ToString() == now).FirstOrDefault();

                    if (Theprevious.Beddebris || Theprevious.BeddingOverlay || Theprevious.Cleantoilet || Theprevious.Clothing || Theprevious.Emptyplacement || Theprevious.Ground || Theprevious.Sheet || Theprevious.Shoeplacement || Theprevious.Trunk || Theprevious.Washsupplies)
                    {
                        foreach (var item in queryacclist)
                        {
                            var header = dbprosutdent_Dbproheadmaster.GetHeadmasterByStudentNumber(item.Studentnumber);
                            if (header != null)
                            {
                                backlist.Add(header);
                            }
                           
                        }

                        for (int i = 0; i < backlist.Count; i++)  //外循环是循环的次数
                        {
                            for (int j = backlist.Count - 1; j > i; j--)  //内循环是 外循环一次比较的次数
                            {

                                if (backlist[i].ID == backlist[j].ID)
                                {
                                    backlist.RemoveAt(j);
                                }

                            }
                        }

                        foreach (var item in backlist)
                        {
                            HygienicDeduction baby = new HygienicDeduction();
                            baby.IsDel = false;
                            baby.CreationTime = DateTime.Now;
                            baby.DormitoryhygieneID = Theprevious.ID;
                            baby.Headmaster = item.ID;
                            baby.Remark = string.Empty;

                            if (dbhygieneduction.AddHygienicDeduction(baby))
                            {
                                ajaxResult.Success = true;
                                ajaxResult.Data = "";
                            }
                        }
                    }

                }
                else
                {
                    ajaxResult.Success = true;
                    ajaxResult.Data = "";
                }


            }
            catch (Exception ex)
            {

                ajaxResult.Success = false;
                ajaxResult.Data = "";
            }

            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
    }
}