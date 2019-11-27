using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{

    [Table("SubstituteTeachCourse")]

    /// <summary>
    /// 代课表单
    /// </summary>
    public class SubstituteTeachCourse
    {

        [Key]
        /// <summary>
        /// 标识列
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public int Applier { get; set; }


        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyDate { get; set; }


        /// <summary>
        /// 理由
        /// </summary>
        public string Reson { get; set; }

        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime TeachDate{get;set;}


        /// <summary>
        /// 上课具体时间 (上午、下午)
        /// </summary>
        public string TeachDateSpec { get; set; }

        /// <summary>
        /// 班级
        /// </summary>
        public int ClassNumber { get; set; }
        
        public bool IsDel { get; set; }


        /// <summary>
        /// 上课老师
        /// </summary>
        public string Teacher { get; set; }
    }
}
