using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{


    public class ProTeacher : BaseBusiness<Teacher>
    {

        private BaseBusiness<ClassTeacher> db_calssteacer;

        private EmployeesInfoManage dbemp;
        /// <summary>
        /// 根据这个班级编号获取教员对象
        /// </summary>
        /// <param name="ClassNumber"></param>
        /// <returns></returns>
        public Teacher ClassTeacher(string ClassNumber)
        {
            db_calssteacer = new BaseBusiness<ClassTeacher>();
            var tempteacherclass = db_calssteacer.GetIQueryable().Where(d => d.ClassNumber == int.Parse(ClassNumber) && d.IsDel == false).FirstOrDefault();
            return this.GetEntity(tempteacherclass.TeacherID);

        }

        /// <summary>
        ///根据员工i编号返回员工对像
        /// </summary>
        /// <param name="empinfoid"></param>
        /// <returns></returns>
        public EmployeesInfo GetEmpInfo(string empinfoid)
        {
            dbemp = new EmployeesInfoManage();
           return dbemp.GetEntity(empinfoid);
        }

        /// <summary>
        /// 根据班级编号返回老师员工对象
        /// </summary>
        /// <param name="classno"></param>
        /// <returns></returns>
        public EmployeesInfo GetEmpInfoByClssno(string classno)
        {
            var a = this.ClassTeacher(classno);
            return this.GetEmpInfo(a.EmployeeId);
        }
    }
}
