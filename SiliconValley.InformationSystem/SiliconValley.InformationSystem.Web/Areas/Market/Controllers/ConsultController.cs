using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Business.Consult_Business;
using SiliconValley.InformationSystem.Entity.Entity;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class ConsultController : BaseMvcController
    {
        ConsultManeger CM_Entity = new ConsultManeger();

        // GET: /Market/Consult/SingleDataView
        public ActionResult ConsultIndex()
        {
            ViewBag.data = CM_Entity.GetConsultTeacher().Select(c => new ConsultShowData
            {
                empId = c.Employees_Id,
                Id = c.Id,
                empName = CM_Entity.GetEmplyeesInfo(c.Employees_Id).EmpName
            }).ToList();
            return View();
        }
        /// <summary>
        /// 显示单个咨询师的分量数据
        /// </summary>
        /// <returns></returns>
        public ActionResult SingleDataView()
        {
            return View();
        }
    }
}