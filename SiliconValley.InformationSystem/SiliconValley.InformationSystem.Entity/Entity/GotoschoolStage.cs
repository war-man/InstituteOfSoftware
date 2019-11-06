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
    /// 升学阶段
    /// </summary>
    [Table(name: "GotoschoolStage")]
   public class GotoschoolStage
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 当前阶段
        /// </summary>
        public int CurrentStageID { get; set; }
        /// <summary>
        /// 下一个阶段
        /// </summary>
        public int NextStageID { get; set; }

    }
}
