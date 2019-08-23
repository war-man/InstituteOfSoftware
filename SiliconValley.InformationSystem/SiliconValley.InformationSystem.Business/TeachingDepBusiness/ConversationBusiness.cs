using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;



    /// <summary>
    /// 谈话业务类
    /// </summary>
    public class ConversationBusiness:BaseBusiness<ConversationRecord>
    {

        BaseBusiness<StudentInformation> db_student = new BaseBusiness<StudentInformation>();
        BaseBusiness<EmployeesInfo> db_emp = new BaseBusiness<EmployeesInfo>();
      

        public List<ConversationRecord> GetConversationRecords()
        {
            return this.GetList().Where(d=>d.IsDel==false).ToList();

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
        public List<ConversationRecordView> GetScreenConversationRecord(string begindate, string enddate, string studentname)
        {

            var studentlist = db_student.GetList().Where(d => d.Name.Contains(studentname)).ToList();


            List<ConversationRecordView> resultlist = new List<ConversationRecordView>();


            foreach (var item in studentlist)
            {
                var list = GetConversationRecords().Where(d => d.Time <= DateTime.Parse(begindate) && d.Time >= DateTime.Parse(enddate) && d.StudenNumber == item.StudentNumber).ToList();

                foreach (var item1 in list)
                {
                    var tempobj = this.GetConversationRecordView(item1);

                  

                    if(tempobj !=null)

                        resultlist.Add(tempobj);

                }
            }


            return resultlist;

        }


    }
}
