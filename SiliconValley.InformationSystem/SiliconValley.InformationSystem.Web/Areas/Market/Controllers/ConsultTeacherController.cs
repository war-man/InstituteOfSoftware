using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.Consult_Business;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    [CheckLogin]
    public class ConsultTeacherController : Controller
    {
        ConsultTeacherManeger Ct_Entiry = new ConsultTeacherManeger();
        private EmployeesInfoManage Employsinfo_Entity;
        //获取当前上传的操作人
        Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();
        // GET: /Market/ConsultTeacher/ConsultTeacherIndex
        public ActionResult ConsultTeacherIndex()
        {
            Employsinfo_Entity = new EmployeesInfoManage();
            //获取所有咨询师数据
            List<ConsultTeacher> c_list = Ct_Entiry.GetList().Where(ct=>ct.IsDelete==false).ToList();
            List<T_ConsultTeacherData> list_data = c_list.Select(c => new T_ConsultTeacherData()
            {
                Id = c.Id,
                EmpName = Ct_Entiry.SeracherEmp(c.Employees_Id).EmpName,
                Phone = Ct_Entiry.SeracherEmp(c.Employees_Id).Phone,
                Education = Ct_Entiry.SeracherEmp(c.Employees_Id).Education,
                BrainImage = Employsinfo_Entity.GetEntity(c.Employees_Id).Image
           }).ToList();
            ViewBag.data = list_data;
            //加载下拉框数据
            ViewBag.Select= list_data.Select(l => new SelectListItem() { Text = l.EmpName, Value = l.Id.ToString() });
            return View();
        }

        public ActionResult ConsultTeacherDefalut(int id)
        {
            ViewBag.id = id;
            return View();
        }
        /// <summary>
        /// 获取给Form表单赋值的数据
        /// </summary>
        /// <param name="id">咨询师ID</param>
        /// <returns></returns>
        public ActionResult GetFormData(int id)
        {
            Employsinfo_Entity = new EmployeesInfoManage();
            //根据咨询师去找员工编号
            ConsultTeacher findc = Ct_Entiry.GetEntity(id);

            if (findc != null)
            {
                EmployeesInfo finde = Ct_Entiry.SeracherEmp(findc.Employees_Id);
                if (finde != null)
                {
                    var fromdata = new T_ConsultTeacherData()
                    {
                        EmpName = finde.EmpName,
                        Id = id,
                        Phone = finde.Phone,
                        Education = finde.Education,
                        BrainImage = Employsinfo_Entity.GetEntity( findc.Employees_Id).Image,
                        IsZhizhi = finde.IsDel,
                        Rmark = findc.Rmark,
                        Politicsstatus = finde.PoliticsStatus,
                        workExperence = finde.WorkExperience,
                        IsZhangzhe = finde.PositiveDate == null ? false : true,
                        ConGrade = findc.ConGrade
                    };
                    return Json(fromdata,JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("数据错误请重试", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("数据错误请重试", JsonRequestBehavior.AllowGet);
            }
            
        }
        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult EdiConsultTeacherFunction()
        {
            Employsinfo_Entity = new EmployeesInfoManage();
            try
            {
                string Id = Request.Form["Id"].ToString();
                string ConGrade = Request.Form["ConGrade"];
                var branImage = SessionHelper.Session["image"];
                string Rmark = Request.Form["Rmark"];
                int myid = Convert.ToInt32(Id);
                ConsultTeacher findc = Ct_Entiry.GetEntity(myid);
               
                if (!string.IsNullOrEmpty(ConGrade))
                {
                    int count = Convert.ToInt32(ConGrade);
                    findc.ConGrade = count;
                }
                if (branImage != null)
                {
                    findc.BrainImage = branImage.ToString();
                }
                findc.Rmark = Rmark;
                Ct_Entiry.Update(findc);
                BusHelper.WriteSysLog("操作人:" + Employsinfo_Entity.GetEntity(UserName.EmpNumber).EmpName + "成功编辑了咨询师数据", Entity.Base_SysManage.EnumType.LogType.编辑数据);
                return Json("ok", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog("操作人:" + Employsinfo_Entity.GetEntity(UserName.EmpNumber).EmpName + "操作时出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                return Json("数据错误！！！", JsonRequestBehavior.AllowGet);
            }             
        }
    }
}