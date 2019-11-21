using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    public class StudnetIntentionController : Controller
    {
        private EmpClassBusiness dbempClass;
        private EIntentionClassXMLHelp eIntentionClassXMLHelp;
        // GET: Obtainemployment/StudnetIntention
        public ActionResult StudnetIntentionIndex()
        {
            //1：获取登陆用户的信息 但是我是在用测试阶段 所以使用一个简单的 empid  杨雪：201908220012
            dbempClass = new EmpClassBusiness();
            var list = dbempClass.GetEmpClassesByempinfoid("201908220012");
            var aa = list.Select(a => new
            {
                ClassNumber = a.ClassNO
            }).ToList();
            ViewBag.list = Newtonsoft.Json.JsonConvert.SerializeObject(aa);
            return View();
        }

        /// <summary>
        /// 开启这个班的学生有权限使用这个填写就业意向表
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult open(string param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                eIntentionClassXMLHelp = new EIntentionClassXMLHelp();
                if (eIntentionClassXMLHelp.isexistence(param0))
                {
                    ajaxResult.Success = false;
                    ajaxResult.Msg = "该班级学生已经可以填写就业意向表！";
                }
                else
                {
                    if (eIntentionClassXMLHelp.AddEIntentionClass(param0))
                    {
                        ajaxResult.Success = true;
                    }
                    else
                    {
                        ajaxResult.Success = false;
                        ajaxResult.Msg = "请联系信息部成员！";
                    }
                }
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
    }
}