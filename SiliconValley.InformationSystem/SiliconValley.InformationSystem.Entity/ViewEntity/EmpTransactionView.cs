using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class EmpTransactionView
    {
        public int TransactionId { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// 异动类型编号 链接MoveType 表的ID
        /// </summary>
        public int TransactionType { get; set; }
       
        /// <summary>
        /// 异动产生时间
        /// </summary>
        public Nullable<System.DateTime> TransactionTime { get; set; }
        /// <summary>
        /// 原部门
        /// </summary>
        public Nullable<int> PreviousDept { get; set; }
        /// <summary>
        /// 原岗位
        /// </summary>
        public Nullable<int> PreviousPosition { get; set; }
        /// <summary>
        /// 现部门
        /// </summary>
        public Nullable<int> PresentDept { get; set; }
        /// <summary>
        /// 现岗位
        /// </summary>
        public Nullable<int> PresentPosition { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public Nullable<bool> IsDel { get; set; }
        /// <summary>
        /// 原来工资
        /// </summary>
        public Nullable<decimal> PreviousSalary { get; set; }
        /// <summary>
        /// 现在工资
        /// </summary>
        public Nullable<decimal> PresentSalary { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 续签前合同起始时间
        /// </summary>
        public Nullable<System.DateTime> BeforeContractStartTime { get; set; }
        /// <summary>
        /// 续签前合同终止时间
        /// </summary>
        public Nullable<System.DateTime> BeforeContractEndTime { get; set; }
        /// <summary>
        /// 续签后合同起始时间
        /// </summary>
        public Nullable<System.DateTime> AfterContractStartTime { get; set; }

        /// <summary>
        /// 续签后合同终止时间
        /// </summary>
        public Nullable<System.DateTime> AfterContractEndTime { get; set; }

        /// <summary>
        /// 异动类型名称
        /// </summary>
        public string Transactionname { get; set; }
        /// <summary>
        /// 原部门名称
        /// </summary>
        public string beforedname { get; set; }
        /// <summary>
        /// 原岗位名称
        /// </summary>
        public string beforepname { get; set; }
        /// <summary>
        /// 现部门名称
        /// </summary>
        public string afterdname { get; set; }
        /// <summary>
        /// 现岗位名称
        /// </summary>
        public string afterpname { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string ename { get; set; }
        /// <summary>
        /// 员工入职时间
        /// </summary>
        public System.DateTime EntryTime { get; set; }
    }
}
