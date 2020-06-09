using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table(name: "HeiHu")]
    public class HeiHu
    {
        [Key]
        public int Id { get; set; }

        public string IdCard { get; set; }
    }
}
