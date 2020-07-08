using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.TM_Data.MyViewEntity
{
    /// <summary>
    /// 教务迟到登记视图
    /// </summary>
    [Table("LaterecordView")]
    public class LaterecordView
    {
        [Key]
       public int Id { get; set; }
        /// <summary>
        /// --班级编号
        /// </summary>
        public int Class_Id { get; set; }
        /// <summary>
        /// --班级名称
        /// </summary>
        public bool ClassNumber { get; set; }
        /// <summary>
        /// --班主任是否到场
        /// </summary>
        public bool IsHavaHeadMaster { get; set; }

        public string HavaHeadMaster { get; set; }
        /// <summary>
        /// --任课老师是否到场
        /// </summary>
        public bool IsHavaTeacher { get; set; }

        public string HavaTeacher { get; set; }
        /// <summary>
        /// --PPT是否在讲
        /// </summary>
       public bool IshavaPPT { get; set; }
        
        public string HavaPPT { get; set; }
        /// <summary>
        /// --应到场人数
        /// </summary>
        public int PersonCount { get; set; }
        /// <summary>
        /// --实到场人数
        /// </summary>
        public int PctualCout { get; set; }
        /// <summary>
        /// --其他说明
        /// </summary>
        public string Reak { get; set; }
        /// <summary>
        /// --日期
        /// </summary>
        public DateTime Createdate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        public static Laterecord ToEntity(LaterecordView news)
        {
            Laterecord ter = new Laterecord();
            ter.Class_Id = news.Class_Id;
            ter.Createdate = news.Createdate;
            ter.Id = news.Id;
            ter.IsHavaHeadMaster = news.IsHavaHeadMaster;
            ter.IshavaPPT = news.IshavaPPT;
            ter.IsHavaTeacher = news.IsHavaTeacher;
            ter.PctualCout = news.PctualCout;
            ter.PersonCount = news.PersonCount;
            ter.Reak = news.Reak;
            ter.CreateUser = news.CreateUser;

            return ter;
        }
    }
}
