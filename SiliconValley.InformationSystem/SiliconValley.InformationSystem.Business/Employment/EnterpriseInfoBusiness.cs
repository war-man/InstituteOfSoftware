using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 企业信息业务类
    /// </summary>
    public class EnterpriseInfoBusiness:BaseBusiness<EnterpriseInfo>
    {

        /// <summary>
        /// 所有的企业信息包括合作企业集合
        /// </summary>
        /// <returns></returns>
        public List<EnterpriseInfo> GetAll() {
           return  this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 返回合作企业集合
        /// </summary>
        /// <returns></returns>
        public List<EnterpriseInfo> GetCooAll() {
            return this.GetAll().Where(a => a.IsCooper == true).ToList();
        }
        /// <summary>
        /// 返回没有合作企业集合
        /// </summary>
        /// <returns></returns>
        public List<EnterpriseInfo> GetNoCooAll() {
            return this.GetAll().Where(a => a.IsCooper == false).ToList();
        }
        /// <summary>
        /// 根据id查找该id对应的对象全部企业信息
        /// </summary>
        /// <param name="EnterID"></param>
        /// <returns></returns>
        public EnterpriseInfo GetEnterByID(int? EnterID) {
            return this.GetAll().Where(a => a.ID == EnterID).FirstOrDefault();
        }

    }
}
