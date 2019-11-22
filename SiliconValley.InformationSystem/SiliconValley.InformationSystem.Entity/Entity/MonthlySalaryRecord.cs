//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SiliconValley.InformationSystem.Entity.MyEntity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table(name: "MonthlySalaryRecord")]
    public partial class MonthlySalaryRecord
    {
        [Key]
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public Nullable<System.DateTime> YearAndMonth { get; set; }//年月份
        //public Nullable<decimal> AttendanceDay { get; set; }
        //public Nullable<decimal> BaseSalary { get; set; }
        //public Nullable<decimal> PositionSalary { get; set; }
        //public Nullable<int> PerformanceScore { get; set; }
        public Nullable<decimal> PerformanceSalary { get; set; }//绩效工资
        //public string NetbookSubsidy { get; set; }
        public Nullable<decimal> OvertimeCharges { get; set; }//加班费用
        public Nullable<decimal> Bonus { get; set; }  //奖金/元
        public Nullable<decimal> LeaveDeductions { get; set; }//（请假）扣款
        public Nullable<decimal> OtherDeductions { get; set; }//其他扣款
        public Nullable<decimal> PersonalSocialSecurity { get; set; }//个人社保
        //public string SocialSecuritySubsidy { get; set; }
        //public Nullable<decimal> PersonalIncomeTax { get; set; }
        public Nullable<decimal> Total { get; set; }//合计
        public Nullable<decimal> PayCardSalary { get; set; }//工资卡工资
        public Nullable<decimal> CasehSalary { get; set; }//现金工资
        public Nullable<bool> IsDel { get; set; }


    }
}
