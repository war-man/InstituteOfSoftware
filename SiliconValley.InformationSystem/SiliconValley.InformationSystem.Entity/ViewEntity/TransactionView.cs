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
        /// 异动ID
        /// </summary>
        public int ID { get; set; }
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
        /// 原班级id
        /// </summary>
        public int OriginalClass { get; set; }
        /// <summary>
        /// 原班级名称
        /// </summary>
        public string OriginalClassName { get; set; }
        /// <summary>
        /// 现在班级id
        /// </summary>
        public int NowCLass { get; set; }
        /// <summary>
        /// 现在班级名称
        /// </summary>
        public string NowCLassName { get; set; }
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
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 申请日期
        /// </summary>
        public DateTime Dateofapplication { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool? IsaDopt { get; set; }
        /// <summary>
        /// 阶段
        /// </summary>
        public string Stage { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime qBeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime qEndTime { get; set; }
        /// <summary>
        /// 是否需要领书
        /// </summary>
        public bool IsBookcollection { get; set; }
        /// <summary>
        /// 耽误学业原因
        /// </summary>
        public string Reasonsfordelay { get; set; }
    }
}
