using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    public class EmpStaffAndStuBusiness:BaseBusiness<EmpStaffAndStu>
    {

        /// <summary>
        /// 返回全部可用的专员带学生就业信息
        /// </summary>
        /// <returns></returns>
        public List<EmpStaffAndStu> GetEmpStaffAndStus() {
           return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 判断这个学生编号是否存在分配
        /// </summary>
        /// <param name="studentno"></param>
        /// <returns></returns>
        public bool isdistribution(string studentno) {
            bool result = false;
            if (this.GetEmpStaffAndStus().Where(a => a.Studentno == studentno).FirstOrDefault()!=null)
            {
                result = true;
            }
            return result;
        }

        
    }
}
