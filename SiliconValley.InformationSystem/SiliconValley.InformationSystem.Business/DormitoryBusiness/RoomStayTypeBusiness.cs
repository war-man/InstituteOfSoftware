using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 房间类型业务类
    /// </summary>
    public class RoomStayTypeBusiness:BaseBusiness<RoomStayType>
    {

        /// <summary>
        /// 获取全部可用的房间类型
        /// </summary>
        /// <returns></returns>
        public List<RoomStayType> GetRoomStayTypes() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
    }
}
