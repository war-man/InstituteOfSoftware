using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
   public class ProjectTeamDetailView
    {
        public int Id { get; set; }
        public ProjectDetailView Project { get; set; }
        public StudentInformation Student { get; set; }
        public string Task { get; set; }
        public string IsAccomplish { get; set; }

    }
}
