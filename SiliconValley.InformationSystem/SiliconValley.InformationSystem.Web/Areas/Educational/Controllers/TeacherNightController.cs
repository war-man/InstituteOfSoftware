using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.DepartmentBusiness;
using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.PositionBusiness;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    [CheckLogin]
    public class TeacherNightController : Controller
    {
        // GET: /Educational/TeacherNight/GetClassData

        TeacherNightManeger TeacherNight_Entity = new TeacherNightManeger();
        TeacherBusiness Teacher_Entity;
        //ClassroomManeger Classoom_Entity;
        BeOnDutyManeger beOnDuty_Entity = new BeOnDutyManeger(); //获取教员晚自习
        #region  教员晚自习值班
        public ActionResult TeacherNightViewIndex()
        {
            return View();
        }

        public ActionResult TeacherTableData(int page, int limit)
        {
            int id = beOnDuty_Entity.GetSingleBeOnButy("教员晚自习", false).Id;
            TeacherNight_Entity = new TeacherNightManeger();
            List<TeacherNight> getall = TeacherNight_Entity.GetAllTeacherNight().Where(t => t.BeOnDuty_Id == id).OrderByDescending(t => t.Id).ToList();
            var data = getall.OrderByDescending(t => t.Id).Skip((page - 1) * limit).Take(limit).Select(t => new
            {
                Id = t.Id,
                IsDelete = t.IsDelete,
                OrwatchDate = t.OrwatchDate,
                Rmark = t.Rmark,
                TearcherName = Reconcile_Com.Employees_Entity.GetEntity(t.Tearcher_Id).EmpName,
                timename = t.timename,
                ClassroomName = Reconcile_Com.Classroom_Entity.GetEntity(t.ClassRoom_id).ClassroomName,
                ClassNumber = Reconcile_Com.ClassSchedule_Entity.GetEntity(t.ClassSchedule_Id).ClassNumber,
                AttendDate = t.AttendDate
            }).ToList();
            var jsondata = new { count = getall.Count, code = 0, msg = "", data = data };
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AnpaiTeacherNight()
        {
            return null;
        }


        public ActionResult AddDataView()
        {
            Teacher_Entity = new TeacherBusiness(); //获取所有老师
            List<SelectListItem> sle_teacher = Teacher_Entity.GetTeachers().Select(t => new SelectListItem() { Text = Reconcile_Com.Employees_Entity.GetEntity(t.EmployeeId).EmpName, Value = t.EmployeeId }).ToList();
            sle_teacher.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.teacher = sle_teacher;

            List<SelectListItem> sle_class = Reconcile_Com.GetClass().Select(cl => new SelectListItem() { Text = cl.ClassNumber, Value = cl.id.ToString() }).ToList();  //获取班级
            sle_class.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.myclass = sle_class;


            BaseDataEnumManeger baseDataEnum_Entity = new BaseDataEnumManeger();//加载校区
            List<SelectListItem> schooladdress = baseDataEnum_Entity.GetsameFartherData("校区地址").Select(s => new SelectListItem() { Text = s.Name, Value = s.Id.ToString() }).ToList();
            schooladdress.Add(new SelectListItem() { Text = "--请选择--", Value = "0" });
            ViewBag.Schooladdress = schooladdress.OrderBy(s => s.Value).ToList();
            return View();
        }

        /// <summary>
        /// 系统自动安排晚自习方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SystemAnpaiFunction()
        {
            TeacherNight_Entity = new TeacherNightManeger();
            string[] times = Request.Form["times"].Split('到');
            DateTime start = Convert.ToDateTime(times[0]);
            DateTime end = Convert.ToDateTime(times[1]);
            AjaxResult a = TeacherNight_Entity.AnpaiNight(start, end);
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 手动安排晚自习方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult HandAnpaiFunction()
        {
            AjaxResult a = new AjaxResult();
            TeacherNight_Entity = new TeacherNightManeger();
            TeacherNight new_t = new TeacherNight();
            new_t.OrwatchDate = Convert.ToDateTime(Request.Form["mytime"]);
            new_t.ClassRoom_id = Convert.ToInt32(Request.Form["ClassRoom_id"]);
            new_t.ClassSchedule_Id = Convert.ToInt32(Request.Form["classShdule_sele"]);
            new_t.IsDelete = false;
            new_t.Tearcher_Id = Request.Form["teacher_sele"];
            new_t.Rmark = Request.Form["ramke"];
            new_t.timename = Request.Form["timename"];
            new_t.AttendDate = DateTime.Now;
          
            new_t.BeOnDuty_Id = beOnDuty_Entity.GetSingleBeOnButy("教员晚自习", false).Id;
            bool istrue= TeacherNightandEvningStudet.IsUpdateTeacherNightData(new_t.OrwatchDate,Convert.ToInt32(new_t.ClassSchedule_Id));//判断是否可以安排老师值班
            if (istrue==true)
            {
                //判断是否有重复的数据
                int count = TeacherNight_Entity.GetAllTeacherNight().Where(tea => tea.ClassSchedule_Id == new_t.ClassSchedule_Id && tea.OrwatchDate == new_t.OrwatchDate).ToList().Count;
                if (count > 0)
                {
                    a.Success = false;
                    a.Msg = "该班级已安排值班老师";
                }
                else
                {
                    a = TeacherNight_Entity.Add_data(new_t);
                    if (a.Success)
                    {
                        //安排值班成功后需要修改学生晚自习数据
                        a = TeacherNightandEvningStudet.SetEvningStudentData(new_t.OrwatchDate, Convert.ToInt32(new_t.ClassSchedule_Id),new_t.Tearcher_Id);
                        if (a.Success)
                        {
                            a.Msg = "安排成功";
                        }
                        else
                        {
                            a.Msg = "安排失败，请刷新重试！！！";
                        }
                    }
                }
            }
            else
            {
                a.Success = false;
                a.Msg = "该班级在此日期没有安排晚自习，请先安排该班级的晚自习再安排老师值班";
            }
             
            return Json(a, JsonRequestBehavior.AllowGet); ;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteFunction(int id)
        {
            AjaxResult a = new AjaxResult();
            TeacherNight_Entity = new TeacherNightManeger();
            TeacherNight findata = TeacherNight_Entity.GetEntity(id);
            a = TeacherNight_Entity.My_Delete(id);
            if (a.Success)
            {
                
                a= TeacherNightandEvningStudet.SetEvningStudentData(findata.OrwatchDate,Convert.ToInt32( findata.ClassSchedule_Id), null);
                if (a.Success==false)
                {
                    a.Msg = "删除失败，请刷新重试！！！";
                }
            }
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult EditView(int id)
        {
            
            TeacherNight_Entity = new TeacherNightManeger();//获取数据
            TeacherNight find_data = TeacherNight_Entity.GetEntity(id);
            ViewBag.time = find_data.OrwatchDate.ToString("yyyy-MM-dd");
            Teacher_Entity = new TeacherBusiness();
            List<SelectListItem> teacherlist = Teacher_Entity.GetTeachers().Select(t => new SelectListItem() { Text = Reconcile_Com.GetEmpName(t.EmployeeId), Value = t.EmployeeId, Selected = find_data.Tearcher_Id == t.EmployeeId ? true : false }).ToList();

            ViewBag.teacherlist = teacherlist;
            ViewBag.className = Reconcile_Com.GetClassName(Convert.ToInt32(find_data.ClassSchedule_Id));
            ViewBag.classroom = Reconcile_Com.Classroom_Entity.GetEntity(find_data.ClassRoom_id).ClassroomName;
            return View(find_data);
        }

        [HttpPost]
        public ActionResult EditFunction(TeacherNight t)
        {
            TeacherNight find = TeacherNight_Entity.GetEntity(t.Id);
            t.ClassSchedule_Id = find.ClassSchedule_Id;
            t.BeOnDuty_Id = find.BeOnDuty_Id;
            t.AttendDate = find.AttendDate;
            t.ClassRoom_id = find.ClassRoom_id;
            AjaxResult a = new AjaxResult();
            bool istrue= TeacherNightandEvningStudet.IsUpdateTeacherNightData(t.OrwatchDate,Convert.ToInt32(t.ClassSchedule_Id));
            if (istrue==true)
            {
                 a = TeacherNight_Entity.Edit_Data(t);
                if (a.Success)
                {
                    //学生晚自习数据
                   a=  TeacherNightandEvningStudet.SetEvningStudentData(t.OrwatchDate,Convert.ToInt32( t.ClassSchedule_Id), t.Tearcher_Id);                     
                }
            }
            else
            {
                a.Success = false;
                a.Msg = "该日期没有安排该班级的晚自习！，请重新选择日期或去安排班级在这个日期的晚自习";
            }

            return Json(a, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetEmtpyClassroom()
        {
            int s_id = Convert.ToInt32(Request.Form["schooladdress"]);
            List<TreeClass> tree =Reconcile_Com.Classroom_Entity.GetAddreeClassRoom(s_id).Select(c => new TreeClass() { id = c.Id.ToString(), title = c.ClassroomName }).ToList();
            return Json(tree, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取班级晚自习安排数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetClassData()
        {
            int classid=Convert.ToInt32(Request.Form["classid"]);
            DateTime date = Convert.ToDateTime(Request.Form["date"]);
            //获取班级在这个日期中的晚自习安排数据
            EvningSelfStudy findata= TeacherNightandEvningStudet.GetEvningData(date, classid);
            AjaxResult a = new AjaxResult();
            if (findata!=null)
            {
                var new_data = new
                {
                    classroomname = Reconcile_Com.Classroom_Entity.GetEntity(findata.Classroom_id).ClassroomName,
                    classroomid = findata.Classroom_id,
                    timename=findata.curd_name,
                    teacherid=findata.emp_id
                };
                a.Success = true;
                a.Data = new_data;
            }
            else
            {
                a.Success = false;
                a.Msg = "该班级在此日期中没有安排晚自习";
            }

            return Json(a,JsonRequestBehavior.AllowGet);
        }

        #region 调课或上课日期更换
        public ActionResult ClassadjustmentView(bool id)
        {
            ViewBag.Is = id;//如果是0--》只需要调换日期1-->日期往前推迟或往后推迟
            return View();
        }

        public ActionResult ClassadjustmentFunction()
        {
            int id = beOnDuty_Entity.GetSingleBeOnButy("教员晚自习", false).Id;
            DateTime old = Convert.ToDateTime(Request.Form["oldtime"]);
            DateTime new_old = Convert.ToDateTime(Request.Form["newtime"]);
            bool Whychangedate = Convert.ToBoolean(Request.Form["mybool"]);
            List<TeacherNight> list = TeacherNight_Entity.GetAllTeacherNight();
            int count = (old - new_old).Days;
            if (Whychangedate)//调课
            {
                list = list.Where(t => t.OrwatchDate >= old && t.BeOnDuty_Id == id).ToList();
            }
            else //日期更改
            {
                list = list.Where(t => t.OrwatchDate == old && t.BeOnDuty_Id == id).ToList();
            }
            AjaxResult a = TeacherNight_Entity.Update_Date(Whychangedate, list, count, new_old);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #endregion



        #region 班主任晚自习值班
        public ActionResult ClassMasterIndex()
        {
            return View();
        }

        public ActionResult GetClassMasterFunction(int page, int limit)
        {
            int id1 = beOnDuty_Entity.GetSingleBeOnButy("周末值班", false).Id;
            int id2 = beOnDuty_Entity.GetSingleBeOnButy("班主任晚自习", false).Id;
            TeacherNight_Entity = new TeacherNightManeger();
            List<TeacherNight> getall = TeacherNight_Entity.GetAllTeacherNight().Where(t => t.BeOnDuty_Id == id1 || t.BeOnDuty_Id == id2).OrderByDescending(t => t.Id).ToList();
            var data = getall.OrderByDescending(t => t.Id).Skip((page - 1) * limit).Take(limit).Select(t => new
            {
                Id = t.Id,
                OrwatchDate = t.OrwatchDate,
                Rmark = t.Rmark,
                TearcherName = Reconcile_Com.Employees_Entity.GetEntity(t.Tearcher_Id).EmpName,
                timename = t.timename,
                AttendDate = t.AttendDate
            }).ToList();
            var jsondata = new { count = getall.Count, code = 0, msg = "", data = data };
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MasterAddView()
        {
            return View();
        }

        /// <summary>
        /// 周末值班
        /// </summary>
        /// <returns></returns>
        public ActionResult WeekenddutyView()
        {
            DepartmentManage Deparment_Entity = new DepartmentManage();
            List<SelectListItem> list = Deparment_Entity.GetDepartments().Where(d => d.DeptName.Contains("教质部") || d.DeptName.Contains("就业部")).Select(d => new SelectListItem() { Text = d.DeptName, Value = d.DeptId.ToString() }).ToList();//获取所有有效的部门
            list.Add(new SelectListItem() { Text = "--请选择--", Value = "0" });
            ViewBag.dep = list.OrderBy(d => d.Value).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult GetDepEmp()
        {
            int did = Convert.ToInt32(Request.Form["depid"]);//获取部门id
            EmployeesInfoManage Employeesinfo_Entity = new EmployeesInfoManage();
            List<SelectListItem> list = Employeesinfo_Entity.GetEmpsByDeptid(did).Where(e => e.IsDel == false).Select(e => new SelectListItem() { Text = e.EmpName, Value = e.EmployeeId }).ToList();//获取所属部门的所有未辞职的员工

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult WeekEnddutyFunction()
        {
            bool types = Convert.ToBoolean(Request.Form["Type"]);
            int typeid1 = beOnDuty_Entity.GetSingleBeOnButy("周末值班", false).Id;
            int typeid2 = beOnDuty_Entity.GetSingleBeOnButy("班主任晚自习", false).Id;
            string[] tid = Request.Form["tid"].Split(',');//值班老师员工编号
            DateTime date = Convert.ToDateTime(Request.Form["time"]);//值班日期
            List<TeacherNight> night = new List<TeacherNight>();
            foreach (string id in tid)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    TeacherNight new_t = new TeacherNight();
                    new_t.AttendDate = DateTime.Now;
                    new_t.BeOnDuty_Id = types == true ? typeid2 : typeid1;
                    new_t.IsDelete = false;
                    new_t.OrwatchDate = date;
                    new_t.Tearcher_Id = id;
                    new_t.timename = types == true ? "晚自习值班" : "周末值班";
                    night.Add(new_t);
                }
            }

            AjaxResult a = TeacherNight_Entity.Add_masterdata(night);

            return Json(a, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditMasterView(int id)
        {
            TeacherNight find = TeacherNight_Entity.GetEntity(id);
            EmployeesInfoManage e = new EmployeesInfoManage();
            ViewBag.empName = e.GetEntity(find.Tearcher_Id).EmpName;
            return View(find);
        }

        public ActionResult EditMasterFunction(TeacherNight new_t)
        {
            TeacherNight find = TeacherNight_Entity.GetEntity(new_t.Id);
            find.OrwatchDate = new_t.OrwatchDate;
            AjaxResult a = TeacherNight_Entity.Edit_Data(find);
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditDateView()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditDateFuntion()
        {
            DateTime time = Convert.ToDateTime(Request.Form["time"]);
            string idlist = Request.Form["ids"];
            bool isAll = Convert.ToBoolean(Request.Form["IsAll"]);
            AjaxResult a = new AjaxResult();
            List<TeacherNight> list = new List<TeacherNight>();
            if (isAll) //值班数据全部改为某日期
            {
                int id1 = beOnDuty_Entity.GetSingleBeOnButy("周末值班", false).Id;
                int id2 = beOnDuty_Entity.GetSingleBeOnButy("班主任晚自习", false).Id;
                List<TeacherNight> getall = TeacherNight_Entity.GetAllTeacherNight().Where(t => t.BeOnDuty_Id == id1 || t.BeOnDuty_Id == id2).OrderByDescending(t => t.Id).ToList();
                a = TeacherNight_Entity.Update_Date(false, getall, 0, time);
            }
            else
            {
                string[] list_id = idlist.Split(',');
                foreach (string id in list_id)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        int myid = Convert.ToInt32(id);
                        list.Add(TeacherNight_Entity.GetEntity(myid));
                    }
                }
               a= TeacherNight_Entity.Update_Date(false, list, 0, time);
            }
            return Json(a,JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult EditDateChangeView()
        {
            return View();
        }

        public ActionResult EditDateChangeFuntion()
        {
            DateTime oldtime = Convert.ToDateTime(Request.Form["oldtime"]);
            DateTime newtime = Convert.ToDateTime(Request.Form["newtime"]);
            string idlist = Request.Form["ids"];
            bool isAll = Convert.ToBoolean(Request.Form["IsAll"]);
            int count = (newtime-oldtime  ).Days;
            AjaxResult a = new AjaxResult();
            List<TeacherNight> list = new List<TeacherNight>();
            if (isAll) //值班数据全部改为某日期
            {
                int id1 = beOnDuty_Entity.GetSingleBeOnButy("周末值班", false).Id;
                int id2 = beOnDuty_Entity.GetSingleBeOnButy("班主任晚自习", false).Id;
                List<TeacherNight> getall = TeacherNight_Entity.GetAllTeacherNight().Where(t => t.BeOnDuty_Id == id1 || t.BeOnDuty_Id == id2 ).OrderByDescending(t => t.Id).ToList();
                getall= getall.Where(t => t.OrwatchDate >= oldtime).ToList();
                list = getall;
            }
            else
            {
                string[] list_id = idlist.Split(',');
                foreach (string id in list_id)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        int myid = Convert.ToInt32(id);
                        list.Add(TeacherNight_Entity.GetEntity(myid));
                    }
                }                
            }
            a = TeacherNight_Entity.Update_Date(true, list, count, newtime);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}