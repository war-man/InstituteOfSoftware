using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Util;
    using SiliconValley.InformationSystem.Business;
    using SiliconValley.InformationSystem.Entity.Base_SysManage;
    using System.IO;
    [CheckLogin]
    public class ProjectController : Controller
    {
        // GET: Teaching/Project
        private readonly TeacherClassBusiness db_teacherclass;
        private readonly TeacherBusiness db_teacher;
        private readonly ProjectBusiness db_project;

        public ProjectController()
        {
            db_teacher = new TeacherBusiness();
            db_teacherclass = new TeacherClassBusiness();
            db_project = new ProjectBusiness();


        }

        public ActionResult ProjectIndex()
        {
          ViewBag.ProjectTypes =  db_project.ProjectTypes();


            return View();
        }


        [HttpGet]
        public ActionResult operasProjectView(int id)
        {

            ViewBag.Project = db_project.ConvertToDetailView(db_project.GetList().Where(d => d.ProjectID == id).FirstOrDefault());



            return View();
        }


        /// <summary>
        /// 获取所有项目
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult GetAllProject(int page)
        {

            AjaxResult result = new AjaxResult();

            List<ProjectDetailView> resultlist = new List<ProjectDetailView>();

            try
            {
                Base_UserModel user = Base_UserBusiness.GetCurrentUser();

                var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();

                var list = db_project.GetProjectTasks();


                foreach (var item in list)
                {
                    var obj = db_project.ConvertToDetailView(item);

                    if (obj != null)
                    {
                        resultlist.Add(obj);
                    }
                }


                result.ErrorCode = 200;
                result.Data = resultlist;
                result.Msg = "成功";




            }
            catch (Exception ex)
            {
                result.ErrorCode = 500;
                result.Data = resultlist;
                result.Msg = ex.Message;
            }


            var objresult = new
            {

                status = 0,
                message = "",
                total = resultlist.Count,
                data = resultlist.Skip((page - 1) * 6).Take(6).ToList()

            };

            return Json(objresult, JsonRequestBehavior.AllowGet);


        }

        /// <summary>
        /// 创建新的项目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateProjectView()
        {
            //提供项目类型
            BaseBusiness<ProjectType> db_projecttype = new BaseBusiness<ProjectType>();

            var list = db_projecttype.GetList().ToList();
            ViewBag.projecttype = list;

            return View();
        }

        public ActionResult SelectStudentView()
        {
            //获取当前登陆的教员

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();

            var classlist = db_teacherclass.GetCrrentMyClass(teacherid: teacher.TeacherID);

            ViewBag.classlist = classlist;

            return View();
        }

        /// <summary>
        /// 创建新的项目 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateProject(ProjectTasks project)
        {
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();

            AjaxResult result = new AjaxResult();

            project.BeginDate = DateTime.Now;
            project.IsDel = false;
            //project.EndDate =null;
            project.Tutor = teacher.TeacherID;

            try
            {
                db_project.Insert(project);
                result.Data = null;
                result.ErrorCode = 200;
                result.Msg = "成功";

                Base_UserBusiness.WriteSysLog("创建项目", EnumType.LogType.添加数据);


            }
            catch (Exception ex)
            {

                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = ex.Message;

                Base_UserBusiness.WriteSysLog(ex.Message, EnumType.LogType.添加数据);
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetProejctGroup(int projectid)
        {


            var result = db_project.ConvertToDetailView(db_project.GetProjectTasks().Where(d => d.ProjectID == projectid).FirstOrDefault());

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取组员的项目模块
        /// </summary>
        /// <param name="studentnumber"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProjectModularByStudent(string studentnumber, string projectid)
        {
            AjaxResult result = new AjaxResult();

            List<ProjectGroupMember> resultlist = new List<ProjectGroupMember>();

            try
            {
                resultlist = db_project.GetProjectModularByStudent(studentnumber, int.Parse(projectid));
                result.ErrorCode = 200;
                result.Data = resultlist;
                result.Msg = "成功";

            }
            catch (Exception ex)
            {
                result.ErrorCode = 500;
                result.Data = resultlist;
                result.Msg = ex.Message;
            }





            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddGroupItem(string studentnumber, int projectid)
        {

            AjaxResult result = new AjaxResult();

            var temp1 = studentnumber.Split(',');

            var temp3 = temp1.ToList();
            temp3.RemoveAt(temp1.Length - 1);

            try
            {


                db_project.AddGroupItem(temp3, projectid);
                result.ErrorCode = 200;
                result.Msg = "成功";
                result.Data = null;


            }
            catch (Exception ex)
            {

                result.ErrorCode = 200;
                result.Msg = "失败";
                result.Data = ex.Message;
            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UngetProjectItem(int classnumber, int projectid)
        {

            return Json(db_project.UngetProjectItem(projectid, classnumber));

        }


        /// <summary>
        /// 给组员添加模块
        /// </summary>
        /// <returns></returns>
        public ActionResult AddModular(string projectid, string studentnumber, string ModularName)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                db_project.AddModular(projectid, studentnumber, ModularName);

                result.Data = null;
                result.ErrorCode = 200;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {

                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = "失败";
            }

            return Json(result, JsonRequestBehavior.AllowGet);



        }


        /// <summary>
        /// 删除项目组成员
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="studentnumber"></param>
        /// <returns></returns>
        public ActionResult RemoveTeamItem(int projectid, string studentnumber)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                db_project.RemoveTeamItem(projectid, studentnumber);

                result.Data = null;
                result.ErrorCode = 200;
                result.Msg = "成功";

            }
            catch (Exception ex)
            {

                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = "失败";
            }


            return Json(result, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 修改项目组长
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult updateGroupLearder(int projectid, string newstudentnumber)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                db_project.updateGroupLearder(projectid, newstudentnumber);

                result.Data = null;
                result.Msg = "成功";
                result.ErrorCode = 200;
            }
            catch (Exception)
            {

                result.Data = null;
                result.Msg = "失败";
                result.ErrorCode = 500;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 项目进度视图
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public ActionResult Projectschedule(int projectid)
        {

            //返回项目信息

            var obj = db_project.GetProjectTasks().Where(d => d.ProjectID == projectid).FirstOrDefault();

            var resultonj = db_project.ConvertToDetailView(obj);

            ViewBag.Project = resultonj;

            return View();
        }


        /// <summary>
        /// 获取项目组员信息
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult ProjectTeaminfo(int projectid)
        {
            AjaxResult result = new AjaxResult();

            List<ProjectTeamDetailView> resultlist = new List<ProjectTeamDetailView>();

            try
            {
                var templist = db_project.ProjectTeamInfo(projectid);

                foreach (var item in templist)
                {

                    var obj = db_project.GetTeamDetailView(item);

                    if (obj != null)
                    {
                        resultlist.Add(obj);
                    }

                }

                result.ErrorCode = 200;
                result.Msg = "成功";
                result.Data = resultlist;

            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Msg = "失败";
                result.Data = resultlist;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }



        public ActionResult GetModuler(int projectid, string studentnumber)
        {

            AjaxResult result = new AjaxResult();

            List<ProjectGroupMember> resultlist = new List<ProjectGroupMember>();
            try
            {
                resultlist = db_project.GetProjectModularByStudent(studentnumber, projectid);

                result.ErrorCode = 200;

                result.Data = resultlist;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {
                result.ErrorCode = 200;

                result.Data = resultlist;
                result.Msg = ex.Message;
            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 改变项目模块进度状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditModelStatus(int modelid, string status, int projectid)
        {
            var currentstatus = status == "true" ? true : false;

            //判断项目是否已经停止开发

            var project = db_project.GetProjectTasks().Where(d => d.ProjectID == projectid).FirstOrDefault();

            AjaxResult result = new AjaxResult();

            if (project.IsStop == true)
            {

                result.ErrorCode = 300;
                result.Data = null;
                result.Msg = "失败";

            }
            else

            {
                try
                {

                    db_project.EditModelStatus(modelid, currentstatus);

                    result.ErrorCode = 200;
                    result.Data = null;
                    result.Msg = "成功";

                }
                catch (Exception ex)
                {

                    result.ErrorCode = 500;
                    result.Data = null;
                    result.Msg = "失败";
                }
            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 获取项目没有完成的模块
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult GetNoFinshModel(int projectid)
        {
            AjaxResult result = new AjaxResult();

            List<ProjectGroupMember> resultlist = new List<ProjectGroupMember>();

            try
            {
                resultlist = db_project.GetNoFinshModel(projectid);

                result.ErrorCode = 200;
                result.Data = resultlist;
                result.Msg = "成功";

            }
            catch (Exception exd)
            {

                result.ErrorCode = 500;
                result.Data = resultlist;
                result.Msg = exd.Message;
            }


            return Json(result, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 停止项目的开发
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public ActionResult StopProject(int projectid)
        {

            AjaxResult result = new AjaxResult();

            try
            {
                db_project.StopProject(projectid);

                result.ErrorCode = 200;
                result.Data = null;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = null;
                result.Msg = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);


        }



        /// <summary>
        /// 项目的图片展示管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProjectShowImage(int projectid)
        {

            var project = db_project.GetProjectTasks().Where(d => d.ProjectID == projectid).FirstOrDefault();

            List<string> imagelist = new List<string>();

            if (project.ShowImages != null)
            {

                var ary1 = project.ShowImages.Split(',').ToList();
                ary1.RemoveAt(ary1.Count - 1);

                if (ary1.Count > 0)
                {
                    foreach (var item in ary1)
                    {

                        imagelist.Add(item);

                    }
                }
            }



            ViewBag.imagelist = imagelist;

            //提供项目信息

            ProjectDetailView projectinfo = db_project.ConvertToDetailView(db_project.GetProjectTasks().Where(d => d.ProjectID == projectid).FirstOrDefault());

            ViewBag.Project = projectinfo;

            return View();


        }


        /// <summary>
        /// 项目图片的上传
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectImageUpload(int projectid)
        {

            AjaxResult result = new AjaxResult();

            result.ErrorCode = 200;
            result.Data = null;
            result.Msg = "成功";

            var files = Request.Files;

            string names = string.Empty;

            if (files.Count > 0)
            {
                for (int i = 0; i < files.Count; i++)
                {

                   var filename = files[i].FileName;
                    //获取扩展名

                   var exten = Path.GetExtension(filename);

                    TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);

                    var tt = Convert.ToInt64(ts.TotalSeconds).ToString();

                    var timespan = tt;

                    // 保存文件

                    try
                    {
                        files[i].SaveAs(Server.MapPath("/Areas/Teaching/Images/ProjectShowImages/" + timespan + exten));

                        //将文件名称存入数据库

                       var project = db_project.GetProjectTasks().Where(d => d.ProjectID == projectid).FirstOrDefault();

                        project.ShowImages += timespan + exten + ",";

                        db_project.Update(project);


                    }
                    catch (Exception ex)
                    {
                        result.ErrorCode = 500;
                        result.Data = null;
                        result.Msg = "图片保存失败";

                        break;
                    }

                    


                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveProjectImage(int projectid, string image)
        {
            // 1,2,3,4,5,
            AjaxResult result = new AjaxResult();

            try
            {
                var project = db_project.GetProjectTasks().Where(d => d.ProjectID == projectid).FirstOrDefault();

                var images = project.ShowImages;

                string newimages = images.Replace(image + ",", "");

                project.ShowImages = newimages;

                db_project.Update(project);


                result.ErrorCode = 200;
                result.Data = null;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = null;
                result.Msg = ex.Message;
            }

          




            return Json(result, JsonRequestBehavior.AllowGet);

        }



        /// <summary>
        /// 对项目进行筛选
        /// </summary>
        /// <param name="projectname">项目名称  </param>
        /// <param name="date">日期上限</param>
        /// <param name="projecttypeid">项目类型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult preparationProject(string projectname, string date, int projecttypeid, int page)
        {


            AjaxResult result = new AjaxResult();

            List<ProjectDetailView> resultlist = new List<ProjectDetailView>();


            try
            {
                if (string.IsNullOrEmpty(date) && projecttypeid == 0)
                {

                    //按项目名称查询

                    var templist = db_project.GetProjectTasks().Where(d => d.ProjectName.Contains(projectname)).ToList();

                    foreach (var item in templist)
                    {
                        var obj = db_project.ConvertToDetailView(item);

                        resultlist.Add(obj);
                    }


                }
                else if (string.IsNullOrEmpty(date))
                {

                    //查询条件 ：项目名称、类型


                    var templist = db_project.GetProjectTasks().Where(d => d.ProjectName.Contains(projectname) && d.ProjectType == projecttypeid).ToList();

                    foreach (var item in templist)
                    {
                        var obj = db_project.ConvertToDetailView(item);

                        resultlist.Add(obj);
                    }

                }
                else if (projecttypeid == 0)
                {
                    //查询条件 ：项目名称、时间
                    var templist = db_project.GetProjectTasks().Where(d => d.ProjectName.Contains(projectname) && d.BeginDate >=DateTime.Parse(date)).ToList();

                    foreach (var item in templist)
                    {
                        var obj = db_project.ConvertToDetailView(item);

                        resultlist.Add(obj);
                    }
                }
                else
                {
                    //查询条件 ：项目名称、时间
                    var templist = db_project.GetProjectTasks().Where(d => d.ProjectName.Contains(projectname) && d.BeginDate >= DateTime.Parse(date) && d.ProjectType == projecttypeid).ToList();

                    foreach (var item in templist)
                    {
                        var obj = db_project.ConvertToDetailView(item);

                        resultlist.Add(obj);
                    }
                }


                result.ErrorCode = 200;
                result.Data = resultlist;
                result.Msg = "成功";

            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = resultlist;
                result.Msg = ex.Message;
            }


            var objresult = new
            {

                status = 0,
                message = "",
                total = resultlist.Count,
                data = resultlist.Skip((page - 1) * 6).Take(6).ToList()

            };

            return Json(objresult, JsonRequestBehavior.AllowGet);
        }
    }
}