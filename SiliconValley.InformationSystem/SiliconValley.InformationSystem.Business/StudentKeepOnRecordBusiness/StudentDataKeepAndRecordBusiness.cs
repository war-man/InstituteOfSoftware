using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.StuSatae_Maneger;
using SiliconValley.InformationSystem.Business.Psychro;

namespace SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness
{
   public class StudentDataKeepAndRecordBusiness: BaseBusiness<StudentPutOnRecord>
    {
        StuStateManeger Statu_Entity = new StuStateManeger();
        SchoolYearPlanBusiness Syb_Entity = new SchoolYearPlanBusiness();
        #region 给市场用的方法
        /// <summary>
        /// 这是一个获取报名学生的方法
        /// </summary>
        /// <param name="EmpyId">员工编号</param>
        /// <returns></returns>
        public List<StudentPutOnRecord> GetrReport(string EmpyId)
        {
            //根据员工获取报名的数据
          return  this.GetList().Where(s => s.StuStatus_Id == 2 && s.EmployeesInfo_Id == EmpyId).ToList();
        }
        /// <summary>
        /// 获取某一年的每个月的上门量
        /// </summary>
        /// <param name="YeanName"></param>
        /// <returns></returns>
        public int[] GetYearGotoCount(DateTime YeanName)
        {
           List<StudentPutOnRecord> student_data= this.GetList().Where(s => s.StuIsGoto == true && s.StuVisit <= YeanName).ToList();//获取匹配的数据
            int j = 1;                                                                                                               //拿到一月到12的人数            
            int[] ary = new int[12];
            for (int i = 0; i < 12; i++)
            {
                ary[i] = GetMonthCount(student_data, j);
                j++;
            }
            return ary;
        }
        /// <summary>
        /// 这个方法是在学生报名之后就修改学生状态的方法
        /// </summary>
        /// <param name="id">备案id</param>
        /// <returns></returns>
        public bool ChangeStudentState(int id)
        {
            StudentPutOnRecord find_s= this.GetEntity(id);
            if (find_s!=null)
            {
               StuStatus find_statu= Statu_Entity.GetStu("已报名");
                if (find_statu!=null)
                {
                    find_s.StuStatus_Id = find_statu.Id;
                    find_s.StatusTime = DateTime.Now;
                    this.Update(find_s);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public int GetMonthCount(List<StudentPutOnRecord> student_list,int monthName)
        {
            var count = student_list.Select(s => Convert.ToDateTime(s.StuVisit).Month).Where(s => s == monthName).ToList().Count;
            return count;
        }
        /// <summary>
        /// 获取上门量
        /// </summary>
        /// <param name="PlanId"></param>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        public List<StudentPutOnRecord> GetGoSchoolByPlan(int? PlanId ,string EmpId)
        {
            //当前查询的计划
            var nowplan= Syb_Entity.GetPlanByID(PlanId);
            //拿到下一个计划
            var nextplan= Syb_Entity.GetNextPlan(nowplan);

            List<StudentPutOnRecord> list_s = this.GetList().Where(s => s.EmployeesInfo_Id == EmpId && s.StuIsGoto==true).ToList();
            List<StudentPutOnRecord> resultlist = new List<StudentPutOnRecord>();
            foreach (var item in list_s)
            {
                if (item.StuVisit>=nowplan.PlanDate)
                {
                    if (nextplan.ID!=0)
                    {
                        if (item.StuVisit<=nextplan.PlanDate)
                        {
                            resultlist.Add(item);
                        }
                    }
                    else
                    {
                        resultlist.Add(item);
                    }
                }
            }
            return resultlist;
        }
        /// <summary>
        /// 获取备案量
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="PlanId"></param>
        /// <returns></returns>
        public List<StudentPutOnRecord> GetBeanCount(string EmpId,int? PlanId)
        {
            //当前查询的计划
            var nowplan = Syb_Entity.GetPlanByID(PlanId);
            //拿到下一个计划
            var nextplan = Syb_Entity.GetNextPlan(nowplan);
            List<StudentPutOnRecord> list_s = this.GetList().Where(s => s.EmployeesInfo_Id == EmpId).ToList();
            List<StudentPutOnRecord> resultlist = new List<StudentPutOnRecord>();
            foreach (var item in list_s)
            {
                if (item.StuDateTime >= nowplan.PlanDate)
                {
                    if (nextplan.ID != 0)
                    {
                        if (item.StuVisit <= nextplan.PlanDate)
                        {
                            resultlist.Add(item);
                        }
                    }
                    else
                    {
                        resultlist.Add(item);
                    }
                }
            }
            return resultlist;
        }
        /// <summary>
        /// 获取报名量
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="PlanId"></param>
        /// <returns></returns>
        public List<StudentPutOnRecord> GetBaoMingCount(string EmpId, int? PlanId)
        {
            //当前查询的计划
            var nowplan = Syb_Entity.GetPlanByID(PlanId);
            //拿到下一个计划
            var nextplan = Syb_Entity.GetNextPlan(nowplan);
            List<StudentPutOnRecord> list_s = this.GetList().Where(s => s.EmployeesInfo_Id == EmpId && s.StuStatus_Id==2).ToList();
            List<StudentPutOnRecord> resultlist = new List<StudentPutOnRecord>();
            foreach (var item in list_s)
            {
                if (item.StatusTime >= nowplan.PlanDate)
                {
                    if (nextplan.ID != 0)
                    {
                        if (item.StuVisit <= nextplan.PlanDate)
                        {
                            resultlist.Add(item);
                        }
                    }
                    else
                    {
                        resultlist.Add(item);
                    }
                }
            }
            return resultlist;
        }
        #endregion
    }
}
