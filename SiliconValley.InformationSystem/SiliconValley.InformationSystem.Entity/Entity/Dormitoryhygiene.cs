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

    [Table(name: "Dormitoryhygiene")]
    public partial class Dormitoryhygiene
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 乱挂衣物
        /// </summary>
        public bool Clothing { get; set; }
        /// <summary>
        /// 洗漱用品
        /// </summary>
        public bool Washsupplies { get; set; }
        /// <summary>
        /// 厕所整洁
        /// </summary>
        public bool Cleantoilet { get; set; }
        /// <summary>
        /// 鞋子摆放
        /// </summary>
        public bool Shoeplacement { get; set; }
        /// <summary>
        /// 床上堆放杂物
        /// </summary>
        public bool Beddebris { get; set; }
        /// <summary>
        /// 行李箱
        /// </summary>
        public bool Trunk { get; set; }
        /// <summary>
        /// 空床摆放
        /// </summary>
        public bool Emptyplacement { get; set; }
        /// <summary>
        /// 床单成面
        /// </summary>
        public bool Sheet { get; set; }
        /// <summary>
        /// 被子叠放
        /// </summary>
        public bool BeddingOverlay { get; set; }
        /// <summary>
        /// 检查人
        /// </summary>
        public int Inspector { get; set; }
        /// <summary>
        /// 房间编号
        /// </summary>
        public int DorminfoID { get; set; }
        /// <summary>
        /// 地面
        /// </summary>
        public bool Ground { get; set; }
        public bool IsDel { get; set; }
        public DateTime Addtime { get; set; }
        public string Remarks { get; set; }

        public DateTime RegisterTime { get; set; }
    }
}
