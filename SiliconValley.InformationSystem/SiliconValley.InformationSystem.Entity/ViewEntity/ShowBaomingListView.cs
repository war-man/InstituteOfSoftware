using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于显示报名数据
    /// </summary>
    public class ShowBaomingListView
    {
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }
        /// <summary>
        /// 所在班级
        /// </summary>
        public string ClassNo { get; set; }
        /// <summary>
        /// 任课老师
        /// </summary>
        public string ProfessionalTeacher { get; set; }
        /// <summary>
        /// 班主任
        /// </summary>
        public string Headmaster { get; set; }
        /// <summary>
        /// 招生老师
        /// </summary>
        public string ChannelStaffName { get; set; }
        /// <summary>
        /// 备案日期
        /// </summary>
        public DateTime BeianDate { get; set; }
        /// <summary>
        /// 上门日期
        /// </summary>
        public DateTime GoSchoolDate { get; set; }
        /// <summary>
        /// 报名日期
        /// </summary>
        public DateTime BaomingDate { get; set; }
        /// <summary>
        /// 之前的学校
        /// </summary>
        public string OldSchoolName { get; set; }
    }
}
