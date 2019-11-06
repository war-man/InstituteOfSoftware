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


        /// <summary>
        /// 根据主键或类型名称查询单条数据
        /// </summary>
        /// <param name="Id">Id或者是名称</param>
        /// <param name="Iskey">true-按主键查询,false-按名称查询</param>
        /// <returns></returns>
        public CourseType FindSingeData(string Id, bool Iskey)
        {
            CourseType new_c = new CourseType();
            if (Iskey)
            {
                //是主键
                int find_id = Convert.ToInt32(Id);
                new_c = this.GetEntity(find_id);
            }
            else
            {
                //不是主键
                new_c = this.GetList().Where(c => c.TypeName == Id).FirstOrDefault();
            }

            return new_c;
        }
    }
}
