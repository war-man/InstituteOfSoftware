using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    public class EmpClassBusiness : BaseBusiness<EmpClass>
    {
        /// <summary>
        /// 获取所有的专员带班记录
        /// </summary>
        /// <returns></returns>
        public List<EmpClass> GetEmpClass()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        // <summary>
        /// 根据就业专员id获取就业专员带班记录
        /// </summary>
        /// <param name="EmplotStaffID"></param>
        /// <returns></returns>
        public List<EmpClass> GetEmpsByEmpID(int EmplotStaffID)
        {
            return this.GetEmpClass().Where(a => a.EmpStaffID == EmplotStaffID).ToList();
        }

        /// <summary>
        /// 获取所有的班级对象
        /// </summary>
        /// <returns></returns>
        public List<ClassSchedule> GetClassALl()
        {
            ClassScheduleBusiness dbclass = new ClassScheduleBusiness();
            return dbclass.GetIQueryable().Where(a => a.IsDelete == false).ToList();
        }

        /// <summary>
        /// 根据班级id获取已毕业班级
        /// </summary>
        /// <param name="classiD"></param>
        /// <returns></returns>
        public ClassSchedule GetClassedByID(string classiD)
        {
            return this.GetClassALl().Where(a => a.ClassNumber == classiD && a.IsDelete == false && a.ClassStatus == true).FirstOrDefault();
        }
        /// <summary>
        /// 根据班级id获取正在学习的班级
        /// </summary>
        /// <param name="classiD"></param>
        /// <returns></returns>
        public ClassSchedule GetClassingByID(string classiD)
        {
            return this.GetClassALl().Where(a => a.ClassNumber == classiD && a.IsDelete == false && a.ClassStatus == false).FirstOrDefault();
        }
        /// <summary>
        /// 获取带班已毕业的
        /// </summary>
        /// <param name="emps"></param>
        /// <returns></returns>
        public List<ClassSchedule> GetClassedList(List<EmpClass> emps)
        {
            List<ClassSchedule> classedList = new List<ClassSchedule>();
            foreach (var item in emps)
            {
                ClassSchedule classed=this.GetClassedByID(item.ClassNO);
                classedList.Add(classed);
            }
            return classedList;
        }

        /// <summary>
        /// 获取带班未毕业的
        /// </summary>
        /// <param name="emps"></param>
        /// <returns></returns>
        public List<ClassSchedule> GetClassingList(List<EmpClass> emps)
        {
            List<ClassSchedule> classedList = new List<ClassSchedule>();
            foreach (var item in emps)
            {
                ClassSchedule classed = this.GetClassingByID(item.ClassNO);
                classedList.Add(classed);
            }
            return classedList;
        }

    }
}
