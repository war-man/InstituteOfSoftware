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
    [Table(name: "GoodsType")]
    public partial class GoodsType
    {
        [Key]
        public int Id { get; set; }
        public string GoodsTypeName { get; set; }
        public string Reak { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> AddTime { get; set; }


    }
}
