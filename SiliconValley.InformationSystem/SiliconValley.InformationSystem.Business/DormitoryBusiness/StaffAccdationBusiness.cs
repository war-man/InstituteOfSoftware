using SiliconValley.InformationSystem.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    public class StaffAccdationBusiness : BaseBusiness<StaffAccdation>
    {
        /// <summary>
        /// 获取员工的现在的居住信息
        /// </summary>
        /// <returns></returns>
        public List<StaffAccdation> GetStaffAccdationings() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }


        /// <summary>
        /// 获取现在员工居住信息根据员工编号
        /// </summary>
        /// <param name="EmpinfoID"></param>
        /// <returns></returns>
        public StaffAccdation GetStaffAccdationsByEmpinfoID(string EmpinfoID)
        {
            return this.GetStaffAccdationings().Where(a => a.EmployeeId == EmpinfoID).FirstOrDefault();

        }

        /// <summary>
        /// 根据房间编号获取现在居住信息
        /// </summary>
        /// <param name="DorminfoID"></param>
        /// <returns></returns>
        public List<StaffAccdation> GetStaffAccdationsByDorminfoID(int DorminfoID) {
            return this.GetStaffAccdationings().Where(a => a.DormId== DorminfoID).ToList ();
        }
    }
}
