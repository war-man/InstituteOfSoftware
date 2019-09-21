using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    //y用于显示报名数据
    public class ShowBaomingListView
    {

        public string StudentName { get; set; }

        public string ClassNo { get; set; }

        public string ProfessionalTeacher { get; set; }

        public string Headmaster { get; set; }

        public string ChannelStaffName { get; set; }

        public DateTime BeianDate { get; set; }

        public DateTime GoSchoolDate { get; set; }

        public DateTime BaomingDate { get; set; }

        public string OldSchoolName { get; set; }
    }
}
