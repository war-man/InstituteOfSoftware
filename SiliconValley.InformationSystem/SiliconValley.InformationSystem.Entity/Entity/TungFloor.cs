using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    /// <summary>
    /// 栋楼层表
    /// </summary>
    [Table(name: "TungFloor")]
   public  class TungFloor
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 楼层Id
        /// </summary>
        public int FloorId { get; set;}
        /// <summary>
        /// 栋id
        /// </summary>
        public int TungId { get; set; }
        /// <summary>
        /// false可用，true 不可用
        /// </summary>
        public bool IsDel { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
