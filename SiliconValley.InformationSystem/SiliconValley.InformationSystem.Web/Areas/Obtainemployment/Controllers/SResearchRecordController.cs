using SiliconValley.InformationSystem.Business.Employment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    public class SResearchRecordController : Controller
    {
        private EmpClassBusiness dbempClassBusiness;
        // GET: Obtainemployment/SResearchRecord
        public ActionResult SResearchRecordIndex()
        {
            return View();
        }

        /// <summary>
        /// 访谈学生页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SResearchRecordRegister() {

            //1：获取登陆用户的信息 但是我是在用测试阶段 所以使用一个简单的 empid  杨雪：201908220012

            dbempClassBusiness = new EmpClassBusiness();
            var list= dbempClassBusiness.GetEmpClassesByempinfoid("201908220012");
            var aa= list.Select(a => new
            {
                ClassNumber = a.ClassNO
            }).ToList();
            ViewBag.list = Newtonsoft.Json.JsonConvert.SerializeObject(aa);
            return View();
        }
    }
}