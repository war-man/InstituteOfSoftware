using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 班级业务类
    /// </summary>
   public class ProClassSchedule:BaseBusiness<ClassSchedule>
    {
        private EmpQuarterClassBusiness dbempQuarterClass;
        /// <summary>
        /// 获取没有毕业的还在用的班级
        /// </summary>
        /// <returns></returns>
        public List<ClassSchedule> GetClassSchedules() {
            return this.GetIQueryable().Where(a => a.IsDelete == false && a.ClassStatus == false).ToList();
            
        }

        /// <summary>
        /// 根据班级编号获取正在使用的班级
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public ClassSchedule GetNotgraduatedClassByclassid(int  classid) {
            return this.GetClassSchedules().Where(a => a.id == classid).FirstOrDefault();
        }

        /// <summary>
        /// 获取S4的班级对象表
        /// </summary>
        /// <returns></returns>
        public List<ClassSchedule> GetS4Classes() {
            return this.GetClassSchedules().Where(a => a.grade_Id == 4).ToList();
        }
        /// <summary>
        /// 获取s4 阶段的班级 而且没有规划到毕业计划的班级
        /// </summary>
        public List<ClassSchedule> GetClassGraduating()
        {
            var classdata = this.GetS4Classes();
            dbempQuarterClass = new EmpQuarterClassBusiness();
            var querydata = dbempQuarterClass.GetEmpQuarters();
            for (int i = classdata.Count - 1; i >= 0; i--)
            {
                foreach (var item in querydata)
                {
                    if (classdata[i].id == item.Classid)
                    {
                        classdata.Remove(classdata[i]);
                        break;
                    }
                }
            }
            return classdata;
        }

        /// <summary>
        /// 毕业班级
        /// </summary>
        /// <returns></returns>
        public List<ClassSchedule> GetClassSchedulesed() {
            return this.GetIQueryable().Where(a => a.IsDelete == false && a.ClassStatus == true).ToList();
        }

        /// <summary>
        /// 传入的班级编号是否是毕业班级
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public bool isgraduationclass(int classid)
        {
            var query = this.GetClassSchedulesed().Where(a => a.id == classid).FirstOrDefault();
            if (query != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
