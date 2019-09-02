using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;

namespace SiliconValley.InformationSystem.Business.Consult_Business
{
   public class ConsultTeacherManeger:BaseBusiness<ConsultTeacher>
    {
        EmployeesInfoManage e_Entity = new EmployeesInfoManage();
         /// <summary>
         /// 当咨询师入职成功时需要调用这个方法
         /// </summary>
         /// <param name="EmplyeId">员工编号</param>
         /// <returns></returns>
        public bool AddConsultTeacherData(string EmplyeId)
        {
            //获取当前上传的操作人
            string UserName = Base_UserBusiness.GetCurrentUser().UserName;
            try
            {
                ConsultTeacher new_c = new ConsultTeacher() { Employees_Id = EmplyeId };
                this.Insert(new_c);
                return true;
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog("操作人:" + UserName + "出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                return false;
            }
            
        }
        /// <summary>
        /// 根据Id查找员工
        /// </summary>
        /// <param name="EmplyeId">员工Id</param>
        /// <returns></returns>
        public EmployeesInfo SeracherEmp(string EmplyeId)
        {
            return e_Entity.GetEntity(EmplyeId);
        }
        /// <summary>
        /// 获取员工集合
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetEmp_List()
        {
            return e_Entity.GetList().Where(e=>e.IsDel==false).ToList();
        }
    }
}
