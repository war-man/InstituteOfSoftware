using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView
{
   public class EmpStaffAndStuView
    {

        public int ID { get; set; }
        /// <summary>
        /// 季度id
        /// </summary>
        public int  QuarterID { get; set; }
        public string  Quartertitle { get; set; }
        /// <summary>
        ///就业阶段 1：第一次就业 2：第二次就业
        /// </summary>
        public string EmploymentStage { get; set; }
        public string Areaname { get; set; }
        public int AreaID { get; set; }
        public string Salary { get; set; }
        public string StudentName { get; set; }
        public string StudentNO { get; set; }
        public string classno { get; set; }
        public string empname { get; set; }
        public string EmploymentState { get; set; }

    }
}
