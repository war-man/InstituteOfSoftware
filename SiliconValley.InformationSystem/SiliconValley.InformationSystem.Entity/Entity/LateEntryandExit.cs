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
    [Table(name: "LateEntryandExit")]
    public partial class LateEntryandExit
    {
        [Key]
        public int ID { get; set; }
        public string Student_Number { get; set; }
        public Nullable<System.DateTime> Entryandexittime { get; set; }
        public string Dormitoryaddress { get; set; }
        public Nullable<bool> Dateofregistration { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Remarks { get; set; }

        public virtual StudentInformation StudentInformation { get; set; }
    }
}
