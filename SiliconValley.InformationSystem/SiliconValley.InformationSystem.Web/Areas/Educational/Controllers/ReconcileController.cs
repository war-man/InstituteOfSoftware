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
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    //[CheckLogin]
    public class ReconcileController : BaseMvcController
    {
        // GET: /Educational/Reconcile/GetTecherAll
        static readonly ReconcileManeger Reconcile_Entity = new ReconcileManeger();
        private EmployeesInfoManage dbemployeesInfo;
        private TeacherClassBusiness TeacherClass_Entity;
        // CourseTypeBusiness CurseType_Entity;
        #region 大批量课表安排
        public ActionResult ReconcileIndexViews()
        {
            //加载阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.grandlist = g_list;
            //加载校区
            BaseDataEnumManeger baseDataEnum_Entity = new BaseDataEnumManeger();
            List<SelectListItem> schooladdress = baseDataEnum_Entity.GetsameFartherData("校区地址").Select(s=>new SelectListItem() { Text=s.Name,Value=s.Id.ToString()}).ToList();
            schooladdress.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.Schooladdress = schooladdress;
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
        /// 获取课程
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
                var find_clist = Reconcile_Entity.GetCurr(find_c.grade_Id, true,Convert.ToInt32( find_c.Major_Id));
                var josndata = new { classData = new_c, c_list = find_clist, stataus = "true" };
                return Json(josndata, JsonRequestBehavior.AllowGet);                   
            }
            else
            {
                var josndata = new { classData = "", c_list = "", stataus = "false" };
                return Json(josndata, JsonRequestBehavior.AllowGet);
            }
            
        }
        //系统排课业务处理方法
        [HttpPost]
        public ActionResult PaikeFunction()
        {
            AjaxResult a = new AjaxResult();
            StringBuilder db = new StringBuilder();
            try
            {
                TeacherClassBusiness TeacherClass_Entity = new TeacherClassBusiness();
                int y1_id = Reconcile_Com.GetGrand_Id().Where(c => c.GrandName.Equals("Y1")).FirstOrDefault().Id; //获取Y1阶段Id               
                string grand_Id = Request.Form["mygrand"];//获取班级阶段
                int class_Id = Convert.ToInt32(Request.Form["class_select"]);//获取班级
                int classroom_Id = Convert.ToInt32(Request.Form["myclassroom"]);//获取教室
                int kengcheng = Convert.ToInt32(Request.Form["kecheng"]);//获取课程编号
                string ke = Reconcile_Com.Curriculum_Entity.GetEntity(kengcheng).CourseName;//获取课程名称
                string time = Request.Form["time"];//获取上课时间
                string techar = Request.Form["teachersele"];//获取任课老师
                DateTime startTime = Convert.ToDateTime(Request.Form["startTime"]);//获取排课日期
                bool DoubleRest = true;//默认双休
                //开始排课
                Curriculum find_c = Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.CourseName == ke).FirstOrDefault();
                //查看这个课程的课时数
                int Kcount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(find_c.CourseCount / 4.0)));  
                //判断该班级这个课程是否已排完课
                int count = Reconcile_Entity.GetList().Where(r => r.Curriculum_Id == ke && r.ClassSchedule_Id == class_Id).ToList().Count;
                if (count>0)
                {                   
                    a.Msg = Reconcile_Com.ClassSchedule_Entity.GetEntity(class_Id).ClassNumber + "的" + ke + "已有排课数据，为了避免重复，请删除之后再操作！！";
                    a.Success = false;
                } else if (count== Kcount)
                {
                    a.Msg = Reconcile_Com.ClassSchedule_Entity.GetEntity(class_Id).ClassNumber + "的" + ke + "已有排课数据,请安排其他课程！！";
                    a.Success = false;
                }
                else
                {                                       
                    //获取单休双休月份
                    GetYear find_g = Reconcile_Entity.MyGetYear(startTime.Year.ToString(), Server.MapPath("~/Xmlconfigure/Reconcile_XML.xml"));
                    if (startTime.Month<=find_g.EndmonthName && startTime.Month>=find_g.StartmonthName)
                    {
                        //单休
                        DoubleRest = false;
                    }
                    List<Reconcile> new_list = new List<Reconcile>();
                    //判断是否是Y1的班级
                    if (y1_id.ToString() == grand_Id)
                    {
                      
                        #region 初中生
                           List<Reconcile> get_new_data = Reconcile_Entity.MiddleStudentReconcileFunction(DoubleRest, kengcheng, startTime, time, classroom_Id, techar, class_Id, y1_id);
                         // a = Reconcile_Entity.Inser_list(get_new_data);
                        #endregion
                    }
                    else
                    {

                        #region 高中生
                         
                        List<Reconcile> get_new_data = Reconcile_Entity.HeghtStudentReconcileFunction(DoubleRest, kengcheng, startTime, time, classroom_Id, techar, class_Id);
                        a = Reconcile_Entity.Inser_list(get_new_data);
                        #endregion
                    }
                               
                }
            }
            catch (Exception ex)
            {
                a.Msg = ex.Message;
                a.Success = false;
            }
             
            return Json(a,JsonRequestBehavior.AllowGet);
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
                    classroom = Reconcile_Com.Classroom_Entity.GetEntity(r.ClassRoom_Id).ClassroomName,//教室
                    curriName = r.Curriculum_Id,//课程
                    Sketime = r.Curse_Id,//课程时间字段
                    ADate = r.AnPaiDate,
                    Teacher= r.EmployeesInfo_Id==null?"无": Reconcile_Com.Employees_Entity.GetEntity(r.EmployeesInfo_Id).EmpName
                });
                var jsondata = new { code = 0, msg = "", count = lisr_r.Count, data = mydata };
                return Json(jsondata, JsonRequestBehavior.AllowGet);
            }
             
        }
        
        //获取空教室
        [HttpPost]
        public ActionResult GetEmptyRoom()
        {
            string timeName= Request.Form["timeName"];
            DateTime Time =Convert.ToDateTime( Request.Form["Time"]);
            string currname = Request.Form["curname"];
            string type = Request.Form["typecurr"];
            List<Classroom> c_list2 = new List<Classroom>();
            if (type == "军事课")
            {
                c_list2 = Reconcile_Entity.GetClassrooms(timeName, Time).Where(c=>c.ClassroomName=="操场").ToList();                 
                return Json(c_list2, JsonRequestBehavior.AllowGet);
            }
            else if(type=="0")
            {
                List<Classroom> c_list = Reconcile_Entity.GetClassrooms(timeName, Time);
                c_list.Add(new Classroom() { ClassroomName = "--请选择--", Id = 0 });
                c_list2 = c_list.OrderBy(c => c.Id).ToList();
                return Json(c_list2, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<Classroom> c_list = Reconcile_Entity.GetClassrooms(timeName, Time).Where(c => c.ClassroomName != "操场" && c.ClassroomName != "报告厅").ToList();
                c_list.Add(new Classroom() { ClassroomName = "--请选择--", Id = 0 });
                c_list2 = c_list.OrderBy(c => c.Id).ToList();
                return Json(c_list2, JsonRequestBehavior.AllowGet);
            }
             
        }
        
        //获取某个校区的空教室
        [HttpPost]
        public ActionResult GetSchoolEmptyRoom()
        {
            AjaxResult a = new AjaxResult();
            //获取日期
            DateTime date =Convert.ToDateTime( Request.Form["dateval"]);
            //获取上课时间
            string timename = Request.Form["timenameval"];
            //获取所属校区
            int belongto_SchoolAdress =Convert.ToInt32( Request.Form["schooldaddressval"]);
            List<Classroom> data= Reconcile_Entity.GetSchoolEmptyRoomFunction(timename, date, belongto_SchoolAdress);
            data.Add(new Classroom { ClassroomName = "--请选择--", Id = 0 });
            a.Data = data.OrderBy(c => c.Id).ToList();            
            return Json(a, JsonRequestBehavior.AllowGet);
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
                    c_list= Reconcile_Com.Curriculum_Entity.GetRelevantCurricul(find_grand.Id, null,false);
                }
                else
                {
                    //获取专业Id
                    Specialty find_s = Reconcile_Com.Specialty_Entity.FindNameSame(find_m);
                    if (find_s != null)
                    {
                        c_list = Reconcile_Com.Curriculum_Entity.GetRelevantCurricul(find_grand.Id, find_s.Id,false);
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
            List<EmployeesInfo> emplist= Reconcile_Entity.GetMarjorTeacher(curr_id, timename, anpai);
            List<SelectListItem> result= emplist.Select(e => new SelectListItem() { Text = e.EmpName, Value = e.EmployeeId }).ToList();
            return Json(result,JsonRequestBehavior.AllowGet); 
        }
        //获取非专业课的老师
        [HttpPost]
        public ActionResult GetNoMajoiThercher()
        {
            DateTime anpai = Convert.ToDateTime(Request.Form["Time"]);//获取日期
            string currname = Request.Form["Curr"];//获取课程类型
            string timename = Request.Form["timeName"];//获取上课时间
            int class_id =Convert.ToInt32( Request.Form["class_id"]);//获取班级
            List<EmployeesInfo> e_list = new List<EmployeesInfo>();            
            PositionManage position = new PositionManage();
            ClassSchedule find_c = Reconcile_Com.ClassSchedule_Entity.GetEntity(class_id);//获取班级数据
            int grand_id = find_c.grade_Id;//获取阶段
            switch (currname)
            {               
                case "职素":
                    //获取可以上职素课的班主任
                    //如果是S4的就要获取就业部的老师
                    e_list = Reconcile_Entity.GetMasTeacher(anpai, timename);
                    break;
                case "军事":
                    //获取教官
                    e_list = Reconcile_Entity.GetSir(anpai,timename);                    
                    break;
                case "英语":
                    //获取阶段                                           
                        Curriculum find_curr= Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.Grand_Id == find_c.grade_Id && c.CourseName == currname).FirstOrDefault();
                        e_list= Reconcile_Entity.GetMarjorTeacher(find_curr.CurriculumID, timename, anpai);                                    
                    break;
                case "数学": 
                    //获取数学老师
                    Curriculum find_curr1 = Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.Grand_Id == null && c.CourseName == currname).FirstOrDefault();
                    e_list = Reconcile_Entity.GetMarjorTeacher(find_curr1.CurriculumID, timename, anpai);                    
                    break;
                case "语文": 
                    //获取语文老师
                    Curriculum find_curr2 = Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.Grand_Id == null && c.CourseName == currname).FirstOrDefault();
                    e_list = Reconcile_Entity.GetMarjorTeacher(find_curr2.CurriculumID, timename, anpai);                    
                    break;
                case "班会":
                    //获取这个班级的班主任
                    //如果这个班级是S4的话，获取就业部的老师
                    //判断班级是否是S4阶段
                    Grand find_g=  Reconcile_Com.GetGrand_Id().Where(g => g.GrandName.Equals("S4", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    if (find_c.grade_Id==find_g.Id)
                    {
                        EmploymentStaff find_saff=  Reconcile_Com.EmploymentStaff_Entity.GetStaffByclassid(find_c.id);
                        bool s1 = Reconcile_Entity.IsHaveClass(find_saff.EmployeesInfo_Id, timename, anpai);
                        if (s1 == false)
                        {
                            e_list.Add(Reconcile_Com.Employees_Entity.GetEntity(find_saff.EmployeesInfo_Id));
                        }
                    }
                    else
                    {
                        EmployeesInfo e = Reconcile_Com.GetZhisuTeacher(class_id).Data as EmployeesInfo;
                        if (e!=null)
                        {
                            bool s1 = Reconcile_Entity.IsHaveClass(e.EmployeeId, timename, anpai);
                            if (s1 == false)
                            {
                                e_list.Add(e);
                            }
                        }
                         
                    }                     
                    break;
            }
            List<SelectListItem> select = e_list.Select(s => new SelectListItem() { Value=s.EmployeeId ,Text=s.EmpName }).ToList();                  
            return Json(select, JsonRequestBehavior.AllowGet);
        }
        //手动排课页面
        public ActionResult ManualReconcileView()
        {
            //加载所有阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.Child_grandlist = g_list;
            //获取课程类型
            List<SelectListItem> t_list = Reconcile_Com.CourseType_Entity.GetCourseTypes().Select(t => new SelectListItem() { Text = t.TypeName, Value = t.TypeName }).ToList();
            t_list.Add(new SelectListItem() { Text = "其他", Value = "0", Selected = true });
            ViewBag.Child_typelist = t_list;
            BaseDataEnumManeger baseDataEnum_Entity = new BaseDataEnumManeger();
            List<SelectListItem> schooladdress = baseDataEnum_Entity.GetsameFartherData("校区地址").Select(s => new SelectListItem() { Text = s.Name, Value = s.Id.ToString() }).ToList();
            schooladdress.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.schooladdress = schooladdress;
            return View();
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
                else if(type=="0")
                {
                    new_r.Curriculum_Id = Request.Form["childview_currname"].ToString();
                }
                else
                {
                    new_r.Curriculum_Id = Request.Form["childview_currname"].ToString().Substring(0, 2);
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
                bool Is= Reconcile_Entity.IsExcit(new_r,true);
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
        /// <summary>
        /// 获取XX班级上课时间段
        /// </summary>
        public ActionResult TimeName(int id)
        {
            ClassSchedule find_c = Reconcile_Com.ClassSchedule_Entity.GetEntity(id);
            string time = Reconcile_Com.ClassSchedule_Entity.GetClassTime(find_c.id);//上课时间类型
            return Json(time,JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 非专业课排课
        public ActionResult AnpaiNotMarginView()
        {
            //加载阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.orthergrand = g_list.OrderBy(g => g.Value).ToList();
            CourseTypeBusiness CurseType_Entity = new CourseTypeBusiness();
            BaseDataEnumManeger dataEnum_Entity = new BaseDataEnumManeger();
            //获取所有有效的课程类型
            List<SelectListItem> find_cur_list = CurseType_Entity.GetCourseTypes().Where(c => !c.TypeName.Contains("专业") && !c.TypeName.Contains("语文") && !c.TypeName.Contains("数学")).Select(c => new SelectListItem() { Text = c.TypeName, Value = c.TypeName }).ToList();
            ViewBag.select = find_cur_list;

            //获取有效校区
            List<SelectListItem> schoolAddress = dataEnum_Entity.GetsameFartherData("校区地址").Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();

            ViewBag.schoolAddress = schoolAddress;

            return View();
        }

        /// <summary>
        /// 获取所有老师
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMasterAlldata()
        {
           string type_name= Request.Form["Type"];
           
            List<SelectListItem> Item = new List<SelectListItem>();
            if (type_name.Contains("职素"))
            {
               Item = Reconcile_Entity.GetMaster_All(false).Select(e => new SelectListItem() { Text = e.EmpName, Value = e.EmployeeId }).ToList();
            }else if (type_name.Contains("军事"))
            {
                Item = Reconcile_Entity.GetInstructorAll().Select(e => new SelectListItem() { Text = e.EmpName, Value = e.EmployeeId }).ToList();
            }else if (type_name.Contains("英语"))
            {
                Item= Reconcile_Entity.GetEnglishTeacherAll().Select(e => new SelectListItem() { Text = e.EmpName, Value = e.EmployeeId }).ToList(); ;
            }else if (type_name.Contains("班会"))
            {
                Item = Reconcile_Entity.GetMaster_All(true).Select(e => new SelectListItem() { Text = e.EmpName, Value = e.EmployeeId }).ToList();
            }
           
            return Json(Item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EmptyClassRoom()
        {
            int schoold_Id =Convert.ToInt32( Request.Form[""]);
            List<SelectListItem> classroomlist= Reconcile_Com.Classroom_Entity.GetAddreeClassRoom(schoold_Id).Select(c=>new SelectListItem() { Text=c.ClassroomName,Value=c.Id.ToString()}).ToList();
            return Json(classroomlist,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 安排非专业课程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AnpaiNotMarginFunction()
        {
            string currrtype = Request.Form["currtype"];//获取安排的课程类型
            string timename =  Request.Form["anpaitime"];//上课时间           
            int classid =Convert.ToInt32( Request.Form[""]);//获取班级
            int classroomid = Convert.ToInt32(Request.Form[""]);//获取教室
            string teacherid = Request.Form[""];//获取老师

            return Json(null, JsonRequestBehavior.AllowGet);
        }

  
        public ActionResult GetClassTimeName(int id)
        {
            ClassSchedltData new_c = new ClassSchedltData();
            BaseDataEnumManeger basedata_entity = new BaseDataEnumManeger();
            ClassSchedule find_c = Reconcile_Com.ClassSchedule_Entity.GetEntity(id);
            BaseDataEnum find_b= basedata_entity.GetEntity(find_c.BaseDataEnum_Id);
            AjaxResult a = new AjaxResult();
            if (find_b!=null)
            {
                a.Success = true;
                a.Data = find_b.Name;
            }
            else
            {
                a.Success = false;
                a.Msg = "数据错误，请重试";
            }
            return Json(a,JsonRequestBehavior.AllowGet);

        }
        #endregion         

        #region 排课表的查询,删除,编辑
        public ActionResult SerachReconcile_Index()
        {
            //加载阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.grandlist = g_list;
            return View();
        }
        
        public ActionResult GetReconAllData(int page,int limit)
        {
            List<Reconcile> all = Reconcile_Entity.AllReconcile().OrderByDescending(r => r.Id).ToList();//获取所有排课数据                   
          
          
            string class_select1 = Request.QueryString["class_select1"];
            string starTime = Request.QueryString["starTime"];
            string endTime = Request.QueryString["endTime"];
            string teachername = Request.QueryString["teachername"];
            string Time2 = Request.QueryString["Time2"];
            string curr_select1 = Request.QueryString["curr_select1"];

            if (!string.IsNullOrEmpty(class_select1))
            {
                int class_id = Convert.ToInt32(class_select1);
                if (class_id!=0)
                {
                    all = all.Where(r => r.ClassSchedule_Id == class_id).ToList();
                }
                 
            }

            if (!string.IsNullOrEmpty(starTime))
            {
                DateTime dd = Convert.ToDateTime(starTime);
                all= all.Where(r => r.AnPaiDate >= dd).ToList();
            }

            if (!string.IsNullOrEmpty(endTime))
            {
                DateTime dd = Convert.ToDateTime(endTime);
                all= all.Where(r => r.AnPaiDate <= dd).ToList();
            }

            if (!string.IsNullOrEmpty(teachername))
            {
               List<EmployeesInfo> fin_lsite= Reconcile_Com.Employees_Entity.GetAll().Where(e => e.EmpName == teachername).ToList();
                if (fin_lsite.Count==1)
                {
                    all = all.Where(r => r.EmployeesInfo_Id == fin_lsite[0].EmployeeId).ToList();
                }else if (fin_lsite.Count == 2)
                {
                    all = all.Where(r => r.EmployeesInfo_Id == fin_lsite[0].EmployeeId || r.EmployeesInfo_Id == fin_lsite[1].EmployeeId).ToList();
                }
            }

            if (!string.IsNullOrEmpty(Time2))
            {
                DateTime dd = Convert.ToDateTime(Time2);
                all = all.Where(r => r.AnPaiDate==dd).ToList();
            }

            if (!string.IsNullOrEmpty(curr_select1))
            {
                if (curr_select1!="0")
                {
                    all = all.Where(r => r.Curriculum_Id == curr_select1).ToList();
                }
            }
            var mydata= all.Skip((page - 1) * limit).Take(limit).Select(r => new
            {
               Id=r.Id,
               class_id=r.ClassSchedule_Id,
               class_name=Reconcile_Com.ClassSchedule_Entity.GetEntity(r.ClassSchedule_Id).ClassNumber,//获取班级名称
               teacher_id=r.EmployeesInfo_Id,
               teacher_name= r.EmployeesInfo_Id==null?"无": Reconcile_Com.Employees_Entity.GetEntity(r.EmployeesInfo_Id).EmpName,//获取任课老师名称
               currname=r.Curriculum_Id,
               timeName=r.Curse_Id,
               time=r.AnPaiDate,
               classroom_id=r.ClassRoom_Id,
               rake=r.Rmark,
               classroom_name=Reconcile_Com.Classroom_Entity.GetEntity( r.ClassRoom_Id).ClassroomName//获取教室名称
            }).ToList();

            var jsondata = new { code = 0, msg = "", data = mydata, count = all.Count };
            return Json(jsondata,JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Delete(int id)
        {
            AjaxResult a = Reconcile_Entity.DeleteReconcile(id);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
         
        public ActionResult GetCurrrlistdata(int id)
        {
                List<SelectListItem> listItems = Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.Grand_Id == id && c.CourseName != "英语").Select(c => new SelectListItem() { Text = c.CourseName, Value = c.CourseName }).ToList();
                return Json(listItems, JsonRequestBehavior.AllowGet);
            
        }
        
        public ActionResult Edit_rView(int id)
        {          
            Reconcile find_r= Reconcile_Entity.GetEntity(id);
            ClassSchedule find_c = Reconcile_Com.ClassSchedule_Entity.GetEntity(find_r.ClassSchedule_Id);
            //获取课程
            List<SelectListItem> currlist = new List<SelectListItem>();

            currlist.Add(new SelectListItem() { Text = find_r.Curriculum_Id, Value = find_r.Curriculum_Id });
            if (find_r.Curriculum_Id!="自习")
            {
                currlist.Add(new SelectListItem() { Text = "自习", Value = "自习" });
            }                                               
            ViewBag.currlist = currlist;            
            ReconView data =new ReconView() {
                id=find_r.Id,
                class_name = Reconcile_Com.ClassSchedule_Entity.GetEntity(find_r.ClassSchedule_Id).ClassNumber,
                classroom_name = Reconcile_Com.Classroom_Entity.GetEntity(find_r.ClassRoom_Id).ClassroomName,
                currname = find_r.Curriculum_Id,
                teachername =find_r.EmployeesInfo_Id==null? null: Reconcile_Com.Employees_Entity.GetEntity(find_r.EmployeesInfo_Id).EmpName,
                curdname =find_r.Curse_Id,
                anpaidate= find_r.AnPaiDate.Year+"-"+find_r.AnPaiDate.Month+"-"+find_r.AnPaiDate.Day ,
                ramak=find_r.Rmark
            };
            return View(data);
        }
        [HttpPost]
        public ActionResult EditFunction(ReconView new_r)
        {
            AjaxResult a= Reconcile_Entity.Updata_data(new_r);
            return Json(a,JsonRequestBehavior.AllowGet);
        }        
        
        public ActionResult BigDataUpdate()
        {
            return View();
        }
        /// <summary>
        /// 所有数据调课
        /// </summary>
        /// <returns></returns>
        public ActionResult BigDataAID()
        {
            DateTime startme=Convert.ToDateTime( Request.Form["oldTime"]);
            DateTime endtime = Convert.ToDateTime(Request.Form["endtime"]);
            var days = endtime.Subtract(startme);
            int count = days.Days;
            bool s= Reconcile_Entity.AidAllData(startme, count);
            return Json(s, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 班级数据大批量调课
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassBigDataAID()
        {
            //获取阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.grandlist = g_list;
            return View();
        }
        [HttpPost]
        public ActionResult ClassBigDataFunction()
        {
            int class_id=Convert.ToInt32( Request.Form["class_select"]);
            DateTime startime = Convert.ToDateTime(Request.Form["starTime"]);
            DateTime endtime = Convert.ToDateTime(Request.Form["endTime"]);
            var days =  endtime.Subtract(startime);
            int count = days.Days;
            bool s= Reconcile_Entity.AidClassData(startime, count, class_id);
            return Json(s,JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 调课
        /// <summary>
        /// 修改班级上课时间段页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateClassView()
        {
            //获取阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.grandlist = g_list;

            //获取上课时间段
            BaseDataEnumManeger base_Entity = new BaseDataEnumManeger();
            List<BaseDataEnum> basesataenum = base_Entity.GetsameFartherData("上课时间类型");
            ViewBag.baseE = basesataenum;
            return View();
        }
        
        public ActionResult UpdateClassFunction()
        {
            int class_id=Convert.ToInt32( Request.Form["class_select"]);
            string time = Request.Form["time"];
            BaseDataEnumManeger base_Entity = new BaseDataEnumManeger();
            int time2 = base_Entity.GetsameFartherData("上课时间类型").Where(b=>b.Name==time).FirstOrDefault().Id;
            AjaxResult a= Reconcile_Com.ClassSchedule_Entity.Modifyclasstime(class_id, time2);
            if (a.Success==true)
            {
                //修改调课单
                DateTime dd = DateTime.Now;
                string date = dd.Year + "-" + dd.Month + "-" + dd.Day;
                DateTime dd2= Convert.ToDateTime(date);
                List<Reconcile> find_list= Reconcile_Entity.AllReconcile().Where(r => r.AnPaiDate >= dd2 && r.ClassSchedule_Id == class_id).ToList();
               AjaxResult a2= Reconcile_Entity.Update_data2(find_list, time);
                if (a2.Success==true && a.Success==true)
                {
                    return Json(a2, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    a2.Msg = "修改排课数据失败，请联系开发人员！！！";
                    return Json(a2, JsonRequestBehavior.AllowGet);
                }
                 
            }
            else
            {
                return Json(a, JsonRequestBehavior.AllowGet);
            }
             
        }

        public ActionResult UpdateClassTeacher()
        {
            //获取阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.grandlist = g_list;
            return View();
        }
        /// <summary>
        /// 获取所有上专业课老师
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTecherAll(int id)
        {
            dbemployeesInfo = new EmployeesInfoManage();
            List<TeacherData> tlist = new List<TeacherData>();
            List<Teacher> teachers = Reconcile_Com.Teacher_Entity.GetTeachers();
            foreach (Teacher item in teachers)
            {
                EmployeesInfo find_e= dbemployeesInfo.GetEntity(item.EmployeeId);
               
                 
                if (find_e!=null)
                {
                    string name = Reconcile_Com.PositionBusiness.GetEntity(find_e.PositionId).PositionName;
                    if (name != "英语老师" || name != "数学老师" || name != "语文老师")
                    {
                        TeacherData t = new TeacherData();
                        t.Emp_id = find_e.EmployeeId;
                        t.Name = find_e.EmpName;
                        t.Teacher_id = item.TeacherID;
                        tlist.Add(t);
                    }
                    
                }

               
            }
            //获取该班级的专业老师
            TeacherClass_Entity = new TeacherClassBusiness();
            ClassTeacher find_ct=  TeacherClass_Entity.GetList().Where(t => t.ClassNumber == id && t.IsDel == false).FirstOrDefault();
            int teacher_id = find_ct == null ? 0 :Convert.ToInt32( find_ct.TeacherID);
            var data = new { t_id=teacher_id,list_teacher= tlist };
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateClassTeacherFuntion()
        {
            AjaxResult a = new AjaxResult();
            try
            {
                int class_id = Convert.ToInt32(Request.Form["class_select"]);
                int teacher_id = Convert.ToInt32(Request.Form["teacher_id"]);
                ClassTeacher find_ct = Reconcile_Com.TeacherClass_Entity.GetList().Where(t => t.ClassNumber == class_id && t.IsDel == false).FirstOrDefault();
                if (find_ct != null)
                {
                    find_ct.TeacherID = teacher_id;
                    Reconcile_Com.TeacherClass_Entity.Update(find_ct);

                    //更新排课表
                    DateTime dd = DateTime.Now;
                    string date = dd.Year + "-" + dd.Month + "-" + dd.Day;
                    DateTime dd2 = Convert.ToDateTime(date);
                    List<Reconcile> find_list = Reconcile_Entity.AllReconcile().Where(r => r.AnPaiDate >= dd2 && r.ClassSchedule_Id == class_id).ToList();
                    string emp = Reconcile_Com.Teacher_Entity.GetEntity(teacher_id).EmployeeId;
                    a= Reconcile_Entity.Update_date3(find_list, emp);
                    if (a.Success==false)
                    {
                        a.Msg = "修改排课数据错误！！！";
                    }
                }
                else
                {
                    a.Success = false;
                    a.Msg = "修改数据失败！！！";
                }
                return Json(a, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = "系统错误！！！";
                return Json(a,JsonRequestBehavior.AllowGet);
            }
            
        }

        /// <summary>
        /// 全体上课日期调换
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangReconciledata()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangReconcileFunction()
        {
            DateTime startime = Convert.ToDateTime(Request.Form["starTime"]);
            DateTime endtime = Convert.ToDateTime(Request.Form["endTime"]);
            List<ClassSchedule> class_s= Reconcile_Com.GetClass();
            List<Reconcile> lisr_r= Reconcile_Entity.AllReconcile().Where(r => r.AnPaiDate == startime).ToList();
            List<Reconcile> find_lidt = new List<Reconcile>();
            foreach (Reconcile i in lisr_r)
            {
                int count = class_s.Where(s => s.id == i.ClassSchedule_Id).ToList().Count;
                if (count>0)
                {
                    find_lidt.Add(i);
                }
            }
            AjaxResult a= Reconcile_Entity.update_date4(find_lidt, endtime);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 指定班级上课日期调换
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangClassDateView()
        {
            //获取阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.grandlist = g_list;
            return View();
        }
        [HttpPost]
        public ActionResult ChangClassDataFunction()
        {
            int class_id = Convert.ToInt32(Request.Form["class_select"]);
            DateTime startime = Convert.ToDateTime(Request.Form["starTime"]);
            DateTime endtime = Convert.ToDateTime(Request.Form["endTime"]);
            List<Reconcile> find_list = Reconcile_Entity.AllReconcile().Where(r => r.ClassSchedule_Id == class_id && r.AnPaiDate == startime).ToList();
           AjaxResult a=  Reconcile_Entity.update_date4(find_list, endtime);
            return Json(a,JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}