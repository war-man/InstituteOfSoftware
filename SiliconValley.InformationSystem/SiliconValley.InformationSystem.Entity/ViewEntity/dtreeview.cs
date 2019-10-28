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
        public string nodeId { get; set; }//节点ID
        public string context { get; set; }//节点内容
        public bool last { get; set; }//是否叶子节点
        public int level { get; set; }//层级
        public string parentId { get; set; }//父节点ID
        public List<dtreeview> children { get; set; }
        public bool spread { get; set; }//节点展开状态

    }
}
