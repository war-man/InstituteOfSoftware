using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using Base_SysManage;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using EmployeesBusiness;
   public class TeacherBusiness:BaseBusiness<Teacher>
    {

        //员工 业务上下文
        private readonly EmployeesInfoManage db_emp;



        //阶段 业务上下文

        private readonly GrandBusiness db_grand;

        //专业上下文

        private readonly SpecialtyBusiness db_specialty;
        public TeacherBusiness()
        {
            db_grand = new GrandBusiness();
            db_emp = new EmployeesInfoManage();
            db_specialty = new SpecialtyBusiness();

        }



        /// <summary>
        /// 根据ID获取教员
        /// </summary>
        /// <param name="">教员ID</param>
        /// <returns>教员实体</returns>
        public Teacher GetTeacherByID(int id)
        {
           return  this.GetList().Where(t => t.TeacherID == id).FirstOrDefault();
        }

        /// <summary>
        /// / 更具员工编号获取员工
        /// </summary>
        /// <param name="EmpNo">员工编号</param>
        /// <returns>员工实体</returns>
        public EmployeesInfo GetEmpByEmpNo(string EmpNo)
        {
           return db_emp.GetList().Where(d => d.EmployeeId == EmpNo).FirstOrDefault();
               
        }


        /// <summary>
        /// 根据教员ID 获取教员阶段
        /// </summary>
        /// <param name="id">j教员ID</param>
        public List<Grand> GetGrandByTeacherID(int id)
        {
            List<Grand> resultList = new List<Grand>();

            BaseBusiness<TecharOnstageBearing> business = new BaseBusiness<TecharOnstageBearing>();

           var bus = business.GetList().Where(t => t.TeacherID == id).ToList();

            foreach (var item in bus)
            {
                resultList.Add( db_grand.GetList().Where(t => t.Id == item.Stage).FirstOrDefault());
            }

            return resultList;
        }

        /// <summary>
        /// 根据教员ID 获取教员的技术专业
        /// </summary>
        /// <param name="ID">教员ID</param>
        /// <returns>返回专业集合</returns>
        public List<Specialty> GetMajorByTeacherID(int ID)
        {
            List<Specialty> returnList = new List<Specialty>();

            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

           var majors= baseBusiness.GetList().Where(t=>t.TeacherID==ID).ToList();

            foreach (var item in majors)
            {
                returnList.Add(db_specialty.GetList().Where(t=>t.Id==item.ID).FirstOrDefault());

            }

            return returnList;


        }


        /// <summary>
        /// 获取教员的转业 并且对应阶段
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dictionary<Specialty,Grand> GetMajorInGrandByTeacherID(int id)
        {
            Dictionary<Specialty, Grand> dic = new Dictionary<Specialty, Grand>();

            BaseBusiness<TecharOnstageBearing> business = new BaseBusiness<TecharOnstageBearing>();

           var businesslist = business.GetList().Where(t => t.TeacherID == id).ToList();

            var majorlist = db_specialty.GetList();

            var grandlist = db_grand.GetList();

            foreach (var item in businesslist)
            {

                Specialty specialty = new Specialty();

                foreach (var item1 in majorlist)
                {
                    if (item1.Id == item.Major)
                    {
                        specialty = item1;

                        break;
                       
                    }
                }

                Grand g = new Grand();

                foreach (var item2 in grandlist)
                {
                    if (item2.Id == item.Stage)
                    {
                        g = item2;

                        break;

                    }
                }

                dic.Add(specialty,g);

            }


            return dic;
        }
    }
}
