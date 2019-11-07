using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    //这个类是学生信息综合类
   public class My_StudentDataOne
    {
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentputonereadName{ get; set; }
        /// <summary>
        /// 是否报名
        /// </summary>
        public string IsExitsSchool { get; set; }
        /// <summary>
        /// 备案日期
        /// </summary>
        public Nullable<System.DateTime> RecordData { get; set; }
        /// <summary>
        /// 是否上门参观
        /// </summary>
        public string IsVistSchool { get; set; }
        /// <summary>
        /// 咨询师名称
        /// </summary>
        public string ConsultTeacherName { get; set; }
        /// <summary>
        /// 分量日期
        /// </summary>
        public Nullable<System.DateTime> CoultData { get; set; }
        /// <summary>
        /// 咨询次数
        /// </summary>
        public Nullable<int> ConultNumber { get; set; }
        /// <summary>
        /// 所在班级
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 班主任
        /// </summary>
        public string ClassTeacher { get; set; }
        /// <summary>
        /// 技术老师
        /// </summary>
        public string Teacher { get; set; }
        /// <summary>
        /// 目前所在阶段
        /// </summary>
        public string Grand { get; set; }
        /// <summary>
        /// 备案人
        /// </summary>
        public string DataputRecordMan { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 专业名称
        /// </summary>
        public string ZhuanyeName { get; set; }
    }
}
