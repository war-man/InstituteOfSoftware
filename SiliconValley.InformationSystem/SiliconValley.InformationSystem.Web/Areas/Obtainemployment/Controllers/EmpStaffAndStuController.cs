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
    public class EmpStaffAndStuController : Controller
    {
        private EmpStaffAndStuBusiness dbempStaffAndStu;
        private EmploymentAreasBusiness dbemploymentAreas;
        private EmploymentStaffBusiness dbemploymentStaff;
        private StudentIntentionBusiness dbstudentIntention;
        private ProStudentInformationBusiness dnproStudentInformation;
        // GET: Obtainemployment/EmpStaffAndStu
        public ActionResult EmpStaffAndStuIndex()
        {
            dbemploymentAreas = new EmploymentAreasBusiness();

            var list1 = dbemploymentAreas.GetAll();
            ViewBag.arealist = list1;
            dbemploymentStaff = new EmploymentStaffBusiness();
            var empstaff = dbemploymentStaff.GetALl().Select(a => new {
                a.ID,
                EmpStaffName = dbemploymentStaff.GetEmpInfoByEmpID(a.ID).EmpName
            }).ToList();
            ViewBag.empstaff = Newtonsoft.Json.JsonConvert.SerializeObject(empstaff);

            return View();
        }
        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="string0">地区id</param>
        /// <param name="string1">学生编号</param>
        /// <returns></returns>
        public ActionResult table00(int page, int limit, int string0, string string1)
        {

            dbempStaffAndStu = new EmpStaffAndStuBusiness();
            List<EmpStaffAndStuView> data = dbempStaffAndStu.Getnodistribution();

            if (string0 != -1)
            {
                data = data.Where(a => a.AreaID == string0).ToList();
            }
            if (!string.IsNullOrEmpty(string1))
            {
                List<string> studentno = string1.Split('-').ToList();
                for (int i = data.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < data.Count; j++)
                    {
                        if (data[i].StudentNO.ToString() != studentno[j])
                        {
                            if (j == studentno.Count - 1)
                            {
                                data.Remove(data[i]);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            var data1 = data.OrderByDescending(a => a.EmploymentStage).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = data.Count(),
                data = data1
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        /// <summary>
        /// 分配添加
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult distribution(addEmpStaffAndStuView param0) {


            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbstudentIntention = new StudentIntentionBusiness();
                dbempStaffAndStu = new EmpStaffAndStuBusiness();
                foreach (var item in param0.quarterIDAndStudentnos)
                {
                    ///先获取第一次跟第二次，如果第二次，判断第一次这个员工id是否等于这个当前传入过来的员工id，如果等于，判断第二次是否为null，
                    ///如果不为null，isdel改为true，如果没有就没有，该第一次的isdistribution=true；
                    ///判断这个学生是否在第二阶段出现过，如果有就只要修改这个员工编号就行
                    ///判断这个学生是否出现过在第一阶段中 如果有变成第二阶段 如果没有就是第一阶段
                    ///第一次是员工A 转交给员工B 之后B搞不定又转交给A



                    var obj1 = dbempStaffAndStu.GetStage1Bystudentno(item.Studentno);
                    var obj2 = dbempStaffAndStu.GetStage2Bystudentno(item.Studentno);
                    if (obj1.EmpStaffID == param0.EmpStaffID)
                    {
                        if (obj2 != null)
                        {
                            obj2.IsDel = true;
                            dbempStaffAndStu.Update(obj2);
                        }

                        obj1.Ising = true;
                        dbempStaffAndStu.Update(obj1);

                    }
                    else
                    {
                        if (obj2 != null)
                        {
                            obj2.EmpStaffID = param0.EmpStaffID;
                        }
                        else
                        {
                            EmpStaffAndStu andStu = new EmpStaffAndStu();
                            andStu.Date = DateTime.Now;
                            if (obj1 != null)
                            {
                                andStu.EmploymentStage = 2;
                            }
                            else
                            {
                                andStu.EmploymentStage = 1;
                            }

                            andStu.EmploymentState = 1;
                            andStu.EmpStaffID = param0.EmpStaffID;
                            andStu.IsDel = false;
                            andStu.QuarterID = item.QuarterID;
                            andStu.Remark = string.Empty;
                            andStu.Studentno = item.Studentno;
                            andStu.Ising = true;
                            dbempStaffAndStu.Insert(andStu);
                            

                        }
                    }
                    StudnetIntention intention = dbstudentIntention.GetInformationBystudentno(item.Studentno);
                    intention.isdistribution = true;
                    dbstudentIntention.Update(intention);

                }

                ajaxResult.Success = true;
            }
            catch (Exception ex)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 根据专员id返回带学生记录
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult loadstudent(int param0) {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {

                dbempStaffAndStu = new EmpStaffAndStuBusiness();
                dnproStudentInformation = new ProStudentInformationBusiness();
                var data= dbempStaffAndStu.GetEmploymentState1ByEmpid(param0);
                ajaxResult.Data = data.Select(a => new
                {
                    a.Studentno,
                    studentname = dnproStudentInformation.GetEntity(a.Studentno).Name
                }).ToList();
                ajaxResult.Success = true;
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        ///加载学生的的信息
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult loadform(string param0) {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbempStaffAndStu = new EmpStaffAndStuBusiness();
                ajaxResult.Data= dbempStaffAndStu.studentnoconversionempstaffandstubiew(param0);
                ajaxResult.Success = true;
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
    
        }

        /// <summary>
        /// 取消分配
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult Redistribution(string param0) {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbempStaffAndStu = new EmpStaffAndStuBusiness();
                dbstudentIntention = new StudentIntentionBusiness();
                var aa = dbempStaffAndStu.GetEmpStaffAndStuingBystudentno(param0);
                aa.Ising = false;
                dbempStaffAndStu.Update(aa);
                var bb = dbstudentIntention.GetStudnetIntentionByStudentNO(param0);
                bb.isdistribution = false;
                dbstudentIntention.Update(bb);
                ajaxResult.Success = true;
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