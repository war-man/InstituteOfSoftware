using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.Consult_Business;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Business.StuSatae_Maneger;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    [CheckLogin]
    public class FollwingInfoController : BaseMvcController
    {
        ConsultManeger CM_Entity = new ConsultManeger();
        StuStateManeger ST_Entity = new StuStateManeger();
        EmployeesInfoManage Enplo_Entity;
        ConsultTeacherManeger ConsultTeacher;
        // GET: /Market/FollwingInfo/GetTableData
        
        static int f_id = 0;
        public ActionResult FollwingInfoIndex()
        {
            ConsultTeacher = new ConsultTeacherManeger();

            //判断是哪个咨询师
            //获取当前上传的操作人
            Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();
            f_id = ConsultTeacher.GetIQueryable().Where(cc => cc.Employees_Id == UserName.EmpNumber).FirstOrDefault()==null?0: ConsultTeacher.GetIQueryable().Where(cc => cc.Employees_Id == UserName.EmpNumber).FirstOrDefault().Id;
            return View();
        }
        /// <summary>
        /// 初始化柱状图
        /// </summary>
        /// <returns></returns>
        public ActionResult GetImageData()
        {
            ConsultTeacher = new ConsultTeacherManeger();

            //判断是哪个咨询师
            if (f_id!=0)
            {
                List<ConsultZhuzImageData> ConsultZhuzImageData_data = CM_Entity.GetImageData(f_id.ToString());
                return Json(ConsultZhuzImageData_data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
            
        }

        public ActionResult GetMonData(int MonthName,string Status)
        {
            List<StudentPutOnRecord> result = new List<StudentPutOnRecord>();
            if (f_id!=0)
            {
                //判断是哪个咨询师         
                if (Status == "完成量")
                {
                    result = CM_Entity.GetStudentData(MonthName, "1", f_id);
                }
                else if (Status == "未完成量")
                {
                    result = CM_Entity.GetStudentData(MonthName, "2", f_id);
                }
                else if (Status == "分量数")
                {
                    result = CM_Entity.GetStudentData(MonthName, "3", f_id);
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
             
        }
        /// <summary>
        /// 查询是否有该学生
        /// </summary>
        /// <param name="id">学生名称</param>
        /// <returns></returns>
        public ActionResult GetMonthStudent(string id)
        {
            List<ExportStudentBeanData> stu_list = CM_Entity.GetStudentPutRecored(id,true);
            List<ExportStudentBeanData> find_list = new List<ExportStudentBeanData>();
            List<Consult> find_consult = new List<Consult>();
            if (f_id!=0)
            {                
                foreach (ExportStudentBeanData item2 in stu_list)
                {
                    find_consult = CM_Entity.GetIQueryable().Where(c => c.TeacherName == f_id && c.StuName==item2.Id).ToList();//获取XX咨询师的分量情况
                    if (find_consult.Count>0)
                    {
                        find_list.Add(item2);
                    }                     
                }
                //获取这个学生的跟踪信息次数
                int j = 0;
                List<FollwingInfo> find = CM_Entity.GetFollwingManeger().GetIQueryable().ToList();
                if (find_consult.Count == 1)
                {
                    int i = 0;
                    foreach (FollwingInfo item in find)
                    {
                        foreach (Consult c in find_consult)
                        {
                            if (item.Consult_Id == c.Id)
                            {
                                i++;
                            }
                        }
                    }
                    j = i;
                    SessionHelper.Session["Number"] = j;
                }
                var jsondata = new
                {
                    data = find_list,
                    Number = j
                };
                return Json(jsondata, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
   
        }

        //这是一个添加页面
        public ActionResult AddFollwingInfo(int id)
        {
            StudentPutOnRecord find_Stu= CM_Entity.GetSingleStudent(id);
            int consult_id = CM_Entity.GetIQueryable().Where(c=>c.StuName==id).FirstOrDefault().Id;           
            SessionHelper.Session["consult_id"] = consult_id;
            int Number= CM_Entity.GetFollwingManeger().GetIQueryable().Where(c => c.Consult_Id == consult_id).ToList().Count;
            ViewBag.Name = find_Stu.StuName;
            ViewBag.Sex = find_Stu.StuSex;
            ViewBag.Number = Number;
            return View();
        }
        //这是一个添加方法
        public ActionResult AddFunction()
        {
            //获取当前上传的操作人
            Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();
            Enplo_Entity = new EmployeesInfoManage();
            try
            {
                string Rank = Request.Form["Rank"];
                string Rmark = Request.Form["Rmark"];
                string TailAfterSituation = Request.Form["TailAfterSituation"];
                FollwingInfoManeger FM_Entity = CM_Entity.GetFollwingManeger();
                FollwingInfo new_f = new FollwingInfo();
                new_f.Consult_Id = Convert.ToInt32(SessionHelper.Session["consult_id"]);
                new_f.FollwingDate = DateTime.Now;
                new_f.IsDelete = false;
                new_f.TailAfterSituation = TailAfterSituation;
                new_f.Rank = Rank;
                new_f.Rmark = Rmark;
                FM_Entity.Insert(new_f);
                BusHelper.WriteSysLog(Enplo_Entity.GetEntity(UserName.EmpNumber).EmpName +"添加了一条跟踪学生信息", Entity.Base_SysManage.EnumType.LogType.添加数据);
                return Json("ok",JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //将错误填写到日志中     
                BusHelper.WriteSysLog(Enplo_Entity.GetEntity(UserName.EmpNumber).EmpName + "添加数据时出现:"+ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                return Json("添加跟踪信息失败，请重试！！！", JsonRequestBehavior.AllowGet);
            }
             
        }

        //这是一个编辑页面
        public ActionResult EditView(int id)
        {
            StudentPutOnRecord find_stu = CM_Entity.GetSingleStudent(id);
            //获取该学生的Id
            ViewBag.Name = find_stu.StuName;
            ViewBag.Sex = find_stu.StuSex;
            //获取跟踪总数
            Consult find_consult = CM_Entity.GetIQueryable().Where(c => c.StuName == id).FirstOrDefault();
            FollwingInfoManeger FM_Entity = CM_Entity.GetFollwingManeger();
            int count= FM_Entity.GetIQueryable().Where(f => f.Consult_Id == find_consult.Id).ToList().Count;
            ViewBag.Number = count + "次";
            //获取这个学生的所有咨询信息
            Consult find_c= CM_Entity.GetIQueryable().Where(c => c.StuName == id).FirstOrDefault();
            List<FollwingInfo> flist= CM_Entity.GetFollwingManeger().GetIQueryable().Where(f => f.Consult_Id == find_c.Id).ToList();
            ViewBag.flist = flist;
            return View();
        }
        //获取某个跟踪信息
        public ActionResult GetSingFollwingData(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                int fid = Convert.ToInt32(Id);
               FollwingInfo find=  CM_Entity.GetFollwingManeger().GetEntity(fid);
                return Json(find,JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("no", JsonRequestBehavior.AllowGet);
            }
        }
        //这是一个编辑方法
        public ActionResult EditFunction()
        {
            if (f_id!=0)
            {
                try
                {
                    string MyRank = Request.Form["MyRank"];
                    string My_TailAfterSituation = Request.Form["My_TailAfterSituation"];
                    string My_Rmark = Request.Form["My_Rmark"];
                    int F_Id = Convert.ToInt32(Request.Form["F_Id"]);
                    FollwingInfo find_f = CM_Entity.GetFollwingManeger().GetEntity(F_Id);
                    find_f.Rank = MyRank;
                    find_f.Rmark = My_Rmark;
                    find_f.TailAfterSituation = My_TailAfterSituation;
                    CM_Entity.GetFollwingManeger().Update(find_f);
                    //BusHelper.WriteSysLog(Enplo_Entity.GetEntity(UserName.EmpNumber).EmpName + "成功编辑了一条跟踪信息数据", Entity.Base_SysManage.EnumType.LogType.编辑数据);
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    //BusHelper.WriteSysLog(Enplo_Entity.GetEntity(UserName.EmpNumber).EmpName + "编辑跟踪信息时出现:"+ex, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                    return Json("系统错误，请重试!!!", JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return null;
            }
             
        }
        //这是找到多个学生显示的页面
        public ActionResult ListStudentView(string id)
        {
            string[] stulist = id.Split(',');
            List<ExportStudentBeanData> student = new List<ExportStudentBeanData>();

            foreach (string item1 in stulist)
            {
                if (!string.IsNullOrEmpty(item1))
                {
                     
                     student.AddRange(CM_Entity.GetStudentPutRecored(item1,false));
                        
                }
            }
            List<StudentData> data = student.Select(s => new StudentData() {
                Id=Convert.ToInt32(s.Id),
                stuSex = s.StuSex,
                StuName = s.StuName,
                StuPhone = s.Stuphone,
                StuSchoolName = s.StuSchoolName,
                StuAddress = s.StuAddress,
                StuInfomationType_Id = s.stuinfomation,//CM_Entity.getTypeName(s.StuInfomationType_Id.ToString(), true).Name,
                StuStatus_Id = s.StatusName,//ST_Entity.GetIdGiveName(s.StuStatus_Id.ToString(),true).Success==true? (ST_Entity.GetIdGiveName(s.StuStatus_Id.ToString(), true).Data as StuStatus).StatusName:null,
                StuIsGoto = s.StuisGoto == false ? "否" : "是",
                StuVisit = s.StuVisit,
                EmployeesInfo_Id = s.empName,//CM_Entity.GetEmplyeesInfo(s.EmployeesInfo_Id).EmpName,
                StuDateTime = s.BeanDate,
                StuEntering = s.StuEntering,
                AreName =s.RegionName,//CM_Entity.GetRegionName(s.Region_id).RegionName,
                Party=s.Party
            }).ToList();
            ViewBag.Student = data;
            return View();
        }

        /// <summary>
        /// 获取分量学生数据
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult GetTableData(int limit,int page)
        {
            List<Consult> find_consult= CM_Entity.GetList().Where(c => c.TeacherName == f_id).ToList();

            StudentDataKeepAndRecordBusiness s_Entity = new StudentDataKeepAndRecordBusiness();

            List<ExportStudentBeanData> list = new List<ExportStudentBeanData>();
            foreach (Consult item in find_consult)
            {
                list.Add(s_Entity.whereStudentId(item.StuName.ToString()));
            }

            string Name = Request.QueryString["name"];
            string Phone = Request.QueryString["phone"];
            string StarDate = Request.QueryString["staTime"];
            string EndDate = Request.QueryString["endTime"];

            if (!string.IsNullOrEmpty(Name))
            {
                list = list.Where(l => l.StuName.Contains(Name)).ToList();
            }

            if (!string.IsNullOrEmpty(Phone))
            {
                list = list.Where(l => l.Stuphone==Phone).ToList();
            }

            if (!string.IsNullOrEmpty(StarDate) && StarDate!="")
            {
                DateTime d1 = Convert.ToDateTime(StarDate);
                list = list.Where(l => l.BeanDate >= d1).ToList();
            }

            if (!string.IsNullOrEmpty(EndDate) && EndDate!="")
            {
                DateTime d2 = Convert.ToDateTime(EndDate);
                list = list.Where(l => l.BeanDate <= d2).ToList();
            }
            var mydata = list.OrderByDescending(l => l.Id).Skip((page - 1) * limit).Take(limit).Select(l=>new {
                Id=l.Id,
                StuName=l.StuName,
                StuSex= l.StuSex,
                Stuphone = l.Stuphone,
                BeanDate= CM_Entity.AccordingStuIdGetConsultData(Convert.ToInt32(l.Id)).ComDate
            }).ToList();
            var data = new {data= mydata, count=list.Count,code=0,msg=""};

            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public ActionResult MyFollwingInfoIndex()
        {
            ConsultTeacher = new ConsultTeacherManeger();

            //判断是哪个咨询师

            //获取当前上传的操作人
            Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();

            f_id = ConsultTeacher.GetIQueryable().Where(cc => cc.Employees_Id == UserName.EmpNumber).FirstOrDefault() == null ? 0 : ConsultTeacher.GetIQueryable().Where(cc => cc.Employees_Id == UserName.EmpNumber).FirstOrDefault().Id;

            return View();
        }
    }
}