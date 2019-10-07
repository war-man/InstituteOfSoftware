using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 宿舍楼层业务类
    /// </summary>
    public class DormitoryfloorBusiness:BaseBusiness<Dormitoryfloor>
    {
        /// <summary>
        /// 获取全部可用的楼层
        /// </summary>
        /// <returns></returns>
        public List<Dormitoryfloor> GetDormitoryfloors() {
            return this.GetIQueryable().Where(a => a.IsDelete == false).ToList();
        }

        /// <summary>
        /// 根据楼层id获取楼层对象
        /// </summary>
        /// <param name="FloorID"></param>
        /// <returns></returns>
        public Dormitoryfloor GetDormitoryfloorByFloorID(int FloorID) {
            return this.GetDormitoryfloors().Where(a => a.ID == FloorID).FirstOrDefault();
        }
    }
}
