using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 用于晚归登记显示学生信息 业务类
    /// </summary>
    public class ProStudentInformationViewBusiness
    {
        private dbprosutdent_dbproheadmaster dbprosutdent_dbproheadmaster;
        private  ProStudentInformationBusiness dbproStudentInformationBusiness;
        private NotreturningLateBusiness dbnotreturningLateBusiness;
        private AccdationinformationBusiness dbacc;
        private DormInformationBusiness dbdorm;
        private EmployeesInfoManage dbemployeesInfoManage;

        /// <summary>
        /// 根据姓名返回学生涉及信息
        /// </summary>
        /// <param name="name0"></param>
        /// <returns></returns>
        public List<ProStudentInformationView> GetProStudentInformationViews(string name1) {
            dbproStudentInformationBusiness = new ProStudentInformationBusiness();
            dbprosutdent_dbproheadmaster = new dbprosutdent_dbproheadmaster();
            dbacc = new AccdationinformationBusiness();
            dbnotreturningLateBusiness = new NotreturningLateBusiness();
            dbdorm = new DormInformationBusiness();
            dbemployeesInfoManage = new EmployeesInfoManage();
            var list0= dbproStudentInformationBusiness.GetIQueryable().Where(a => a.Name == name1).ToList();
            var list1 = new List<ProStudentInformationView>();
            foreach (var item in list0)
            {
                ProStudentInformationView proStudentInformationView = new ProStudentInformationView();
                proStudentInformationView.ClassNO = dbprosutdent_dbproheadmaster.GetClassScheduleByStudentNumber(item.StudentNumber).ClassNumber;
                proStudentInformationView.Count = dbnotreturningLateBusiness.GeNotreturningLatesByStudentNumber(item.StudentNumber).Count;
                var obj0 = dbacc.GetAccdationByStudentNumber(item.StudentNumber);

                if (obj0==null)
                {
                    proStudentInformationView.DormNO = string.Empty;
                }
                else
                {
                    proStudentInformationView.DormNO = dbdorm.GetEntity(obj0.DormId).DormInfoName;
                }
                
                var obj1 = dbprosutdent_dbproheadmaster.GetHeadmasterByStudentNumber(item.StudentNumber);
                var obj2= dbemployeesInfoManage.GetEntity(obj1.informatiees_Id);
                proStudentInformationView.MasterName = obj2.EmpName;
                proStudentInformationView.MasterPhone = obj2.Phone;
                proStudentInformationView.StudentNmber = item.StudentNumber;
                proStudentInformationView.StuPhone = item.Telephone;
                proStudentInformationView.SutdentName = item.Name;
                list1.Add(proStudentInformationView);
            }
            return list1;
        }
    }
}
