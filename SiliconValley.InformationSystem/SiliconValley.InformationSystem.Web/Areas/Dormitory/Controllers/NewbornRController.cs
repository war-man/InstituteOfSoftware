using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.Psychro;
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
    /// 新生入住控制器
    /// </summary>
    public class NewbornRController : Controller
    {
        private ConversionToViewBusiness dbconversion;

        private dbacc_dbstu dbaccstu;

        private AccdationinformationBusiness dbacc;

        private DormitoryLeaderBusiness dbleader;

        private DormInformationBusiness dbdorm;


        private dbacc_dbroomnumber dbaccroomnumber;

        private RoomdeWithPageXmlHelp dbroomxml;

        private TungFloorBusiness dbtungfloor;

        private ProStudentInformationBusiness dbprostudent;

        // GET: Dormitory/NewbornR
        public ActionResult NewbornRIndex()
        {
            return View();
        }


        /// <summary>
        /// 用于加载未居住的学生
        /// </summary>
        /// <returns></returns>
        public ActionResult UninhabitedList(int page, int limit)
        {
            dbconversion = new ConversionToViewBusiness();
            List<ProStudentView> resultdata = new List<ProStudentView>();

            dbaccstu = new dbacc_dbstu();
            List<StudentInformation> data = dbaccstu.GetUninhabitedData();

            var data2 = dbconversion.StudentInformationToProStudentView(data, false);

            var resultdata1 = data2.OrderByDescending(a => a.StudentNumber).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = resultdata.Count(),
                data = resultdata1
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);

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


                var xmlroomtype = dbroomxml.GetRoomType(RoomTypeEnum.RoomType.StudentRoom);
                //学生宿舍
                data = data.Where(a => a.RoomStayTypeId == xmlroomtype).ToList();


                foreach (var item in data)
                {
                    if (!dbaccroomnumber.IsFull(item.ID, item.RoomStayNumberId))
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
            dbleader = new DormitoryLeaderBusiness();
            dbprostudent = new ProStudentInformationBusiness();
            List<DormInfoView> dormInfoViews = new List<DormInfoView>();
            foreach (var item in data)
            {
                DormInfoView myview = new DormInfoView();
                myview.ID = item.ID;
                myview.DormInfoName = item.DormInfoName;

                var queryleader = dbleader.GetLeader(item.ID);
                if (queryleader == null)
                {
                    myview.StudentName = "暂定";
                }
                else
                {
                    var querystudent = dbprostudent.GetStudent(queryleader.StudentNumber);
                    myview.StudentName = querystudent.Name;
                }

                dormInfoViews.Add(myview);
            }
            return dormInfoViews;
        }

        /// <summary>
        /// 添加寝室长
        /// </summary>
        /// <param name="StudentNumber"></param>
        /// <param name="DormInfoID"></param>
        /// <returns></returns>
        public ActionResult AddLeader(string StudentNumber, int DormInfoID)
        {
            dbleader = new DormitoryLeaderBusiness();
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                DormitoryLeader queryleader = dbleader.GetLeader(DormInfoID);
                if (queryleader != null)
                {
                    if (dbleader.Cancellation(queryleader))
                    {
                        ajaxResult.Data = "";
                        ajaxResult.Success = true;
                    }
                    else
                    {
                        ajaxResult.Data = "";
                        ajaxResult.Success = false;
                    }
                }
                DormitoryLeader dormitoryLeader = new DormitoryLeader();
                dormitoryLeader.CreationTime = DateTime.Now;
                dormitoryLeader.DormInfoID = DormInfoID;
                dormitoryLeader.IsDelete = false;
                dormitoryLeader.StudentNumber = StudentNumber;
                if (dbleader.AddLeader(dormitoryLeader))
                {
                    ajaxResult.Data = "";
                    ajaxResult.Success = true;
                }
                else
                {
                    ajaxResult.Data = "";
                    ajaxResult.Success = false;
                }

            }
            catch (Exception ex)
            {

                ajaxResult.Data = "";
                ajaxResult.Success = false;
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }



    }
}