using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 咨询师分量
    /// </summary>
   public class DivTwoData
    {
        /// <summary>
        /// 分量的Id
        /// </summary>
        public int ConsultId { get; set; }
        /// <summary>
        /// 咨询师Id
        /// </summary>
        public Nullable<int> ConsultTeacherId { get; set; }
        /// <summary>
        /// 咨询师名称
        /// </summary>
        public string ConsultTeacherName { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName
        {
            get;set;
        }
        /// <summary>
        /// 学生ID
        /// </summary>
        public Nullable<int> StudentId
        {
            get; set;
        }
    }
    /// <summary>
    /// 统计分量完成或未完成的量
    /// </summary>
    public class ALLDATA
    {
        /// <summary>
        /// 未完成或完成
        /// </summary>
        public int Name
        { get; set; }

        public List<DivTwoData> list { get; set; }
    }
}
