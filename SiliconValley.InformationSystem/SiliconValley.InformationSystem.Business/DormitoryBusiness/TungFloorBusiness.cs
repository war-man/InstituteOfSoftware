using SiliconValley.InformationSystem.Business.Common;
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


        /// <summary>
        /// 自定义添加栋与楼的关系
        /// </summary>
        /// <param name="TungID">栋id</param>
        /// <param name="FloorID">楼id</param>
        /// <returns></returns>
        public bool CustomAdd(int TungID,int FloorID) {
            TungFloor tungFloor = new TungFloor();
            tungFloor.CreationTime = DateTime.Now;
            tungFloor.FloorId = FloorID;
            tungFloor.TungId = TungID;
            tungFloor.IsDel = false;
            tungFloor.Remark = "创建于" + tungFloor.CreationTime.Year + tungFloor.CreationTime.Month + tungFloor.CreationTime.Day + "添加楼层操作";
            tungFloor.TungId = TungID;
            try
            {
                this.Insert(tungFloor);
                BusHelper.WriteSysLog(tungFloor.Remark + "Dormitory/DormitoryInfo/ForTungAddFloor", Entity.Base_SysManage.EnumType.LogType.添加数据);
                return true;
            }
            catch (Exception ex)
            {

                BusHelper.WriteSysLog(ex.Message + "Dormitory/DormitoryInfo/ForTungAddFloor", Entity.Base_SysManage.EnumType.LogType.添加数据error);
                return false;
            }
        }
    }
}
