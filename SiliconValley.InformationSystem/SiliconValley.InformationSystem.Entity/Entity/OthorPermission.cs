using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    /// <summary>
    /// 其他权限
    /// </summary>
    /// 
    [Table(name: "OthorPermission")]
    public class OthorPermission
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 权限值
        /// </summary>
        public string PermissionValue { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desct { get; set; }

    }
}
