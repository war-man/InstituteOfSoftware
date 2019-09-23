using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Util;
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Entity.Base_SysManage;
    using SiliconValley.InformationSystem.Business;
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;

    /// <summary>
    /// 满意度调查控制器
    /// </summary>
    /// 
    [CheckLogin]
    public class SatisfactionSurveyController : Controller
    {


        // GET: Teaching/SatisfactionSurvey
        BaseBusiness<Department> db_dep = new BaseBusiness<Department>();

        BaseBusiness<EmployeesInfo> db_emp = new BaseBusiness<EmployeesInfo>();

        BaseBusiness<Headmaster> db_headmaster = new BaseBusiness<Headmaster>();

        Base_UserMapRoeBusinessL db_userrole = new Base_UserMapRoeBusinessL();

        private readonly SatisfactionSurveyBusiness db_survey;

        TeacherClassBusiness db_teacherclass = new TeacherClassBusiness();

        BaseBusiness<ClassSchedule> db_class = new BaseBusiness<ClassSchedule>();

        private readonly TeacherBusiness db_teacher;

        public SatisfactionSurveyController()
        {
            db_survey = new SatisfactionSurveyBusiness();



            db_teacher = new TeacherBusiness();
        }
        public ActionResult SatisfactionIndex()
        {


            var permisslist = PermissionManage.GetOperatorPermissionValues();

            ViewBag.Permisslist = permisslist;

            return View();
        }


        /// <summary>
        /// 满意度调查配置文件
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfigSetting()
        {

            return View();
        }

        public ActionResult satisfactionItemSettingView()
        {
            //获取调查项



            //获取所有部门
            ViewBag.Department = db_dep.GetList().Where(d => d.IsDel == false).ToList();

            return View();
        }

        public ActionResult satisfactionItemTypeSettingView()
        {

            //获取所有部门
            ViewBag.Department = db_dep.GetList().Where(d => d.IsDel == false).ToList();



            return View();

        }


        /// <summary>
        /// 获取调查具体项
        /// </summary>
        /// <param name="DepID">部门ID</param>
        /// <param name="itemTypeid">类型ID</param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult GetSurveyItemData(int DepID, int itemTypeid, int page, int limit)
        {


            List<SatisfactionSurveyView> resultlist = new List<SatisfactionSurveyView>();

            try
            {
                var list = db_survey.Screen(DepID, itemTypeid);

                foreach (var item in list)
                {

                    var viewobj = db_survey.ConvertModelView(item);

                    if (viewobj != null)
                    {
                        resultlist.Add(viewobj);
                    }
                }

            }
            catch (Exception ex)
            {

                Base_UserBusiness.WriteSysLog("查询数据出错了 位置 ：满意度调查GetSurveyData ", EnumType.LogType.加载数据);
            }

            int count = resultlist.Count;

            var obj = new {

                code = 0,
                msg = "",
                count = count,
                data = resultlist.Skip((page - 1) * limit).Take(limit).ToList()


            };

            return Json(obj, JsonRequestBehavior.AllowGet);


        }



        /// <summary>
        /// 获取调查项类型数据
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult GetSurveyItemTypeData(string typename, int depid)
        {
            AjaxResult result = new AjaxResult();

            List<SatisficingType> resultlist = new List<SatisficingType>();

            try
            {
                resultlist = db_survey.Screen(typename, depid);

                result.ErrorCode = 200;
                result.Msg = "成功";
                result.Data = resultlist;

            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Msg = ex.Message;
                result.Data = resultlist;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 添加调查项类型
        /// </summary>
        /// <param name="typename">类名称</param>
        /// <param name="depid">部门</param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult AddSurveyItemType(string typename, int depid)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                db_survey.AddSurveyItemType(typename, depid);

                result.ErrorCode = 200;
                result.Msg = "成功";
                result.Data = null;
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Msg = ex.Message;
                result.Data = null;
            }



            return Json(result, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public ActionResult GetGetSurveyItemTypeDataByDepid(int depid)
        {
            AjaxResult result = new AjaxResult();

            List<SatisficingType> resultlist = new List<SatisficingType>();

            try
            {

                resultlist = db_survey.Screen(null, depid);

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

        /// <summary>
        /// 添加满意度调查具体项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddSurveyItem()
        {
            //提供部门数据

            ViewBag.Dep = db_dep.GetList().Where(d => d.IsDel == false).ToList();

            return View();
        }

        /// <summary>
        /// 添加满意度调查具体项
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddSurveyItem(SatisficingItem satisficingItem)
        {
            AjaxResult result = new AjaxResult();

            satisficingItem.IsDel = false;

            try
            {
                db_survey.Insert(satisficingItem);

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


            return Json(result);

        }


        /// <summary>
        /// 调查项内容列表视图
        /// </summary>
        /// <param name="itemtypeid">类型id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SurveylistView(int itemtypeid)
        {

            //提供具体调查项

            ViewBag.itemlist = db_survey.GetAllSatisfactionItems().Where(d => d.ItemType == itemtypeid).ToList();


            return View();

        }


        /// <summary>
        /// 删除调查项类型
        /// </summary>
        /// <returns></returns>
        public ActionResult DelItemType(int typeid)
        {

            AjaxResult result = new AjaxResult();


            try
            {
                db_survey.RemoveItemType(typeid);

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
        /// 删除调查具体项
        /// </summary>
        /// <param name="itemid">具体项ID</param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult delSurveyItem(int itemid)
        {

            AjaxResult result = new AjaxResult();

            try
            {
                var delobj = db_survey.GetAllSatisfactionItems().Where(d => d.ItemID == itemid).FirstOrDefault();

                db_survey.Delete(delobj);

                result.ErrorCode = 200;
                result.Msg = "成功";
                result.Data = null;
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Msg = ex.Message;
                result.Data = null;
            }


            return Json(result, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 满意度调查记录视图
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveyHistoryView()
        {

            //判断登录的角色

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();

            //获取员工的岗位

            var teacherview = db_teacher.GetTeacherView(teacher.TeacherID);

            //获取员工权限

            ViewBag.TeacherView = teacherview;


            var permisslist = PermissionManage.GetOperatorPermissionValues();

            ViewBag.Permisslist = permisslist;



            return View();






        }


        /// <summary>
        /// 获取员工的满意度调查记录
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SurveyHistoryData(string empid, string date, string classnumber,int Curriculum)
        {


            if (string.IsNullOrEmpty(empid))
            {
                //判断登录的角色

                Base_UserModel user = Base_UserBusiness.GetCurrentUser();

               

                empid = user.EmpNumber;


            }


            AjaxResult result = new AjaxResult();

            var ss = DateTime.Parse(date);

            List<SatisfactionSurveyDetailView> resultlist = new List<SatisfactionSurveyDetailView>();


            try
            {



                resultlist = db_survey.SurveyHistoryData(empid, date, Curriculum, classnumber);

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



        /// <summary>
        /// 获取阶段教员
        /// </summary>
        /// <returns></returns>

        public ActionResult selectTeacherByGrand(int page, int limit)
        {
            //判断条件需要改为权限


            AjaxResult result = new AjaxResult();
            List<EmployeesInfo> resultlist = new List<EmployeesInfo>();

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();


            var emp = db_emp.GetList().Where(d => d.EmployeeId == teacher.EmployeeId).FirstOrDefault();



            //S1S2教学主任

            if (emp.PositionId == 3 || emp.PositionId == 2011)
            {

                var list1 = db_teacher.BrushSelectionByGrand(1);


                var list2 = db_teacher.BrushSelectionByGrand(2);

                list1.AddRange(list2);

                var templist = list1.Distinct().ToList();

                foreach (var item in templist)
                {

                    var obj = db_emp.GetList().Where(d => d.IsDel == false && d.EmployeeId == item.EmployeeId).FirstOrDefault();

                    resultlist.Add(obj);
                }

            }

            //S3教学主任

            if (emp.PositionId == 2013 || emp.PositionId == 2014)
            {

                var list2 = db_teacher.BrushSelectionByGrand(3);


                foreach (var item in list2)
                {

                    var obj = db_emp.GetList().Where(d => d.IsDel == false && d.EmployeeId == item.EmployeeId).FirstOrDefault();

                    resultlist.Add(obj);
                }



            }

            //S4教学主任

            if (emp.PositionId == 2015 || emp.PositionId == 2016)
            {

                var list2 = db_teacher.BrushSelectionByGrand(3);


                foreach (var item in list2)
                {

                    var obj = db_emp.GetList().Where(d => d.IsDel == false && d.EmployeeId == item.EmployeeId).FirstOrDefault();

                    resultlist.Add(obj);
                }



            }


            if (emp.PositionId == 2012)
            {

                EmployeesInfoManage employeesInfoManage = new EmployeesInfoManage();


                var templist = db_emp.GetList();

                foreach (var item in templist)
                {
                    var depid = employeesInfoManage.GetDept(item.PositionId);

                    if (depid.DeptId == 2)
                    {
                        resultlist.Add(item);
                    }

                }



            }




            var objresult = new {

                code = 0,
                msg = "",
                count = resultlist.Count,
                data = resultlist.Skip((page - 1) * limit).Take(limit)

            };



            return Json(objresult, JsonRequestBehavior.AllowGet);



        }


        /// <summary>
        /// 获取阶段班主任
        /// </summary>
        /// <returns></returns>
        public ActionResult selectClassLaderByGrand()
        {
            //未完成

            AjaxResult result = new AjaxResult();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 选择员工视图
        /// </summary>
        /// <returns></returns>
        public ActionResult selectEmpView()
        {


            return View();
        }


        /// <summary>
        /// 获取满意度调查记录
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public ActionResult SurveyHistoryDataByID(int id)
        {
            AjaxResult result = new AjaxResult();



            try
            {
                //***获取对象的岗位在决定获取那个部门的数据

                var obj1 = db_survey.GetSatisficingResultByID(id);

                var obj2 = db_survey.ConvertToViewModel(obj1);


                EmployeesInfoManage empmanage = new EmployeesInfoManage();
                var dep = empmanage.GetDept(obj2.Emp.PositionId);

                //获取了部门的调查类型

                var templist = db_survey.Screen(null, dep.DeptId);

                var list1 = new List<string>();

                //组装数据
                foreach (var item in templist)
                {

                    list1.Add(item.TypeName);

                }


                List<object> list2 = new List<object>();
                var deatilitemlist = obj2.detailitem;

                foreach (var item in templist)
                {

                    var templist4 = deatilitemlist.Where(d => d.SatisficingItem.ItemID == item.ID).ToList();

                    var score = 0;

                    foreach (var item1 in templist4)
                    {
                        score += (int)item1.Scores;
                    }


                    // 调查类型和分数的对象

                    var itemtypesocreobj = new
                    {

                        value = score,
                        name = item.TypeName

                    };

                    list2.Add(itemtypesocreobj);



                }


                //最后返回的对象
                var obj = new
                {

                    itemTypelist = list1,
                    Data = obj2,
                    itemTypeScores = list2
                };



                result.ErrorCode = 200;
                result.Data = obj;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {

                result.ErrorCode = 200;
                result.Data = null;
                result.Msg = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);



        }


        /// <summary>
        /// 获取班级
        /// </summary>
        /// <returns></returns>
        /// 

        public ActionResult GetClassNumber(int page, int limit)
        {



            List<ClassTableView> resultlist = new List<ClassTableView>();
            List<ClassTableView> returnlist = new List<ClassTableView>();

            //

            //当前用户
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();
            //角色列表
            var list = db_userrole.CurrentUserRoles();

            //教学校长角色roleID
            var roel1 = db_userrole.GetSysRoleAtConfig("039e4630ba5a1-31320968-d1dd-4275-ae71-a06da0731a2d");

            //S1S2教学主任roleid
            var role2 = db_userrole.GetSysRoleAtConfig("039e4630ba5a1-31320968-d1dd-4275-ae71-a06da0731a2e");

            //S1S2教学副主任roleid
            var role3 = db_userrole.GetSysRoleAtConfig("039e4630ba5a1-31320968-d1dd-4275-ae71-a06da0731a2f");

            //S3教学主任Roleid
            var role4 = db_userrole.GetSysRoleAtConfig("039e4630ba5a1-31320968-d1dd-4275-ae71-a06da0731a2g");
            //S3教学副主任roleid 
            var role5 = db_userrole.GetSysRoleAtConfig("039e4630ba5a1-31320968-d1dd-4275-ae71-a06da0731a2h");

            //S4教学主任
            var role6 = db_userrole.GetSysRoleAtConfig("039e4630ba5a1-31320968-d1dd-4275-ae71-a06da0731a2i");

            //S4教学副主任
            var role7 = db_userrole.GetSysRoleAtConfig("039e4630ba5a1-31320968-d1dd-4275-ae71-a06da0731a2j");

            //班主任
            var role8 = db_userrole.GetSysRoleAtConfig("039e4630ba5a1-31320968-d1dd-4275-ae71-a06da0731a2l");

            //教员
            var role9 = db_userrole.GetSysRoleAtConfig("039e4630ba5a1-31320968-d1dd-4275-ae71-a06da0731a2j");

            foreach (var item in list)
            {

                //教学校长
                if (item.RoleId == roel1.RoleId)
                {

                    //所有班级列表
                    var classlist = db_class.GetList().Where(d => d.IsDelete == false).ToList();

                    foreach (var item1 in classlist)
                    {

                        //转换类型
                        var tempobj = db_teacherclass.GetClassTableView(item1);
                        resultlist.Add(tempobj);

                    }
                }

                //S1S2教学主任 S1S2教学副主任
                if (item.RoleId == role2.RoleId || item.RoleId == role3.RoleId)
                {
                    //获取S1S2班级

                    //所有班级列表
                    var classlist = db_class.GetList().Where(d => d.IsDelete == false).ToList();

                    //筛选出S1S2的班级

                    foreach (var item1 in classlist)
                    {

                        if (item1.grade_Id == 1 || item1.grade_Id == 2)
                        {

                            //转换类型
                            var tempobj = db_teacherclass.GetClassTableView(item1);
                            resultlist.Add(tempobj);
                        }

                    }
                }


                //S3教学主任 S3教学副主任
                if (item.RoleId == role4.RoleId || item.RoleId == role5.RoleId)
                {
                    //获取S3班级

                    //所有班级列表
                    var classlist = db_class.GetList().Where(d => d.IsDelete == false).ToList();

                    //筛选出S3的班级

                    foreach (var item1 in classlist)
                    {

                        if (item1.grade_Id == 3)
                        {

                            //转换类型
                            var tempobj = db_teacherclass.GetClassTableView(item1);
                            resultlist.Add(tempobj);
                        }

                    }

                }

                //S4教学主任 S4教学副主任

                if (item.RoleId == role6.RoleId || item.RoleId == role7.RoleId)
                {

                    //获取S4的班级

                    //所有班级列表
                    var classlist = db_class.GetList().Where(d => d.IsDelete == false).ToList();

                    //筛选出S3的班级

                    foreach (var item1 in classlist)
                    {

                        if (item1.grade_Id == 4)
                        {

                            //转换类型
                            var tempobj = db_teacherclass.GetClassTableView(item1);
                            resultlist.Add(tempobj);
                        }

                    }




                }


                //教员
                if (item.RoleId == role9.RoleId)
                {
                    //获取自己的班级


                    var templist = db_teacherclass.GetCrrentMyClass(teacher.TeacherID);

                    foreach (var item1 in templist)
                    {
                        //转换类型
                        var tempobj = db_teacherclass.GetClassTableView(item1);
                        resultlist.Add(tempobj);
                    }
                }

                //班主任
                if (item.RoleId == role8.RoleId)
                {
                    //未完成
                }


            }

            //去掉重复的班级

            foreach (var item in resultlist)
            {
                if (!IsContainClass(returnlist, item))
                {
                    returnlist.Add(item);
                }

            }

            var obj = new {

                code = 0,
                msg = "",
                count = returnlist.Count,
                data = returnlist.Skip((page - 1) * limit).Take(limit).ToList()
            };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// 判断班级列表里面是否存在了一个班级
        /// </summary>
        /// <returns></returns>
        public bool IsContainClass(List<ClassTableView> sorces, ClassTableView classtableview)
        {

            foreach (var item in sorces)
            {
                if (item.ClassNumber == classtableview.ClassNumber)
                {
                    return true;
                }
            }

            return false;

        }


        public ActionResult selectClassView()
        {
            return View();
        }

        /// <summary>
        /// 获取我的班级
        /// </summary>
        /// <returns></returns>
        public ActionResult GetClassNumberByEmp(string EmpID, int limit, int page)
        {

            List<ClassTableView> resultlist = new List<ClassTableView>();

            //判断员工是哪个部门的

           var emp = db_emp.GetList().Where(d => d.EmployeeId == EmpID).FirstOrDefault();

            EmployeesInfoManage empmanage = new EmployeesInfoManage();

            var dep = empmanage.GetDept(emp.PositionId);


            //如果是教质部
            if (dep.DeptId == 1)
            {

                var headmaster = db_headmaster.GetList().Where(d => d.informatiees_Id == emp.EmployeeId).FirstOrDefault();

                BaseBusiness<HeadClass> db_masterclass = new BaseBusiness<HeadClass>();

               var templist = db_masterclass.GetList().Where(d => d.LeaderID == headmaster.ID).ToList();

                var classtemplist = new List<ClassSchedule>();

                foreach (var item in templist)
                {

                   var tempobj = db_class.GetList().Where(d => d.ClassNumber == item.ClassID).FirstOrDefault();

                    resultlist .Add( db_teacherclass.GetClassTableView(tempobj));
                }

            }

            //如果是教学部

            if (dep.DeptId == 2)
            {

                //获取班级

                var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == emp.EmployeeId).FirstOrDefault();

                var templist  = db_teacherclass.GetCrrentMyClass(teacher.TeacherID);

                foreach (var item in templist)
                {
                    resultlist.Add( db_teacherclass.GetClassTableView(item));
                }

            }


            var obj = new {

                code=0,
                msg="",
                count=resultlist.Count,
                data=resultlist.Skip((page-1)*limit).Take(limit).ToList()
            };

            return Json(obj, JsonRequestBehavior.AllowGet);



        }



        /// <summary>
        /// 满意度调查详细视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SurveyDetail(int id)
        {
            var obj = db_survey.GetSatisficingResultByID(id);

           var resultobj = db_survey.ConvertToViewModel(obj);

            ViewBag.SurveyDetail = resultobj;

            return View();
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="EmpID"></param>
        /// <param name="ClassNumber"></param>
        /// <returns></returns>
        public ActionResult GetPieImageData(string EmpID, string ClassNumber, int courseid, string date)
        {

            //首先判断员工部门
            var emp = db_emp.GetList().Where(d => d.EmployeeId == EmpID).FirstOrDefault();

            EmployeesInfoManage empmanage = new EmployeesInfoManage();

            var dep = empmanage.GetDept(emp.PositionId);


            var list = db_survey.SurveyHistoryData(EmpID, date, courseid, ClassNumber);

            //下面进行组装数据

            //获取调查类型

            List<SatisficingType> temptypelist = new List<SatisficingType>();

            List<string> typelist = new List<string>();


            temptypelist = temptypelist = db_survey.GetSatisficingTypes().Where(d => d.DepartmentID == dep.DeptId).ToList();

            foreach (var item in temptypelist)
            {
                typelist.Add(item.TypeName);
            }


            List<PieServiceHelper> tyepscorelist = new List<PieServiceHelper>();

            //创建帮助类
            foreach (var item in temptypelist)
            {
                PieServiceHelper pieServiceHelper = new PieServiceHelper();
                pieServiceHelper.id = item.ID;
                pieServiceHelper.name = item.TypeName;
                pieServiceHelper.value = 0;

                tyepscorelist.Add(pieServiceHelper);
            }


            List<object> itemscoreObj = new List<object>();

            foreach (var item in list)
            {

                foreach (var item1 in item.detailitem)
                {

                    tyepscorelist.Where(d => d.id == item1.SatisficingItem.ItemType).FirstOrDefault().value += (int)item1.Scores;

                }

            }



            var obj = new {

                typelist = typelist,

                tyepscorelist = tyepscorelist
            };



            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取教员在班级教过的课程
        /// </summary>
        /// <param name="empid">员工ID/param>
        /// <param name="classnumber">班级编号 </param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCursor(string empid, string classnumber)
        {


            AjaxResult result = new AjaxResult();
            List<Curriculum> list = new List<Curriculum>();
            CourseBusiness db_course = new CourseBusiness();
            List<Curriculum> Resultlist = new List<Curriculum>();
            try
            {
                //获取教员
                var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == empid).FirstOrDefault();

                //排课业务类
                BaseBusiness<Reconcile> db_reconile = new BaseBusiness<Reconcile>();

                //排课集合
                var templist = db_reconile.GetList().Where(d => d.Teacher_Id == teacher.TeacherID && d.ClassSchedule_Id == classnumber).ToList();

              

              

                foreach (var item in templist)
                {
                    list.Add(db_course.GetCurriculas().Where(d => d.CurriculumID == item.Curriculum_Id &&d.IsDelete==false).FirstOrDefault());
                }

                //去掉重复项

                foreach (var item in list)
                {
                    if (!db_course.isContain(Resultlist, item))
                    {
                        Resultlist.Add(item);
                    }
                }

                result.ErrorCode = 200;
                result.Data = Resultlist;
                result.Msg = "成功";

            }
            catch (Exception ex)
            {
                result.ErrorCode = 500;
                result.Data = Resultlist;
                result.Msg = ex.Message;

            }


            return Json(result,JsonRequestBehavior.AllowGet);


        }
    }
} 