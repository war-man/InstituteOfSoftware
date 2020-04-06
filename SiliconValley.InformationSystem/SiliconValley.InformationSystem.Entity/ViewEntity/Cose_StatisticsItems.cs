using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    using SiliconValley.InformationSystem.Entity.MyEntity;

    /// <summary>
    /// 员工费用组成项 
    /// </summary>
   public class Cose_StatisticsItems
    {
        /// <summary>
        /// 员工
        /// </summary>
        public EmployeesInfo Emp { get; set; }

        /// <summary>
        /// 课时费
        /// </summary>
        public decimal EachingHourCost { set; get; }

        /// <summary>
        /// 值班费
        /// </summary>
        public decimal DutyCost { get; set; }

        /// <summary>
        /// 监考费
        /// </summary>
        public decimal InvigilateCost { get; set; }

        /// <summary>
        /// 阅卷费
        /// </summary>
        public decimal MarkingCost { get; set; }

        /// <summary>
        /// S1S2内训费
        /// </summary>
        public decimal S1S2_InternalTrainingCost { get; set; }

        /// <summary>
        /// S3S4内训费
        /// </summary>
        public decimal S3S4_InternalTrainingCost { get; set; }


        /// <summary>
        /// 课程研发费用
        /// </summary>
        public decimal CurriculumDevelopmentCost { get; set; }


    }
}
