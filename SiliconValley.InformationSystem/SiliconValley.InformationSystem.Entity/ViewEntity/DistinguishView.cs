using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    public class DistinguishView
    {
        public List<DormInformationView> leftroom { get; set; }

        public List<DormInformationView> rightroom { get; set; }
    }
}
