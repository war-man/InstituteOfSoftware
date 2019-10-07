using SiliconValley.InformationSystem.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
   public  class TungFloorBusiness:BaseBusiness<TungFloor>
    {
        /// <summary>
        /// 获取全部可用的数据
        /// </summary>
        /// <returns></returns>
        public List<TungFloor> GetTungFloors()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();

        }

        /// <summary>
        /// 根据栋楼id返回栋楼对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public TungFloor GetTungFloorByID(int ID) {
            return this.GetTungFloors().Where(a => a.Id == ID).FirstOrDefault();
        }

        /// <summary>
        /// 获取栋id的栋楼层对象集合
        /// </summary>
        /// <param name="TungID"></param>
        /// <returns></returns>
        public List<TungFloor> GetTungFloorByTungID(int TungID) {
            return this.GetTungFloors().Where(a => a.TungId == TungID).ToList();
        }

        /// <summary>
        /// 根据栋ID和楼层ID返回栋楼层对象
        /// </summary>
        /// <param name="TungID"></param>
        /// <param name="FloorID"></param>
        /// <returns></returns>
        public TungFloor GetTungFloorByTungIDAndFloorID(int? TungID,int? FloorID) {
            return this.GetTungFloors().Where(a => a.TungId == TungID && a.FloorId == FloorID).FirstOrDefault();
        }
    }
}
