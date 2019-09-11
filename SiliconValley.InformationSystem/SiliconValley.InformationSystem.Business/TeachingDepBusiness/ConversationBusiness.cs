using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;



    /// <summary>
    /// 谈话业务类
    /// </summary>
    public class ConversationBusiness:BaseBusiness<ConversationRecord>
    {

        BaseBusiness<StudentInformation> db_student = new BaseBusiness<StudentInformation>();
        BaseBusiness<EmployeesInfo> db_emp = new BaseBusiness<EmployeesInfo>();
      

        /// <summary>
        /// 获取我所有的谈话记录
        /// </summary>
        /// <returns></returns>
        public List<ConversationRecord> GetConversationRecords()
        {
            var currentlogin = Base_UserBusiness.GetCurrentUser();
            return this.GetList().Where(d=>d.IsDel==false && d.EmployeeId==currentlogin.EmpNumber).ToList();

        }



        /// <summary>
        /// 获取视图模型
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public ConversationRecordView GetConversationRecordView(ConversationRecord record)
        {

            ConversationRecordView recordView = new ConversationRecordView();

            recordView.Content = record.Content;
            recordView.EmployeeId = record.EmployeeId;
            recordView.EmpName = db_emp.GetList().Where(d=>d.EmployeeId==record.EmployeeId).FirstOrDefault().EmpName;
            recordView.ID = record.ID;
            recordView.IsDel = record.IsDel;
            recordView.Result = record.Result;
            recordView.StudenNumber = record.StudenNumber;
            recordView.StudentName = db_student.GetList().Where(d=>d.IsDelete==false && d.StudentNumber==record.StudenNumber).FirstOrDefault().Name;
            recordView.Theme = record.Theme;
            recordView.Time = record.Time;


            return recordView;


        }

        /// <summary>
        /// 筛选谈话记录
        /// </summary> 
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <param name="studentname"></param>
        /// <returns></returns>
        public List<ConversationRecordView> GetScreenConversationRecord(string begindate, string studentname)
        {

            var currentlogin = Base_UserBusiness.GetCurrentUser();

            var studentlist = db_student.GetList().Where(d => d.Name.Contains(studentname)).ToList();


            List<ConversationRecordView> resultlist = new List<ConversationRecordView>();


            foreach (var item in studentlist)
            {
                var list = GetConversationRecords().Where(d => d.Time >= DateTime.Parse(begindate)  && d.StudenNumber == item.StudentNumber  &&d.EmployeeId== currentlogin.EmpNumber).ToList();




                foreach (var item1 in list)
                {
                    var tempobj = this.GetConversationRecordView(item1);

                  

                    if(tempobj !=null)

                        resultlist.Add(tempobj);

                }
            }


            return resultlist;

        }



        /// <summary>
        /// 获取学员的被访谈记录
        /// </summary>
        /// <param name="studentnumber">学员编号</param>
        /// <returns></returns>
        public List<ConversationRecordView> GetConversationRecordByStudentNumber(string studentnumber)
        {

         var temp = this.GetConversationRecords().Where(d=>d.StudenNumber==studentnumber).ToList();

            List<ConversationRecordView> resultlist = new List<ConversationRecordView>();

            foreach (var item in temp)
            {
               var obj = this.GetConversationRecordView(item);
                if (obj != null)
                    resultlist.Add(obj);
            }

            return resultlist;

        }


    }
}
