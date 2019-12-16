using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 学员保险模型表
    /// </summary>
   public class InsuranceView
    {
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentID { get; set; }

        public int ID { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        public string ClassNumber { get; set; }
        /// <summary>
        /// 保险人姓名
        /// </summary>
        public string Nameofinsurer { get; set; }
        /// <summary>
        /// 保险人电话
        /// </summary>
        public string Telephonenumber { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public bool Sex { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Dateofbirth { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDcardNo { get; set; }
        /// <summary>
        /// 监护人姓名
        /// </summary>
        public string NameofGuardian { get; set; }
        /// <summary>
        /// 监护人电话
        /// </summary>
        public string Guardiansphone { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public Nullable<DateTime> Startdate { get; set; }
        /// <summary>
        /// 到期日期
        /// </summary>
        public Nullable<DateTime> Duedate { get; set; }
        /// <summary>
        /// 保费
        /// </summary>
        public Nullable<double> premium { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
