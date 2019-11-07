using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.ClassesBusiness
{
  public  class HeadmasterBusiness:BaseBusiness<Headmaster>
    {

        //班主任带班
        BaseBusiness<HeadClass> Hoadclass = new BaseBusiness<HeadClass>();
        //添加班主任
        public bool AddHeadmaster(string informatiees_Id)
        {
            bool str = true;
          
            try
            {
                Headmaster informatiees = new Headmaster();
            informatiees.IsDelete = false;
            informatiees.AddTime = DateTime.Now;
            informatiees.informatiees_Id = informatiees_Id;
                
                this.Insert(informatiees);
                BusHelper.WriteSysLog("添加班主任成功", Entity.Base_SysManage.EnumType.LogType.添加数据);

            }
            catch (Exception ex)
            {
                
                str = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return str;

        }
          /// <summary>
          /// 班主任离职
          /// </summary>
          /// <param name="informatiees_Id">员工编号</param>
          /// <returns></returns>
        public bool removeHeadmaster(string informatiees_Id)
        {
            bool str = true;

            try
            {
             var x=   this.GetList().Where(a => a.informatiees_Id == informatiees_Id).FirstOrDefault();
                x.IsDelete = true;
                BusHelper.WriteSysLog("修改班主任状态", Entity.Base_SysManage.EnumType.LogType.编辑数据);

            }
            catch (Exception ex)
            {

                str = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
            return str;

        }
        //员工表
        EmployeesInfoManage employeesInfoManage = new EmployeesInfoManage();
        //学员班级
        ScheduleForTraineesBusiness scheduleForTraineesBusiness = new ScheduleForTraineesBusiness();
        //班主任职业素养培训
        BaseBusiness<Professionala> ProfessionalaBusiness = new BaseBusiness<Professionala>();

        //班主任离职时间
        public bool QuitEntity(string informatiees_Id)
        {
            bool str = true;
            try
            {
                var x = this.GetEntity(informatiees_Id);
                x.IsDelete = true;
                this.Update(x);
            }
            catch (Exception ex)
            {

                str = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
            
            }
            return str;
        }
        /// <summary>
        /// 带班业务左右回调操作
        /// </summary>
        /// <param name="id">带班人id</param>
        /// <param name="ClassName">班级名称</param>
        /// <param name="Index">添加或者删除(1则删除)</param>
        /// <returns></returns>
        public bool HeadClassEnti(string id,string ClassName,int Index)
        {
            bool str = true;
            List<HeadClass> list = new List<HeadClass>();
            ClassName = ClassName.Substring(0, ClassName.Length - 1);
            string[] ClassNames = ClassName.Split(',');
            foreach (var item in ClassNames)
            {
                var mysex = Hoadclass.GetList().Where(a => a.IsDelete == false && a.LeaderID == int.Parse(id) && a.ClassID == item).FirstOrDefault();

                if (Index==0)
                {
                    HeadClass headClass = new HeadClass();
                    headClass.AddTime = DateTime.Now;
                    headClass.LeadTime = DateTime.Now;
                    headClass.ClassID = item;
                    headClass.IsDelete = false;
                    headClass.LeaderID = int.Parse(id);
                    list.Add(headClass);
                }
                else
                {
                    list.Add(mysex);
                }
                    
               
            }
            try
            {
                if (Index == 1)
                {
                   
                    Hoadclass.Delete(list);
                    BusHelper.WriteSysLog("删除数据", Entity.Base_SysManage.EnumType.LogType.删除数据);
                }
                else
                {
                    Hoadclass.Insert(list);
                    BusHelper.WriteSysLog("添加数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
                  
                }
            }
            catch (Exception ex)
            {
                str = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.系统异常);
                
            }
            return str;
        
        }
        /// <summary>
        /// 通过班级号获取班主任
        /// </summary>
        /// <param name="ClassName">班级名称</param>
        /// <returns></returns>
        public EmployeesInfo ClassHeadmaster(string ClassName)
        {
            var mysex = Hoadclass.GetList().Where(a =>  a.ClassID == ClassName).FirstOrDefault();
            var leid =mysex==null?new Headmaster(): this.GetEntity(mysex.LeaderID);
            return leid == null ? new EmployeesInfo() : employeesInfoManage.GetEntity(leid.informatiees_Id);
        }
        /// <summary>
        /// 带班业务按钮操作
        /// </summary>
        /// <param name="id">带班人id</param>
        /// <param name="ClassName">班级名称</param>
        /// <returns></returns>
        public bool HeadClassEntis(string id, string ClassName)
        {
            bool str = true;
            List<HeadClass> list = new List<HeadClass>();
            if (!string.IsNullOrEmpty( ClassName))
            {
                ClassName = ClassName.Substring(0, ClassName.Length - 1);
                string[] ClassNames = ClassName.Split(',');
                foreach (var item in ClassNames)
                {
                    HeadClass headClass = new HeadClass();
                    headClass.AddTime = DateTime.Now;
                    headClass.LeadTime = DateTime.Now;
                    headClass.ClassID = item;
                    headClass.IsDelete = false;
                    headClass.LeaderID = int.Parse(id);
                    list.Add(headClass);
                }
            }
         
            try
            {
                 var mysex = Hoadclass.GetList().Where(a => a.IsDelete == false && a.LeaderID == int.Parse(id)).ToList();
                    Hoadclass.Delete(mysex);
                    BusHelper.WriteSysLog("删除数据", Entity.Base_SysManage.EnumType.LogType.删除数据);
                if (list.Count>0)
                {
                    Hoadclass.Insert(list);
                    BusHelper.WriteSysLog("添加数据", Entity.Base_SysManage.EnumType.LogType.添加数据);
                }
                  
            }
            catch (Exception ex)
            {
                str = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.系统异常);

            }
            return str;

        }
        /// <summary>
        /// 根据学员学号获取当前班主任
        /// </summary>
        /// <param name="StudentID">学员id</param>
        /// <returns></returns>
        public EmployeesInfo Listheadmasters(string StudentID)
        {
          var ClassID=  scheduleForTraineesBusiness.GetList().Where(q => q.CurrentClass == true && q.StudentID == StudentID).FirstOrDefault().ClassID;//获取班级号
            var leid = Hoadclass.GetList().Where(c => c.IsDelete == false && c.EndingTime == null && c.ClassID == ClassID).FirstOrDefault().LeaderID;//查询带班班长id
          var Empid = this.GetEntity(leid).informatiees_Id;//员工编号
            return employeesInfoManage.GetEntity(Empid);
        }
        /// <summary>
        /// 班主任职业素养培训数据
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="limit">多少条数据</param>
        /// <returns></returns>
        public List<Professionala> GetDateProfessionala(int page, int limit)
        {
            SessionHelper.Session["Professionalapage"] = page;
            SessionHelper.Session["Professionalalimit"] = limit;
           var list= ProfessionalaBusiness.GetList().Where(a => a.Dateofregistration == false).ToList();
           return list.OrderBy(a => a.ID).Skip((page - 1) * limit).Take(limit).ToList();
        }
        /// <summary>
        /// 记录班主任职业素养培训课件
        /// </summary>
        /// <param name="professionala">数据对象</param>
        /// <returns></returns>
        public object AddProfessionala(Professionala professionala)
        {
            var result=new object();

            try
            {
                professionala.Dateofregistration = false;
                professionala.AddTime = DateTime.Now;
                ProfessionalaBusiness.Insert(professionala);
                result = new {
                    Success = true,
                    Msg = "录入成功",
                    page = SessionHelper.Session["Professionalapage"],
                    limit = SessionHelper.Session["Professionalalimit"]
                };

                BusHelper.WriteSysLog("班主任职业素养课件添加", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {
                result = new
                {
                    Success = false,
                    Msg = "服务器错误！",
                    page = SessionHelper.Session["Professionalapage"],
                    limit = SessionHelper.Session["Professionalalimit"]
                };

                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
               
            }
            return result;
        }
        /// <summary>
        /// 查询单条班主任职业素养课件培训
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public Professionala FineProfessionala(int id)
        {
            return ProfessionalaBusiness.GetEntity(id);
        }
    }
}
