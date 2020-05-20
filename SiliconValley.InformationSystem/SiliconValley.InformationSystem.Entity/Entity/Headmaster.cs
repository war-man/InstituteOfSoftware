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
    using System.Data.SqlClient;

    [Table(name: "Headmaster")]
    public partial class Headmaster
    {

        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public string informatiees_Id { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public Nullable<bool> IsDelete { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public Nullable<System.DateTime> AddTime { get; set; }
        /// <summary>
        /// true--表示该班主任可以上职素课，false--表示班主任不可以上职素课
        /// </summary>
        public Nullable<bool> IsAttend
        {
            get;set;
        }

        //public EmployeesInfo informatiees
        //{
        //    get
        //    {

        //        string sql = "select*from EmployeesInfo where EmployeeId="+ this.informatiees_Id;
        //        SqlDate sqlDate = new SqlDate();
        //        List<EmployeesInfo> x1 = sqlDate.SqlsFine<EmployeesInfo>(sql);
        //        return x1[0];
        //    }
        //}

    }
}
