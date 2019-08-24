using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;

namespace SiliconValley.InformationSystem.Business.ClassSchedule_Business
{
   public class ClassScheduleBusiness:BaseBusiness<ClassSchedule>
    {
        //学生委员职位
        BaseBusiness<Members> MemBers = new BaseBusiness<Members>();
        //学生委员
        BaseBusiness<ClassMembers> Business = new BaseBusiness<ClassMembers>();
        //根据班级号查询出所有学员
        ScheduleForTraineesBusiness ss = new ScheduleForTraineesBusiness();
        //班级群管理
        BaseBusiness<GroupManagement> GetBase = new BaseBusiness<GroupManagement>();
        /// <summary>
        /// 通过班级名称获取学号，姓名，职位
        /// </summary>
        /// <returns></returns>
        public List<ClassStudentView> ClassStudentneList(string classid)
        {
           
            List<ClassStudentView> listview = new List<ClassStudentView>();
            var list = ss.ClassStudent(classid);
            foreach (var item in list)
            {
                ClassStudentView classStudentView = new ClassStudentView();
                if (Business.GetList().Where(a => a.Studentnumber == item.StudentNumber && a.IsDelete == false).ToList().Count>0)
                {
                  classStudentView.Nameofmembers = MemBers.GetList().Where(c => c.ID == Business.GetList().Where(a => a.Studentnumber == item.StudentNumber && a.IsDelete == false).First().Typeofposition && c.IsDelete == false).FirstOrDefault().Nameofmembers;
             
                }
               
                classStudentView.Name = item.Name;
            
                classStudentView.StuNameID = item.StudentNumber;
                if (classStudentView!=null)
                {
                    listview.Add(classStudentView);
                }
            }
            return listview;



        }

        /// <summary>
        /// 根据班级查询返回出班级人数,微信号,QQ号,班级名称
        /// </summary>
        /// <param name="claassid"></param>
        /// <returns></returns>
        public List< ClassdetailsView> Listdatails( string claassid)
        {
            int count = ss.ClassStudent(claassid).Count();
            List<ClassdetailsView> list = new List<ClassdetailsView>();
            ClassdetailsView classdetailsView = new ClassdetailsView();
          var x=  GetBase.GetList().Where(a => a.ClassNumber == claassid && a.IsDelete == false).FirstOrDefault();
            if (x != null)
            {
                classdetailsView.count = count;
                classdetailsView.QQ = x.QQGroupnumber;
                classdetailsView.WeChat = x.WechatGroupNumber;
                classdetailsView.ClassName = claassid;
            }
            else
            {
                classdetailsView.count = count;
                classdetailsView.QQ = "未记录";
                classdetailsView.WeChat = "未记录";
                classdetailsView.ClassName = claassid;
            }
            list.Add(classdetailsView);
            return list;
        }
    }
}
