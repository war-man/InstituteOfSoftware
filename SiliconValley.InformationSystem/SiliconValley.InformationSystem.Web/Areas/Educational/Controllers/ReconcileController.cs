using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Business.PositionBusiness;
using SiliconValley.InformationSystem.Business.DepartmentBusiness;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    public class ReconcileController : Controller
    {
        // GET: /Educational/Reconcile/NewsRexoncileView
          static readonly ReconcileManeger Reconcile_Entity = new ReconcileManeger();
          static readonly EmployeesInfoManage Employees_Entity = new EmployeesInfoManage();
          static readonly TeacherBusiness Teacher_Entity = new TeacherBusiness();
        static Recon_Login_Data GetBaseData(string Emp)
        {
            Recon_Login_Data new_re = new Recon_Login_Data();
            EmployeesInfo employees= Employees_Entity.GetEntity(Emp);
            //获取部门
            DepartmentManage department = new DepartmentManage();
            Department find_d1= department.GetList().Where(d => d.DeptName == "s1、s2教学部").FirstOrDefault();
            Department find_d2 = department.GetList().Where(d => d.DeptName == "s3教学部").FirstOrDefault();
            //获取岗位
            PositionManage position = new PositionManage();
            Position find_p = position.GetEntity(employees.PositionId);
            if (find_p.PositionName=="教务" && find_p.DeptId==find_d1.DeptId)
            {
                //s1.s2教务
                new_re.ClassRoom_Id = Reconcile_Com.ClassSchedule_Entity.BaseDataEnum_Entity.GetSingData("继善高科校区", false).Id;
                new_re.IsOld = true;
            }
            else
            {
                //s3教务
                new_re.ClassRoom_Id = Reconcile_Com.ClassSchedule_Entity.BaseDataEnum_Entity.GetSingData("达嘉维康校区", false).Id;
                new_re.IsOld = false;
            }
            return new_re;
        }
        //获取当前登录员是哪个校区的教务
          static int base_id = GetBaseData("201911190041").ClassRoom_Id;
          static bool IsOld = GetBaseData("201911190041").IsOld;//确定教务
        #region 高中生课表安排
        public ActionResult ReconcileIndexViews()
        {
            //加载阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData(IsOld).Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.grandlist = g_list;                        
            return View();
        }
        /// <summary>
        /// 通过阶段获取班级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetClassScheduleSelect(int id)
        {
            var c_list= Reconcile_Entity.GetGrandClass(id).ToList();
            return Json(c_list,JsonRequestBehavior.AllowGet);
        }         
        /// <summary>
        /// 通过班级名称获取班级其他数据
        /// </summary>
        /// <param name="id">班级名称</param>
        /// <returns></returns>
        public ActionResult GetClassDate(int id)
        {
            if (id!=0)
            {
                ClassSchedltData new_c = new ClassSchedltData();
                ClassSchedule find_c = Reconcile_Com.ClassSchedule_Entity.GetEntity(id);
                new_c.Name = find_c.ClassNumber;//班级名称
                string marjon = Reconcile_Com.ClassSchedule_Entity.GetClassGrand(find_c.id, 1);//专业
                new_c.marjoiName = marjon;
                string grand = Reconcile_Com.ClassSchedule_Entity.GetClassGrand(find_c.id, 2);//阶段
                new_c.GrandName = grand;
                string time = Reconcile_Com.ClassSchedule_Entity.GetClassTime(find_c.id);//上课时间类型
                new_c.ClassDate = time;
                //获取某个阶段某个专业的所有课程                 
                
                if (marjon=="无")
                {
                    int grand_id = Reconcile_Com.Grand_Entity.FindNameGetData(grand).Id;
                   var find_clist = Reconcile_Com.Curriculum_Entity.GetList().Where(c=>c.Grand_Id== grand_id && c.MajorID==null && c.IsDelete==false).Select(c => new { CourseName = c.CourseName, CurriculumID = c.CurriculumID }).ToList();
                    var josndata = new { classData = new_c, c_list = find_clist, stataus = "true" };
                    return Json(josndata, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var find_clist = Reconcile_Com.Curriculum_Entity.GetRelevantCurricul(Reconcile_Com.Grand_Entity.FindNameGetData(grand).Id, Reconcile_Com.Specialty_Entity.FindNameSame(marjon).Id).Select(c => new { CourseName = c.CourseName, CurriculumID = c.CurriculumID }).ToList();
                    var josndata = new { classData = new_c, c_list = find_clist, stataus = "true" };
                    return Json(josndata, JsonRequestBehavior.AllowGet);
                }                 
            }
            else
            {
                var josndata = new { classData = "", c_list = "", stataus = "false" };
                return Json(josndata, JsonRequestBehavior.AllowGet);
            }
            
        }
        //排课
        [HttpPost]
        public ActionResult PaikeFunction()
        {
            TeacherClassBusiness TeacherClass_Entity = new TeacherClassBusiness();
            
            StringBuilder db = new StringBuilder();
            string grand_Id = Request.Form["mygrand"];
            int class_Id =Convert.ToInt32(Request.Form["class_select"]);
            int classroom_Id =Convert.ToInt32(Request.Form["myclassroom"]);
            int kengcheng =Convert.ToInt32( Request.Form["kecheng"]);
            string ke = Reconcile_Com.Curriculum_Entity.GetEntity(kengcheng).CourseName;
            string time = Request.Form["time"];
            string techar = Request.Form["teachersele"];
            DateTime startTime =Convert.ToDateTime( Request.Form["startTime"]);
            //判断该班级这个课程是否已排完课
            int count= Reconcile_Entity.GetList().Where(r => r.Curriculum_Id == ke && r.ClassSchedule_Id == class_Id).ToList().Count;
            if (count>0)
            {               
                db.Append(class_Id + "的" + ke + "已排好,请选择其他课程");
               
            }
            else
            {
                    //开始排课
                    Curriculum find_c = Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.CourseName == ke).FirstOrDefault();
                    //查看这个课程的课时数
                    int Kcount = Convert.ToInt32(find_c.CourseCount) / 4;
                    //获取单休双休月份
                    GetYear find_g = Reconcile_Entity.MyGetYear("2019", Server.MapPath("~/Xmlconfigure/Reconcile_XML.xml"));
                    List<Reconcile> new_list = new List<Reconcile>();
                    for (int i = 0; i <= Kcount; i++)
                    {
                        Reconcile r = new Reconcile();
                        //判断是否是单休
                        if (startTime.Month >= find_g.StartmonthName && startTime.Month <= find_g.EndmonthName)
                        {
                            //单休
                            if (Reconcile_Entity.IsSaturday(startTime.AddDays(i)) == 2)
                            {
                                //如果是周日
                                r.AnPaiDate = startTime.AddDays(i + 1);
                                i++;
                                Kcount++;
                            }
                            else
                            {
                                r.AnPaiDate = startTime.AddDays(i);
                            }
                        }
                        else
                        {
                            //双休
                            if (Reconcile_Entity.IsSaturday(startTime.AddDays(i)) == 1)
                            {
                                //如果是周六
                                r.AnPaiDate = startTime.AddDays(i + 2);
                                i = i + 2;
                                Kcount = Kcount + 2;
                            }
                            else
                            {
                                r.AnPaiDate = startTime.AddDays(i);
                            }
                        }
                        r.ClassRoom_Id = classroom_Id;
                        r.ClassSchedule_Id = class_Id;
                        r.EmployeesInfo_Id = techar;
                        if (i == Kcount)
                        {
                            //课程考试
                            bool iscurr = Reconcile_Entity.IsEndCurr(ke);
                            if (iscurr)
                            {
                                r.Curriculum_Id = "升学考试";
                            }
                            else
                            {

                            }
                            r.Curriculum_Id = ke + "考试";
                        }
                        else
                        {
                            r.Curriculum_Id = ke;
                        }
                        r.NewDate = DateTime.Now;
                        r.Curse_Id = time;
                        r.IsDelete = false;
                        new_list.Add(r);
                    }
                    if (Reconcile_Entity.IsExcit(new_list[0]))
                    {
                        db.Append(new_list[0].ClassSchedule_Id+"的"+new_list[0].Curriculum_Id+"已安排！！！");
                    }
                    else
                    {
                        if (Reconcile_Entity.Inser_list(new_list))
                        {
                            db.Append("ok");
                        }
                    }                                              
            }
            return Json(db.ToString(),JsonRequestBehavior.AllowGet);
        }
        //排课数据
        public ActionResult GetTableData(int limit ,int page)
        {
            int classname =Convert.ToInt32( Request.QueryString["classname"]);//班级名称
            if (classname<=0)
            {                   
                return Json(new { code = 0, msg = "", count = 0, data = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<Reconcile> lisr_r = Reconcile_Entity.GetList().Where(r => r.ClassSchedule_Id == classname).ToList();
                var mydata = lisr_r.Skip((page - 1) * limit).Take(limit).Select(r => new {
                    Id = r.Id,
                    classname = Reconcile_Com.ClassSchedule_Entity.GetEntity(r.ClassSchedule_Id).ClassNumber,//班级名称
                    classroom = r.ClassRoom_Id==null?"无": Reconcile_Com.Classroom_Entity.GetEntity(r.ClassRoom_Id).ClassroomName,//教室
                    curriName = r.Curriculum_Id,//课程
                    Sketime = r.Curse_Id,//课程时间字段
                    ADate = r.AnPaiDate,
                    Teacher= r.EmployeesInfo_Id==null?"无":Employees_Entity.GetEntity(r.EmployeesInfo_Id).EmpName
                });
                var jsondata = new { code = 0, msg = "", count = lisr_r.Count, data = mydata };
                return Json(jsondata, JsonRequestBehavior.AllowGet);
            }
             
        }
        //修改排课数据页面
        public ActionResult EditView(int id)
        {
            //获取有效教室
            int base_id = Reconcile_Com.ClassSchedule_Entity.BaseDataEnum_Entity.GetSingData("继善高科校区", false).Id;
            ViewBag.Editclassrrrom = Reconcile_Entity.GetEffectioveClassRoom(base_id).Select(c => new SelectListItem() { Text = c.ClassroomName, Value = c.Id.ToString() }).ToList();
            Reconcile find_r= Reconcile_Entity.GetEntity(id);
            return View(find_r);
        }        
        //手动排课页面
        public ActionResult ManualReconcileView()
        {
            //获取阶段
            //加载所有阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData(IsOld).Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.Child_grandlist = g_list;
            //获取课程类型
            List<SelectListItem> t_list= Reconcile_Com.CourseType_Entity.GetCourseTypes().Select(t=>new SelectListItem() { Text=t.TypeName,Value=t.TypeName}).ToList();
            t_list.Add(new SelectListItem() { Text="其他",Value="0",Selected=true});
            ViewBag.Child_typelist= t_list;
            return View();
        }
        //获取空教室
        [HttpPost]
        public ActionResult GetEmptyRoom()
        {
            string timeName= Request.Form["timeName"];
            DateTime Time =Convert.ToDateTime( Request.Form["Time"]);

           List<Classroom> c_list= Reconcile_Entity.GetClassrooms(timeName, base_id, Time);
            c_list.Add(new Classroom() {ClassroomName="--请选择--",Id=0});
            List<Classroom> c_list2 = c_list.OrderBy(c => c.Id).ToList();
            return Json(c_list2,JsonRequestBehavior.AllowGet);
        }
        //根据班级获取课程
        public ActionResult CaseClassGetCurr(int id)
        {
            List<Curriculum> c_list = new List<Curriculum>();
            string find_m= Reconcile_Com.ClassSchedule_Entity.GetClassGrand(id, 1);//专业名称

            string fing_g= Reconcile_Com.ClassSchedule_Entity.GetClassGrand(id, 2);//阶段名称
            Grand find_grand= Reconcile_Com.Grand_Entity.FindNameGetData(fing_g);
            if (find_grand!=null)
            {
                if (find_m=="无")
                {
                    c_list= Reconcile_Com.Curriculum_Entity.GetRelevantCurricul(find_grand.Id, null);
                }
                else
                {
                    //获取专业Id
                    Specialty find_s = Reconcile_Com.Specialty_Entity.FindNameSame(find_m);
                    if (find_s != null)
                    {
                        c_list = Reconcile_Com.Curriculum_Entity.GetRelevantCurricul(find_grand.Id, find_s.Id);
                    }
                }
                 
            }
            c_list.Add(new Curriculum() { CourseName = "--请选择--", CurriculumID = 0 });
            c_list=c_list.OrderBy(c => c.CurriculumID).ToList();
            return Json(c_list,JsonRequestBehavior.AllowGet);
        }
        //根据专业课程获取专业老师
        [HttpPost]
        public ActionResult GetTeacher()
        {
            DateTime anpai= Convert.ToDateTime(Request.Form["Time"]);
            int curr_id = Convert.ToInt32(Request.Form["Curr"]);
            string timename = Request.Form["timeName"];
            List<Teacher> find_t= Reconcile_Com.GoodSkill_Entity.GetTeachers(curr_id);
            List<EmployeesInfo> all= Employees_Entity.GetList();
            List<EmployeesInfo> find_e = new List<EmployeesInfo>();
            foreach (EmployeesInfo item1 in all)
            {
                foreach (Teacher item2 in  find_t)
                {
                    if (item2.EmployeeId==item1.EmployeeId)
                    {
                        bool s = Reconcile_Entity.IsHaveClass(item2.EmployeeId, timename, anpai);
                        if (s==false)
                        {
                            find_e.Add(item1);
                        }
                       
                    }
                }
            }
            List<SelectListItem> result= find_e.Select(e => new SelectListItem() { Text = e.EmpName, Value = e.EmployeeId }).ToList();

            return Json(result,JsonRequestBehavior.AllowGet); 
        }
        //获取非专业课的老师
        [HttpPost]
        public ActionResult GetNoMajoiThercher()
        {
            DateTime anpai = Convert.ToDateTime(Request.Form["Time"]);
            string currname = Request.Form["Curr"];
            string timename = Request.Form["timeName"];
            List<EmployeesInfo> e_list = new List<EmployeesInfo>();
            List<SelectListItem> select = new List<SelectListItem>();
            PositionManage position = new PositionManage();
            switch (currname)
            {               
                case "职素":
                    //获取可以上职素课的班主任
                    e_list = Reconcile_Entity.GetMasTeacher();
                    break;
                case "军事":
                    //获取教官
                    e_list = Reconcile_Entity.GetSir(IsOld);                    
                    break;
                case "英语":
                    //获取英语老师
                   Position find_p1= position.GetList().Where(p=>p.PositionName=="英语老师").FirstOrDefault();
                    List<EmployeesInfo> e2= Employees_Entity.GetEmpByPid(find_p1.Pid);
                    break;
                case "数学":
                    Position find_p2= position.GetList().Where(p => p.PositionName == "数学老师").FirstOrDefault();
                    e_list= Employees_Entity.GetEmpByPid(find_p2.Pid);
                    //获取数学老师
                    break;
                case "语文":
                    Position find_p3= position.GetList().Where(p => p.PositionName == "语文老师").FirstOrDefault();
                    e_list= Employees_Entity.GetEmpByPid(find_p3.Pid);
                    //获取语文老师
                    break;
            }
            foreach (EmployeesInfo item in e_list)
            {
                bool s = Reconcile_Entity.IsHaveClass(item.EmployeeId, timename, anpai);
                if (s==false)
                {
                    SelectListItem sele = new SelectListItem();
                    sele.Value = item.EmployeeId;
                    sele.Text = item.EmpName;
                    select.Add(sele);
                }
            }                   
            return Json(select, JsonRequestBehavior.AllowGet);
        }
        //手动排课数据提交
        [HttpPost]
        public ActionResult AddHandDataFunction()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                Reconcile new_r = new Reconcile();
                //获取班级
                new_r.ClassSchedule_Id =Convert.ToInt32( Request.Form["child_class"]);
                //获取类型
                string type = Request.Form["childview_Currtype"];
                if (type.Contains("专业"))
                {
                    //获取课程Id                 
                    int cur_id = Convert.ToInt32(Request.Form["childview_currname"]);
                    new_r.Curriculum_Id = Reconcile_Com.Curriculum_Entity.GetEntity(cur_id).CourseName;
                }
                else
                {
                    new_r.Curriculum_Id = Request.Form["childview_currname"];
                }
                
                //获取任课老师
                new_r.EmployeesInfo_Id = Request.Form["teacher_child"];
                //获取上课地点
                int room_Id = Convert.ToInt32(Request.Form["childview_room"]);//上课地点可能是室外
                if (room_Id!=0)
                {
                    new_r.ClassRoom_Id = room_Id;
                }                 
                //获取上课时间
                new_r.Curse_Id = Request.Form["childview_timetype"];                                                          
                new_r.IsDelete = false;
                new_r.NewDate = DateTime.Now;
                //排课的时间
                new_r.AnPaiDate = Convert.ToDateTime(Request.Form["childview_AnpiDate"]);
                bool Is= Reconcile_Entity.IsExcit(new_r);
                if (Is==false)
                {
                   bool isture= Reconcile_Entity.AddData(new_r);
                    if (isture)
                    {
                        sb.Append("ok");
                    }
                    else
                    {
                        sb.Append("系统错误，请重试!!!");
                    }
                    
                }
                else
                {
                    sb.Append("已有重复数据！！！");
                }                
                
            }
            catch (Exception)
            {
                sb.Append("系统错误，请重试!!!");
            }
            return Json(sb.ToString(),JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 生成课表
        public ActionResult GetGenerateTimetable()
        {
            #region 生成非专业课程
            //获取班级
            if (IsOld)
            {
                 int Grand_s1id = Reconcile_Com.GetGrand_Id("S1");
                 int Grand_s2id = Reconcile_Com.GetGrand_Id("S2");
                 int Grand_y1id = Reconcile_Com.GetGrand_Id("Y1");
                //获取S1，S2阶段的班级
                List<ClassSchedule> class_list1= Reconcile_Entity.GetGrandClass(Grand_s1id);
                List<ClassSchedule> class_list2 = Reconcile_Entity.GetGrandClass(Grand_s2id);
                List<ClassSchedule> class_list3 = Reconcile_Entity.GetGrandClass(Grand_y1id);
                foreach (ClassSchedule item in class_list2)
                {
                    class_list1.Add(item);
                }
                foreach (ClassSchedule item in class_list3)
                {
                    class_list1.Add(item);
                }
                int baid1 = Reconcile_Com.GetBase_Id("上课时间类型", "上午");
                int baid2 = Reconcile_Com.GetBase_Id("上课时间类型", "下午");
                List<ClassSchedule> monring= class_list1.Where(c => c.BaseDataEnum_Id == baid1).ToList();//获取上午班
                List<ClassSchedule> afternoon = class_list1.Where(c=>c.BaseDataEnum_Id==baid2).ToList();//获取下午班
                List<Classroom> getroom1 = Reconcile_Entity.GetClassrooms("上午", base_id, Convert.ToDateTime("2019-11-25")).Where(c => c.ClassroomName != "报告厅" && c.ClassroomName != "操场").ToList(); 
                List<Classroom> getroom2 = Reconcile_Entity.GetClassrooms("下午", base_id, Convert.ToDateTime("2019-11-25")).Where(c => c.ClassroomName != "报告厅" && c.ClassroomName != "操场").ToList();
                Reconcile_Entity.mmm("英语", Convert.ToDateTime("2019-11-25"), monring, getroom1);
                Reconcile_Entity.mmm("英语", Convert.ToDateTime("2019-11-25"), afternoon, getroom2);
            }
             
            #endregion
            //获取XX校区的所有教室
            List<Classroom> c_list = Reconcile_Com.Classroom_Entity.GetList().Where(c => c.BaseData_Id == base_id).OrderBy(c => c.Id).ToList();
            ViewBag.classroom_all= c_list.Select(c=>new SelectListItem() { Text=c.ClassroomName,Value=c.Id.ToString()}).ToList();
            //获取所有教室的上午班级上课情况
            ViewBag.mongingOne= Reconcile_Entity.GetPaiDatas(Convert.ToDateTime("2019-11-27"), "上午12节", c_list);
            ViewBag.mongingTwo = Reconcile_Entity.GetPaiDatas(Convert.ToDateTime("2019-11-27"), "上午34节", c_list);
            //下午
            ViewBag.afternoonOne = Reconcile_Entity.GetPaiDatas(Convert.ToDateTime("2019-11-27"), "下午12节", c_list);
            ViewBag.afternoonTwo = Reconcile_Entity.GetPaiDatas(Convert.ToDateTime("2019-11-27"), "下午34节", c_list);
            //晚自习
            return View();
        }
        #endregion        
    }
}