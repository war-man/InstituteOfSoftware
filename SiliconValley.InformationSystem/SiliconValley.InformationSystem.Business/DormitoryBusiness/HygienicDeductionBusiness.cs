using SiliconValley.InformationSystem.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 班主任卫生扣分记录
    /// </summary>
   public class HygienicDeductionBusiness:BaseBusiness<HygienicDeduction>
    {
        /// <summary>
        /// 返回有效数据
        /// </summary>
        /// <returns></returns>
        public List<HygienicDeduction> GetHygienicDeductions() {
           return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 跟班主任编号返回数据
        /// </summary>
        /// <param name="headmasterid"></param>
        /// <returns></returns>
        public List<HygienicDeduction> GetHygienicByHeadmaster(int headmasterid) {
           return this.GetHygienicDeductions().Where(a => a.Headmaster == headmasterid).ToList();
        }

        /// <summary>
        /// 添加扣分项
        /// </summary>
        /// <param name="hygienicDeduction"></param>
        /// <returns></returns>
        public bool AddHygienicDeduction(HygienicDeduction hygienicDeduction) {
            bool result = false;
            try
            {
                this.Insert(hygienicDeduction);
                result = true;
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }
    }
}
