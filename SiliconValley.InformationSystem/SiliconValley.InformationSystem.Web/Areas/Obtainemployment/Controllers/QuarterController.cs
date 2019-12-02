using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    /// <summary>
    /// 季度
    /// </summary>
    public class QuarterController : Controller
    {

        private QuarterBusiness dbquarter;
        private ProClassSchedule dbproClassSchedule;
        private EmpQuarterClassBusiness dbempQuarterClass;
        // GET: Obtainemployment/Quarter
        public ActionResult QuarterIndex()
        {
            dbquarter = new QuarterBusiness();
            var querydata = dbquarter.yearplan();
            ViewBag.list = Newtonsoft.Json.JsonConvert.SerializeObject(querydata);
            return View();
        }

        /// <summary>
        /// 添加计划页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult addquarter()
        {
            dbproClassSchedule = new ProClassSchedule();
            var querydata = dbproClassSchedule.GetClassGraduating();
            ViewBag.classlist = Newtonsoft.Json.JsonConvert.SerializeObject(querydata);
            return View();
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult addquarter(Quarter param0, string param1)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbquarter = new QuarterBusiness();
                dbempQuarterClass = new EmpQuarterClassBusiness();
                string[] classlist = param1.Split(',');
                if (classlist.Length != 0)
                {
                    param0.IsDel = false;
                    var nowtime = DateTime.Now;
                    param0.RegDate = nowtime;
                    dbquarter.Insert(param0);
                    dbquarter = new QuarterBusiness();
                    var bb = dbquarter.GetQuarters().Where(a => a.RegDate.ToString() == nowtime.ToString()).FirstOrDefault(); ;
                    for (int i = 0; i < classlist.Length; i++)
                    {
                        EmpQuarterClass empQuarterClass = new EmpQuarterClass();
                        empQuarterClass.Classid =int.Parse(classlist[i]);
                        empQuarterClass.IsDel = false;
                        empQuarterClass.QuarterID = bb.ID;
                        empQuarterClass.RegDate = DateTime.Now;
                        empQuarterClass.Remark = string.Empty;
                        dbempQuarterClass.Insert(empQuarterClass);
                    }
                }
                else
                {
                    ajaxResult.Success = false;
                    ajaxResult.Msg = "莫乱搞！";
                }

                ajaxResult.Success = true;


            }
            catch (Exception ex)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请及时反馈信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult SearchData(int page, int limit, string param0)
        {

            dbquarter = new QuarterBusiness();
            dbempQuarterClass = new EmpQuarterClassBusiness();
            var nowdate = DateTime.Now.Year;
            var querydata = dbquarter.GetQuartersByYear(nowdate);
            if (!string.IsNullOrEmpty(param0))
            {
                querydata = querydata.Where(a => a.RegDate.Year == int.Parse(param0)).ToList();
            }
            var resultlist = new List<QuarterView>();
            foreach (var item in querydata)
            {
                QuarterView view = new QuarterView();
                view.ID = item.ID;
                view.QuaTitle = item.QuaTitle;
                view.RegDate = item.RegDate;
                view.Remark = item.Remark;
                view.peoplenumber = item.peoplenumber;
                var querylist1 = dbempQuarterClass.GetEmpQuartersByYearID(item.ID);
                var selectlist = querylist1.Select(a => new
                {
                    a.ID,
                    a.Classid
                }).ToList();
                view.EmpQuarterClassList = selectlist;
                resultlist.Add(view);

            }
            var resultdata1 = resultlist.OrderByDescending(a => a.RegDate).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = resultlist.Count(),
                data = resultdata1
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 修改毕业计划
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult uptquarter(int param0)
        {
            dbquarter = new QuarterBusiness();
            dbempQuarterClass = new EmpQuarterClassBusiness();
            dbproClassSchedule = new ProClassSchedule();
            var checkclasslist = new List<ClassSchedule>();
            var queryquarter = dbquarter.GetEntity(param0);
            var querydata = dbproClassSchedule.GetClassGraduating();
            var queryempQuarterclass = dbempQuarterClass.GetEmpQuartersByYearID(queryquarter.ID);
            foreach (var item in queryempQuarterclass)
            {
                var queryclass = dbproClassSchedule.GetEntity(item.Classid);
                checkclasslist.Add(queryclass);
            }
            querydata.AddRange(checkclasslist);
            ViewBag.classlist = Newtonsoft.Json.JsonConvert.SerializeObject(querydata);
            ViewBag.checkclasslist = Newtonsoft.Json.JsonConvert.SerializeObject(checkclasslist);
            ViewBag.queryformdata = Newtonsoft.Json.JsonConvert.SerializeObject(queryquarter);

            return View();
        }


        /// <summary>
        /// post提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult uptquarter(Quarter param0, string param1)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbquarter = new QuarterBusiness();
                dbempQuarterClass = new EmpQuarterClassBusiness();
                var queryquarter = dbquarter.GetEntity(param0.ID);
                var quarterclasslist = dbempQuarterClass.GetEmpQuartersByYearID(param0.ID);
                List<string> classlist = param1.Split(',').ToList();

                if (classlist.Count != 0)
                {
                    queryquarter.QuaTitle = param0.QuaTitle;
                    queryquarter.Remark = param0.Remark;
                    queryquarter.peoplenumber = param0.peoplenumber;
                    dbquarter.Update(queryquarter);

                    for (int i = classlist.Count - 1; i >= 0; i--)
                    {
                        for (int j = quarterclasslist.Count - 1; j >= 0; j--)
                        {
                            if (classlist[i] == quarterclasslist[j].Classid.ToString())
                            {
                                classlist.Remove(classlist[i]);
                                quarterclasslist.Remove(quarterclasslist[j]);
                                break;
                            }
                        }
                    }
                    foreach (var item in quarterclasslist)
                    {
                        item.IsDel = true;
                        dbempQuarterClass.Update(item);
                    }
                    for (int i = 0; i < classlist.Count; i++)
                    {
                        EmpQuarterClass empQuarterClass = new EmpQuarterClass();
                        empQuarterClass.Classid = int.Parse(classlist[i]);
                        empQuarterClass.IsDel = false;
                        empQuarterClass.QuarterID = queryquarter.ID;
                        empQuarterClass.RegDate = DateTime.Now;
                        empQuarterClass.Remark = string.Empty;
                        dbempQuarterClass.Insert(empQuarterClass);
                    }
                }
                else
                {
                    ajaxResult.Success = false;
                    ajaxResult.Msg = "莫乱搞！";
                }

                ajaxResult.Success = true;


            }
            catch (Exception ex)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请及时反馈信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

    }
}