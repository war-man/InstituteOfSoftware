using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Business.Consult_Business;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Business.ClassesBusiness;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class ConsultController : BaseMvcController
    {
        ConsultManeger CM_Entity = new ConsultManeger();
        StudentInformationBusiness ST_Entity = new StudentInformationBusiness();//获取在读学生
        ScheduleForTraineesBusiness SB_Entity = new ScheduleForTraineesBusiness();//获取班级
        HeadmasterBusiness HB_Entity = new HeadmasterBusiness();//获取班主任
        // GET: /Market/Consult/ListStudentView
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
       /// <summary>
       /// 这是获取柱状图数据
       /// </summary>
       /// <param name="id">咨询师Id</param>
       /// <returns></returns>
        public ActionResult GetImageData(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                List<ConsultZhuzImageData> list = CM_Entity.GetImageData(null);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<ConsultZhuzImageData> list = CM_Entity.GetImageData(id);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
 
        }
        /// <summary>
        /// 获取某个咨询师今年某个月完成的量和未完成的量
        /// </summary>
        /// <param name="monthName">月份</param>
        /// <param name="myid">咨询师Id</param>
        /// <returns></returns>
        public ActionResult GetTwoDivData(int MonthName, int Myid,int Staue)
        {           
           List<ALLDATA> list= CM_Entity.GetTeacherMonthCount(MonthName, Myid, Staue);
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 这是跟踪未报名学生情况显示页面
        /// </summary>
        /// <returns></returns>
        public ActionResult FollwingInfoView(string id)
        {
            int stuId = Convert.ToInt32(id);
            Consult find_c= CM_Entity.TongStudentIdFindConsult(stuId);
            List<FollwingInfo> f_list = CM_Entity.GetFllowInfoData(find_c.Id);
            ViewBag.Flowingdata = f_list;            
            return View();
        }  
        //获取所有咨询师
        public ActionResult GetConsultTeacherData(int id)
        {            
                List<ConsultTeacher> list_ConsultTeacher = CM_Entity.GetConsultTeacher();//获取所有咨询师
                List<ZhuanghuanluData> list_Zhang = new List<ZhuanghuanluData>();
                foreach (ConsultTeacher item in list_ConsultTeacher)
                {                   
                    ZhuanghuanluData zhanghua = new ZhuanghuanluData();
                    zhanghua.TeacherName =CM_Entity.GetEmplyeesInfo( item.Employees_Id).EmpName;
                    int count = CM_Entity.GetCount(item.Id, id);//得到分量
                    int wangchen = CM_Entity.GetWangcenCount(item.Id, id);//得到完成量
                    if (wangchen!=0 && count!=0)
                    {
                       zhanghua.Number =Convert.ToDouble(wangchen) / Convert.ToDouble(count);
                    }else
                    {
                     zhanghua.Number = 0;
                }
                     
                    list_Zhang.Add(zhanghua);
                }
                 var data = list_Zhang.OrderByDescending(c => c.Number).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);                
        }
        //查看是否有这个学生
        public ActionResult SeracherIsno(string id)
        {
            List<StudentPutOnRecord> find = CM_Entity.GetStudentPutRecored().Where(s => s.StuName == id).ToList();
            return Json(find,JsonRequestBehavior.AllowGet);
        }
        //查到一个或多个学生的页面显示
        public ActionResult ListStudentView(string id,string type)
        {
            if (type == "1")
            {               
               My_StudentDataOne mystudentdata = new My_StudentDataOne();
                //单个数据
               int stu_id= Convert.ToInt32(id);//得到学生备案Id
                //获取备案信息
               StudentPutOnRecord find_spt= CM_Entity.GetSingleStudent(stu_id);
                if (find_spt!=null)
                {
                    mystudentdata.RecordData = find_spt.StuDateTime;
                    mystudentdata.IsVistSchool = find_spt.StuIsGoto == true ? "是" : "否";
                    mystudentdata.IsExitsSchool = string.IsNullOrEmpty(find_spt.StuDateTime.ToString()) == true ? "否" : "是";
                    Consult find_c= CM_Entity.FindStudentIdGetConultdata(find_spt.Id);//获取分量数据
                    if (find_c!=null)
                    {
                        mystudentdata.CoultData = find_c.ComDate;//分量日期
                        mystudentdata.ConultNumber = CM_Entity.GetFollwingCount(find_c.Id);//获取跟踪次数
                        ConsultTeacher find_ct = CM_Entity.GetSingleFollwingData(find_c.TeacherName);//获取咨询师信息
                        if (find_ct != null)
                        {
                            mystudentdata.ConsultTeacherName = CM_Entity.GetEmplyeesInfo(find_ct.Employees_Id).EmpName;//获取咨询师名称
                        }                        
                    }
                    
                }
                //根据学生备案Id去找学生学号
               StudentInformation find_s= ST_Entity.GetList().Where(s => s.StudentPutOnRecord_Id == stu_id).FirstOrDefault();
                if (find_s!=null)
                {
                    //根据学号找班级
                   ScheduleForTrainees  className= SB_Entity.SutdentCLassName(find_s.StudentNumber);
                    if (className!=null)
                    {
                        mystudentdata.ClassName = className.ClassID;//获取班级名称
                    }
                }
               EmployeesInfo find_e= HB_Entity.Listheadmasters(find_s.StudentNumber);
                if (find_e!=null)
                {
                    mystudentdata.ClassTeacher = find_e.EmpName;//获取班主任
                }
            }
            else  
            {
                //多个数据

            }
            return View();
        }

    }
}