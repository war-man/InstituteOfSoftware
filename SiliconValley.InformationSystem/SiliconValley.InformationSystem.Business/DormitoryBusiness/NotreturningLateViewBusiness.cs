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
    public class NotreturningLateViewBusiness
    {
        private NotreturningLateBusiness dbnotreturningLateBusiness;

        private ProStudentInformationBusiness dbproStudentInformationBusiness;

        private ProHeadmaster dbproHeadmaster;

        private InstructorListBusiness dbinstructorListBusiness;

        private EmployeesInfoManage dbemployeesInfoManage;
        
        /// <summary>
        /// 返回晚归列表
        /// </summary>
        /// <param name="name0">学生名字</param>
        /// <param name="startime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public List<NotreturningLateView> GetNotreturningLateView(string name0, DateTime startime, DateTime endtime) {
            dbnotreturningLateBusiness = new NotreturningLateBusiness();
            dbproStudentInformationBusiness = new ProStudentInformationBusiness();
            dbproHeadmaster = new ProHeadmaster();
            dbinstructorListBusiness = new InstructorListBusiness();
            dbemployeesInfoManage = new EmployeesInfoManage();
            var list0 = dbnotreturningLateBusiness.GetNotreturningLates().Where(a => a.RegisterTime >= startime && a.RegisterTime <= endtime).ToList();
            List<NotreturningLate> list1 = new List<NotreturningLate>();
            if (!string.IsNullOrEmpty(name0))
            {
               var list2= dbproStudentInformationBusiness.GetIQueryable().Where(a => a.Name == name0).ToList();
                foreach (var item in list2)
                {
                    list1.AddRange(list0.Where(a => a.StudentNumber == item.StudentNumber).ToList());
                }
            }
            else
            {
                list1.AddRange(list0);
            }
            List<NotreturningLateView> list3 = new List<NotreturningLateView>();
            foreach (var item in list1)
            {
                NotreturningLateView notreturningLateView = new NotreturningLateView();
                var obj1= dbproHeadmaster.GetEntity(item.HeadMasterID);
               var obj2 =dbemployeesInfoManage.GetEntity(obj1.informatiees_Id);
                notreturningLateView.HeadMasterName = obj2.EmpName;
                var obj3 = dbinstructorListBusiness.GetEntity(item.Inspector);
                var obj4 = dbemployeesInfoManage.GetEntity(obj3.EmployeeNumber);
                notreturningLateView.InspectorName = obj4.EmpName;
                notreturningLateView.Reason = item.Reason;
                notreturningLateView.RegisterTime = item.RegisterTime;
                var obj5 = dbproStudentInformationBusiness.GetEntity(item.StudentNumber);
                notreturningLateView.StudentName = obj5.Name;
                notreturningLateView.StudentNumber = obj5.StudentNumber;
                list3.Add(notreturningLateView);
            }
            return list3;
        }

    }
}
