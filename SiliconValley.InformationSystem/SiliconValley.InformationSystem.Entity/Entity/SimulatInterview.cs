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

    [Table(name: "SimulatInterview")]
    public partial class SimulatInterview
    {
        [Key]
        public int ID { get; set; }
        public string StudentNo { get; set; }
        public Nullable<int> EntStaffID { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string knowMyself { get; set; }
        public string ShowAbility { get; set; }
        public string PerIntQues { get; set; }
        public string Resume { get; set; }
        public string ObjectInfo { get; set; }
        public string Pandect { get; set; }
    
       
    }
}
