 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Entity.ViewEntity;

namespace SiliconValley.InformationSystem.Business.ClassesBusiness
{

  public  class ScheduleForTraineesBusiness:BaseBusiness<ScheduleForTrainees>
    {       
        /// <summary>
        /// 通过班级名称获取学员
        /// </summary>
        /// <param name="ClassID"></param>
        /// <returns></returns>
        public List<StudentInformation> ClassStudent(int ClassID)
        {
            //CurrentClass是否为正常状态 查询使用以参数为班级的学员学号
            var stuid = this.GetList().Where(a => a.ID == ClassID && a.CurrentClass == true).ToList();
            //查询班级所有学生
            StudentInformationBusiness student = new StudentInformationBusiness();

            List<StudentInformation> resullist = new List<StudentInformation>();

      

            foreach (var item in stuid)
            {
               var studentobj = student.GetList().Where(d => d.IsDelete == false && d.StudentNumber == item.StudentID).FirstOrDefault();

                if (studentobj != null)
                    resullist.Add(studentobj);

            }

            return resullist;
        }
        /// <summary>
        /// 根据学号获取班级名称(ClassID班级名称)
        /// </summary>
        /// <param name="Sutdentid">学员学号</param>
        /// <returns></returns>
        public ScheduleForTrainees SutdentCLassName(string Sutdentid)
        {

            return this.GetList().Where(q => q.CurrentClass == true && q.StudentID == Sutdentid).FirstOrDefault();
        }
    }
}
