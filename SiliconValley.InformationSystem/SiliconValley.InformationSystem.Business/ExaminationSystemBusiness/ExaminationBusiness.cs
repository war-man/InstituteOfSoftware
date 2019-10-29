using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView;

namespace SiliconValley.InformationSystem.Business.ExaminationSystemBusiness
{
    /// <summary>
    /// 考试信息业务类
    /// </summary>
    public class ExaminationBusiness : BaseBusiness<Examination>
    {

        private readonly BaseBusiness<ExamType> db_examtype;
        private readonly QuestionLevelBusiness db_questionlevel;
        /// <summary>
        /// 班主任带班业务实例
        /// </summary>
        private readonly BaseBusiness<HeadClass> db_headclass;

        private readonly BaseBusiness<CandidateInfo> db_candidateinfo;

        private readonly ExaminationRoomBusiness db_examroom;

        private readonly EmployeesInfoManage db_emp;
        /// <summary>
        /// 排课业务实例
        /// </summary>

        private readonly BaseBusiness<Reconcile> db_reconicle;

        private readonly BaseBusiness<ExamRoomDistributed> db_examroomDistributed;

        public ExaminationBusiness()
        {
            db_examtype = new BaseBusiness<ExamType>();
            db_questionlevel = new QuestionLevelBusiness();
            db_headclass = new BaseBusiness<HeadClass>();
            db_candidateinfo = new BaseBusiness<CandidateInfo>();
            db_examroom = new ExaminationRoomBusiness();
            db_examroomDistributed = new BaseBusiness<ExamRoomDistributed>();
            db_emp = new EmployeesInfoManage();

            db_reconicle = new BaseBusiness<Reconcile>();
        }

        public List<Examination> AllExamination()
        {
           return  this.GetList().ToList();
        }


        /// <summary>
        /// 转换模型视图
        /// </summary>
        /// <returns></returns>
        public ExaminationView ConvertToExaminationView(Examination examination)
        {
            ExaminationView view = new ExaminationView();

            view.ID = examination.ID;
            view.BeginDate = examination.BeginDate;
            view.ExamNo = examination.ExamNo;
            view.ExamType = db_examtype.GetList().Where(d => d.ID == examination.ExamType).FirstOrDefault();
            view.Remark = examination.Remark;
            view.TimeLimit = examination.TimeLimit;
            view.Title = examination.Title;
            view.PaperLevel = db_questionlevel.GetList().Where(d => d.LevelID == examination.PaperLevel).FirstOrDefault();

            return view;
        }

        public List<QuestionLevel> AllQuestionLevel()
        {
           return db_questionlevel.GetList();
        }
        /// <summary>
        /// 生成考试编号
        /// </summary>
        /// <returns></returns>
        public string ProductExamNumber(Examination examination)
        {
            string defaultstr = "guigu";

            
            string year = examination.BeginDate.Year.ToString();


            string mouth = examination.BeginDate.Month.ToString();

            if (mouth.ToString().Length == 1)
            {
                mouth = "0" + mouth;
            }
            string day = examination.BeginDate.Day.ToString();

            if (day.Length == 1)
            {
                day = "0" + day;
            }

            Random rad = new Random();
            int value = rad.Next(1000, 10000);
            string suijishu = value.ToString();

            return defaultstr + year + mouth + day + suijishu;
            
        }


        /// <summary>
        /// 判断考试是否已经结束
        /// </summary>
        /// <returns></returns>

        public bool IsEnd(Examination examination)
        {
            DateTime datetime1 = examination.BeginDate;

            DateTime nowdate = DateTime.Now;

            var d = (float)(nowdate -datetime1).TotalHours;



            if (d>examination.TimeLimit)
            {
                return true;
            }

            return false;

        }


        /// <summary>
        /// 班主任获取自己带班的学生 
        /// </summary>
        /// <returns></returns>
        public List<StudentInformation> GetMyStudentData()
        {
            //获取当前登陆的班主任
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            BaseBusiness<Headmaster> dbheadmaster = new BaseBusiness<Headmaster>();
            ///获取到班主任
           var headmaster = dbheadmaster.GetList().Where(d => d.IsDelete == false).Where(d => d.informatiees_Id == user.EmpNumber).FirstOrDefault();

            //获取班主任的班级
           var classs = db_headclass.GetList().Where(d => d.IsDelete == false && d.LeaderID == headmaster.ID).ToList();

            List<StudentInformation> studentlist = new List<StudentInformation>();
            ScheduleForTraineesBusiness scheduleForTraineesBusiness = new ScheduleForTraineesBusiness();

            foreach (var item in classs)
            {
               var templist = scheduleForTraineesBusiness.ClassStudent(item.ClassID);

                if (templist != null)
                {
                    studentlist.AddRange(templist);
                }
            }

            return studentlist;

        }
        /// <summary>
        /// 获取所有考场信息
        /// </summary>
        /// <returns></returns>
        public List<ExaminationRoom> AllExaminationRoom()
        {
            BaseBusiness<ExaminationRoom> exroom = new BaseBusiness<ExaminationRoom>();
            return exroom.GetList();
        }


        /// <summary>
        /// 根据考试获取考场
        /// </summary>
        /// <returns></returns>
        public List<ExaminationRoom> ExaminationRoomByExamID(int examid)
        {
            List<ExaminationRoom> resultlist = new List<ExaminationRoom>();

            List<ExaminationRoom> allexaminationroom = this.AllExaminationRoom();

            foreach (var item in allexaminationroom)
            {
                if (item.Examination == examid)
                {
                    resultlist.Add(item);
                }
            }

            return resultlist;
        }


        /// <summary>
        /// 转化视图模型
        /// </summary>
        /// <returns></returns>
        public ExaminationRoomView ConvertToExaminationView(ExaminationRoom examinationRoom)
        {
            BaseBusiness<Classroom> db_classroom = new BaseBusiness<Classroom>();
            BaseBusiness<Headmaster> db_headmaster = new BaseBusiness<Headmaster>();

            ExaminationRoomView view = new ExaminationRoomView();

            view.Classroom = db_classroom.GetList().Where(d => d.IsDelete == false && d.Id == examinationRoom.Classroom_Id).FirstOrDefault();
            view.Examination = this.AllExamination().Where(d => d.ID == examinationRoom.Examination).FirstOrDefault();
            view.ID = examinationRoom.ID;

            if (examinationRoom.Invigilator1 != null)
            {
                string empid1 = db_headmaster.GetList().Where(d => d.IsDelete == false && d.ID == examinationRoom.Invigilator1).FirstOrDefault().informatiees_Id;
                view.Invigilator1 = db_emp.GetInfoByEmpID(empid1);
            }
            else
            {
                view.Invigilator1 = null;
            }


            if (examinationRoom.Invigilator2 != null)
            {
                string empid2 = db_headmaster.GetList().Where(d => d.IsDelete == false && d.ID == examinationRoom.Invigilator2).FirstOrDefault().informatiees_Id;
                view.Invigilator2 = db_emp.GetInfoByEmpID(empid2);
            }
            else
            {
                view.Invigilator2 = null;
            }


            view.Remark = examinationRoom.Remark;

            return view;
        }

        /// <summary>
        /// 获取未使用的教室
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public List<Classroom> NotUsedClassroom(DateTime dateTime)
        {
            return null;
        }

        /// <summary>
        /// 获取还未开始的考试
        /// </summary>
        /// <returns></returns>
        public List<Examination> NoEndExamination()
        {
           var list =  this.AllExamination();

            List<Examination> resultlist = new List<Examination>();

            foreach (var item in list)
            {
                if (!IsEnd(item))
                {
                    resultlist.Add(item);
                }
            }

            return resultlist;

        }

        /// <summary>
        /// 考生信息
        /// </summary>
        /// <returns></returns>
        public List<CandidateInfo> AllCandidateInfo(int examid)
        {
          return db_candidateinfo.GetList().Where(d => d.Examination == examid).ToList();
        }



        /// <summary>
        /// 学生报名考试
        /// </summary>
        /// <param name="studentlist"></param>
        /// <param name="examid"></param>

        public void subscribeExam(Dictionary<string,bool> studentlist, int examid)
        {
          

            Examination examination = this.AllExamination().Where(d=>d.ID==examid).FirstOrDefault();

            foreach (var item in studentlist)
            {

               var tempstu = AllCandidateInfo(examid).Where(d => d.StudentID == item.Key).FirstOrDefault();

                if (tempstu == null)
                {
                    CandidateInfo candidateInfo = new CandidateInfo();
                    candidateInfo.CandidateNumber = examination.ExamNo.Substring(0, 5) + item.Key;
                    candidateInfo.ComputerPaper = null;
                    candidateInfo.Examination = examid;
                    candidateInfo.IsReExam = item.Value;
                    candidateInfo.Paper = null;
                    candidateInfo.StudentID = item.Key;

                    db_candidateinfo.Insert(candidateInfo);
                }

            }

          

           


        }


        /// <summary>
        /// 取消报名
        /// </summary>
        /// <param name="students"></param>
        /// <param name="examid"></param>
        public void cancelsubscribeExam(List<string> students, int examid)
        {

            foreach (var item in students)
            {
               var tempexamobj = this.AllCandidateInfo(examid).Where(d => d.StudentID == item).FirstOrDefault();

                if (tempexamobj != null)
                {
                    db_candidateinfo.Delete(tempexamobj);
                }

            }

        }


        /// <summary>
        /// 安排监考员
        /// </summary>
        /// <param name="examid">考试号</param>
        /// <param name="examroomid">考场id</param>
        /// <param name="proctorList">监考员</param>
        public void ArrangeTheInvigilator(int examid, int examroomid, List<string> proctorList)
        {
           var exammroom =  this.ExaminationRoomByExamID(examid).Where(d=>d.Classroom_Id == examroomid).FirstOrDefault();
            BaseBusiness<Headmaster> dbheadmaster = new BaseBusiness<Headmaster>();

            //首先先删除
            exammroom.Invigilator1 = null;
            exammroom.Invigilator2 = null;

            db_examroom.Update(exammroom);
            if (proctorList.Count == 0)
            {
                return;
            }

           

            var headmaster =  dbheadmaster.GetList().Where(d => d.IsDelete == false && d.informatiees_Id == proctorList[0]).FirstOrDefault();

            exammroom.Invigilator1 = headmaster.ID;

            var headmaster1 = dbheadmaster.GetList().Where(d => d.IsDelete == false && d.informatiees_Id == proctorList[1]).FirstOrDefault();

            exammroom.Invigilator2 = headmaster1.ID;

            db_examroom.Update(exammroom);




        }

        public List<EmployeesInfo> GetProtorData(int examid, int examroomid)
        {
           var examroom = this.ExaminationRoomByExamID(examid).Where(d => d.Classroom_Id == examroomid).FirstOrDefault();

            List<EmployeesInfo> list = new List<EmployeesInfo>();

            BaseBusiness<Headmaster> dbheadmaster = new BaseBusiness<Headmaster>();

            if (examroom.Invigilator1 != null)
            {
                var headmaster1 = dbheadmaster.GetList().Where(d => d.IsDelete == false && d.ID == examroom.Invigilator1).FirstOrDefault();

                var emp1 = db_emp.GetInfoByEmpID(headmaster1.informatiees_Id);

                if (emp1 != null)
                {
                    list.Add(emp1);
                }
            }

            if (examroom.Invigilator2 != null)
            {
                var headmaster2 = dbheadmaster.GetList().Where(d => d.IsDelete == false && d.ID == examroom.Invigilator2).FirstOrDefault();

                var emp2 = db_emp.GetInfoByEmpID(headmaster2.informatiees_Id);

                if (emp2 != null)
                {
                    list.Add(emp2);
                }
            }
          
         

            return list;
        }

        public List<HeadMasterSortView> GetHeadmastersByGrand(int grandid)
        {
            BaseBusiness<HeadMasterGrand> db_headmastergrand = new BaseBusiness<HeadMasterGrand>();

            //获取所有考试
           var examlist = this.AllExamination();

            //获取未开始的考试
            List<Examination> noendexamlist = new List<Examination>();
            foreach (var item in examlist)
            {
                if (!this.IsEnd(item))
                {
                    noendexamlist.Add(item);
                }
            }

            List<int> headmasterlist = new List<int>();

            foreach (var item in noendexamlist)
            {
               var templist = this.ExaminationRoomByExamID(item.ID).ToList();

                foreach (var item1 in templist)
                {
                    if (item1.Invigilator1 != null)
                    {
                        headmasterlist.Add((int)item1.Invigilator1);
                    }

                    if (item1.Invigilator2 != null)
                    {
                        headmasterlist.Add((int)item1.Invigilator2);
                    }
                }
            }


            var list1 = db_headmastergrand.GetList().Where(d => d.GrandID == grandid).ToList();

            //排除 


            var list = new List<HeadMasterGrand>();

            foreach (var item in list1)
            {
                if (!IsCoaint(headmasterlist, item))
                {
                    list.Add(item);
                }
            }



            EmployeesInfoManage empdb = new EmployeesInfoManage();
            BaseBusiness<Headmaster> db_headmaster = new BaseBusiness<Headmaster>();
            //转型
            List<HeadMasterSortView> resultlist = new List<HeadMasterSortView>();
            foreach (var item in list)
            {
                HeadMasterSortView headMasterSortView = new HeadMasterSortView();
                var headmaster = db_headmaster.GetList().Where(d => d.IsDelete == false && d.ID == item.HeadMasterID).FirstOrDefault();
                headMasterSortView.emp = empdb.GetInfoByEmpID(headmaster.informatiees_Id);
                headMasterSortView.ID = item.ID;
                resultlist.Add(headMasterSortView);
            }

            return resultlist;

        }

        public bool IsCoaint(List<int> sources, HeadMasterGrand headMasterGrand)
        {
            foreach (var item in sources)
            {
                if (item == headMasterGrand.HeadMasterID)
                {
                    return true;
                }
            }

            return false;


        }


        /// <summary>
        /// 考场人员分布
        /// </summary>
        /// <returns></returns>
        public List<ExamRoomDistributed> AllExamroomDistributed(int examid)
        {
           List<ExamRoomDistributed> list = db_examroomDistributed.GetList().Where(d=>d.ExamID == examid).ToList();

            return list;
        }


    }
}
