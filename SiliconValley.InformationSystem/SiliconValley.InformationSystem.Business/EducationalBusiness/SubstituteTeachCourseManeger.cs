using SiliconValley.InformationSystem.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class SubstituteTeachCourseManeger:BaseBusiness<SubstituteTeachCourse>
    {

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="olddata"></param>
        /// <returns></returns>
        public bool Update_data(SubstituteTeachCourse olddata)
        {
            bool s = false;
            try
            {
                this.Update(olddata);
                s = true;
            }
            catch (Exception )
            {
                s = false;                 
            }
            return s;
                 
        }
    }
}
