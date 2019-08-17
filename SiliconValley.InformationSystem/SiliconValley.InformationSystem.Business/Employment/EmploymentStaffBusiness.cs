using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 就业专员业务类
    /// </summary>
   public class EmploymentStaffBusiness:BaseBusiness<EmploymentStaff>
    {
        public List<EmploymentStaff> GetALl() {
            var data= this.GetIQueryable().ToList();
            foreach (var item in data)
            {
                if (IsDel(item.EmployeesInfo_Id))
                {
                    data.Remove(item);
                }
            }
            return data;
        }

        /// <summary>
        /// 判断是否员工是否离职
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public bool IsDel(string EmployeeId) {

            var EmpInfo = GetEmployeesInfoByID(EmployeeId);
            if (EmpInfo!=null)
                return false;
            else
                return true;
            
        }
        /// <summary>
        /// 根据员工编号查找i员工对象
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public EmployeesInfo GetEmployeesInfoByID(string EmployeeId) {
            var NomyEmp = new EmployeesInfoManage();
            var cc= NomyEmp.GetIQueryable().Where(a => a.EmployeeId == EmployeeId&&a.IsDel==false).FirstOrDefault();
            return cc;
        }
        /// <summary>
        /// 返回就业部的员工
        /// </summary>
         public List<EmployeesInfo> EmployeesInfos() {
            BaseBusiness<Department> DepbaseBusiness = new BaseBusiness<Department>();
            BaseBusiness<Position> PositionBusiness = new BaseBusiness<Position>();
            BaseBusiness<EmployeesInfo> EmployeesInfoBusiness = new BaseBusiness<EmployeesInfo>();
            var DepList= DepbaseBusiness.GetIQueryable().Where(a => a.IsDel == false).ToList();
            var PositionList = PositionBusiness.GetIQueryable().Where(a => a.IsDel == false).ToList();
            var EmployStaffList = this.GetIQueryable().Where(a => a.IsDel == false).ToList();
            var EmployInfoList = EmployeesInfoBusiness.GetIQueryable().Where(a => a.IsDel == false).ToList();
            var ResultList = new List<EmployeesInfo>();
            var newResultList = new List<EmployeesInfo>();
            foreach (var item in EmployInfoList)
            {
                var PositionObj = PositionList.Where(a => a.Pid == item.PositionId).First();
                if (PositionObj.DeptId==4)
                {
                    ResultList.Add(item);
                }
            }
            foreach (var item in EmployStaffList)
            {
                foreach (var result in ResultList)
                {
                    if (item.EmployeesInfo_Id!=result.EmployeeId)
                    {
                        newResultList.Add(result);
                    }
                }
            }
            return newResultList;
        }

    }
}
