using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{

   public class TeacherAddModel
    {

        [Required]
        public int TeacherID { get; set; }

        [Display(Name ="员工编号")]
        public string EmployeeId { get; set; }
        [Display(Name = "工作经验")]
        [MaxLength(200, ErrorMessage = "长度不能超过2")]
        public string WorkExperience { get; set; }
        [Display(Name = "项目经验")]
        [MaxLength(200, ErrorMessage = "长度不能超过200")]
        public string ProjectExperience { get; set; }
        [Display(Name = "教学经验")]
        [MaxLength(200, ErrorMessage = "长度不能超过200")]
        public string TeachingExperience { get; set; }
        [Display(Name = "带班经验")]
        [MaxLength(200, ErrorMessage = "长度不能超过200")]
        public string AttendClassStyle { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<int> MinimumCourseHours { get; set; }

    }


}
