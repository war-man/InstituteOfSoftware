using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 升学缴费
    /// </summary>
   public class DetailedcostView
    {
        public int id { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string Stidentid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 当前阶段
        /// </summary>
        public string CurrentStageID { get; set; }
        /// <summary>
        /// 阶段id
        /// </summary>
        public int StagesID { get; set; }
        /// <summary>
        /// 升学阶段
        /// </summary>
        public string NextStageID { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amountofmoney { get; set; }
        /// <summary>
        /// 应交费用
        /// </summary>
        public decimal ShouldJiao { get; set; }
        /// <summary>
        /// 欠费
        /// </summary>
        public decimal Surplus { get; set; }
        /// <summary>
        /// 是否交齐
        /// </summary>
        public string Isitinturn { get; set; }
        /// <summary>
        /// 班主任
        /// </summary>
        public string HeadmasterName { get; set; }
        /// <summary>
        /// 班主任联系电话
        /// </summary>
        public string Phone { get; set; }
    }
}
