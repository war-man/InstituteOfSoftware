using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 学生缴费信息表
    /// </summary>
   public class tuition
    {
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IdCard { get; set; }

        /// <summary>
        /// 应缴费用
        /// </summary>
        public decimal ShouldMongy { get; set; }

        /// <summary>
        /// 实缴费用
        /// </summary>
        public decimal ActualMongy { get; set; }

        /// <summary>
        /// 阶段
        /// </summary>
        public string GrandName { get; set; }

        /// <summary>
        /// ture--交了预录费，false--没有交
        /// </summary>
        public bool Istur { get; set; }

        /// <summary>
        /// 预录费
        /// </summary>
        public decimal? Prerecordingfee { get; set; }

        /// <summary>
        /// 缴费详情
        /// </summary>
        public List<tuitiondetails> tuitiondetails { get; set; }
    }
}
