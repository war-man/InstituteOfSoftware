using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.CourseSchedulingSysBusiness
{

    using SiliconValley.InformationSystem.Entity.MyEntity;

    /// <summary>
    /// 课程业务类
    /// </summary>
   public class CurriculumBusiness:BaseBusiness<Curriculum>
    {


        public bool IsHave(List<Curriculum> sources, int curricid)
        {

            foreach (var item in sources)
            {
                if (item.CurriculumID == curricid)
                    return true;
                
            }

            return false;
        }

    }
}
