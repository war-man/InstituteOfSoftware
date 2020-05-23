using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;
namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 教务处员工费用统计
    /// </summary>
    public class Staff_Cost_StatisticesDetailView
    {

        public Staff_Cost_StatisticesDetailView()
        {
            SatisfactionScore = 0;
            teachingitems = new List<TeachingItem>();
        }

        public EmployeesInfo emp
        {
            get; set;
        }

        /// <summary>
        /// 专业课课时
        /// </summary>
        public List<TeachingItem> teachingitems { get; set; }

        /// <summary>
        /// 内训次数
        /// </summary>
        public int InternalTraining_Count { get ; set; }

        /// <summary>
        /// 职业素养课，语数外 体育 次数
        /// </summary>
        public  List<TeachingItem> otherTeaccher_count { get; set; }

        /// <summary>
        /// 底课时
        /// </summary>
        public float BottomClassHour { get; set; }


        /// <summary>
        /// 满意度调查分数
        /// </summary>
        public float SatisfactionScore { get; set; }

        /// <summary>
        /// 阅卷份数
        /// </summary>
        public int Marking_item { get; set; }

        /// <summary>
        /// 监考次数
        /// </summary>
        public int Invigilate_Count { get; set; }

        /// <summary>
        /// 值班次数
        /// </summary>
        public List<DutyCount> Duty_Count { get; set; }

        /// <summary>
        /// ppt章节数
        /// </summary>
        public int PPT_Node {
            get;set;
        }

        /// <summary>
        /// 研发教材数量
        /// </summary>
        public int TeachingMaterial_Node { get; set; }




    }
}
