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
            bool result = true;
            try
            {
                ConsultTeacher new_c = new ConsultTeacher();
                new_c.Employees_Id = EmplyeId;
                new_c.IsDelete = false;
                //new_c.ConGrade = 5;
                this.Insert(new_c);
                result = true;
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog("操作人:" + UserName + "出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                result= false;
            }
            return result;
            
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
        //这是一个咨询师的离职方法
        public bool DeltConsultTeacher(string Empyee_Id)
        {
           ConsultTeacher find_c= this.GetList().Where(c => c.Employees_Id == Empyee_Id).FirstOrDefault();
            bool result = false;
            if (find_c!=null)
            {
                find_c.IsDelete = true;
                this.Update(find_c);
                result = true;
            }            
                return result;
        }
    }
}
