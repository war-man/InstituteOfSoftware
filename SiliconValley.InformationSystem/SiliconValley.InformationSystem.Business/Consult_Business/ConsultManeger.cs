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
        FollwingInfoManeger Fi_Entity = new FollwingInfoManeger();
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
        /// <summary>
        /// 获取今年的某个月份的分量次数
        /// </summary>
        /// <param name="monthName">月份名称</param>
        /// <param name="IsTure">未完成或者已完成</param>
        /// <returns></returns>
        public int GetMonthData(int monthName,string IsTure)
        {
            //获取当前的年份
            int year = DateTime.Now.Year;
            int list_count = 0;
            if (IsTure== "未完成")
            {
                list_count= this.GetList().Select(g => new { monthName = Convert.ToDateTime(g.ComDate).Month, yearName = Convert.ToDateTime(g.ComDate).Year, Isboming = GetSingleStudent(g.StuName).StatusTime == null ? false : true }).Where(g => g.yearName == year && g.monthName == monthName && g.Isboming==false).ToList().Count;
            }
            else if (IsTure == "已完成")
            {
                list_count= this.GetList().Select(g => new { monthName = Convert.ToDateTime(g.ComDate).Month, yearName = Convert.ToDateTime(g.ComDate).Year, Isboming = GetSingleStudent(g.StuName).StatusTime == null ? false : true }).Where(g => g.yearName == year && g.monthName == monthName && g.Isboming == true).ToList().Count;
            }

            return list_count;
        }
        /// <summary>
        /// 获取某个咨询师今年的所有月份分量信息
        /// </summary>
        /// <param name="monthName">月份</param>
        /// <param name="ConsultTeacherid">咨询师Id</param>
        ///    /// <param name="IsTure">完成、未完成</param>
        /// <returns></returns>
        public int GetTeacherMonthCount(int monthName, int? ConsultTeacherid,string IsTure)
        {
            //获取当前年份
            int year = DateTime.Now.Year;
            int teacherCount = 0;
            //获取当前年份的要查询的月份的分量数据
            List<Consult> Getlist_all = this.GetList().Where(c => Convert.ToDateTime(c.ComDate).Year == year && Convert.ToDateTime(c.ComDate).Month == monthName).ToList();
            if (IsTure=="完成")
            {                
                 
                //获取属于查询的咨询师的分量数据
                List<Consult> consults = Getlist_all.Where(c => c.TeacherName == ConsultTeacherid && Convert.ToDateTime(GetSingleStudent(c.StuName).StatusTime).Month == monthName).ToList();
                teacherCount = consults.Count;
            }
            else if (IsTure=="未完成")
            {
                //获取属于查询的咨询师的分量数据
                List<Consult> consults = Getlist_all.Where(c => c.TeacherName == ConsultTeacherid && (GetSingleStudent(c.StuName).StatusTime == null || Convert.ToDateTime(GetSingleStudent(c.StuName).StatusTime).Month != monthName)).ToList();
                teacherCount = consults.Count;
            }

            return teacherCount;
        }
        /// <summary>
        /// 获取今年所有月份咨询师完成量与未完成量
        /// </summary>
        /// <param name="Id">咨询师Id</param>
        /// <returns></returns>
        public List<ConsultZhuzImageData> GetImageData(string Id)
        {
            List<ConsultZhuzImageData> list_CZD = new List<ConsultZhuzImageData>();
            if (string.IsNullOrEmpty(Id))
            {               
                for (int i = 1; i <= 12; i++)
                {
                    ConsultZhuzImageData New_cc = new ConsultZhuzImageData();
                    New_cc.MonthName = i;
                    New_cc.wanchengcount = GetMonthData(i, "已完成");
                    New_cc.nowanchengcount = GetMonthData(i, "未完成");
                    list_CZD.Add(New_cc);
                }
                return list_CZD;
            }
            else
            {
                int t_Id = Convert.ToInt32(Id);
                for (int i = 1; i <= 12; i++)
                {
                    ConsultZhuzImageData New_cc = new ConsultZhuzImageData();
                    New_cc.MonthName = i;
                    New_cc.wanchengcount = GetTeacherMonthCount(i,t_Id, "完成");
                    New_cc.nowanchengcount = GetTeacherMonthCount(i, t_Id, "未完成");
                    list_CZD.Add(New_cc);
                }
                return list_CZD;
            }
             
 
        }
        /// <summary>
        /// 获取某个咨询师某个月的未完成跟完成数据
        /// </summary>
        /// <param name="monthName"></param>
        /// <param name="ConsultTeacherid"></param>
        /// <returns></returns>
        public List<ALLDATA> GetTeacherMonthCount(int monthName,int teacherid,int Number)
        {
            //获取当前年份
            int year = DateTime.Now.Year;
            List<ALLDATA> aLLDATAs = new List<ALLDATA>();
            //获取当前年份的要查询的月份的分量数据
            List<Consult> Getlist_all = this.GetList().Where(c => Convert.ToDateTime(c.ComDate).Year == year && Convert.ToDateTime(c.ComDate).Month == monthName).ToList();
            if (Number==0)
            {
                //完成的量
                List<Consult> ok_all = Getlist_all.Where(c => c.TeacherName == teacherid && Convert.ToDateTime(GetSingleStudent(c.StuName).StatusTime).Month == monthName).ToList();
                ALLDATA a_list = new ALLDATA();//完成的
                List<DivTwoData> towdata = new List<DivTwoData>();
                foreach (Consult item1 in ok_all)
                {
                    a_list.Name = 1;
                    DivTwoData d = new DivTwoData();
                    d.ConsultId = item1.Id;
                    d.StudentId = item1.StuName;
                    d.StudentName = GetSingleStudent(item1.StuName).StuName;
                    d.ConsultTeacherId = item1.TeacherName;
                    d.ConsultTeacherName = GetEmplyeesInfo(Ct_Entity.GetEntity(item1.TeacherName).Employees_Id).EmpName;
                    towdata.Add(d);
                }
                a_list.list = towdata;

                aLLDATAs.Add(a_list);

                return aLLDATAs;
            }
            else
            {
                //未完成的量
                List<Consult> no_all = Getlist_all.Where(c => c.TeacherName == teacherid && (Convert.ToDateTime(GetSingleStudent(c.StuName).StatusTime).Month != monthName || GetSingleStudent(c.StuName).StatusTime == null)).ToList();
                ALLDATA aa_list = new ALLDATA();//未完成的
                List<DivTwoData> towdata2 = new List<DivTwoData>();
                foreach (Consult item1 in no_all)
                {
                    aa_list.Name = 0;
                    DivTwoData d = new DivTwoData();
                    d.ConsultId = item1.Id;
                    d.StudentId = item1.StuName;
                    d.StudentName = GetSingleStudent(item1.StuName).StuName;
                    d.ConsultTeacherId = item1.TeacherName;
                    d.ConsultTeacherName = GetEmplyeesInfo(Ct_Entity.GetEntity(item1.TeacherName).Employees_Id).EmpName;
                    towdata2.Add(d);
                }
                aa_list.list = towdata2;
                aLLDATAs.Add(aa_list);

                return aLLDATAs;
            }         
        }
        /// <summary>
        /// 通过备案学生找分量信息
        /// </summary>
        /// <param name="id">备案学生Id</param>
        /// <returns></returns>
        public Consult TongStudentIdFindConsult(int id)
        {
            return this.GetList().Where(c => c.StuName == id).FirstOrDefault();
         
        }
        /// <summary>
        /// 根据分量查询备案学生跟踪信息
        /// </summary>
        /// <param name="ConsultId">分量Id</param>
        /// <returns></returns>
        public List<FollwingInfo> GetFllowInfoData(int? ConsultId)
        {
          return  Fi_Entity.GetList().Where(f => f.Consult_Id == ConsultId).ToList();
        }
    }
}
