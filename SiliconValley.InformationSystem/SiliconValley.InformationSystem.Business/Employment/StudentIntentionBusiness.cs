using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 就业学生的意向填写单
    /// </summary>
    public class StudentIntentionBusiness : BaseBusiness<StudnetIntention>
    {
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private EmpClassBusiness dbempClass;
        private EmpQuarterClassBusiness dbempQuarterClass;
        private QuarterBusiness dbquarter;
        /// <summary>
        /// 获取现在正在使用的就业意向的全部由数据
        /// </summary>
        /// <returns></returns>
        public List<StudnetIntention> GetStudentIntentions()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据学生编号返回这个意向表
        /// </summary>
        /// <param name="StudentNO"></param>
        /// <returns></returns>
        public StudnetIntention GetStudnetIntentionByStudentNO(string StudentNO)
        {
            return this.GetStudentIntentions().Where(a => a.StudentNO == StudentNO).FirstOrDefault();
        }

        /// <summary>
        /// 根据班级编号返回该班级学生的就业意向数据
        /// </summary>
        /// <param name="classno"></param>
        /// <returns></returns>
        public List<StudnetIntention> GetStudnetIntentionsByclassid(int classid)
        {
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            List<StudnetIntention> result = new List<StudnetIntention>();
            var los = dbproScheduleForTrainees.GetTraineesByClassid(classid);
            var ddd = this.GetStudentIntentions();
            foreach (var item in ddd)
            {
                foreach (var item1 in los)
                {
                    if (item.StudentNO == item1.StudentID)
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据计划id 以及学生id 获取这个对象
        /// </summary>
        /// <param name="quarterid"></param>
        /// <param name="studentno"></param>
        /// <returns></returns>
        public StudnetIntention GetIntention(int quarterid, string studentno)
        {
            return this.GetStudentIntentions().Where(a => a.QuarterID == quarterid && a.StudentNO == studentno).FirstOrDefault();
        }

        /// <summary>
        /// 根据就业专员id 返回这个带班班级学生的就业意向
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public List<StudnetIntention> GetIntentionsByEmpid(int empid)
        {
            dbempClass = new EmpClassBusiness();
            var empclassid = dbempClass.GetEmpsByEmpID(empid);
            List<StudnetIntention> result = new List<StudnetIntention>();
            foreach (var item in empclassid)
            {
                result.AddRange(this.GetStudnetIntentionsByclassid(item.ClassId));
            }
            return result;
        }

        /// <summary>
        /// 根据季度返回的数据
        /// </summary>
        /// <param name="Quarterid"></param>
        /// <returns></returns>
        public List<StudnetIntention> GetIntentionsByQuarterid(int Quarterid)
        {
            dbempQuarterClass = new EmpQuarterClassBusiness();
            List<EmpQuarterClass> list1 = dbempQuarterClass.GetEmpQuartersByQuarterID(Quarterid);
            return this.empQuarterClassconversiontoStudnetIntention(list1);
        }
        /// <summary>
        /// 季度id以及员工编号
        /// </summary>
        /// <param name="empid"></param>
        /// <param name="Quarterid"></param>
        /// <returns></returns>
        public List<StudnetIntention> GetIntentionsByEmpidAndQuarterid(int empid, int Quarterid)
        {
            dbempClass = new EmpClassBusiness();
            dbempQuarterClass = new EmpQuarterClassBusiness();

            List<EmpClass> list1 = dbempClass.GetEmpsByEmpID(empid);
            List<EmpQuarterClass> list2 = dbempQuarterClass.GetEmpQuartersByQuarterID(Quarterid);
            for (int i = list1.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < list2.Count; j++)
                {
                    if (list2[j].Classid != list1[i].ClassId)
                    {
                        if (j == list2.Count - 1)
                        {
                            list1.RemoveAt(i);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return this.empclassconversiontoStudnetIntention(list1);
        }
        /// <summary>
        /// 将员工带班记录转化为学生意向
        /// </summary>
        /// <returns></returns>
        public List<StudnetIntention> empclassconversiontoStudnetIntention(List<EmpClass> data)
        {
            List<StudnetIntention> result = new List<StudnetIntention>();
            foreach (var item in data)
            {
                result.AddRange(this.GetStudnetIntentionsByclassid(item.ClassId));
            }
            return result;
        }
        /// <summary>
        /// 将季度带班记录转化为学生意向
        /// </summary>
        /// <returns></returns>
        public List<StudnetIntention> empQuarterClassconversiontoStudnetIntention(List<EmpQuarterClass> data)
        {
            List<StudnetIntention> result = new List<StudnetIntention>();
            foreach (var item in data)
            {
                result.AddRange(this.GetStudnetIntentionsByclassid(item.Classid));
            }
            return result;
        }

        /// <summary>
        /// 根据年度获取数据
        /// </summary>
        /// <returns></returns>
        public List<StudnetIntention> GetStudnetIntentionsByYear(int year)
        {
            dbquarter = new QuarterBusiness();
            List<Quarter> list1 = dbquarter.GetQuartersByYear(year);
            return this.quarterconversionStudnetIntention(list1);
        }

        /// <summary>
        /// 根据年份以及就业专员id
        /// </summary>
        /// <param name="year"></param>
        /// <param name="empid"></param>
        /// <returns></returns>
        public List<StudnetIntention> GetStudnetIntentionsByYearAndEmpid(int year,int empid)
        {
            dbquarter = new QuarterBusiness();
            List<Quarter> list1 = dbquarter.GetQuartersByYearandempid(year,empid);
            return this.quarterconversionStudnetIntention(list1);
        }

        /// <summary>
        /// 将季度集合转化为学生一意向集合
        /// </summary>
        /// <param name="list1"></param>
        /// <returns></returns>
        public List<StudnetIntention> quarterconversionStudnetIntention(List<Quarter> list1) {
            List<StudnetIntention> result = new List<StudnetIntention>();
            foreach (var item in list1)
            {
                result.AddRange(this.GetIntentionsByQuarterid(item.ID));
            }
            return result;
        }

    }
}
