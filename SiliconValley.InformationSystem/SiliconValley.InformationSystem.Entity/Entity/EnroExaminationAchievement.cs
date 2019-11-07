using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table(name: "EnroExaminationAchievement")]
    public class EnroExaminationAchievement
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNumber { get; set; }
        /// <summary>
        /// 报考专业
        /// </summary>
        public string Major { get; set; }
        /// <summary>
        /// 层次
        /// </summary>
        public string Arrangement { get; set; }
        /// <summary>
        /// 报考科类
        /// </summary>
        public string Families { get; set; }
        /// <summary>
        /// 院校名称
        /// </summary>
        public string Nameofinstitution { get; set; }
        /// <summary>
        /// 考生号
        /// </summary>
        public string Birthnumber { get; set; }
        /// <summary>
        /// 成绩
        /// </summary>
        public int Achievement { get; set; }
        /// <summary>
        /// 录取情况
        /// </summary>
        public string Admissionsituation { get; set;
        }
    }
}
