using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table(name: "Teachingtraining")]
    public partial class Teachingtraining
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 培训人
        /// </summary>
        public int Trainee { get; set; }
        /// <summary>
        /// 培训标题
        /// </summary 
        public string TrainingTitle { get; set; }
        /// <summary>
        /// 培训内容
        /// </summary>
        public string Trainingcontent { get; set; }
        /// <summary>
        /// 培训日期
        /// </summary>
        public Nullable<System.DateTime> TrainingDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public Nullable<bool> Isdel { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public Nullable<System.DateTime> AddTime { get; set; }

        /// <summary>
        /// 阶段
        /// </summary>
        public int Grand { get; set; }


    }
}
