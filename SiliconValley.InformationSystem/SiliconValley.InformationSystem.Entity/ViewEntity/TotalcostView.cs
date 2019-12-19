using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 缴费总额结算
    /// </summary>
   public class TotalcostView
    {
        /// <summary>
        /// 类型id
        /// </summary>
        public Nullable<int> TypeID
        {
            get;set;
        }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 总额
        /// </summary>
        public Nullable<decimal> Total { get; set; }
        /// <summary>
        /// 子级
        /// </summary>
        public List<TotalcostView> ListTotalcostView { get;set; }
        /// <summary>
        /// 实例化
        /// </summary>
        public TotalcostView()
        {
            ListTotalcostView = new List<TotalcostView>();
        }


    }
}
