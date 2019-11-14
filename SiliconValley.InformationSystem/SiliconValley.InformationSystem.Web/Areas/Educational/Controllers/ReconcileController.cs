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

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    public class ReconcileController : Controller
    {
        // GET: /Educational/Reconcile/GetTableData

          static readonly ReconcileManeger Reconcile_Entity = new ReconcileManeger();
          static readonly EmployeesInfoManage Employees_Entity = new EmployeesInfoManage();
          static readonly TeacherBusiness Teacher_Entity = new TeacherBusiness();
        #region 高中生课表
        public ActionResult ReconcileIndexViews()
        {
            //加载所有阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.grandlist = g_list;            
            //获取有效的教室
            //获取当前登录员是哪个角色是哪个校区的教务
            int base_id= ReconcileManeger.ClassSchedule_Entity.BaseDataEnum_Entity.GetSingData("继善高科校区",false).Id;
            ViewBag.classrrrom= Reconcile_Entity.GetEffectioveClassRoom(base_id).Select(c=>new SelectListItem() { Text=c.ClassroomName,Value=c.Id.ToString()}).ToList();
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
        public ActionResult GetClassDate(string id)
        {
            if (!string.IsNullOrEmpty(id) && id!="0")
            {
                ClassSchedltData new_c = new ClassSchedltData();
                ClassSchedule find_c = ReconcileManeger.ClassSchedule_Entity.GetEntity(id);
                new_c.Name = find_c.ClassNumber;//班级名称
                string marjon = ReconcileManeger.ClassSchedule_Entity.GetClassGrand(find_c.ClassNumber, 1);//专业
                new_c.marjoiName = marjon;
                string grand = ReconcileManeger.ClassSchedule_Entity.GetClassGrand(find_c.ClassNumber, 2);//阶段
                new_c.GrandName = grand;
                string time = ReconcileManeger.ClassSchedule_Entity.GetClassGrand(find_c.ClassNumber, 3);//上课时间类型
                new_c.ClassDate = time;
                //获取某个阶段某个专业的所有课程                 
                
                if (marjon=="无")
                {
                    int grand_id = ReconcileManeger.Grand_Entity.FindNameGetData(grand).Id;
                   var find_clist = ReconcileManeger.Curriculum_Entity.GetList().OrderBy(c=>c.Sort).Where(c=>c.Grand_Id== grand_id && c.MajorID==null && c.IsDelete==false).Select(c => new { CourseName = c.CourseName, CurriculumID = c.CurriculumID }).ToList();
                    var josndata = new { classData = new_c, c_list = find_clist, stataus = "true" };
                    return Json(josndata, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var find_clist = ReconcileManeger.Curriculum_Entity.GetRelevantCurricul(ReconcileManeger.Grand_Entity.FindNameGetData(grand).Id, ReconcileManeger.Specialty_Entity.FindNameSame(marjon).Id).Select(c => new { CourseName = c.CourseName, CurriculumID = c.CurriculumID }).ToList();
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
            string class_Id =Request.Form["class_select"];
            int classroom_Id =Convert.ToInt32( Request.Form["myclassroom"]);
            string kengcheng = Request.Form["kecheng"];
            string time = Request.Form["time"];
            DateTime startTime =Convert.ToDateTime( Request.Form["startTime"]);
            //判断该班级这个课程是否已排完课
            int count= Reconcile_Entity.GetList().Where(r => r.Curriculum_Id == kengcheng && r.ClassSchedule_Id == class_Id).ToList().Count;
            if (count>0)
            {
                string coursename= ReconcileManeger.Curriculum_Entity.GetEntity(kengcheng).CourseName;
                db.Append(class_Id + "的" + coursename + "已排好,请选择其他课程");
               
            }
            else
            {
                //判断该课程是否安排了教学老师
                ClassTeacher Ishave= TeacherClass_Entity.GetList().Where(t => t.IsDel == false && t.ClassNumber == class_Id).FirstOrDefault();
                if (Ishave!=null)
                {
                    //开始排课
                    Curriculum find_c = ReconcileManeger.Curriculum_Entity.GetList().Where(c => c.CourseName == kengcheng).FirstOrDefault();
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
                        r.EmployeesInfo_Id = Teacher_Entity.GetEntity(Ishave.TeacherID).EmployeeId;
                        if (i == Kcount)
                        {
                            //课程考试
                            bool iscurr = Reconcile_Entity.IsEndCurr(kengcheng);
                            if (iscurr)
                            {
                                r.Curriculum_Id = "升学考试";
                            }
                            else
                            {

                            }
                            r.Curriculum_Id = kengcheng + "考试";
                        }
                        else
                        {
                            r.Curriculum_Id = kengcheng;
                        }
                        r.NewDate = DateTime.Now;
                        r.Curse_Id = time;
                        r.IsDelete = false;
                        new_list.Add(r);
                    }

                    if (Reconcile_Entity.Inser_list(new_list))
                    {
                        db.Append("ok");
                    }
                }
                else
                {
                    db.Append("请联系教学主任安排"+ class_Id+"班级"+"的"+ kengcheng+"课程的教学老师！！！");
                }
                           
            }
            return Json(db.ToString(),JsonRequestBehavior.AllowGet);
        }
        //排课数据
        public ActionResult GetTableData(int limit ,int page)
        {
            string classname = Request.QueryString["classname"];//班级名称
            if (string.IsNullOrEmpty(classname))
            {                   
                return Json(new { code = 0, msg = "", count = 0, data = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<Reconcile> lisr_r = Reconcile_Entity.GetList().Where(r => r.ClassSchedule_Id == classname).ToList();
                var mydata = lisr_r.Skip((page - 1) * limit).Take(limit).Select(r => new {
                    Id = r.Id,
                    classname = r.ClassSchedule_Id,//班级名称
                    classroom = ReconcileManeger.Classroom_Entity.GetEntity(r.ClassRoom_Id).ClassroomName,//教室
                    curriName = r.Curriculum_Id,//课程
                    Sketime = r.Curse_Id,//课程时间字段
                    ADate = r.AnPaiDate,
                    Teacher=Employees_Entity.GetEntity(r.EmployeesInfo_Id).EmpName
                });
                var jsondata = new { code = 0, msg = "", count = lisr_r.Count, data = mydata };
                return Json(jsondata, JsonRequestBehavior.AllowGet);
            }
             
        }
        //修改排课数据页面
        public ActionResult EditView(int id)
        {
            //获取有效教室
            int base_id = ReconcileManeger.ClassSchedule_Entity.BaseDataEnum_Entity.GetSingData("继善高科校区", false).Id;
            ViewBag.Editclassrrrom = Reconcile_Entity.GetEffectioveClassRoom(base_id).Select(c => new SelectListItem() { Text = c.ClassroomName, Value = c.Id.ToString() }).ToList();
            Reconcile find_r= Reconcile_Entity.GetEntity(id);
            return View(find_r);
        }

        //生成课表页面
        public ActionResult NewsRexoncileView()
        {
            //Mydate d= Reconcile_Entity.GetMydate(Convert.ToDateTime("2019-11-12"));
            //获取当前登录员是哪个角色是哪个校区的教务
            int base_id = ReconcileManeger.ClassSchedule_Entity.BaseDataEnum_Entity.GetSingData("继善高科校区", false).Id;
            Reconcile_Entity.mmm(Convert.ToDateTime("2019-11-13"), base_id);
            return View();
        }
       //手动排课页面
        public ActionResult ManualReconcileView()
        {
            //获取阶段
            //加载所有阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.Child_grandlist = g_list;
            //获取课程类型
            List<SelectListItem> t_list= ReconcileManeger.CourseType_Entity.GetCourseTypes().Select(t=>new SelectListItem() { Text=t.TypeName,Value=t.TypeName}).ToList();
            t_list.Add(new SelectListItem() { Text="其他",Value="0"});
            ViewBag.Child_typelist= t_list;
            return View();
        }
        #endregion
    }
}