using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.CourseSyllabusBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    public class CourseTypeBusiness : BaseBusiness<CourseType>
    {

        /// <summary>
        /// 获取课程类型集合
        /// </summary>
        /// <returns></returns>
        public List<CourseType> GetCourseTypes()
        {
            return this.GetList().Where(d=>d.IsDelete==false).ToList();
        }
    }
}
