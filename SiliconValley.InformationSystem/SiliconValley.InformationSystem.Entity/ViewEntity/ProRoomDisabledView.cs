using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于房间禁用页面，
    /// </summary>
   public class ProRoomDisabledView
    {
        public int ID { get; set; }
        public string ProhibitRemark { get; set; }
        public int RoomStayTypeId { get; set; }
        public int SexType { get; set; }
        public string DormInfoName { get; set; }
        public string Address { get; set; }
    }
}
