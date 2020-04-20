using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.CourseSyllabusBusiness
{
    using SiliconValley.InformationSystem.Entity.Entity;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    public class TeacherHoursCostBusiness : BaseBusiness<TeacherHourCost>
    {

        public List<TeacherHourCost> teacherHourCosts(bool IsUsing = true)
        {
            return this.GetList().Where(d => d.IsUsing == IsUsing).ToList();
        }

    }
}
