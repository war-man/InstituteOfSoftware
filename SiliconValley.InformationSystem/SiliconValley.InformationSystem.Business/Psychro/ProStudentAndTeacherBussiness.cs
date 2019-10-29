using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Psychro
{
    /// <summary>
    /// 学生 学生班级 班级 班主任 班主任班级 员工 业务类
    /// </summary>
    public class ProStudentAndTeacherBussiness
    {
        private EmployeesInfoManage dbempinfo;

        private ProScheduleForTrainees dbschedtrain;

        private ProStudentInformationBusiness dbstudent;

        private ProHeadClass dbheadclass;

        private ProHeadmaster dbmaster;

        /// <summary>
        /// 根据学生的编号获取班主任员工对象
        /// </summary>
        /// <param name="Studentnumber"></param>
        /// <returns></returns>
        public EmployeesInfo GetEmpinfoByStudentNumber(string Studentnumber)
        {
            dbschedtrain = new ProScheduleForTrainees();
            dbheadclass = new ProHeadClass();
            dbmaster = new ProHeadmaster();
            dbempinfo = new EmployeesInfoManage();
            ScheduleForTrainees queryScheduleForTrainees = dbschedtrain.GetTraineesByStudentNumber(Studentnumber);
            HeadClass queryHeadClass= dbheadclass.GetClassByClassNO(queryScheduleForTrainees.ClassID);
            Headmaster queryHeadmaster= dbmaster.GetEntity(queryHeadClass.LeaderID);
            return dbempinfo.GetEntity(queryHeadmaster.informatiees_Id);
        }
    }
}
