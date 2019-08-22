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
                ClassSchedule classed = this.GetClassedByID(item.ClassNO);
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

        /// <summary>
        /// 获取s3跟s4的班级以及没有毕业的
        /// </summary>
        /// <returns></returns>
        public List<ClassSchedule> GetS3Class()
        {
            var resultdata = this.GetClassALl().Where(a => a.grade_Id == 3 || a.grade_Id == 4).ToList();
            return resultdata.Where(a => a.ClassStatus == false & a.IsDelete == false).ToList();
        }
        /// <summary>
        /// 获取所有的毕业班
        /// </summary>
        /// <returns></returns>
        public List<ClassSchedule> GetGraduations()
        {
            var resultdata = this.GetClassALl().Where(a => a.grade_Id == 4).ToList();
            return resultdata.Where(a => a.ClassStatus == true & a.IsDelete == false).ToList();
        }


        /// <summary>
        /// 判断班级是否是毕业班
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool IsGraduation(ClassSchedule schedule)
        {
            if (schedule.IsDelete == false && schedule.ClassStatus == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取s3或者时s4没有毕业也还没分配专员的对象集合
        /// </summary>
        /// <returns></returns>
        public List<ClassSchedule> NoDistribution()
        {
            // 获取s3跟s4的班级以及没有毕业的对象
            var alldata = this.GetS3Class();
            var resultdata = this.GetS3Class();
            //带班记录
            var empclasslist = this.GetEmpClass();
            //分配的班级它的班级编号就会出现在这个带班记录中
            foreach (var item in alldata)
            {
                foreach (var empclass in empclasslist)
                {
                    if (item.ClassNumber == empclass.ClassNO)
                    {
                        resultdata.Remove(item);
                    }
                }
            }
            return resultdata;
        }

        /// <summary>
        /// 获取所有的阶段对象
        /// </summary>
        /// <returns></returns>
        public List<Grand> GetGrandAll()
        {
            BaseBusiness<Grand> dbgrand = new BaseBusiness<Grand>();
            return dbgrand.GetIQueryable().Where(a => a.IsDelete == false).ToList();
        }

        /// <summary>
        /// 根据阶段id 获取阶段对象
        /// </summary>
        /// <returns></returns>
        public Grand GetGrandByID(int GrandID)
        {
            return this.GetGrandAll().Where(a => a.Id == GrandID).FirstOrDefault();

        }
        /// <summary>
        /// 根据班级编号返回阶段对象
        /// </summary>
        /// <param name="ClassNo"></param>
        /// <returns></returns>
        public Grand GetGrandByClassNo(string ClassNo)
        {
            var classdata = this.GetClassALl().Where(a => a.ClassNumber == ClassNo).FirstOrDefault();
            return this.GetGrandByID(classdata.grade_Id);
        }
    }
}
