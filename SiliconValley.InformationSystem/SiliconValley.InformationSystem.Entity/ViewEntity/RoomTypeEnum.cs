using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 房间涉及到的枚举
    /// </summary>
   public class RoomTypeEnum
    {
        /// <summary>
        /// 房间类型
        /// </summary>
        public enum RoomType
        {
            StudentRoom,
            VisitRoom,
            StaffRoom,
            Warehouse
        }

        /// <summary>
        /// 性别类型
        /// </summary>
        public enum SexType {
            Male,
            Female
        }

        public enum ShowType {
            高管,
            教职主任,
            后勤主任,
            教官
        }

       
    }
}
