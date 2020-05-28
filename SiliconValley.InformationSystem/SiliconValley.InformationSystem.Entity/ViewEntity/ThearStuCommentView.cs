using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class ThearStuCommentView
    {

        public int Id { get; set; }

        public EmployeesInfo CommnetEr { get; set; }

        public StudentInformation CommnetObj { get; set; }

        public string Commnet { get; set; }

        public DateTime CommnetDate { get; set; }
    }
}
