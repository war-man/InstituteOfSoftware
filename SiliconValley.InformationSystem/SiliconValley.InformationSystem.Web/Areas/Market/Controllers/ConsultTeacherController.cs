using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.Consult_Business;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class ConsultTeacherController : Controller
    {
        ConsultTeacherManeger Ct_Entiry = new ConsultTeacherManeger();
        // GET: /Market/ConsultTeacher/GetfontTemplateData
        public ActionResult ConsultTeacherIndex()
        {
            //获取所有咨询师数据
            List<ConsultTeacher> c_list = Ct_Entiry.GetList();
            ViewBag.data = c_list.Select(c => new T_ConsultTeacherData()
            {
                Id=c.Id,
                EmpName = Ct_Entiry.SeracherEmp(c.Employees_Id).EmpName,
                Phone = Ct_Entiry.SeracherEmp(c.Employees_Id).Phone,
                Education = Ct_Entiry.SeracherEmp(c.Employees_Id).Education,
                BrainImage = c.BrainImage
            }).ToList();
            return View();
        }

        public ActionResult ConsultTeacherDefalut()//int id
        {
            //根据咨询师去找员工编号
            //ConsultTeacher findc= Ct_Entiry.GetEntity(id);
            //EmployeesInfo finde = Ct_Entiry.SeracherEmp(findc.Employees_Id);

            return View();
        }
    }
}