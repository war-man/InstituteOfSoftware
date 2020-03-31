using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 显示每个咨询师分配了哪些学生
    /// </summary>
   public class ConsultAndStedent
    {
        /// <summary>
        /// 咨询师iD
        /// </summary>
        public string TeacherId
        {
            get;set;
        }
        /// <summary>
        /// 咨询师姓名
        /// </summary>
        public string TeacherName
        {
            get;set;
        }
        /// <summary>
        /// 这个集合是存储这个咨询师分配的学生
        /// </summary>
        /// <param name="s">学生实体</param>
        /// <returns></returns>
        public List<StudentPutOnRecord> ListStudent
        {
            get;
            set;
        }
    }
}
