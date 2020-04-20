using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational
{
    [CheckLogin]
    /// <summary>
    /// 课表控制器
    /// </summary>
    public class TimetableController : Controller
    {
        // GET: /Educational/Timetable/TimeTableIndex
         
       
        BaseDataEnumManeger BaseDataEnum_Entity ;
        ClassroomManeger Classroom_Entity;
        TimeTableManeger TimeTable;   
        
        public ActionResult TimeTableIndex()
        {
            //获取所有校区
            BaseDataEnum_Entity = new BaseDataEnumManeger();
            ViewBag.Addree= BaseDataEnum_Entity.GetsameFartherData("校区地址");
            return View();
        }

        [HttpPost]
        public ActionResult GetReconcileData()
        {
            DateTime time = Convert.ToDateTime(Request.Form["SelectTime"]);//获取日期
            int classid = Convert.ToInt32(Request.Form["campus"]);//获取校区
            TimeTable = new TimeTableManeger();
            Classroom_Entity = new ClassroomManeger();
            //获取选择校区的所有教室
            List<Classroom> c_list = Classroom_Entity.GetAddreeClassRoom(classid);
            List<SelectListItem> tabledata = c_list.Select(c => new SelectListItem() { Text = c.ClassroomName, Value = c.Id.ToString() }).ToList();
            //获取教室的上午班级上课情况
            List<AnPaiData> mongingOne = TimeTable.GetPaiDatas(time, "上午12节", c_list);
            List<AnPaiData> mongingTwo = TimeTable.GetPaiDatas(time, "上午34节", c_list);
            //下午
            List<AnPaiData> afternoonOne = TimeTable.GetPaiDatas(time, "下午12节", c_list);
            List<AnPaiData> afternoonTwo = TimeTable.GetPaiDatas(time, "下午34节", c_list);
            //晚自习
            List<AnPaiData> ngintone = ReconcileManeger.EvningSelfStudy_Entity.getAppoint("晚一", time, c_list);
            List<AnPaiData> nginttwo = ReconcileManeger.EvningSelfStudy_Entity.getAppoint("晚二", time, c_list);
            var datajson = new { tablethead = tabledata, MymongingOne = mongingOne, MymongingTwo = mongingTwo, MyafternoonOne = afternoonOne, MyafternoonTwo = afternoonTwo, MyngintOne = ngintone, MyngintTwo = nginttwo };
            return Json(datajson, JsonRequestBehavior.AllowGet);
        }
    }
}