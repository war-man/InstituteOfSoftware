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
    [Table("Classactivities")]
    public partial class Classactivities
    {
        [Key]
        public int ID { get; set; }
        public string ClassNumber { get; set; }
        public string place { get; set; }
        public Nullable<int> Number { get; set; }
        public string Activitytype { get; set; }
        public Nullable<System.DateTime> Activitytime { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> Addtime { get; set; }
    }
}
