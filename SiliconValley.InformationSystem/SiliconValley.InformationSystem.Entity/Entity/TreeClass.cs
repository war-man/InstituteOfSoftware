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
        public string title//节点标题
        {
            get;
            set;
        }

        public List<TreeClass> children//子节点
        {
            get;
            set;
        }

        public string href//url
        {
            get;
            set;
        }

        public bool spread//节点是否初始展开默认false
        {
            get;
            set;
        }

        public bool @checked//节点是否初始为选中状态 默认false
        {
            get;
            set;
        }

        public bool disable//节点是否为禁用状态 默认false
        {
            get;
            set;
        }         
    }
}
