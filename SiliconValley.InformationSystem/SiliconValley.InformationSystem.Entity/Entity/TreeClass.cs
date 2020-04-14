using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
   public class TreeClass
    {
        public string id
        {
            get;
            set;
        }
        /// <summary>
        /// 节点标题
        /// </summary>
        public string title 
        {
            get;
            set;
        }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<TreeClass> children
        {
            get;
            set;
        }

        /// <summary>
        /// url
        /// </summary>
        public string href
        {
            get;
            set;
        }

        /// <summary>
        /// 节点是否初始展开默认false
        /// </summary>
        public bool spread
        {
            get;
            set;
        }

        /// <summary>
        /// 节点是否初始为选中状态 默认false
        /// </summary>
        public bool @checked
        {
            get;
            set;
        }

        /// <summary>
        /// 节点是否为禁用状态 默认false
        /// </summary>
        public bool disable
        {
            get;
            set;
        }
        /// <summary>
        /// 节点级别是多少
        /// </summary>
        public Nullable<int> grade {
            get;
            set;
        }
    }
}
