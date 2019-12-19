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
        private DormInformationBusiness dbdorm;
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

        /// <summary>
        /// 添加员工居住信息
        /// </summary>
        /// <param name="staffAccdation"></param>
        /// <returns></returns>
        public bool AddStaffacc(StaffAccdation staffAccdation) {
            bool result = false;
            try
            {
                this.Insert(staffAccdation);
                result = true;
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        /// <summary>
        /// 根据栋楼层 返回这一层的员工居住信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<StaffAccdation> GHetstaffaccByTungfloorid(int Tungfloorid) {
            List<StaffAccdation> result = new List<StaffAccdation>();
            dbdorm = new DormInformationBusiness();
            var dormlist= dbdorm.GetDormsForStaffByTungfloorid(Tungfloorid);
            foreach (var item in dormlist)
            {
                result.AddRange(this.GetStaffAccdationsByDorminfoID(item.ID));
            }
            return result;
        }


        /// <summary>
        ///删除居住信息
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool DelStaffacc(string empid) {
            bool result = false;
           var data=  this.GetStaffAccdationsByEmpinfoID(empid);
            if (data==null)
            {
                result = true;
            }
            else
            {
                data.IsDel = true;
                this.Update(data);
                result =true;
            }
            return result;
        }
    }
}
