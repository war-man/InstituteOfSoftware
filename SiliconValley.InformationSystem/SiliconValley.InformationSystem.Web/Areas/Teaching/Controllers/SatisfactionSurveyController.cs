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
    using SiliconValley.InformationSystem.Util;
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Entity.Base_SysManage;
    using SiliconValley.InformationSystem.Business;
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    /// <summary>
    /// 满意度调查控制器
    /// </summary>
    /// 
    [CheckLogin]
    public class SatisfactionSurveyController : Controller
    {


        // GET: Teaching/SatisfactionSurvey
        BaseBusiness<Department> db_dep = new BaseBusiness<Department>();

        private readonly SatisfactionSurveyBusiness db_survey;
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
        public ActionResult GetSurveyItemTypeData(string  typename, int depid)
        {
            AjaxResult result = new AjaxResult();

            List<SatisficingType> resultlist = new List<SatisficingType>();

            try
            {
                resultlist = db_survey.Screen(typename,  depid);

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

            return Json(result,JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 添加调查项类型
        /// </summary>
        /// <param name="typename">类名称</param>
        /// <param name="depid">部门</param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult AddSurveyItemType(string typename,int depid)
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

            return Json (result, JsonRequestBehavior.AllowGet);
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
                result.Msg =ex.Message;
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


            return Json(result,JsonRequestBehavior.AllowGet);

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
           

            return View();






        }


        /// <summary>
        /// 获取员工的满意度调查记录
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SurveyHistoryData(string empid, string date)
        {

            AjaxResult result = new AjaxResult();

            var ss = DateTime.Parse(date);

            List<SatisfactionSurveyDetailView> resultlist = new List<SatisfactionSurveyDetailView>();


            try
            {

                resultlist = db_survey.SurveyHistoryData(empid, date);

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


            return Json(result,JsonRequestBehavior.AllowGet);

        }


    }
}