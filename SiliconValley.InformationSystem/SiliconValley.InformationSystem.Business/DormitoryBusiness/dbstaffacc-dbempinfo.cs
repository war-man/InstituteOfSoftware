using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 员工业务类以及员工居住信息业务类
    /// </summary>
    public class dbstaffacc_dbempinfo
    {
        private StaffAccdationBusiness dbstaffacc;
        private EmployeesInfoManage dbempinfo;

        /// <summary>
        /// 获取没有居住在学校的员工
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetUninhabitedData() {
            dbstaffacc = new StaffAccdationBusiness();
            dbempinfo = new EmployeesInfoManage();
            List<EmployeesInfo> employeesInfos = dbempinfo.GetAll();
            List<StaffAccdation> staffAccdations = dbstaffacc.GetStaffAccdationings();
            for (int i = employeesInfos.Count - 1; i >= 0; i--)
            {
                foreach (var item in staffAccdations)
                {
                    if (employeesInfos.Count > 0)
                    {
                        if (employeesInfos[i].EmployeeId == item.EmployeeId)
                        {
                            employeesInfos.Remove(employeesInfos[i]);
                            break;
                        }
                    }

                }
            }
            return employeesInfos;
        }

        /// <summary>
        /// 获取居住信息
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetinhabitedData() {
            dbstaffacc = new StaffAccdationBusiness();
            dbempinfo = new EmployeesInfoManage();
            List<EmployeesInfo> employeesInfos = new List<EmployeesInfo>();
            List<StaffAccdation> staffAccdations = dbstaffacc.GetStaffAccdationings();
            foreach (var item in staffAccdations)
            {
                employeesInfos.Add(dbempinfo.GetEntity(item.EmployeeId));

            }
            return employeesInfos;
        }
    }
}
