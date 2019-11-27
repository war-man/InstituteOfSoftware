using SiliconValley.InformationSystem.Entity.MyEntity;
using System.Collections.Generic;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    /// <summary>
    /// 教员阶段方向 业务类
    /// </summary>
    public class TecharOnstageBearingBusiness:BaseBusiness<TecharOnstageBearing>
    {

        public List<TecharOnstageBearing> AllTeacherOnstageBearing()
        {
            return this.GetList();
        }

    }
}
