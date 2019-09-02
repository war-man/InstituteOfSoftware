using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.Consult_Business;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class ConsultTeacherController : Controller
    {
        ConsultTeacherManeger Ct_Entiry = new ConsultTeacherManeger();
        // GET: Market/ConsultTeacher/ConsultTeacherIndex
        public ActionResult ConsultTeacherIndex()
        {
            //获取所有咨询师数据
            List<ConsultTeacher> c_list = Ct_Entiry.GetList();
            ViewBag.data = c_list.Select(c => new {
                EmpName = Ct_Entiry.SeracherEmp( c.Employees_Id).EmpName,
                Phone = Ct_Entiry.SeracherEmp(c.Employees_Id).Phone,
                Education = Ct_Entiry.SeracherEmp(c.Employees_Id).Education,
                ConsultType =c.ConsultType==false?"初中":"高中",
                BrainImage=c.BrainImage
            }).ToList();
            return View();
        }
    }
}