using SiliconValley.InformationSystem.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 栋业务类
    /// </summary>
    public class TungBusiness:BaseBusiness<Tung>
    {

        /// <summary>
        /// 获取可用的栋
        /// </summary>
        /// <returns></returns>
        public List<Tung> GetTungs() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据栋id获取栋对象
        /// </summary>
        /// <param name="TungID"></param>
        /// <returns></returns>
        public Tung GetTungByTungID(int TungID) {
            return this.GetTungs().Where(a => a.Id == TungID).FirstOrDefault();
        }
    }
}
