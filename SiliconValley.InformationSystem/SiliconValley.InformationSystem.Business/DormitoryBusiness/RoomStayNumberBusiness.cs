using SiliconValley.InformationSystem.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    public class RoomStayNumberBusiness : BaseBusiness<RoomStayNumber>
    {
        /// <summary>
        /// 获取全部的可用的数量类型
        /// </summary>
        /// <returns></returns>
        public List<RoomStayNumber> GetRoomStayNumbers()
        {
            return this.GetIQueryable().Where(a => a.IsDelete == false).ToList();
            
        }

        /// <summary>
        /// 根据数量类型id获取数量对象
        /// </summary>
        /// <param name="RoomStayNumberId">数量类型id</param>
        /// <returns></returns>
        public RoomStayNumber GetRoomStayNumberByRoomStayNumberId(int RoomStayNumberId) {
            return  this.GetRoomStayNumbers().Where(a => a.Id == RoomStayNumberId).FirstOrDefault();
           
        }
    }
}
