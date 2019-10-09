using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于page的model
    /// </summary>
   public class DormInformationView
    {

        /// <summary>
        /// 房间id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 房间类型id
        /// </summary>
        public int RoomStayTypeId { get; set; }

        /// <summary>
        ///是否满人
        /// </summary>
        public bool isfull { get; set; }

        /// <summary>
        /// 1 男 2女
        /// </summary>
        public int SexType { get; set; }

        /// <summary>
        /// 房间居住数量
        /// </summary>
        public int RoomStayNumber { get; set; }

        /// <summary>
        ///房间号码
        /// </summary>
        public string DormInfoName { get; set; }
    }
}
