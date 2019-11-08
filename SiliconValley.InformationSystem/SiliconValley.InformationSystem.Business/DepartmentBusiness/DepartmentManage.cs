using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DepartmentBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;

    /// <summary>
    /// 部门表
    /// </summary>
    public class DepartmentManage : BaseBusiness<Department>
    {

        /// <summary>
        /// 获取没有被禁用的部门
        /// </summary>
        /// <returns></returns>
        public List<Department> GetDepartments() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 根据这个部门名称返回部门对象
        /// </summary>
        /// <param name="param1"></param>
        /// <returns></returns>
        public Department GetDepartmentByName(string param1) {
           return this.GetDepartments().Where(a => a.DeptName == param1).FirstOrDefault();
        }

    }
}
