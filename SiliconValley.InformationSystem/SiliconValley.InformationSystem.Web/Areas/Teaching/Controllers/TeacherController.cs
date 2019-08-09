using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Util;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Util;


    [CheckLogin]
    public class TeacherController : Controller
    {
        // GET: Teaching/Teacher


        // 教员上下文
        private readonly TeacherBusiness db_teacher;

        private readonly EmployeesInfoManage db_emp;

        private readonly TecharOnstageBearingBusiness db_teacher_sBearing;

        private readonly SpecialtyBusiness db_specialty;

        public TeacherController()
        {
            db_teacher = new TeacherBusiness();
            db_emp = new EmployeesInfoManage();
            db_teacher_sBearing = new TecharOnstageBearingBusiness();
            db_specialty = new SpecialtyBusiness();
        }
        public ActionResult TeachersInfo()
        {

            return View();
        }

        public ActionResult TeacherData(int limit,int page)
        {



            var list = db_teacher.GetList().Skip((page -1) * limit).Take(limit);

            var returnlist = new List<object>();


            //组装返回对象
            foreach (var item in list)
            {
                var emp = db_emp.GetList().Where(t => t.EmployeeId == item.EmployeeId).FirstOrDefault();

                //获取专业信息
                var major = db_teacher_sBearing.GetList().Where(t => t.TeacherID == item.TeacherID).FirstOrDefault();

               var majorresult = db_specialty.GetList().Where(t => t.Id == major.Major).FirstOrDefault(); ;


                var obj = new {
                    TeacherID = item.TeacherID,
                    AttendClassStyle = item.AttendClassStyle,
                    ProjectExperience = item.ProjectExperience,
                    TeachingExperience = item.TeachingExperience,
                    WorkExperience = item.WorkExperience,
                    TeacherName = emp.EmpName,
                    Major = majorresult.SpecialtyName
                };

                returnlist.Add(obj);


            }

            //var returnlist = list.Select(x => new { TeacherID = x.TeacherID, TeacherName = x.TeachingExperience, WorkExperience = x.WorkExperience });

            var objresult = new
            {
                code = 0,
                msg = "",
                count = list.Count(),
                data = returnlist
            };

            return Json(objresult, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 对教员的操作的视图
        /// </summary>
        /// <param name="ID">教员Id</param>
        /// <returns>视图</returns>
        [HttpGet]
        public ActionResult Operating(int? ID)
        {

            if (ID == null)
            {
                return View();

            }



            Teacher teacher = db_teacher.GetList().Where(t => t.TeacherID == ID).ToList().FirstOrDefault();

            var emp = db_emp.GetList().Where(t=>t.EmployeeId==teacher.EmployeeId).FirstOrDefault();


            //获取专业信息


            //var major = db_teacher_sBearing.GetList().Where(t => t.TeacherID == ID).FirstOrDefault();

            var majorresult = db_specialty.GetList().Select(d=>new SelectListItem() { Value=d.Id.ToString(),Text=d.SpecialtyName});

            ViewBag.Major = majorresult;

            ViewBag.Emp = emp;

            return View(teacher);

        }


        public ActionResult GetTeacherByID(int Id)
        {
           Teacher teacher = db_teacher.GetList().Where(t => t.TeacherID == Id).ToList().FirstOrDefault();

            return Json(teacher, JsonRequestBehavior.AllowGet);


        }
        /// <summary>
        /// 修改教员信息
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns>结果类</returns>

        [HttpPost]
        public ActionResult DoEdit(Teacher teacher)
        {
            AjaxResult result = null;

           var t = db_teacher.GetList().Where(x => x.TeacherID == teacher.TeacherID).FirstOrDefault();

            t.AttendClassStyle = teacher.AttendClassStyle.Trim();
            t.ProjectExperience = teacher.ProjectExperience.Trim();
            t.TeachingExperience = teacher.TeachingExperience.Trim();
            t.WorkExperience = teacher.WorkExperience.Trim();

            try
            {
                db_teacher.Update(t);

                result = new SuccessResult();
                result.Msg = "修改成功";

                result.Success = true;

            }
            catch (Exception)
            {
                result = new ErrorResult();
                result.ErrorCode = 500;
                result.Msg = "服务器错误1";

               
               
            }


            return Json(result,JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 教员详细
        /// </summary>
        /// <param name="id">教员ID</param>
        /// <returns>教员详细视图</returns>
        public ActionResult TeacherDetailView(int id)
        {

            TeacherDetailView teacherResult = new TeacherDetailView();

            //获取教员信息
             Teacher t = db_teacher.GetTeacherByID(id);

            //获取教员基本信息
             EmployeesInfo emp = db_teacher.GetEmpByEmpNo(t.EmployeeId);
            teacherResult.EmpNo = emp.EmployeeId;
            teacherResult.Birthday = emp.Birthday;
            teacherResult.Name = emp.EmpName;
            teacherResult.Phone = emp.Phone;
            teacherResult.Sex = emp.Sex;
            teacherResult.TeacherID = t.TeacherID;
            
            //获取教员阶段信息
             teacherResult.Grands = db_teacher.GetGrandByTeacherID(t.TeacherID);

            //获取教员专业信息
            teacherResult.Major= db_teacher.GetMajorByTeacherID(t.TeacherID);
           
            //获取技术信息
            teacherResult.AttendClassStyle = t.AttendClassStyle;
            teacherResult.ProjectExperience = t.ProjectExperience;
            teacherResult.TeachingExperience = t.TeachingExperience;
            teacherResult.WorkExperience = t.WorkExperience;

            return View(teacherResult);

        }

        /// <summary>
        /// 编辑教员专业 阶段视图
        /// </summary>
        /// <param name="id">教员Id</param>
        /// <returns>视图</returns>
        public ActionResult EditMajorAndGrandView(int id)
        {
            //获取教员专业阶段

            Dictionary<Specialty, Grand> dic = db_teacher.GetMajorInGrandByTeacherID(id);

            Dictionary<Specialty, List<Grand>> result = new Dictionary<Specialty, List<Grand>>();

            foreach (var key in dic.Keys)
            {
                //List<Grand> s = new List<Grand>();
                if (ContainDic(result, key))
                {
                    result[key].Add(dic[key]);
                }
                else
                {
                    List<Grand> grandlist = new List<Grand>();
                    grandlist.Add(dic[key]);

                    result.Add(key, grandlist);
                }
            }

            ViewBag.MajorAndGrand_Dic = result;

            return View();

        }

        /// <summary>
        /// 判断key是否在另一个字典包含
        /// </summary>
        /// <returns></returns>
        public bool ContainDic(Dictionary<Specialty, List<Grand>> source, Specialty key)
        {

            foreach (var item in source.Keys)
            {
                if(item==key)
                {

                    return true;
                }
            }

            return false;

        }

    }
}