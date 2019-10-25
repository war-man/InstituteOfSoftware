using System;
using System.Collections.Generic;
using System.Linq;

namespace SiliconValley.InformationSystem.Business.EmployeesBusiness
{
    using SiliconValley.InformationSystem.Business.Channel;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Business.PositionBusiness;
    using SiliconValley.InformationSystem.Business.DepartmentBusiness;
    using SiliconValley.InformationSystem.Business.SchoolAttendanceManagementBusiness;
    /// <summary>
    /// 员工业务类
    /// </summary>
    public class EmployeesInfoManage:BaseBusiness<EmployeesInfo>
    {

        /// <summary>
        ///  获取所属岗位对象
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Position GetPosition(int pid)
        {
            PositionManage pmanage = new PositionManage();
            var str = pmanage.GetEntity(pid);
            return str;
        }
        /// <summary>
        /// 根据员工编号获取所属岗位对象
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public Position GetPositionByEmpid(string empid) {
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            PositionManage pmanage = new PositionManage();
            var pstr = pmanage.GetEntity(emanage.GetEntity(empid).PositionId);
            return pstr;
        }

        /// <summary>
        /// 根据员工编号获取所属部门对象
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public Department GetDeptByEmpid(string empid)
        {
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            DepartmentManage dmanage = new DepartmentManage();
            var dstr = dmanage.GetEntity(GetPositionByEmpid(empid).DeptId);
            return dstr;
        }

        /// <summary>
        /// 获取所属岗位的所属部门对象
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Department GetDept(int pid)
        {
            DepartmentManage deptmanage = new DepartmentManage();
            var str = deptmanage.GetEntity(GetPosition(pid).DeptId);
            return str;
        }
        /// <summary>
        /// 根据部门编号获取部门对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Department GetDeptById(int id) {
            DepartmentManage deptmanage = new DepartmentManage();
            var dept = deptmanage.GetEntity(id);
            return dept;
        }
        /// <summary>
        /// 根据岗位编号获取岗位对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Position GetPobjById(int id) {
            PositionManage pmanage = new PositionManage();
            return pmanage.GetEntity(id);
        }
        /// <summary>
        /// 根据类型编号获取员工异动类型对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MoveType GetETById(int id) {
            MoveTypeManage mtmanage = new MoveTypeManage();
            return mtmanage.GetEntity(id);
        }


        /// <summary>
        /// 渠道
        /// </summary>
        private ChannelStaffBusiness dbchannel;
        /// <summary>
        /// 获取所有没有离职的员工
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetAll() {
          return  this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 根据员工编号获取员工对象
        /// </summary>
        /// <param name="empinfoid">员工编号</param>
        /// <returns></returns>
        public EmployeesInfo GetInfoByEmpID(string empinfoid) {
            return this.GetEntity(empinfoid);
        }

        /// <summary>
        /// 添加员工借资
        /// </summary>
        /// <param name="debit"></param>
        /// <returns></returns>
        public bool Borrowmoney(Debit debit) {
            BaseBusiness<Debit> dbdebit = new BaseBusiness<Debit>();
            bool result = false;
            try
            {
                dbdebit.Insert(debit);
                result = true;
            }
            catch (Exception)
            {

            }
            return result;
        }


        /// <summary>
        /// 查询是市场主任员工集合
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetChannelStaffZhuren() {
           return this.GetAll().Where(a => a.PositionId == 1006).ToList();
        }

        /// <summary>
        /// 获取副主任
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetChannelStaffFuzhuren() {
            return this.GetAll().Where(a => a.PositionId == 2010).ToList();
        }

        /// <summary>
        /// 根据渠道员工id获取员工对象
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public EmployeesInfo GetInfoByChannelID(int ChannelID) {
            dbchannel = new ChannelStaffBusiness();
            var channel= dbchannel.GetChannelByID(ChannelID);
            return this.GetInfoByEmpID(channel.EmployeesInfomation_Id);
        }
        /// <summary>
        /// 杨校
        /// </summary>
        /// <returns></returns>
        public EmployeesInfo GetYangxiao() {
            return this.GetAll().Where(a => a.EmployeeId == "201908290017").FirstOrDefault();
        }
        /// <summary>
        /// 判断是否是渠道主任
        /// </summary>
        /// <param name="empinfoid">员工编号</param>
        /// <returns></returns>
        public bool IsChannelZhuren(string empinfoid) {
            bool iszhuren = false;
            var data = this.GetChannelStaffZhuren();
            foreach (var item in data)
            {
                if (item.EmployeeId==empinfoid)
                {
                    iszhuren = true;
                }
            }
            return iszhuren;
        }
        /// <summary>
        /// 判断是不是主任
        /// </summary>
        /// <param name="empinfo"></param>
        /// <returns></returns>
        public bool IsChannelZhuren(EmployeesInfo empinfo) {
            bool iszhuren = false;
            if (empinfo.PositionId== 1006)
            {
                iszhuren = true;
            }
            return iszhuren;
        }

        /// <summary>
        /// 判断是不是副主任
        /// </summary>
        /// <param name="empinfo"></param>
        /// <returns></returns>
        public bool IsFuzhiren(EmployeesInfo empinfo)
        {
            bool isfuzhuren = false;
            if (empinfo.PositionId == 2010)
            {
                isfuzhuren = true;
            }
            return isfuzhuren;
        }

        #region tangmin--Write
        /// <summary>
        /// 根据员工Id或者员工名称查询名称
        /// </summary>
        /// <param name="name">员工编号或员工名称</param>
        /// <param name="key">true---按编号查，false---按名称查</param>
        /// <returns></returns>
        public EmployeesInfo FindEmpData(string name, bool key)
        {
            EmployeesInfo employees = new EmployeesInfo();
            if (key)
            {
                employees = this.GetEntity(name);
            }
            else
            {
                employees = this.GetList().Where(e => e.EmpName == name).FirstOrDefault();
            }
            return employees;
        }

        #endregion
    }
}
