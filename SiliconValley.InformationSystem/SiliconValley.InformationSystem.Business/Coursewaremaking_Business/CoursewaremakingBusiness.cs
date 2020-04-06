using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Coursewaremaking_Business
{
    /// <summary>
    /// 课件制作业务类
    /// </summary>
   public class CoursewaremakingBusiness: BaseBusiness<Coursewaremaking>
    {
        /// <summary>
        /// 课程制作上传
        /// </summary>
        /// <param name="coursewaremaking"></param>
        /// <returns></returns>
        public bool AddCoursewaremaking(Coursewaremaking coursewaremaking)
        {
            bool bit = false;
            try
            {
                coursewaremaking.Submissiontime = DateTime.Now;
                this.Insert(coursewaremaking);
                bit = true;
                BusHelper.WriteSysLog("课程上传成功", Entity.Base_SysManage.EnumType.LogType.添加数据);
            } 
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
              
            }
            return bit;
            
        }
        /// <summary>
        /// 删除课件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveCoursewaremaking(int id)
        {
            bool bit = false;
            try
            {
                this.Delete(this.GetEntity(id));
                bit = true;
                BusHelper.WriteSysLog("课程删除成功", Entity.Base_SysManage.EnumType.LogType.删除数据);
            }
            catch (Exception ex)
            {

                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.删除数据);
            }
            return bit;

           
        }
    }
}
