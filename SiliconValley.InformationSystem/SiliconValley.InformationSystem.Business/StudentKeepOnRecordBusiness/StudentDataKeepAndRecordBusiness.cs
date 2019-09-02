using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness
{
   public class StudentDataKeepAndRecordBusiness: BaseBusiness<StudentPutOnRecord>
    {
        /// <summary>
        /// 这是一个获取报名学生的方法
        /// </summary>
        /// <param name="EmpyId">员工编号</param>
        /// <returns></returns>
        public List<StudentPutOnRecord> GetrReport(string EmpyId)
        {
            //根据员工获取报名的数据
          return  this.GetList().Where(s => s.StuStatus_Id == 2 && s.EmployeesInfo_Id == EmpyId).ToList();
        }



    }
}
