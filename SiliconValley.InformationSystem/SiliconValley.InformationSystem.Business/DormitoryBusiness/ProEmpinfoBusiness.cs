using SiliconValley.InformationSystem.Business.DepartmentBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.PositionBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// Pro 员工业务类
    /// </summary>
   public class ProEmpinfoBusiness
    {
        //private DepartmentManage dbdpte;
        private EmployeesInfoManage dbempinfo;
        private PositionManage dbposition;

        /// <summary>
        /// 根据部门编号返回员工列表
        /// </summary>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        public List<EmployeesInfo> GetEmployeesInfosByDepteID(int DepteID)
        {
            dbposition = new PositionManage();
            dbempinfo = new EmployeesInfoManage();
            var list0 = dbposition.GetPositionByDepeID(DepteID);
            var list1 = dbempinfo.GetAll();
            var resutl0 = new List<EmployeesInfo>();
            for (int i = list1.Count-1; i>=0 ; i--)
            {
                foreach (var item in list0)
                {
                    if (list1[i].PositionId==item.Pid)
                    {
                        resutl0.Add(list1[i]);
                    }
                }
            }
            return resutl0;
        }
    }
}
