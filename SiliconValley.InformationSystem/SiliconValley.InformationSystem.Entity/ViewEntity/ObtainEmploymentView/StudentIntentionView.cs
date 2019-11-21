using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView
{
    /// <summary>
    /// 学生就业意向的视图model
    /// </summary>
  public  class StudentIntentionView
    {
        /// <summary>
        /// 班级名称
        /// </summary>
        public string classnumnber { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 学生编号
        /// </summary>
        public string StudentNO { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 亲属关系
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// 亲属名字
        /// </summary>
        public string RelativesName { get; set; }
        /// <summary>
        /// 亲属电话
        /// </summary>
        public string Familyphone { get; set; }

        /// <summary>
        /// 籍贯
        /// </summary>
        public string Nation { get; set; }

        /// <summary>
        /// 高中学校
        /// </summary>
        public string StuSchoolName { get; set; }

        /// <summary>
        /// 意向城市
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Salary { get; set; }

    }
}
