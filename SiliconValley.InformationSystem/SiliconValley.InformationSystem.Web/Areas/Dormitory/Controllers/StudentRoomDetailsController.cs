using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
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
    public class StudentRoomDetailsController : Controller
    {
        private ConversionToViewBusiness dbconversion;
        private ProStudentInformationBusiness dbprostudent;
        private DormInformationBusiness dbdorm;
        private ProClassSchedule dbproClassSchedule;
        private RoomdeWithPageXmlHelp dbroomxml;
        private ProHeadmaster dbpromaster;
        private ProClassSchedule dbproclass;
        private ProHeadClass dbproheadclass;
        private ProScheduleForTrainees dbprotrainees;
        private EmployeesInfoManage dbempinfo;
        private DormitoryLeaderBusiness dbleader;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        // GET: Dormitory/StudentRoomDetails
        public ActionResult StudentRoomDetailsIndex()
        {
            return View();
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <returns></returns>
        public ActionResult Seachoption(string param0, string param1)
        {
            dbconversion = new ConversionToViewBusiness();
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                switch (param0)
                {
                    case "name0":
                        dbprostudent = new ProStudentInformationBusiness();
                        List<StudentInformation> data = dbprostudent.GetStudentInSchoolData().Where(a => a.Name == param1).ToList();
                        List<ProStudentView> resultstudentdata = dbconversion.StudentInformationToProStudentView(data, true);
                        ajaxResult.Data = resultstudentdata;
                        if (resultstudentdata.Count != 0)
                        {
                            ajaxResult.Success = true;
                        }
                        else
                        {
                            ajaxResult.Success = false;
                            ajaxResult.Msg = "没有该学生的居住信息。";
                        }
                        break;
                    case "name1":
                        dbproClassSchedule = new ProClassSchedule();
                        var obj0  = dbproClassSchedule.GetClassSchedules().Where(a => a.ClassNumber == param1).FirstOrDefault();

                        if (obj0 != null)
                        {
                            dbproScheduleForTrainees = new ProScheduleForTrainees();
                            var list0 = dbproScheduleForTrainees.GetTraineesByClassid(obj0.id);
                            ajaxResult.Data = dbconversion.ScheduleForTraineesToProStudentView(list0, true); ;
                            ajaxResult.Success = true;

                        }
                        else
                        {
                            ajaxResult.Success = false;
                            ajaxResult.Msg = "没有该班级。";
                        }
                        break;
                    case "name2":
                        dbdorm = new DormInformationBusiness();
                        ajaxResult.Data = dbdorm.GetDorms().Where(a => a.DormInfoName == param1).FirstOrDefault();

                        if (ajaxResult.Data != null)
                        {
                            ajaxResult.Success = true;
                        }
                        else
                        {
                            ajaxResult.Success = false;
                            ajaxResult.Msg = "没有该寝室。";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ajaxResult.Data = "";
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！为你及时处理。";
                throw;
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        /// <summary>
        /// 房间详细页面
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentRoomdeWithPage(int DorminfoID)
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

        /// <summary>
        /// 加载学生详细右侧数据信息
        /// </summary>
        /// <param name="Studentnumber"></param>
        /// <returns></returns>
        public ActionResult Loadformdata(string Studentnumber)
        {
            AjaxResult ajaxResult = new AjaxResult();
            dbprostudent = new ProStudentInformationBusiness();
            dbpromaster = new ProHeadmaster();
            dbproclass = new ProClassSchedule();
            dbproheadclass = new ProHeadClass();
            dbprotrainees = new ProScheduleForTrainees();
            dbempinfo = new EmployeesInfoManage();
            try
            {
                ProStudentPageView view = new ProStudentPageView();

                StudentInformation querystudent = dbprostudent.GetStudent(Studentnumber);
                view.StudentNumber = querystudent.StudentNumber;
                view.StudentName = querystudent.Name;
                view.StudentPhone = querystudent.Telephone;
                ScheduleForTrainees querytrainees = dbprotrainees.GetTraineesByStudentNumber(Studentnumber);
                view.ClassNO = querytrainees.ClassID;
                HeadClass queryheadclass = dbproheadclass.GetClassByClassid(querytrainees.ID_ClassName);
                
                if (queryheadclass == null)
                {
                    view.StaffName = "";
                    view.StaffPhone = "";
                }
                else
                {
                    Headmaster querymaster = dbpromaster.GetHeadById(queryheadclass.LeaderID);
                    EmployeesInfo queryempinfo = dbempinfo.GetInfoByEmpID(querymaster.informatiees_Id);
                    view.StaffName = queryempinfo.EmpName;
                    view.StaffPhone = queryempinfo.Phone;
                }
                ajaxResult.Data = view;
                ajaxResult.Success = true;
            }
            catch (Exception ex)
            {
                ajaxResult.Data = "";
                ajaxResult.Success = false;
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

      /// <summary>
      /// 任命寝长
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
        /// <summary>
        /// 搜索名字出现重复的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Studentloadlistwith()
        {
            return View();
        }
    }
}