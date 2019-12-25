using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Psychro
{
    /// <summary>
    /// 异动业务类
    /// </summary>
    public class MrdEmpTransactionBusiness : BaseBusiness<EmpTransaction>
    {
        private SchoolYearPlanBusiness dbplan;
        /// <summary>
        /// 没有被伪删除的员工异动数据
        /// </summary>
        /// <returns></returns>
        public List<EmpTransaction> GetTransactions()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 根据员工编号获取这个员工的异动记录集合
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public List<EmpTransaction> GetTransactionsByEmpID(string EmployeeId)
        {
            return this.GetTransactions().Where(a => a.EmployeeId == EmployeeId).ToList();
        }

        /// <summary>
        /// 拿取降职跟升职的数据
        /// </summary>
        /// <returns></returns>
        public List<EmpTransaction> GetTransactionsByMoveType12()
        {
            return this.GetTransactions().Where(a => a.TransactionType == 1 || a.TransactionType == 2).ToList();
        }

        /// <summary>
        /// 根据年度计划返回该员工的升降职位的最后一跳数据
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="nowplan"></param>
        /// <returns></returns>
        public EmpTransaction GetTransactionByPlan_EmpID(string EmployeeId, SchoolYearPlan nowplan)
        {
            dbplan = new SchoolYearPlanBusiness();
             //获取了这个员工的升降职记录
             var emptralist = this.GetTransactionsByEmpID(EmployeeId);

            //拿去下一个计划对象
            var nextplan = dbplan.GetNextPlan(nowplan);


            //排除掉不属于这个阶段的记录
            List<EmpTransaction> onlist = new List<EmpTransaction>();

            foreach (var item in emptralist)
            {
                //有下一个计划
                if (nextplan.ID != 0)
                {
                    if (nowplan.PlanDate <= item.TransactionTime && item.TransactionTime < nextplan.PlanDate)
                    {
                        onlist.Add(item);
                    }
                }
                else
                {
                    if (nowplan.PlanDate <= item.TransactionTime)
                    {
                        onlist.Add(item);
                    }

                }
            }
            return onlist.OrderByDescending(a => a.TransactionTime).FirstOrDefault();
            
        }
    }
}
