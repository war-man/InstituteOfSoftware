using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于居住信息页面model
    /// </summary>
    public class AccdationView
    {
        public int ID { get; set; }
        /// <summary>
        /// 床位id
        /// </summary>
        public int BedId { get; set; }

        public string Studentnumber { get; set; }

        public string StudentName { get; set; }

       

    }
}
