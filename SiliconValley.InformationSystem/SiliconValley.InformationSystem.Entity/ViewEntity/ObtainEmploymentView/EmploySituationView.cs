using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView
{
    public class EmploySituationView
    {
        public int ID { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string StudentNO { get; set; }
        public string StudentName { get; set; }
        /// <summary>
        /// 未就业原因
        /// </summary>
        public string NoReasons { get; set; }

        public Nullable<int> EntinfoID { get; set; }

        public string EntinfoName { get; set; }
        /// <summary>
        /// 实际工资
        /// </summary>
        public string RealWages { get; set; }
        /// <summary>
        /// 预计薪资
        /// </summary>
        public string Salary { get; set; }

        public string City { get; set; }
        public string Telephone { get; set; }

        public string classno { get; set; }

        public string empname { get; set; }
    }
}
