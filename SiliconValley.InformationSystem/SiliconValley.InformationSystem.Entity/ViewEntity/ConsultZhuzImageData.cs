﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class ConsultZhuzImageData
    {
        /// <summary>
        /// 月份
        /// </summary>
        public int MonthName
        {
            get;
            set;
        }
        /// <summary>
        /// 完成数量
        /// </summary>
        public int wanchengcount
        {
            get;set;
        }
        /// <summary>
        /// 未完成数量
        /// </summary>
        public int nowanchengcount
        {
            get;set;
        }
    }
}