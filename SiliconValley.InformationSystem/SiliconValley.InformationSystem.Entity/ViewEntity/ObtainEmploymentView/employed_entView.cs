using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView
{
    /// <summary>
    /// 在学生就业登记的时候使用 涉及到公司名称之类的数据
    /// </summary>
    public class employed_entView
    {
        /// <summary>
        /// 学生编号
        /// </summary>
        public string StudentNO { get; set; }


        /// <summary>
        /// 公司id
        /// </summary>
        public Nullable<int> EntinfoID { get; set; }
        /// <summary>
        /// 实际工资
        /// </summary>
        public string RealWages { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string EntName { get; set; }
        /// <summary>
        /// 公司地址
        /// </summary>
        public string EntAddress { get; set; }
        /// <summary>
        /// 规模
        /// </summary>
        public string EntScale { get; set; }
        /// <summary>
        /// 性质
        /// </summary>
        public string EntNature { get; set; }

        /// <summary>
        /// 字符串  专业id
        /// </summary>
        public string EntSpee { get; set; }

    }
}
