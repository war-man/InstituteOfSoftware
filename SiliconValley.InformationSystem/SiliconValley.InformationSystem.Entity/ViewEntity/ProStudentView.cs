using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于显示为居住的学生列表的视图模型
    /// </summary>
    public class ProStudentView
    {
        public string StudentNumber { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Sex { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public string ClassNO { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Reack { get; set; }


        public string Telephone { get; set; }

        public int DormID { get; set; }

        public string DormName{ get; set; }

        public string datatype { get; set; }

        public string EmpinfoName { get; set; }
    }
}
