using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiliconValley.InformationSystem.Business.Employment
{
    public class EmpClassBusiness : BaseBusiness<EmpClass>
    {

        private EmploymentStaffBusiness dbemploymentStaffBusiness;
        /// <summary>
        /// 获取所有的专员带班记录
        /// </summary>
        /// <returns>专员带班集合</returns>
        public List<EmpClass> GetEmpClassFormServer()
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
            return this.GetEmpClassFormServer().Where(a => a.EmpStaffID == EmplotStaffID).ToList();
        }

        /// <summary>
        /// 获取所有的班级对象
        /// </summary>
        /// <returns></returns>
        public List<ClassSchedule> GetClassFormServer()
        {

            ClassScheduleBusiness dbclass = new ClassScheduleBusiness();
            return dbclass.GetIQueryable().Where(a => a.IsDelete == false).ToList();


        }

        /// <summary>
        /// 根据班级id获取已毕业班级
        /// </summary>
        /// <param name="classiD"></param>
        /// <returns></returns>
        public ClassSchedule GetClassedByID(int classiD)
        {
            return this.GetClassFormServer().Where(a => a.id == classiD && a.IsDelete == false && a.ClassStatus == true).FirstOrDefault();
        }
        /// <summary>
        /// 根据班级id获取正在学习的班级
        /// </summary>
        /// <param name="classiD"></param>
        /// <returns></returns>
        public ClassSchedule GetClassingByID(int classiD)
        {
            return this.GetClassFormServer().Where(a => a.id == classiD && a.IsDelete == false && a.ClassStatus == false).FirstOrDefault();
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
                ClassSchedule classed = this.GetClassedByID(item.ClassId);
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
                ClassSchedule classed = this.GetClassingByID(item.ClassId);
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
            var resultdata = this.GetClassFormServer().Where(a => a.grade_Id == 3 || a.grade_Id == 4).ToList();
            return resultdata.Where(a => a.ClassStatus == false & a.IsDelete == false).ToList();
        }
        /// <summary>
        /// 获取所有的毕业班
        /// </summary>
        /// <returns></returns>
        public List<ClassSchedule> GetGraduations()
        {
            var resultdata = this.GetClassFormServer().Where(a => a.grade_Id == 4).ToList();
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
            var empclasslist = this.GetEmpClassFormServer();
            //分配的班级它的班级编号就会出现在这个带班记录中

            for (int i = alldata.Count - 1; i >= 0; i--)
            {
                foreach (var empclass in empclasslist)
                {
                    if (alldata[i].id == empclass.ClassId)
                    {
                        alldata.Remove(alldata[i]);
                        break;
                    }
                }
            }
            return alldata;
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
        public Grand GetGrandByID(int? GrandID)
        {
            return this.GetGrandAll().Where(a => a.Id == GrandID).FirstOrDefault();

        }
        /// <summary>
        /// 根据班级编号返回阶段对象
        /// </summary>
        /// <param name="ClassNo"></param>
        /// <returns></returns>
        public Grand GetGrandByClassid(int classid)
        {
            var classdata = this.GetClassFormServer().Where(a => a.id == classid).FirstOrDefault();
            return this.GetGrandByID(classdata.grade_Id);
        }
        /// <summary>
        /// 添加专员带班
        /// </summary>
        /// <param name="empClass"></param>
        /// <returns></returns>

        public bool AddEmpClass(EmpClass empClass)
        {

            bool result = false;
            try
            {
                this.Insert(empClass);
                result = true;

                //BusHelper.WriteSysLog("Obtainemployment区域EmpClass控制器ClassToEmpstaff方法成功", EnumType.LogType.上传文件异常);
            }
            catch (Exception ex)
            {
                result = false;
                //BusHelper.WriteSysLog("Obtainemployment区域EmpClass控制器ClassToEmpstaff方法", EnumType.LogType.上传文件异常);
            }
            return result;
        }

        /// <summary>
        /// 根据员工编号返回员工带班记录
        /// </summary>
        /// <param name="empinfoid"></param>
        /// <returns></returns>
        public List<EmpClass> GetEmpClassesByempinfoid(string empinfoid)
        {
            dbemploymentStaffBusiness = new EmploymentStaffBusiness();
            var a = dbemploymentStaffBusiness.GetEmploymentByEmpInfoID(empinfoid);
            return this.GetEmpsByEmpID(a.ID);
        }

    }
}
