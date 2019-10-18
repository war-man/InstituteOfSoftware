using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{

    [Table("HygienicDeduction")]
    /// <summary>
    /// 卫生扣分表 关联着卫生扣分详细表跟班主任id
    /// </summary>
    public class HygienicDeduction
    {
        [Key]
        public int ID { get; set; }
        public int DormitoryhygieneID { get; set; }
        public int Headmaster { get; set; }
        public string Remark { get; set; }
        public bool IsDel { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
