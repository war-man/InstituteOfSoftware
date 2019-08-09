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
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using System.Linq.Dynamic;

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
        public Dictionary<Specialty,List<Grand>> GetMajorInGrandByTeacherID(int id)
        {
            Dictionary<Specialty, List<Grand>> dic = new Dictionary<Specialty, List<Grand>>();

            BaseBusiness<TecharOnstageBearing> business = new BaseBusiness<TecharOnstageBearing>();

           var businesslist = business.GetList().Where(t => t.TeacherID == id).ToList().OrderBy(d=>d.Major).ToList();

            var majorlist = db_specialty.GetList().OrderBy(d=>d.Id).ToList();

            var grandlist = db_grand.GetList();

            foreach (var item in majorlist)
            {

                foreach (var item1 in businesslist)
                {

                    if (item1.Major == item.Id)
                    {

                        if (!ContainDic(dic, item))
                        {
                            Specialty specialty = item;

                            var grand = db_grand.GetList().Where(d => d.Id == item1.Stage).FirstOrDefault();

                            List<Grand> grandss = new List<Grand>();
                            grandss.Add(grand);

                            dic.Add(specialty, grandss);


                        }
                        else
                        {
                            dic[item].Add(db_grand.GetList().Where(d => d.Id == item1.Stage).FirstOrDefault());
                        }

                      

                    }

                }


            }

        


            return dic;
        }

        public bool ContainDic(Dictionary<Specialty, List<Grand>> source, Specialty key)
        {

            foreach (var item in source.Keys)
            {
                if (item == key)
                {

                    return true;
                }
            }

            return false;

        }

        public TeacherDetailView GetTeacherView(int id)
        {
            TeacherDetailView teacherResult = new TeacherDetailView();

            //获取教员信息
            Teacher t = this.GetTeacherByID(id);

            //获取教员基本信息
            EmployeesInfo emp = this.GetEmpByEmpNo(t.EmployeeId);
            teacherResult.EmpNo = emp.EmployeeId;
            teacherResult.Birthday = emp.Birthday;
            teacherResult.Name = emp.EmpName;
            teacherResult.Phone = emp.Phone;
            teacherResult.Sex = emp.Sex;
            teacherResult.TeacherID = t.TeacherID;

            //获取教员阶段信息
            teacherResult.Grands = this.GetGrandByTeacherID(t.TeacherID);

            //获取教员专业信息
            teacherResult.Major = this.GetMajorByTeacherID(t.TeacherID);

            //获取技术信息
            teacherResult.AttendClassStyle = t.AttendClassStyle;
            teacherResult.ProjectExperience = t.ProjectExperience;
            teacherResult.TeachingExperience = t.TeachingExperience;
            teacherResult.WorkExperience = t.WorkExperience;

            return teacherResult;

        }



        /// <summary>
        /// 获取教员的专业没有阶段
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="marjorId"></param>
        /// <returns></returns>
        public List<Grand> GetNoGrand(int teacherId,int majorId)
        {


            List<Grand> resultList = new List<Grand>();

            Specialty Major = db_specialty.GetSpecialtyByID(majorId);
            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var haveGrands = baseBusiness.GetList().Where(t => t.TeacherID == teacherId && t.Major == majorId).ToList().OrderBy(t=>t.Stage).ToList();

            //获取全部阶段 
           List<Grand> grands = db_grand.GetList().OrderBy(t=>t.Id).ToList();

            foreach (var item in grands)//1 2 3
            {
                if (!contains(item.Id, haveGrands))
                {
                    resultList.Add(item);

                }
            }

            //var ss = resultList.Distinct() as List<Grand>;

            return resultList;
        }

        public bool contains(int id, List<TecharOnstageBearing> grands)
        {
           return  grands.Where(e=>e.Stage==id).ToList().Count > 0 ;

        }

        public List<Grand> GetHaveGrand(int teacherid,int majorid)
        {

            List<Grand> grands = new List<Grand>();

            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var lsit= baseBusiness.GetList().Where(d => d.TeacherID == teacherid && d.Major == majorid).ToList();

            var grandlist = db_grand.GetList();

            foreach (var item in grandlist)
            {
                foreach (var item1 in lsit)
                {
                    if (item.Id == item1.Stage)
                    {
                        grands.Add(item);
                    }
                }
            }

            return grands;

        }

        
        public List<EmployeesInfo> employeesInfos()
        {

            BaseBusiness<Department> DepbaseBusiness = new BaseBusiness<Department>();

            var deplist= DepbaseBusiness.GetList().Where(d=>d.IsDel==false).ToList();

           var emps = db_emp.GetList().Where(d => d.IsDel == false).OrderBy(d=>d.PositionId).ToList();

            BaseBusiness<Position> PobaseBusiness = new BaseBusiness<Position>();

            var ind= PobaseBusiness.GetList().Where(d => d.IsDel == false).Where(d=>d.DeptId==2).ToList().OrderBy(d=>d.Pid).ToList();


            List<EmployeesInfo> result = new List<EmployeesInfo>();

            foreach (var item in emps)
            {
                foreach (var item1 in ind)
                {
                    if (item.PositionId == item1.Pid)
                    {
                        result.Add(item);
                    }
                }
            }

            return result;

           

        }
    }
}
