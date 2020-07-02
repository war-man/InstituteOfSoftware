using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table("Client")]
    public class ReqClient
    {
     
        [Key]
        public string GuidKey { get; set; }

        public string IPAddress { get; set; }

        public DateTime RequestTime { get; set; }

        public string BrowseVersion { get; set; }

        public string OSVersion { get; set; }

    }
}
