using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness
{
    public class AttendanceInfoManage : BaseBusiness<AttendanceInfo>
    {
        RedisCache rc = new RedisCache();
        /// <summary>
        /// 将员工考勤表数据存储到redis服务器中
        /// </summary>
        /// <returns></returns>
        public List<AttendanceInfo> GetADInfoData()
        {
            rc.RemoveCache("InRedisATDData");
            List<AttendanceInfo> atdinfolist = new List<AttendanceInfo>();
            if (atdinfolist == null || atdinfolist.Count == 0)
            {
                atdinfolist = this.GetList();
                rc.SetCache("InRedisATDData", atdinfolist);
            }
            atdinfolist = rc.GetCache<List<AttendanceInfo>>("InRedisATDData");
            return atdinfolist;
        }
        /// <summary>
        /// 员工入职时往员工考勤表加入该员工
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        //public bool AddEmpToAttendanceInfo(string empid)
        //{
        //    bool result = false;
        //    try
        //    {
        //        AttendanceInfo ese = new AttendanceInfo();
        //        ese.EmployeeId = empid;
        //        ese.IsDel = false;
        //        ese.YearAndMonth = DateTime.Now;
        //        this.Insert(ese);
        //        rc.RemoveCache("InRedisATDData");
        //        result = true;
        //        BusHelper.WriteSysLog("考勤表添加员工成功", Entity.Base_SysManage.EnumType.LogType.添加数据);

        //    }
        //    catch (Exception ex)
        //    {
        //        result = false;
        //        BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
        //    }
        //    return result;

        //}

        /// <summary>
        /// 编辑考勤表禁用员工
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool EditEmpStateToAds(string empid)
        {
            bool result = false;
            try
            {
                var ads = this.GetADInfoData().Where(e => e.EmployeeId == empid).FirstOrDefault();
                ads.IsDel = true;
                this.Update(ads);
                rc.RemoveCache("InRedisATDData");
                result = true;
                BusHelper.WriteSysLog("考勤表禁用员工成功", Entity.Base_SysManage.EnumType.LogType.编辑数据);

            }
            catch (Exception ex)
            {
                result = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
            return result;

        }

        //工资表生成的方法
        public bool CreateSalTab(string time)
        {
            bool result = false;
            //try
            //{
            //    var msrlist = this.GetADInfoData().Where(s => s.IsDel == false).ToList();
            //   // EmployeesInfoManage empmanage = new EmployeesInfoManage();
            //    var emplist = empmanage.GetEmpInfoData();
            //    var nowtime = DateTime.Parse(time);

            //    //匹配是否有该月（选择的年月即传过来的参数）的月度工资数据
            //    var matchlist = msrlist.Where(m => DateTime.Parse(m.YearAndMonth.ToString()).Year == nowtime.Year && DateTime.Parse(m.YearAndMonth.ToString()).Month == nowtime.Month).ToList();


            //    //找到已禁用的且为该月份的员工集合
            ////    var forbiddenlist = this.GetEmpMsrData().Where(s => s.IsDel == true || (DateTime.Parse(s.YearAndMonth.ToString()).Year == nowtime.Year && DateTime.Parse(s.YearAndMonth.ToString()).Month == nowtime.Month)).ToList();

            //    for (int i = 0; i < forbiddenlist.Count(); i++)
            //    {//将月度工资表中已禁用的员工去员工表中去除
            //        emplist.Remove(emplist.Where(e => e.EmployeeId == forbiddenlist[i].EmployeeId).FirstOrDefault());
            //    }
            //    if (matchlist.Count() <= 0)
            //    {

            //        foreach (var item in emplist)
            //        {//再将未禁用的员工添加到月度工资表中
            //            MonthlySalaryRecord msr = new MonthlySalaryRecord();
            //            msr.EmployeeId = item.EmployeeId;
            //            msr.YearAndMonth = Convert.ToDateTime(time);
            //            msr.IsDel = false;
            //            this.Insert(msr);
            //        }
            //    }

            //    result = true;
            //}
            //catch (Exception ex)
            //{
            //    result = false;

            //}
            return result;
        }

    }
}
