using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    public class DormitoryView
    {
        public int ID { get; set; }
        /// <summary>
        /// 0的意思是房间类型不是学生入住的，1为男生，2为女生
        /// </summary>
        public int SexType { get; set; }
        /// <summary>
        /// 房间类型id
        /// </summary>
        public int RoomStayTypeId { get; set; }
        /// <summary>
        ///房间号码
        /// </summary>
        public string DormInfoName { get; set; }
        /// <summary>
        /// 房间数量id
        /// </summary>
        public int RoomStayNumberId { get; set; }

        /// <summary>
        /// 方向左右，false为左，true为右
        /// </summary>
        public bool Direction { get; set; }
    }
}
