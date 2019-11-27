using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    /// <summary>
    /// 员工模型视图
    /// </summary>
   public class EmpDetailView
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public Department DDAppId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string EmpName { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public Position PositionId { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        public Nullable<int> Age { get; set; }
        public string Nation { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        public string IdCardNum { get; set; }
        public Nullable<System.DateTime> ContractStartTime { get; set; }
        public Nullable<System.DateTime> ContractEndTime { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public System.DateTime EntryTime { get; set; }

        /// <summary>
        /// 出身日期
        /// </summary>
        public Nullable<System.DateTime> Birthdate { get; set; }
        public string Birthday { get; set; }
        public Nullable<System.DateTime> PositiveDate { get; set; }
        public string UrgentPhone { get; set; }
        public string DomicileAddress { get; set; }
        /// <summary>
        ///住址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }
        public Nullable<bool> MaritalStatus { get; set; }
        public Nullable<System.DateTime> IdCardIndate { get; set; }
        public string PoliticsStatus { get; set; }
        public string WorkExperience { get; set; }
        public Nullable<decimal> ProbationSalary { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public Nullable<System.DateTime> SSStartMonth { get; set; }
        public string BCNum { get; set; }
        public string Material { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDel { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Image { get; set; }
    }
}
