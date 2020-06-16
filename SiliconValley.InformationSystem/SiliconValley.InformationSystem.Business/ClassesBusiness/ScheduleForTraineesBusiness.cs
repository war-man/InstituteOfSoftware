 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Business.ClassDynamics_Business;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;

namespace SiliconValley.InformationSystem.Business.ClassesBusiness
{

  public  class ScheduleForTraineesBusiness:BaseBusiness<ScheduleForTrainees>
    {
        //学员异动类
        ClassDynamicsBusiness classDynamicsBusiness = new ClassDynamicsBusiness();
        //学员状态基础数据
        BaseBusiness<Basicdat> BasicdatBusiness = new BaseBusiness<Basicdat>();

        /// <summary>
        /// 通过班级名称获取学员
        /// </summary>
        /// <param name="ClassID"></param>
        /// <returns></returns>
        public List<StudentInformation> ClassStudent(int ClassID)
        {
            BaseBusiness<ScheduleForTraineesview> ScheduleForTraineesviewBusiness = new BaseBusiness<ScheduleForTraineesview>();
            //CurrentClass是否为正常状态 查询使用以参数为班级的学员学号
            var stuid = ScheduleForTraineesviewBusiness.GetListBySql<ScheduleForTraineesview>("select * from ScheduleForTraineesview where Classid=" + ClassID);

            //查询班级所有学生
            StudentInformationBusiness student = new StudentInformationBusiness();

            List<StudentInformation> resullist = new List<StudentInformation>();
            foreach (var item in stuid)
            {
                var studentobj = student.GetList().Where(a => a.StudentNumber == item.StudentNumber).FirstOrDefault();

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
            ClassScheduleBusiness classSchedule = new ClassScheduleBusiness();
            //IsGraduating
            var x= this.GetList().Where(q => q.CurrentClass == true && q.StudentID == Sutdentid).FirstOrDefault();
            if (x==null)
            {
                var zx = this.GetList().Where(a => a.CurrentClass == false && a.IsGraduating == true&&a.StudentID==Sutdentid).FirstOrDefault();
                if ( zx!= null)
                {
                    return zx;
                }
                else
                {
                    var z = classDynamicsBusiness.GetList().Where(a => a.Studentnumber == Sutdentid).OrderByDescending(a => a.ID).FirstOrDefault();
                    return this.GetList().Where(q => q.StudentID == Sutdentid && q.ID_ClassName == z.FormerClass).FirstOrDefault();
                }
            }
            else
            {
                return x;
            }
        }
        /// <summary>
        /// 获取学生班级或者状态
        /// </summary>
        /// <param name="Sutdentid"></param>
        /// <returns></returns>
        public string ClassNames(string Sutdentid)
        {
            var x = this.GetList().Where(q => q.CurrentClass == true && q.StudentID == Sutdentid).FirstOrDefault();
            if (x==null)
            {
                var zx = this.GetList().Where(a => a.CurrentClass == false && a.IsGraduating == true && a.StudentID == Sutdentid).FirstOrDefault();
                if (zx != null)
                {
                    return "毕业,1";
                }
                else
                {
                    var z = classDynamicsBusiness.GetList().Where(a => a.Studentnumber == Sutdentid && a.IsaDopt == true).OrderByDescending(a => a.ID).FirstOrDefault();
                    return BasicdatBusiness.GetEntity(z.States).Name + ",1";
                }
             
            }
            else
            {
                return x.ClassID+",2";
            }
        }
    }
}
