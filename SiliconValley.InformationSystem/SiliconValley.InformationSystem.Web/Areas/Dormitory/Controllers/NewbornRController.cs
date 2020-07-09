using SiliconValley.InformationSystem.Business;
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
    [CheckLogin]
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

        public ActionResult NewBornrView(string studentNumber)
        {
            dbconversion = new ConversionToViewBusiness();

            dbaccstu = new dbacc_dbstu();

            var data = dbaccstu.GetUninhabitedData().Where(d => d.StudentNumber == studentNumber).ToList();

            //var result = dbconversion.StudentInformationToProStudentView(data, false).FirstOrDefault();

            BaseBusiness<StudentInformation> dbstu = new BaseBusiness<StudentInformation>();

            var result = dbstu.GetEntity(studentNumber);
            
            ViewBag.student = result;

            return View();
        }

        /// <summary>
        /// 学生居住信息
        /// </summary>
        /// <param name="studentNumber"></param>
        /// <returns></returns>
        public ActionResult GetDongByStudent(string studentNumber)
        {
            AjaxResult result = new AjaxResult();


            try
            {
                BaseBusiness<Tung> dbtung = new BaseBusiness<Tung>();
                BaseBusiness<Dormitoryfloor> dbfloor = new BaseBusiness<Dormitoryfloor>();
                BaseBusiness<TungFloor> dbtungfloor = new BaseBusiness<TungFloor>();
                BaseBusiness<BenNumber> dbbed = new BaseBusiness<BenNumber>();
                dbacc = new AccdationinformationBusiness();
                var TungFloor = dbacc.GetDormBystudentno(studentNumber);
                
                if(TungFloor == null)
                {
                    var tempdata = new
                    {
                        TungInfo = "无",
                        FloorInfo = "无",
                        RoomInfo = "无",
                        BedInfo = "无",

                    };

                    result.ErrorCode = 200;
                    result.Data = tempdata;
                    result.Msg = "成功";

                    return Json(result, JsonRequestBehavior.AllowGet);

                }

                var roomNumber = TungFloor.DormInfoName;//房间号码
                var Tungfloorobj = dbtungfloor.GetList().Where(d => d.Id == TungFloor.TungFloorId).FirstOrDefault();
                var tungobj = dbtung.GetList().Where(d => d.Id == Tungfloorobj.TungId).FirstOrDefault(); //栋对象
                var floorobj = dbfloor.GetList().Where(d => d.ID == Tungfloorobj.FloorId).FirstOrDefault();//楼层对象
                var accdation = dbacc.GetAccdationByStudentNumber(studentNumber);
                var bedName = dbbed.GetList().Where(d => d.Id == accdation.BedId).FirstOrDefault().BenNo;//床位号

                var data = new
                {
                    TungInfo = tungobj.TungName,
                    FloorInfo = floorobj.FloorName,
                    RoomInfo = roomNumber,
                    BedInfo = bedName

                };

                result.ErrorCode = 200;
                result.Data = data;
                result.Msg = "成功";

            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = null;
                result.Msg = "失败";

            }

            return Json(result, JsonRequestBehavior.AllowGet);

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

            var resultdata1 = data.OrderByDescending(a => a.StudentNumber).Skip((page - 1) * limit).Take(limit).ToList();

            var data2 = dbconversion.StudentInformationToProStudentView(resultdata1, false);
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = data.Count(),
                data = data2
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SearchUninhabitedList(string studentName, string studentNumber, int limit, int page)
        {
            dbconversion = new ConversionToViewBusiness();
            dbaccstu = new dbacc_dbstu();
            List<StudentInformation> filterData = new List<StudentInformation>();

            List<StudentInformation> data = new List<StudentInformation>();

            var list = dbaccstu.GetUninhabitedData();

            if (list != null) data.AddRange(list);


            if (string.IsNullOrEmpty(studentName) && !string.IsNullOrEmpty(studentNumber))
            {
                //根据编号筛选
                var tempobj = data.Where(d => d.StudentNumber == studentNumber).FirstOrDefault();

                if (tempobj != null) filterData.Add(tempobj);

            }

            if (!string.IsNullOrEmpty(studentName) && string.IsNullOrEmpty(studentNumber))
            {
                var tempobjlist = data.Where(d => d.Name.Contains(studentName)).ToList();

                if (tempobjlist != null) filterData.AddRange(tempobjlist);
            }

            if (string.IsNullOrEmpty(studentName) && string.IsNullOrEmpty(studentNumber))
            {
                filterData.AddRange(data)
            }
            //进行分页
            List<StudentInformation> pagedata = filterData.Skip((page - 1) * limit).Take(limit).ToList();

            //进行 视图转换

            var data2 = dbconversion.StudentInformationToProStudentView(pagedata, false);
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = filterData.Count(),
                data = data2
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