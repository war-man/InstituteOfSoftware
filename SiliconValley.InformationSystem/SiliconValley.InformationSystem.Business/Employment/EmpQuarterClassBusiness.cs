using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 处于毕业计划中的班级 中间表业务类
    /// </summary>
    public class EmpQuarterClassBusiness : BaseBusiness<EmpQuarterClass>
    {
        /// <summary>
        ///获取可用的数据
        /// </summary>
        /// <returns></returns>
        public List<EmpQuarterClass> GetEmpQuarters()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据季度id返回参与季度的班级中间表对象
        /// </summary>
        /// <returns></returns>
        public List<EmpQuarterClass> GetEmpQuartersByYearID(int QuarterID) {
          return  this.GetEmpQuarters().Where(a => a.QuarterID == QuarterID).ToList();
        }

    }
}
