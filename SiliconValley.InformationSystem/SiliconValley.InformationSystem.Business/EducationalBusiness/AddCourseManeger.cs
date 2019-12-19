using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class AddCourseManeger:BaseBusiness<AddCourse>
    {
        public static readonly RedisCache redisCache = new RedisCache();

        /// <summary>
        /// 获取所有加课数据
        /// </summary>
        /// <returns></returns>
        public List<AddCourse> GetALLAddCourseData()
        {
             redisCache.RemoveCache("AddCourseList");
            List<AddCourse> get_AddCourse_list = new List<AddCourse>();
            get_AddCourse_list = redisCache.GetCache<List<AddCourse>>("AddCourseList");
            if (get_AddCourse_list == null || get_AddCourse_list.Count == 0)
            {
                get_AddCourse_list = this.GetList();
                redisCache.SetCache("AddCourseList", get_AddCourse_list);
            }
            return get_AddCourse_list;
        }
        /// <summary>
        /// 加课
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public AjaxResult AddData(AddCourse course)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                this.Insert(course);
                a.Success = true;
                a.Msg = "成功提交";
                redisCache.RemoveCache("AddCourseList");
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = ex.Message;
            }

            return a;
        }
    }
}
