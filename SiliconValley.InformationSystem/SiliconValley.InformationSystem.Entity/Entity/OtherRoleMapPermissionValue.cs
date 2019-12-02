using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table("OtherRoleMapPermissionValue")]
    public class OtherRoleMapPermissionValue
    {
        [Key]
        public int Id { get; set; }

        public string RoleId { get; set; }

        public string PermissionValue { get; set; }

        public string Remark { get; set; }




    }
}
