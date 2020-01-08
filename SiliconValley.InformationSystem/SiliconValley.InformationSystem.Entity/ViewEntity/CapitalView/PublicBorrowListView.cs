using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.CapitalView
{
   public class PublicBorrowListView
    {
        public int ID { get; set; }
        public double DebitMoney { get; set; }
        public string Debitwhy { get; set; }
        public System.DateTime date { get; set; }
        public string EmpName { get; set; }
        public string DeptName { get; set; }
        public string PositionName { get; set; }
        public string Phone { get; set; }
    }
}
