using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 寝室卫生登记
    /// </summary>
    public class DormitoryhygieneBusiness : BaseBusiness<Dormitoryhygiene>
    {
        /// <summary>
        /// 获取全部的卫生登记记录
        /// </summary>
        /// <returns></returns>
        public List<Dormitoryhygiene> GetDormitoryhygienes() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据记录id返回对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Dormitoryhygiene GetDormitoryhygieneByID(int ID) {
            return this.GetDormitoryhygienes().Where(a => a.ID == ID).FirstOrDefault();
        }

        /// <summary>
        /// 根据寝室号返回对应的登记记录列表
        /// </summary>
        /// <param name="DormID"></param>
        /// <returns></returns>
        public List<Dormitoryhygiene> GetDormitoryhygienesByDormID(int DormID) {
            return this.GetDormitoryhygienes().Where(a => a.DorminfoID == DormID).ToList();
        }

        /// <summary>
        /// 添加寝室卫生登记
        /// </summary>
        /// <param name="dormitoryhygiene"></param>
        /// <returns></returns>
        public bool AddDormitoryhygiene(Dormitoryhygiene dormitoryhygiene) {
            bool result = false;
            try
            {
                this.Insert( dormitoryhygiene);
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
