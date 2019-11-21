using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
   public  class ProStudentAndTeacherBussiness
    {
        private EmployeesInfoManage dbemp;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private ProHeadClass dbproHeadClass;
        private ProHeadmaster dbheadmaster;
        public EmployeesInfo GetEmpinfoByStudentNumber(string StudentNumber) {
            dbemp = new EmployeesInfoManage();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbproHeadClass = new ProHeadClass();
            dbheadmaster = new ProHeadmaster();
           var obj0=  dbproScheduleForTrainees.GetTraineesByStudentNumber(StudentNumber);
           var obj1=  dbproHeadClass.GetClassByClassNO(obj0.ClassID);
           return dbheadmaster.GetEmployeesInfoByHeadID(obj1.LeaderID);
        }
    }
}
