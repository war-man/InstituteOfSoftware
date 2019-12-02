using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    public class ProClassScheduleViewBusiness
    {
        private ProClassSchedule dbproClassSchedule;
        private ProHeadClass dbproHeadClass;
        private EmployeesInfoManage dbemp;
        private ProHeadmaster dbproheadmaster;
        private AccdationinformationBusiness dbacc;




       /// <summary>
       ///将实体对象转化为页面model
       /// </summary>
       /// <returns></returns>
        public List<ProClassScheduleView> Conversion(List<ClassSchedule> list0) {
            dbproHeadClass = new ProHeadClass();
            dbemp = new EmployeesInfoManage();
            dbproheadmaster = new ProHeadmaster();
            dbacc = new AccdationinformationBusiness();
            List<ProClassScheduleView> result0 = new List<ProClassScheduleView>();
           
            foreach (var item in list0)
            {
                ProClassScheduleView obj0 = new ProClassScheduleView();
                obj0.ClassNumber = item.ClassNumber;
                var obj1 = dbproHeadClass.GetClassByClassid(item.id);
                if (obj1 != null)
                {
                    var obj2 = dbproheadmaster.GetEntity(obj1.LeaderID);
                    var obj3 = dbemp.GetEntity(obj2.informatiees_Id);
                    obj0.EmpName = obj3.EmpName;
                }
                else
                {
                    obj0.EmpName = "该班级暂未分配班主任";
                }
                result0.Add(obj0);
            }
            return result0;
        }
    }
}
