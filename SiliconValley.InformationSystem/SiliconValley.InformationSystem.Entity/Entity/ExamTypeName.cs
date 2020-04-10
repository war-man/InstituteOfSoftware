using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table(name: "ExamTypeName")]
    public class ExamTypeName
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 类型名称  （升学考试，阶段考试）
        /// </summary>
        public string TypeName { get; set; }
    }
}
