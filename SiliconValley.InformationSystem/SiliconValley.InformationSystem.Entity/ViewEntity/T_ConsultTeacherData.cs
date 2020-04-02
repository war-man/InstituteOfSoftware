using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于显示咨询师信息的实体
    /// </summary>
   public class T_ConsultTeacherData
    {
       /// <summary>
       /// 咨询id
       /// </summary>
       public int Id { get; set; }
       /// <summary>
       /// 咨询师名称
       /// </summary>
       public string EmpName { get; set; }
        /// <summary>
        /// 咨询师电话
        /// </summary>
       public string Phone { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
       public string Education { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
       public string BrainImage { get; set; }
        /// <summary>
        /// 是否在职
        /// </summary>
        public Nullable<bool> IsZhizhi { get; set; }
       /// <summary>
       /// 备注
       /// </summary>
        public string Rmark { get; set; }
        /// <summary>
        /// 政治面貌
        /// </summary>
        public string Politicsstatus { get; set; }
        /// <summary>
        /// 工作经验
        /// </summary>
        public string workExperence { get; set; }
        /// <summary>
        /// 是否转正(判断PositiveDate是否有值就行了)
        /// </summary>
        public bool IsZhangzhe { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public Nullable<int> ConGrade { get; set; }

    }
}
