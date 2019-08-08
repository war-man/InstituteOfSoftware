using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using Base_SysManage;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using EmployeesBusiness;
   public class TeacherBusiness:BaseBusiness<Teacher>
    {

        //员工 业务上下文
        public EmployeesInfoManage db_emp;
        public TeacherBusiness()
        {

            db_emp = new EmployeesInfoManage();

        }



        /// <summary>
        /// 根据ID获取教员
        /// </summary>
        /// <param name="">教员ID</param>
        /// <returns>教员实体</returns>
        public Teacher GetTeacherByID(int id)
        {
           return  this.GetList().Where(t => t.TeacherID == id).FirstOrDefault();
        }

        /// <summary>
        /// / 更具员工编号获取员工
        /// </summary>
        /// <param name="EmpNo">员工编号</param>
        /// <returns>员工实体</returns>
        public EmployeesInfo GetEmpByEmpNo(string EmpNo)
        {
           return db_emp.GetList().Where(d => d.EmployeeId == EmpNo).FirstOrDefault();
                
        }
    }
}
