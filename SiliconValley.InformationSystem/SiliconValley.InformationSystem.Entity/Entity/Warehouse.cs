using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table(name: "Warehouse")]
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }
        public string WarehouseName { get; set; }
        public string Reamk { get; set; }
        public System.DateTime Adddate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        
    }
}
