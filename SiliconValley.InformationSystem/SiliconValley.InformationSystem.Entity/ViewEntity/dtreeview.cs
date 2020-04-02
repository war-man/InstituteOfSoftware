using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于返回dtree数据绑定
    /// </summary>
    public class dtreeview
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        public string nodeId { get; set; }
        /// <summary>
        /// 节点内容
        /// </summary>
        public string context { get; set; }
        /// <summary>
        /// 是否叶子节点
        /// </summary>
        public bool last { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 父节点ID
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// 子数据
        /// </summary>
        public List<dtreeview> children { get; set; }
        /// <summary>
        /// 节点展开状态
        /// </summary>
        public bool spread { get; set; }

    }
}
