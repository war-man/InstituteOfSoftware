using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Util;
using System.Text;
using SiliconValley.InformationSystem.Entity.ViewEntity.TM_Data.MyViewEntity;

namespace SiliconValley.InformationSystem.Web.Areas.Educational
{
    [CheckLogin]
    /// <summary>
    /// 课表控制器
    /// </summary>
    public class TimetableController : Controller
    {
        // GET: /Educational/Timetable/TeacherFunction
        TimeTableManeger TimeTable = new TimeTableManeger();
        
        BaseDataEnumManeger BaseDataEnum_Entity;
        ClassroomManeger Classroom_Entity;
        
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
            List<AnPaiData> ngintone = TimeTable.GetPaiDatas(time,"晚一", c_list);
            List<AnPaiData> nginttwo = TimeTable.GetPaiDatas(time,"晚二", c_list);
            var datajson = new { tablethead = tabledata, MymongingOne = mongingOne, MymongingTwo = mongingTwo, MyafternoonOne = afternoonOne, MyafternoonTwo = afternoonTwo, MyngintOne = ngintone, MyngintTwo = nginttwo };
            return Json(datajson, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 让用户选择要编辑的班级
        /// </summary>
        /// <returns></returns>
        public ActionResult TiShiView(string id)
        {
            string[] rid = id.Split(',');
            List<AnPaiData> data= TimeTable.GetClassName(rid);
            ViewBag.data = data;
            return View();
        }


        public ActionResult TishEvningView(string id)
        {
            List<AnPaiData> list = TimeTable.GetEvningClassName(id.Split(','));
            ViewBag.list = list;
            return View();
        }

        /// <summary>
        /// 教员晚自习值班表
        /// </summary>
        /// <returns></returns>
        public ActionResult TeacherTable()
        {
            //获取所有校区
            BaseDataEnum_Entity = new BaseDataEnumManeger();
            ViewBag.Addree = BaseDataEnum_Entity.GetsameFartherData("校区地址");
            return View();
        }
        /// <summary>
        /// 教员值班操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TeacherFunction()
        {
            AjaxResult a = new AjaxResult();
            string[] ids=  Request.Form["id"].Split(',');
            TimeTable.EvningSelfStudy_Entity = new EvningSelfStudyManeger();
            TimeTable.Reconcile_Entity = new ReconcileManeger();

            List<EvningSelfStudyView> evningSelfStudies = new List<EvningSelfStudyView>();
            foreach (string item in ids)
            {               
                if (!string.IsNullOrEmpty(item))
                {
                    int id = Convert.ToInt32(item);
                    evningSelfStudies.Add(TimeTable.EvningSelfStudy_Entity.FindIdView(id));
                }
            }
                        
            Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();  //获取当前登录人
            //判断这个老师是否带这个班
            StringBuilder sb = new StringBuilder();

            List<EvningSelfStudyView> list1 = new List<EvningSelfStudyView>();//存放带班的数据

            for (int i = 0; i < evningSelfStudies.Count; i++)
            {
                string sql = "select * from Reconcile where EmployeesInfo_Id='"+UserName.EmpNumber+ "' and ClassSchedule_Id ="+evningSelfStudies[i].ClassSchedule_id+ " and AnPaiDate='"+ evningSelfStudies[i].Anpaidate + "'";
                int count = TimeTable.Reconcile_Entity.GetListBySql<Reconcile>(sql).Count();
                if (count <= 0)
                {
                    sb.Append(evningSelfStudies[i].ClassNumber+",");
                    
                }
                else
                {
                    list1.Add(evningSelfStudies[i]);
                }                 
            }

            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                sb.Append("不是你带的班级，不能安排值班！！");
                a.Msg = sb.ToString();
                a.Success = false;
            }
            if (list1.Count>0)
            {
                List<EvningSelfStudy> evnings = new List<EvningSelfStudy>();
                foreach (EvningSelfStudyView c in list1)
                {
                    if (c.emp_id==null)
                    {
                        EvningSelfStudy study = new EvningSelfStudy();
                        study.id = c.id;
                        study.ClassSchedule_id = c.ClassSchedule_id;
                        study.Classroom_id = c.Classroom_id;
                        study.curd_name = c.curd_name;
                        study.Anpaidate = c.Anpaidate;
                        study.Newdate = c.Newdate;
                        study.emp_id = UserName.EmpNumber;
                        study.IsDelete = c.IsDelete;
                        evnings.Add(study);
                    }
                    
                }
                if (evnings.Count>0)
                {
                    a = TimeTable.EvningSelfStudy_Entity.Update_Data(evnings);
                    if (a.Success)
                    {
                        List<TeacherNight> teacherlist = new List<TeacherNight>();
                        //添加值班数据
                        foreach (EvningSelfStudy find_evn in evnings)
                        {
                            TeacherNight teacher = new TeacherNight();
                            teacher.AttendDate = DateTime.Now;
                            teacher.ClassSchedule_Id = find_evn.ClassSchedule_id;
                            teacher.ClassRoom_id = find_evn.Classroom_id;
                            teacher.IsDelete = false;
                            teacher.BeOnDuty_Id = 1;
                            teacher.OrwatchDate = find_evn.Anpaidate;
                            teacher.Tearcher_Id = UserName.EmpNumber;
                            teacher.timename = find_evn.curd_name;
                            teacherlist.Add(teacher);
                        }
                        //判断该数据是否已存在
                        List<TeacherNight> teacherlist2 = new List<TeacherNight>();
                        foreach (TeacherNight it in teacherlist)
                        {
                            if (!TimeTable.Teacher_Entity.Exits(it.OrwatchDate, it.timename, Convert.ToInt32(it.ClassSchedule_Id)).Success)
                            {
                                teacherlist2.Add(it);
                            }
                        }
                        if (teacherlist2.Count > 0)
                        {
                            a = TimeTable.Teacher_Entity.Add_data(teacherlist2);
                        }

                    }
                }
                else
                {
                    a.Msg = "值班数据已安排！";
                    a.Success = false;
                    return Json(a,JsonRequestBehavior.AllowGet);
                }    
                
                
                 
            }

            if (a.Success && !string.IsNullOrEmpty(sb.ToString()))
            {
                a.Msg = sb.ToString();
            }
            return Json(a,JsonRequestBehavior.AllowGet); ;
        }

    }
}