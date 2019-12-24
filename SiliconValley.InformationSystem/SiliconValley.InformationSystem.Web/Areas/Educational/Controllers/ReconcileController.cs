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

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    [CheckLogin]
    public class ReconcileController : BaseMvcController
    {
        // GET: /Educational/Reconcile/TimeName
        static readonly ReconcileManeger Reconcile_Entity = new ReconcileManeger();
        private EmployeesInfoManage dbemployeesInfo;
        static Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();//获取登录人信息
        static Recon_Login_Data GetBaseData(string Emp)
        {
            Recon_Login_Data new_re = new Recon_Login_Data();
            EmployeesInfo employees=Reconcile_Com.Employees_Entity.GetEntity(Emp);
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
        static Recon_Login_Data rr = GetBaseData(UserName.EmpNumber);
          static int base_id = rr.ClassRoom_Id;
          static bool IsOld = rr.IsOld;//确定教务
        #region 大批量课表安排
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
                   var find_clist = Reconcile_Com.Curriculum_Entity.GetList().Where(c=>c.Grand_Id== grand_id && c.MajorID==null && c.IsDelete==false && c.CourseName!="英语").Select(c => new { CourseName = c.CourseName, CurriculumID = c.CurriculumID }).ToList();
                    var josndata = new { classData = new_c, c_list = find_clist, stataus = "true" };
                    return Json(josndata, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var find_clist = Reconcile_Com.Curriculum_Entity.GetRelevantCurricul(Reconcile_Com.Grand_Entity.FindNameGetData(grand).Id, Reconcile_Com.Specialty_Entity.FindNameSame(marjon).Id,false).Select(c => new { CourseName = c.CourseName, CurriculumID = c.CurriculumID }).ToList();
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
            AjaxResult a = new AjaxResult();
            StringBuilder db = new StringBuilder();
            try
            {
                TeacherClassBusiness TeacherClass_Entity = new TeacherClassBusiness();
                int y1_id = Reconcile_Com.GetGrand_Id(true).Where(c => c.GrandName.Equals("Y1")).FirstOrDefault().Id;                
                string grand_Id = Request.Form["mygrand"];
                int class_Id = Convert.ToInt32(Request.Form["class_select"]);
                int classroom_Id = Convert.ToInt32(Request.Form["myclassroom"]);
                int kengcheng = Convert.ToInt32(Request.Form["kecheng"]);
                string ke = Reconcile_Com.Curriculum_Entity.GetEntity(kengcheng).CourseName;
                string time = Request.Form["time"];
                string techar = Request.Form["teachersele"];
                DateTime startTime = Convert.ToDateTime(Request.Form["startTime"]);
                //开始排课
                Curriculum find_c = Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.CourseName == ke).FirstOrDefault();
                //查看这个课程的课时数
                int Kcount = Convert.ToInt32(find_c.CourseCount) / 4;
                //判断该班级这个课程是否已排完课
                int count = Reconcile_Entity.GetList().Where(r => r.Curriculum_Id == ke && r.ClassSchedule_Id == class_Id).ToList().Count;
                if (count== Kcount)
                {                   
                    a.Msg = Reconcile_Com.ClassSchedule_Entity.GetEntity(class_Id).ClassNumber + "的" + ke + "已排好,请选择其他课程";
                    a.Success = false;
                }
                else
                {                                       
                    //获取单休双休月份
                    GetYear find_g = Reconcile_Entity.MyGetYear(startTime.Year.ToString(), Server.MapPath("~/Xmlconfigure/Reconcile_XML.xml"));
                    List<Reconcile> new_list = new List<Reconcile>();
                    //判断是否是Y1的班级
                    if (y1_id.ToString() == grand_Id)
                    {
                        #region 初中生
                        bool IsMargin = true;//true--安排专业课，false--安排语数英
                        string[] strcurr = new string[] { "语文,数学", "数学,英语", "英语,语文" };
                        string[] strtimename = new string[] { "上午12节,上午34节", "下午12节,下午34节" };
                        int nomargin = 0;
                        for (int i = 0; i < Kcount; i++)
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

                            if (IsMargin == true)
                            {
                                r.EmployeesInfo_Id = techar;
                                r.Curse_Id = time;
                                r.Curriculum_Id = ke;
                                r.Curse_Id = time;
                                r.NewDate = DateTime.Now;
                                r.IsDelete = false;
                                new_list.Add(r);
                            }
                            else
                            {
                                Kcount++;
                                string[] currname1 = strcurr[nomargin].Split(',');
                                //安排语数英
                                if (time == "上午")
                                {
                                    string[] timename1 = strtimename[0].Split(',');

                                    for (int j = 0; j < timename1.Length; j++)
                                    {
                                        Reconcile cc1 = new Reconcile();
                                        cc1.AnPaiDate = r.AnPaiDate;
                                        cc1.ClassRoom_Id = classroom_Id;
                                        cc1.ClassSchedule_Id = class_Id;
                                        cc1.Curriculum_Id = currname1[j];
                                        cc1.Curse_Id = timename1[j];
                                        cc1.NewDate = DateTime.Now;
                                        cc1.IsDelete = false;
                                        //获取任课老师
                                        string emid = Reconcile_Entity.GetNomaginTeacher(Convert.ToDateTime(cc1.AnPaiDate), cc1.Curriculum_Id, cc1.Curse_Id, y1_id);
                                        if (!string.IsNullOrEmpty(emid))
                                        {
                                            cc1.EmployeesInfo_Id = emid;
                                        }
                                        else
                                        {
                                            cc1.EmployeesInfo_Id = null;
                                            cc1.Curriculum_Id = "自习";
                                        }
                                        new_list.Add(cc1);
                                    }

                                }
                                else if (time == "下午")
                                {
                                    string[] timename1 = strtimename[1].Split(',');

                                    for (int j = 0; j < timename1.Length; j++)
                                    {
                                        Reconcile cc1 = new Reconcile();
                                        cc1.AnPaiDate = r.AnPaiDate;
                                        cc1.ClassRoom_Id = classroom_Id;
                                        cc1.ClassSchedule_Id = class_Id;
                                        cc1.Curriculum_Id = currname1[j];
                                        cc1.Curse_Id = timename1[j];
                                        cc1.NewDate = DateTime.Now;
                                        cc1.IsDelete = false;
                                        //获取任课老师
                                        string emid = Reconcile_Entity.GetNomaginTeacher(Convert.ToDateTime(cc1.AnPaiDate), cc1.Curriculum_Id, cc1.Curse_Id, y1_id);
                                        if (!string.IsNullOrEmpty(emid))
                                        {
                                            cc1.EmployeesInfo_Id = emid;
                                        }
                                        else
                                        {
                                            cc1.EmployeesInfo_Id = null;
                                            cc1.Curriculum_Id = "自习";
                                        }
                                        new_list.Add(cc1);
                                    }
                                }

                                if (nomargin < 2)
                                {
                                    nomargin++;
                                }
                                else
                                {
                                    nomargin = 0;
                                }
                            }
                            IsMargin = IsMargin ? false : true;
                        }
                        #endregion
                    }
                    else
                    {
                        #region 高中生

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
                                    r.Curriculum_Id = ke + "考试";
                                    r.EmployeesInfo_Id = null;
                                }
                                
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

                        #endregion
                    }
                    a = Reconcile_Entity.Inser_list(new_list);                     
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
                    classroom = r.ClassRoom_Id==null?"无": Reconcile_Com.Classroom_Entity.GetEntity(r.ClassRoom_Id).ClassroomName,//教室
                    curriName = r.Curriculum_Id,//课程
                    Sketime = r.Curse_Id,//课程时间字段
                    ADate = r.AnPaiDate,
                    Teacher= r.EmployeesInfo_Id==null?"无": Reconcile_Com.Employees_Entity.GetEntity(r.EmployeesInfo_Id).EmpName
                });
                var jsondata = new { code = 0, msg = "", count = lisr_r.Count, data = mydata };
                return Json(jsondata, JsonRequestBehavior.AllowGet);
            }
             
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
            List<SelectListItem> t_list = Reconcile_Com.CourseType_Entity.GetCourseTypes().Select(t => new SelectListItem() { Text = t.TypeName, Value = t.TypeName }).ToList();
            if (IsOld==false)
            {
                t_list = t_list.Where(t => !t.Text.Contains("语文") && !t.Value.Contains("数学") && !t.Value.Contains("英语")).ToList();
            }            
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
            string currname = Request.Form["curname"];
            string type = Request.Form["typecurr"];
            List<Classroom> c_list2 = new List<Classroom>();
            if (type == "军事课")
            {
                c_list2 = Reconcile_Entity.GetClassrooms(timeName, base_id, Time).Where(c=>c.ClassroomName=="操场").ToList();                 
                return Json(c_list2, JsonRequestBehavior.AllowGet);
            }
            else if(type=="0")
            {
                List<Classroom> c_list = Reconcile_Entity.GetClassrooms(timeName, base_id, Time);
                c_list.Add(new Classroom() { ClassroomName = "--请选择--", Id = 0 });
                c_list2 = c_list.OrderBy(c => c.Id).ToList();
                return Json(c_list2, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<Classroom> c_list = Reconcile_Entity.GetClassrooms(timeName, base_id, Time).Where(c => c.ClassroomName != "操场" && c.ClassroomName != "报告厅").ToList();
                c_list.Add(new Classroom() { ClassroomName = "--请选择--", Id = 0 });
                c_list2 = c_list.OrderBy(c => c.Id).ToList();
                return Json(c_list2, JsonRequestBehavior.AllowGet);
            }
             
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
            DateTime anpai = Convert.ToDateTime(Request.Form["Time"]);
            string currname = Request.Form["Curr"];
            string timename = Request.Form["timeName"];
            int class_id =Convert.ToInt32( Request.Form["class_id"]);
            List<EmployeesInfo> e_list = new List<EmployeesInfo>();            
            PositionManage position = new PositionManage();
            ClassSchedule find_c = Reconcile_Com.ClassSchedule_Entity.GetEntity(class_id);//获取班级数据
            switch (currname)
            {               
                case "职素":
                    //获取可以上职素课的班主任
                    //如果是S3的就要获取就业部的老师
                    e_list = Reconcile_Entity.GetMasTeacher(anpai, timename,IsOld);
                    break;
                case "军事":
                    //获取教官
                    e_list = Reconcile_Entity.GetSir(IsOld,anpai,timename);                    
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
                    Grand find_g=  Reconcile_Com.GetGrand_Id(false).Where(g => g.GrandName.Equals("S4", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
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
            List<SelectListItem> selects = new List<SelectListItem>();
            selects.Add(new SelectListItem() { Text = "--请选择--", Value = "0" ,Selected=true});
            if (IsOld)
            {
                //s1,s2,y1                               
                selects.Add(new SelectListItem() { Text="英语",Value="英语"});
                selects.Add(new SelectListItem() { Text = "班会", Value = "班会" });
                selects.Add(new SelectListItem() { Text = "军事", Value = "军事" });
                selects.Add(new SelectListItem() { Text = "职素", Value = "职素" });
                selects.Add(new SelectListItem() { Text = "自习", Value = "自习" });
                selects.Add(new SelectListItem() { Text = "晚自习", Value = "晚自习" });
            }
            else
            {
                //s3,s4
                selects.Add(new SelectListItem() { Text = "军事", Value = "军事" });
                selects.Add(new SelectListItem() { Text = "自习", Value = "自习" });
                selects.Add(new SelectListItem() { Text = "晚自习", Value = "晚自习" });
            }
            ViewBag.select = selects;
            return View();
        }

        [HttpPost]
        public ActionResult AnpaiNotMarginFunction()
        {
            string currrtype = Request.Form["currtype"];
            string[] times = Request.Form["anpaitime"].Split('到');
            AjaxResult a= Reconcile_Entity.Thismm(IsOld, base_id, currrtype, Convert.ToDateTime(times[0]), Convert.ToDateTime(times[times.Length - 1]),Server.MapPath("~/Xmlconfigure/Reconcile_XML.xml"));            
            return Json(a,JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region 生成课表
        public ActionResult GetGenerateTimetable()
        {            
            return View();
        }
        public ActionResult GetReconcileData(DateTime id)
        {           
            //获取XX校区的所有教室
            List<Classroom> c_list = Reconcile_Com.Classroom_Entity.GetList().Where(c => c.BaseData_Id == base_id).OrderBy(c => c.Id).ToList();
            List<SelectListItem> tabledata= c_list.Select(c => new SelectListItem() { Text = c.ClassroomName, Value = c.Id.ToString() }).ToList();
            //获取所有教室的上午班级上课情况
            List<AnPaiData> mongingOne = Reconcile_Entity.GetPaiDatas(id, "上午12节", c_list);
            List<AnPaiData> mongingTwo = Reconcile_Entity.GetPaiDatas(id, "上午34节", c_list);
            //下午
            List<AnPaiData> afternoonOne = Reconcile_Entity.GetPaiDatas(id, "下午12节", c_list);
            List<AnPaiData> afternoonTwo = Reconcile_Entity.GetPaiDatas(id, "下午34节", c_list);
            //晚自习
            List<AnPaiData> ngintone =ReconcileManeger.EvningSelfStudy_Entity.getAppoint("晚一",id, c_list);
            List<AnPaiData> nginttwo = ReconcileManeger.EvningSelfStudy_Entity.getAppoint("晚二", id,c_list);
            var datajson = new {tablethead= tabledata, MymongingOne = mongingOne, MymongingTwo=mongingTwo , MyafternoonOne = afternoonOne , MyafternoonTwo = afternoonTwo, MyngintOne= ngintone, MyngintTwo= nginttwo };
            return Json(datajson,JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 排课表的查询,删除,编辑
        public ActionResult SerachReconcile_Index()
        {
            //加载阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData(IsOld).Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.grandlist = g_list;
            return View();
        }
        
        public ActionResult GetReconAllData(int page,int limit)
        {
            List<Reconcile> all = new List<Reconcile>();        
            List<Reconcile> A= Reconcile_Entity.AllReconcile().OrderByDescending(r => r.Id).ToList();//获取所有排课数据 
            //true- 加载S1-S2-Y1排课数据 // false-加载S3-S4排课数据
            //获取阶段集合
            List<Grand> grands = Reconcile_Com.GetGrand_Id(IsOld);
            foreach (Reconcile re in A)
            {
                int g_id =Reconcile_Com.ClassSchedule_Entity.GetEntity(re.ClassSchedule_Id).grade_Id;
                int count = grands.Where(g=>g.Id==g_id).Count();
                if (count>0)
                {
                    all.Add(re);
                }
            }
     
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
            bool s= Reconcile_Entity.AidAllData(startme, count, IsOld);
            return Json(s, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 班级数据大批量调课
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassBigDataAID()
        {
            //获取阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData(IsOld).Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
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

        /// <summary>
        /// 修改班级上课时间段页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateClassView()
        {
            //获取阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData(IsOld).Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
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
            return null;

        }
    }
}