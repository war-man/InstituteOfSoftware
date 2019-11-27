using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{

    /// <summary>
    /// 加课业务类
    /// </summary>
    public class AddCourseBusiness:BaseBusiness<AddCourse>
    {
        /// <summary>
        /// 课程业务类实例
        /// </summary>
        private readonly CourseBusiness db_course;

        public AddCourseBusiness()
        {

            db_course = new CourseBusiness();
        }


       

    }
}
