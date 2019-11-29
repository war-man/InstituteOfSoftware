using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 学员异动模型类
    /// </summary>
   public class TransactionView
    {
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public  string IDnumber { get; set; }
        /// <summary>
        /// 原班级
        /// </summary>
        public string OriginalClass { get; set; }
        /// <summary>
        /// 现在班级
        /// </summary>
        public string NowCLass { get; set; }
        /// <summary>
        /// 原班主任
        /// </summary>
        public string NowHeadmaster { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 宿舍地址
        /// </summary>
        public string Dormitoryaddress { get; set; }    
        /// <summary>
        /// 通讯地址
        /// </summary>
        public string Postaladdress { get; set; }
    }
}
