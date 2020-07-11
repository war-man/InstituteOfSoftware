using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity
{
    /// <summary>
    /// 用于显示学生注册页面的数据
    /// </summary>
   public class ZhuceShowData
    {
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string stuName { get; set; }

        /// <summary>
        /// 学生性别
        /// </summary>
        public string stuSex { set; get; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string stuPhone { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IdCare { get; set; }

        /// <summary>
        /// 学生备案Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 班级编号
        /// </summary>
        public string Class_ID { get; set; }

        public bool YesHei { get; set; }
        /// <summary>
        /// 阶段名称
        /// </summary>
        public string gradeName { get; set; }
    }
}
