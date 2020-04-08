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
    /// 课程制作表
    /// </summary>
    [Table("Coursewaremaking")]
  public  class Coursewaremaking
    {
        [Key]
       public int id { get; set; }
        /// <summary>
        /// 研发人
        /// </summary>
        public string RampDpersonID { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string MakingType { get; set; }
        /// <summary>
        /// 章节数
        /// </summary>
        public int Chaptersnumber { get; set; }
        /// <summary>
        ///  提交时间
        /// </summary>
        public DateTime Submissiontime{ get;set;}
        /// <summary>
        /// 专业
        /// </summary>
        public int? MajorID { get; set; }
        /// <summary>
        /// 阶段
        /// </summary>
        public int StageID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
       public string Title { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string Filepath { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
