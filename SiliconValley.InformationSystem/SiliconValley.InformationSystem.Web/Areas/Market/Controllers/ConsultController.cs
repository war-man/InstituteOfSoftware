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
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.StuSatae_Maneger;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    [CheckLogin]
    public class ConsultController : BaseMvcController
    {
        ConsultManeger CM_Entity = new ConsultManeger();
        StudentInformationBusiness ST_Entity = new StudentInformationBusiness();//获取在读学生
        ScheduleForTraineesBusiness SB_Entity = new ScheduleForTraineesBusiness();//获取班级
        HeadmasterBusiness HB_Entity = new HeadmasterBusiness();//获取班主任
        TeacherClassBusiness TB_Entity = new TeacherClassBusiness();//获取任课老师
        ClassScheduleBusiness CB_Entity = new ClassScheduleBusiness();//获取专业跟阶段
        StuStateManeger SM_Entity = new StuStateManeger();//获取状态
        private EmployeesInfoManage Employsinfo_Entity;
        //获取当前上传的操作人
        Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();

        // GET: /Market/Consult/ConsultIndex
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
                       zhanghua.Number = wangchen;
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
        //获取学生综合数据的方法
        public My_StudentDataOne GetDataStudent(string id)
        {             
                My_StudentDataOne mystudentdata = new My_StudentDataOne();
                //单个数据
                int stu_id = int.Parse(id);//得到学生备案Id                
                //获取备案信息
                StudentPutOnRecord find_spt = CM_Entity.GetSingleStudent(stu_id);
            if (find_spt != null)
            {
                mystudentdata.StudentputonereadName = find_spt.StuName;
                mystudentdata.Sex = find_spt.StuSex == false ? "女" : "男";
                mystudentdata.RecordData = find_spt.StuDateTime;
                mystudentdata.IsVistSchool = find_spt.StuIsGoto == true ? "是" : "否";
                StuStatus find_status = SM_Entity.GetEntity(find_spt.StuStatus_Id);
                if (find_status!=null)
                {
                    mystudentdata.IsExitsSchool = find_status.StatusName;
                }
                 
                EmployeesInfo find_e1 = CM_Entity.GetEmplyeesInfo(find_spt.EmployeesInfo_Id);
                if (find_e1 != null)
                {
                    mystudentdata.DataputRecordMan = find_e1.EmpName;//获取备案人
                }                 
                Region find_r = CM_Entity.GetRegionName(find_spt.Region_id);
                if (find_r != null)
                {
                    mystudentdata.AreaName = find_r.RegionName;//获取区域名称
                }
                 
                Consult find_c = CM_Entity.FindStudentIdGetConultdata(find_spt.Id);//获取分量数据
                if (find_c != null)
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
                StudentInformation find_s = ST_Entity.GetList().Where(s => s.StudentPutOnRecord_Id == stu_id).FirstOrDefault();
                if (find_s != null)
            {
                //根据学号找班级
                ScheduleForTrainees className = SB_Entity.SutdentCLassName(find_s.StudentNumber);
                if (className != null)
                {
                    mystudentdata.ClassName = className.ClassID;//获取班级名称
                    mystudentdata.Teacher = TB_Entity.ClassTeacher(className.ID_ClassName.ToString()).EmpName;//获取任课老师
                    mystudentdata.Grand = CB_Entity.GetClassGrand(className.ID_ClassName, 2);//阶段
                    mystudentdata.ZhuanyeName = CB_Entity.GetClassGrand(className.ID_ClassName, 1);//专业
                }                
                EmployeesInfo find_e = HB_Entity.Listheadmasters(find_s.StudentNumber);
                if (find_e != null)
                {
                    mystudentdata.ClassTeacher = find_e.EmpName;//获取班主任
                }                 
            }                       
                return mystudentdata;
        }
        //查到一个或多个学生的页面显示
        public ActionResult ListStudentView(string id)
        {
            List<My_StudentDataOne> list = new List<My_StudentDataOne>();
            if (id.IndexOf(",") <= -1)
            {
                list.Add(GetDataStudent(id));
            }
            else
            {
                string[] id_list = id.Split(',');
                foreach (string myid in id_list)
                {
                    if (!string.IsNullOrEmpty(myid))
                    {
                        list.Add(GetDataStudent(myid));
                    }                     
                }
            }
            ViewBag.Mystudents = list;
                return View();
        }
        //这是一个分量页面
        public ActionResult ConsultView()
        {
          List<TreeClass> list_treacher = CM_Entity.GetConsultTeacher().Where(ct=>ct.IsDelete==false).Select(ct=>new TreeClass() { id=ct.Id.ToString(),title=CM_Entity.GetEmplyeesInfo(ct.Employees_Id).EmpName}).ToList();
            ViewBag.Teacher = list_treacher;//咨询师
            return View();
        }
        /// <summary>
        /// 获取某个月份未分量的学生
        /// </summary>
        /// <param name="month">月份名称</param>
        /// <returns></returns>
        public ActionResult MonthStudentData(int id)
        {
            //获取未报名学生
            List<StudentPutOnRecord> list_stu = CM_Entity.GetMonStudent(id).Where(s => s.StuStatus_Id != (SM_Entity.GetStu("已报名").Data as StuStatus).Id).ToList();
            //获取未分量的学生
            List<Consult> find_all= CM_Entity.GetIQueryable().ToList();
            List<StudentPutOnRecord> getNoExit = new List<StudentPutOnRecord>();
            foreach (StudentPutOnRecord studentdata in list_stu)
            {
                Consult findvalue= find_all.Where(a => a.StuName == studentdata.Id).FirstOrDefault();
                if (findvalue==null)
                {
                    getNoExit.Add(studentdata);
                }
            }
            var data = getNoExit.Select(s => new {
                Id = s.Id,
                StuName=s.StuName,
                StuSex = s.StuSex,
                StuStatus_Id= SM_Entity.GetEntity(s.StuStatus_Id).StatusName,
                StuPhone=s.StuPhone,
                EmployeesInfo_Id=CM_Entity.GetEmplyeesInfo(s.EmployeesInfo_Id).EmpName,
                Region_id=CM_Entity.GetRegionName(s.Region_id).RegionName
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //添加
        [HttpPost]
        public ActionResult Insertconsult()
        {
            Employsinfo_Entity = new EmployeesInfoManage();
            try
            {
                string studentlist = Request.Form["listid"];
                string consulteacherid = Request.Form["consultTeacher"];
                if (!string.IsNullOrEmpty(studentlist) && !string.IsNullOrEmpty(consulteacherid))
                {
                    string[] idlist = studentlist.Split(',');
                    foreach (string item1 in idlist)
                    {
                        if (!string.IsNullOrEmpty(item1))
                        {
                            Consult new_c = new Consult();
                            new_c.ComDate = DateTime.Now;
                            new_c.IsDelete = false;
                            new_c.StuName = Convert.ToInt32(item1);
                            new_c.TeacherName = Convert.ToInt32(consulteacherid);
                            CM_Entity.Insert(new_c);
                        }
                    }
                }
                else
                {
                    WriteSysLog("用户:"+ Employsinfo_Entity.GetEntity(UserName.EmpNumber).EmpName + "添加分量操作错误", EnumType.LogType.添加数据);
                    return Json("系统错误，请重试!!!",JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                WriteSysLog("用户:" + Employsinfo_Entity.GetEntity(UserName.EmpNumber).EmpName + "添加分量操作出现"+ex.Message, EnumType.LogType.添加数据);
                return Json("系统错误，请重试!!!", JsonRequestBehavior.AllowGet);
            }
            WriteSysLog("用户:" + Employsinfo_Entity.GetEntity(UserName.EmpNumber).EmpName + "添加分量信息成功" , EnumType.LogType.添加数据);
            return Json("ok",JsonRequestBehavior.AllowGet);
        }         
    }
}