using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于存放已交预录费或报名的学生
    /// </summary>
    [Table(name: "SingUpStudent")]
    public class SingUpStudent
    {
        /// <summary>
        /// 序列
        /// </summary>
        [Key]
        public int Number { get; set; }

        /// <summary>
        ///日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StuName { get; set; }

        /// <summary>
        /// 学生性别
        /// </summary>
        public string StuSex { get; set; }

        /// <summary>
        /// 学校
        /// </summary>
        public string SchoolsName { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string AreName { get; set; }

        /// <summary>
        /// 信息来源
        /// </summary>
        public string InfoType { get; set; }

        /// <summary>
        /// 缴费信息
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 咨询师
        /// </summary>
        public string Teacher { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Reak { get; set; }
             
    }
}
