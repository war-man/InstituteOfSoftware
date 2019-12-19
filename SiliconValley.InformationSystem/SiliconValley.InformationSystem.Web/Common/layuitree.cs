using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiliconValley.InformationSystem.Web.Common
{
    public class layuitree
    {

        public layuitree()
        {
            children = new List<layuitree>();
        }
        public string title { get; set; }

        public string id { get; set; }

        public List<layuitree> children {get;set;}

        public bool @checked{ get; set; }


        /// <summary>
        /// 是否展开
        /// </summary>
        public bool spread { get; set; }

        public string field { get; set; }
    }
}