using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 教官业务类
    /// </summary>
    public class InstructorListBusiness:BaseBusiness<InstructorList>
    {
       
        /// <summary>
        /// 添加教官
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool AddInstructorList(string empid) {

            bool result = false;
            try
            {
                InstructorList it = new InstructorList();
                it.EmployeeNumber = empid;
                it.IsDel = false;
                this.Insert(it);
                result = true;
                BusHelper.WriteSysLog("添加教官成功", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {
                result = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return result;
        }

        /// <summary>
        ///教官离职
        /// </summary>
        /// <param name="informatiees_Id">员工编号</param>
        /// <returns></returns>
        public bool RemoveInstructorList(string empid)
        {
            bool str = true;
            try
            {
                var x = this.GetList().Where(a => a.EmployeeNumber == empid).FirstOrDefault();
                x.IsDel = true;
                BusHelper.WriteSysLog("教官状态改变成功", Entity.Base_SysManage.EnumType.LogType.编辑数据);

            }
            catch (Exception ex)
            {

                str = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
            return str;

        }

        public List<InstructorList> GetInstructorLists() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        public InstructorList GetInstructorByempid(string empid) {
           return this.GetInstructorLists().Where(a => a.EmployeeNumber == empid).FirstOrDefault();
        }

    }
}
