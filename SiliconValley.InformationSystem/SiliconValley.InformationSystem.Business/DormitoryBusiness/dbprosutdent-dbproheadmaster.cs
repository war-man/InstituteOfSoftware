using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    public class dbprosutdent_dbproheadmaster
    {
        private ProStudentInformationBusiness dbprosutdent;
        private ProHeadmaster dbproheader;
        private ProClassSchedule dbproclass;
        private ProHeadClass dbheadclass;
        private ProScheduleForTrainees dbprotrainess;
        private EmployeesInfoManage dbemp;
        private ProHeadClass dbproHeadClass;
        /// <summary>
        /// 根据学生编号返回带班班主任对象
        /// </summary>
        /// <param name="StudentNumber"></param>
        /// <returns></returns>
        public Headmaster GetHeadmasterByStudentNumber(string StudentNumber)
        {
            dbprotrainess = new ProScheduleForTrainees();
            dbheadclass = new ProHeadClass();
            dbproheader = new ProHeadmaster();
            ScheduleForTrainees querytrainess = dbprotrainess.GetTraineesByStudentNumber(StudentNumber);
            HeadClass queryheadclass = dbheadclass.GetClassByClassNO(querytrainess.ClassID);
            return dbproheader.GetHeadById(queryheadclass.LeaderID);
        }

        /// <summary>
        /// 根据学生编号返回班级对象
        /// </summary>
        /// <param name="StudentNumber"></param>
        /// <returns></returns>
        public ClassSchedule GetClassScheduleByStudentNumber(string StudentNumber)
        {
            dbproclass = new ProClassSchedule();
            dbprotrainess = new ProScheduleForTrainees();
            var obj0 = dbprotrainess.GetTraineesByStudentNumber(StudentNumber);
            return dbproclass.GetEntity(obj0.ClassID);
        }

        /// <summary>
        /// 返回员工老师员工对象
        /// </summary>
        /// <param name="StudentNumber"></param>
        /// <returns></returns>
        public EmployeesInfo GetEmployees(string StudentNumber) {
            dbemp = new EmployeesInfoManage();
            Headmaster obj0= this.GetHeadmasterByStudentNumber(StudentNumber);
            return dbemp.GetInfoByEmpID(obj0.informatiees_Id);
        }

        /// <summary>
        /// 根据班级返回对应的老师对象
        /// </summary>
        /// <param name="classno"></param>
        /// <returns></returns>
        public Headmaster GetHeadmasterByClassno(string classno)
        {
            dbproHeadClass = new ProHeadClass();
            dbproheader = new ProHeadmaster();
            var obj0 = dbproHeadClass.GetClassByClassNO(classno);
            return dbproheader.GetEntity(obj0.LeaderID);
        }
    }
}
