using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;

namespace SiliconValley.InformationSystem.Business.Consult_Business
{
    public class ConsultManeger : BaseBusiness<Consult>
    {
        EmployeesInfoManage emp_Entity = new EmployeesInfoManage();
        ConsultTeacherManeger Ct_Entity = new ConsultTeacherManeger();
        StudentDataKeepAndRecordBusiness Stu_Entity = new StudentDataKeepAndRecordBusiness();
        /// <summary>
        /// 根据分量Id或g根据咨询Id查询分量数据
        /// </summary>
        /// <param name="key">属性</param>
        /// <param name="value">值</param>
        /// <returns></returns>
         public List<Consult> GetConsultSingle(string key,int value)
        {
            switch (key)
            {
                case "Id":
                   return this.GetList().Where(c=>c.Id==value).ToList();
                case "TeacherName":
                  return  this.GetList().Where(c => c.TeacherName == value).ToList();

            }
            return new List<Consult>();
        }
        /// <summary>
        /// 获取所有咨询师的数据
        /// </summary>
        /// <returns></returns>
        public List<ConsultTeacher> GetConsultTeacher()
        {
            return Ct_Entity.GetList();
        }
        /// <summary>
        /// 获取单个咨询师的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmployeesInfo GetEmplyeesInfo(string id)
        {
            return emp_Entity.GetEntity(id);
        }
        /// <summary>
        /// 获取备案学生数据
        /// </summary>
        /// <returns></returns>
        public List<StudentPutOnRecord> GetStudentPutRecored()
        {
            return Stu_Entity.GetList();
        }
        /// <summary>
        /// 获取当个学生备案数据
        /// </summary>
        /// <param name="id">学生备案Id</param>
        /// <returns></returns>
        public StudentPutOnRecord GetSingleStudent(int? id)
        {
            return Stu_Entity.GetEntity(id);
        }       
        /// <summary>
        /// 获取单个咨询师指定的分量数据
        /// </summary>
        /// <param name="empid">员工id</param>
        /// <returns></returns>
        public ConsultAndStedent GetSingleData(string empid)
        {
            ConsultAndStedent cc1 = new ConsultAndStedent();
            cc1.TeacherId = empid;
            cc1.TeacherName = GetEmplyeesInfo(empid).EmpName;
            //根据员工id找到咨询师Id
            ConsultTeacher find_ct=  Ct_Entity.GetList().Where(c => c.Employees_Id == empid).FirstOrDefault();
            //根据咨询Id找到分量Id
            List<Consult> fin_c=  GetConsultSingle("TeacherName", find_ct.Id);
            //定义一个存储学生的集合
            List<StudentPutOnRecord> div = new List<StudentPutOnRecord>();
            //根据分量Id去找学生
            foreach (Consult item1 in fin_c)
            {
               div.Add(GetSingleStudent(item1.StuName));
            }
            cc1.ListStudent = div;
            return cc1;
        }
        /// <summary>
        /// 获取分量的学生
        /// </summary>
        /// <returns></returns>
        public List<ConsultAndStedent> GetEverythingData()
        {
            List<ConsultAndStedent> con_list = new List<ConsultAndStedent>();
            List<ConsultTeacher> ct_list = Ct_Entity.GetList();
            foreach (ConsultTeacher item in ct_list)
            {
                con_list.Add(GetSingleData(item.Employees_Id));
            }
            return con_list;
        }
    }
}
