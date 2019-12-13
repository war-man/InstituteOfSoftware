using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
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
    public class StudnetIntentionController : Controller
    {
        private EmpClassBusiness dbempClass;
        //private EIntentionClassXMLHelp eIntentionClassXMLHelp;
        private ProStudentInformationBusiness dbproStudentInformation;
        private StudentIntentionBusiness dbstudentIntention;
        private EmploymentAreasBusiness dbemploymentAreas;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private EmploymentJurisdictionBusiness dbemploymentJurisdiction;
        private EmploymentStaffBusiness dbemploymentStaff;
        private StudentDataKeepAndRecordBusiness dbstudentDataKeepAndRecord;
        private ProClassSchedule dbproClassSchedule;
        private EmploymentAreasBusiness dbarea;
        private EmpQuarterClassBusiness dbempQuarterClass;
        private EmpStaffAndStuBusiness dbempStaffAndStu;
        // GET: Obtainemployment/StudnetIntention
        public ActionResult StudnetIntentionIndex()
        {
            //1：获取登陆用户的信息 但是我是在用测试阶段 所以使用一个简单的 empid  杨雪：201908220012
            dbempClass = new EmpClassBusiness();
            var list = dbempClass.GetEmpClassesByempinfoid("201908220012");
            var aa = list.Select(a => new
            {
                ClassNumber = a.ClassId
            }).ToList();
            ViewBag.list = Newtonsoft.Json.JsonConvert.SerializeObject(aa);
            return View();
        }




        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="leave">1为年度，2为计划，3为班级</param>
        /// <param name="string1">学生编号</param>
        /// <param name="string2">eg:年度是2019  计划7  班级1001</param>
        /// <returns></returns>
        public ActionResult table00(int page, int limit, string leave, string string1, string string2)
        {

            dbemploymentStaff = new EmploymentStaffBusiness();
            dbemploymentJurisdiction = new EmploymentJurisdictionBusiness();
            dbstudentIntention = new StudentIntentionBusiness();
            dbstudentDataKeepAndRecord = new StudentDataKeepAndRecordBusiness();
            dbemploymentAreas = new EmploymentAreasBusiness();
            dbproStudentInformation = new ProStudentInformationBusiness();
            var data = new List<StudnetIntention>();
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            EmploymentStaff queryempstaff = dbemploymentStaff.GetEmploymentByEmpInfoID(user.EmpNumber);
            bool isJurisdiction = dbemploymentJurisdiction.isstaffJurisdiction(user);

            switch (leave)
            {
                case "1":
                    var year = int.Parse(string2);
                    if (!isJurisdiction)
                    {
                        //专员 自己带班的数据
                        data = dbstudentIntention.GetStudnetIntentionsByYearAndEmpid(year, queryempstaff.ID);
                    }
                    else
                    {
                        //主任 全部班级带班记录
                        data = dbstudentIntention.GetStudnetIntentionsByYear(year);
                    }

                    break;
                case "2":
                    var quarterid = int.Parse(string2);
                    if (!isJurisdiction)
                    {
                        //专员 自己带班的数据
                        data = dbstudentIntention.GetIntentionsByEmpidAndQuarterid(quarterid, queryempstaff.ID);
                    }
                    else
                    {
                        //主任 全部班级带班记录
                        data = dbstudentIntention.GetIntentionsByQuarterid(quarterid);
                    }

                    break;
                case "3":
                    var classid = int.Parse(string2);
                    data = dbstudentIntention.GetStudnetIntentionsByclassid(classid);
                    break;
            }

            if (!string.IsNullOrEmpty(string1))
            {
                List<string> selfobtainid = string1.Split('-').ToList();
                for (int i = data.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < selfobtainid.Count; j++)
                    {
                        if (data[i].ID.ToString() != selfobtainid[j])
                        {
                            if (j == selfobtainid.Count - 1)
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

            List<StudentIntentionView> Viewdata = new List<StudentIntentionView>();

            foreach (var item in data)
            {
                StudentIntentionView view = new StudentIntentionView();
                view.ID = item.ID;
                view.AreaName = dbemploymentAreas.GetEntity(item.AreaID).AreaName;
                var studentobj = dbproStudentInformation.GetEntity(item.StudentNO);
                view.Familyphone = studentobj.Familyphone;
                var stringlist = studentobj.Guardian.Split(',');
                view.RelativesName = stringlist[0].Replace(" ", "");
                view.Relationship = stringlist[1].Replace(" ", "");
                view.sex = studentobj.Sex == true ? "男" : "女";
                view.StudentName = studentobj.Name;
                view.Telephone = studentobj.Telephone;
                var studentDataKeepAndRecordobj = dbstudentDataKeepAndRecord.GetEntity(studentobj.StudentPutOnRecord_Id);
                view.StuSchoolName = studentDataKeepAndRecordobj.StuSchoolName;
                view.Salary = item.Salary;
                view.Nation = studentobj.Nation;
                view.Familyphone = studentobj.Familyphone;
                view.identitydocument = studentobj.identitydocument;
                view.Date = item.Date;
                Viewdata.Add(view);
            }
            var resultdata1 = Viewdata.OrderByDescending(a => a.Date).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = Viewdata.Count(),
                data = resultdata1
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 修改传入过来的意向id
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult uptstudnetintention(int param0)
        {

            dbstudentIntention = new StudentIntentionBusiness();
            dbproStudentInformation = new ProStudentInformationBusiness();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbemploymentAreas = new EmploymentAreasBusiness();
            dbproClassSchedule = new ProClassSchedule();
            var querydata = dbstudentIntention.GetEntity(param0);
            var stuobj = dbproStudentInformation.GetEntity(querydata.StudentNO);
            StudentIntentionView view = new StudentIntentionView();
            var undres = dbproScheduleForTrainees.GetTraineesByStudentNumber(querydata.StudentNO);
            view.classnumnber = dbproClassSchedule.GetEntity(undres.ID_ClassName).ClassNumber;
            var studentobj = dbproStudentInformation.GetEntity(querydata.StudentNO);
            view.StudentNO = stuobj.StudentNumber;
            view.StudentName = studentobj.Name;
            view.Telephone = studentobj.Telephone;
            view.Salary = querydata.Salary;
            view.AreaID = querydata.AreaID;
            view.ID = param0;
            ViewBag.querydata = Newtonsoft.Json.JsonConvert.SerializeObject(view);

            var aa = dbemploymentAreas.GetAll();
            var cc = aa.Select(a => new
            {
                a.AreaName,
                a.ID
            }).ToList();
            ViewBag.city = Newtonsoft.Json.JsonConvert.SerializeObject(cc);
            return View();

        }

        [HttpPost]
        /// <summary>
        /// 修改意向
        /// </summary>
        /// <returns></returns>
        public ActionResult upttyping(StudentIntentionView param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbstudentIntention = new StudentIntentionBusiness();
                var StudnetIntentionobj = dbstudentIntention.GetEntity(param0.ID);

                if (StudnetIntentionobj != null)
                {
                    dbproStudentInformation = new ProStudentInformationBusiness();
                    var studentobj = dbproStudentInformation.GetStudent(param0.StudentNO);
                    if (studentobj.Telephone != param0.Telephone)
                    {
                        studentobj.Telephone = param0.Telephone;
                        dbproStudentInformation.Update(studentobj);
                    }
                    if (param0.AreaID != 0)
                    {
                        StudnetIntentionobj.AreaID = param0.AreaID;
                        StudnetIntentionobj.Salary = param0.Salary;
                        dbstudentIntention.Update(StudnetIntentionobj);
                        ajaxResult.Success = true;
                    }
                    else
                    {
                        ajaxResult.Success = false;
                        ajaxResult.Msg = "莫乱搞。";
                    }

                }
                else
                {
                    ajaxResult.Success = false;
                    ajaxResult.Msg = "莫乱搞。";
                }
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请及时联系就业班主任!";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param0">班级id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult add(int param0)
        {
            dbproClassSchedule = new ProClassSchedule();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbstudentIntention = new StudentIntentionBusiness();
            dbarea = new EmploymentAreasBusiness();
            var classobj = dbproClassSchedule.GetEntity(param0);
            List<StudentInformation> list1 = dbstudentIntention.GetSurplusStudent(param0);
            List<EmploymentAreas> list2 = dbarea.GetAll();
            ViewBag.list1 = Newtonsoft.Json.JsonConvert.SerializeObject(list1);
            ViewBag.list2 = Newtonsoft.Json.JsonConvert.SerializeObject(list2);
            ViewBag.param0 = classobj.ClassNumber;
            ViewBag.param1 = param0;
            return View();
        }

        [HttpPost]
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult add(StudentIntentionView param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbempQuarterClass = new EmpQuarterClassBusiness();
                dbemploymentStaff = new EmploymentStaffBusiness();
                dbstudentIntention = new StudentIntentionBusiness();
                dbproStudentInformation = new ProStudentInformationBusiness();
                Base_UserModel user = Base_UserBusiness.GetCurrentUser();
                var queryempstaff = dbemploymentStaff.GetEmploymentByEmpInfoID(user.EmpNumber);
                var qmpquarter = dbempQuarterClass.GetEmpQuarters().Where(a => a.Classid == param0.classid).FirstOrDefault();
                dbstudentIntention = new StudentIntentionBusiness();
                StudnetIntention intention = new StudnetIntention();
                intention.AreaID = param0.AreaID;
                intention.Date = DateTime.Now;
                intention.IsDel = false;
                intention.isdistribution = false;
                intention.Remark = param0.Remark;
                intention.Salary = param0.Salary;
                intention.StudentNO = param0.StudentNO;
                intention.Empinfoid = queryempstaff.ID;
                intention.QuarterID = qmpquarter.QuarterID;
                dbstudentIntention.Insert(intention);
                StudentInformation student = dbproStudentInformation.GetEntity(param0.StudentNO);
                student.Telephone = param0.Telephone;
                dbproStudentInformation.Update(student);
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
        /// 根据传入过来的意向id删除 如果这个学生已经分配了就不能进行删除
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult del(int param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbempStaffAndStu = new EmpStaffAndStuBusiness();
                dbstudentIntention = new StudentIntentionBusiness();
                StudnetIntention intention = dbstudentIntention.GetEntity(param0);
                if (dbempStaffAndStu.isdistribution(intention.StudentNO))
                {
                    ajaxResult.Success = false;
                    ajaxResult.Msg = "该学生已经被安排就业！";
                }
                else
                {
                    intention.IsDel = true;
                    dbstudentIntention.Update(intention);
                    ajaxResult.Success = true;
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