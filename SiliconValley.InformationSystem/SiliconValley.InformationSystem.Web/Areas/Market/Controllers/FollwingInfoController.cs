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

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class FollwingInfoController : BaseMvcController
    {
        ConsultManeger CM_Entity = new ConsultManeger();
        StuStateManeger ST_Entity = new StuStateManeger();
        // GET: /Market/FollwingInfo/ListStudentView
        //获取当前上传的操作人
        string UserName = Base_UserBusiness.GetCurrentUser().UserName;
        public ActionResult FollwingInfoIndex()
        {           
            return View();
        }
        /// <summary>
        /// 初始化柱状图
        /// </summary>
        /// <returns></returns>
        public ActionResult GetImageData()
        {
            //判断是哪个咨询师
            List<ConsultZhuzImageData> ConsultZhuzImageData_data = CM_Entity.GetImageData("1");
            return Json(ConsultZhuzImageData_data,JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMonData(int MonthName,string Status)
        {
            List<StudentPutOnRecord> result = new List<StudentPutOnRecord>();
            if (Status=="完成量")
            {
                result=CM_Entity.GetStudentData(MonthName, "1", 1);
            }
            else if (Status == "未完成量")
            {
                result=CM_Entity.GetStudentData(MonthName, "2", 1);
            }
            else if (Status == "分量数")
            {
                result=CM_Entity.GetStudentData(MonthName, "3", 1);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询是否有该学生
        /// </summary>
        /// <param name="id">学生名称</param>
        /// <returns></returns>
        public ActionResult GetMonthStudent(string id)
        {
            List<Consult> con_list = CM_Entity.GetList().Where(c =>CM_Entity.GetSingleStudent( c.StuName).StuName==id).ToList();
            List<StudentPutOnRecord> stu_list = CM_Entity.GetStudentPutRecored();
            List<StudentPutOnRecord> find_list = new List<StudentPutOnRecord>();

            foreach (Consult item2 in con_list)
            {
                foreach (StudentPutOnRecord item1 in stu_list)
                {
                    if (item1.Id == item2.StuName)
                    {
                        find_list.Add(item1);
                    }
                }
            }
            //获取这个学生的跟踪信息次数
            int j = 0;
            List<FollwingInfo> find = CM_Entity.GetFollwingManeger().GetList();
            if (con_list.Count==1)
            {
                int i = 0;
                foreach (FollwingInfo item in find)
                {
                    foreach (Consult c in con_list)
                    {
                        if (item.Consult_Id==c.Id)
                        {
                            i++;
                        }
                    }
                }
                j = i;
                SessionHelper.Session["Number"] = j;
            }
            var jsondata = new {
                data = find_list,
                Number = j
            };
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }

        //这是一个添加页面
        public ActionResult AddFollwingInfo(int id)
        {
            StudentPutOnRecord find_Stu= CM_Entity.GetSingleStudent(id);
            int consult_id = CM_Entity.GetList().Where(c=>c.StuName==id).FirstOrDefault().Id;           
            SessionHelper.Session["consult_id"] = consult_id;
            int Number= CM_Entity.GetFollwingManeger().GetList().Where(c => c.Consult_Id == consult_id).ToList().Count;
            ViewBag.Name = find_Stu.StuName;
            ViewBag.Sex = find_Stu.StuSex;
            ViewBag.Number = Number;
            return View();
        }
        //这是一个添加方法
        public ActionResult AddFunction()
        {
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
                BusHelper.WriteSysLog(UserName+"添加了一条跟踪学生信息", Entity.Base_SysManage.EnumType.LogType.添加数据);
                return Json("ok",JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //将错误填写到日志中     
                BusHelper.WriteSysLog(UserName+"添加数据时出现:"+ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                return Json("添加跟踪信息失败，请重试！！！", JsonRequestBehavior.AllowGet);
            }
             
        }

        //这是一个编辑页面
        public ActionResult EditView(int id)
        {
            StudentPutOnRecord find_stu = CM_Entity.GetSingleStudent(id);
            //获取该学生的Id
            ViewBag.Name = find_stu.StuName;
            ViewBag.Sex = find_stu.StuSex==false?"女":"男";
            ViewBag.Number =Convert.ToInt32(SessionHelper.Session["Number"])+"次";
            //获取这个学生的所有咨询信息
            Consult find_c= CM_Entity.GetList().Where(c => c.StuName == id).FirstOrDefault();
            List<FollwingInfo> flist= CM_Entity.GetFollwingManeger().GetList().Where(f => f.Consult_Id == find_c.Id).ToList();
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
                BusHelper.WriteSysLog(UserName + "成功编辑了一条跟踪信息数据", Entity.Base_SysManage.EnumType.LogType.编辑数据);
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(UserName + "编辑跟踪信息时出现:"+ex, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                return Json("系统错误，请重试!!!",JsonRequestBehavior.AllowGet);
            }
             
        }
        //这是找到多个学生显示的页面
        public ActionResult ListStudentView(string id)
        {
            string[] stulist = id.Split(',');
            List<StudentPutOnRecord> student = CM_Entity.GetStudentPutRecored();
            List<StudentPutOnRecord> findresult = new List<StudentPutOnRecord>();
            foreach (string item1 in stulist)
            {
                if (!string.IsNullOrEmpty(item1))
                {
                    foreach (StudentPutOnRecord item2 in student)
                    {
                        if (item2.Id==Convert.ToInt32(item1))
                        {
                            findresult.Add(item2);
                        }
                    }
                }
            }
            List<StudentData> data = findresult.Select(s => new StudentData() {
                Id=s.Id,
                stuSex = s.StuSex == false ? "男" : "女",
                StuName = s.StuName,
                StuPhone = s.StuPhone,
                StuSchoolName = s.StuSchoolName,
                StuAddress = s.StuAddress,
                StuInfomationType_Id = CM_Entity.getTypeName(s.StuInfomationType_Id.ToString(), true).Name,
                StuStatus_Id = ST_Entity.GetIdGiveName(s.StuStatus_Id.ToString(),true).StatusName,
                StuIsGoto = s.StuIsGoto == false ? "否" : "是",
                StuVisit = s.StuVisit,
                EmployeesInfo_Id = CM_Entity.GetEmplyeesInfo(s.EmployeesInfo_Id).EmpName,
                StuDateTime = s.StuDateTime,
                StuEntering = CM_Entity.GetEmplyeesInfo(s.StuEntering).EmpName,
                AreName =CM_Entity.GetRegionName(s.Region_id).RegionName
            }).ToList();
            ViewBag.Student = data;
            return View();
        }

    }
}