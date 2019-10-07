using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiliconValley.InformationSystem.Entity.Entity
{

    /// <summary>
    /// 房间入住数量类型表
    /// </summary>
    [Table(name: "RoomStayNumber")]
    public class RoomStayNumber
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [Key]
        public int Id { get; set; }

        public int StayNumber { get; set; }
        /// <summary>
        /// false可用，true 不可用
        /// </summary>
        public bool IsDelete { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}

