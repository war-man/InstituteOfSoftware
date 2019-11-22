using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
    public class ReconcileManeger : BaseBusiness<Reconcile>
    {
        //教室业务类
        public static readonly ClassroomManeger Classroom_Entity = new ClassroomManeger();
        //课程业务类
        public static readonly CourseBusiness Curriculum_Entity = new CourseBusiness();
        //课程类型业务类
        public static readonly CourseTypeBusiness CourseType_Entity = new CourseTypeBusiness();
        //班级业务类
        public static readonly ClassScheduleBusiness ClassSchedule_Entity = new ClassScheduleBusiness();
        //阶段业务类
        public static readonly GrandBusiness Grand_Entity = new GrandBusiness();
        //教学老师业务类
        public static readonly TeacherClassBusiness Teacher_Entity = new TeacherClassBusiness();

        //专业业务类
        public static readonly SpecialtyBusiness Specialty_Entity = new SpecialtyBusiness();

        //老师擅长课程业务类
        public static readonly GoodSkillManeger GoodSkill_Entity = new GoodSkillManeger();
       

        /// <summary>
        /// 向Xml文件读取配置
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="XmlFile_url">xml的url</param>
        /// <returns></returns>
        public GetYear MyGetYear(string year, string XmlFile_url)
        {
            GetYear g = new GetYear();
            XElement xx = XElement.Load(XmlFile_url);
            XElement s = xx.Elements("Year").Where(e => e.Attribute("name").Value == year.ToString()).First();//筛选
            g.YearName = year;
            g.StartmonthName = Convert.ToInt32(s.Element("startmonth").Value);
            g.EndmonthName = Convert.ToInt32(s.Element("endmonth").Value);
            return g;

        }
        
        /// <summary>
        /// 判断该日期是否是周六或周末（1--周六，2--周日）
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public int IsSaturday(DateTime date)
        {
            var day = date.DayOfWeek;
            //判断是否为周末

            if (day == DayOfWeek.Sunday)
            {
                return 2;//周日
            }
            else if (day == DayOfWeek.Saturday)
            {
                return 1;//周六
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取阶段集合
        /// </summary>
        /// <returns></returns>
        public List<Grand> GetEffectiveData(bool s)
        {
            List<Grand> g_list = new List<Grand>();
            List<Grand> grands= Grand_Entity.GetList().Where(g=>g.IsDelete==false).ToList();
            if (s)
            {
                //s1s2阶段
                foreach (Grand item in grands)
                {
                    if (item.GrandName=="S1" || item.GrandName=="S2" ||item.GrandName=="Y1")
                    {
                        g_list.Add(item);
                    }
                }
            }
            else
            {
                //s3.s4阶段
                foreach (Grand item in grands)
                {
                    if (item.GrandName != "S1")
                    {
                        if (item.GrandName!="S2")
                        {
                            if (item.GrandName!="Y1")
                            {
                                g_list.Add(item);
                            }
                             
                        }
                       
                    }
                }
            }

            return g_list;
        }
        /// <summary>
        /// 获取属于某个阶段的有效班级集合
        /// </summary>
        /// <param name="grand_id"></param>
        /// <returns></returns>
        public List<ClassSchedule> GetGrandClass(int grand_id)
        {
            //获取有效的班级数据//获取属于某个阶段的班级
            List<ClassSchedule> c_list = ClassSchedule_Entity.GetList().Where(c => c.ClassStatus == false && c.IsDelete == false && c.grade_Id == grand_id && c.ClassstatusID==null).ToList();
            return c_list;
        }
        /// <summary>
        /// 获取班级上课时间
        /// </summary>
        /// <param name="FartherName">父级名称</param>
        /// <returns></returns>
        public List<BaseDataEnum> GetTimeCheckBox(string FartherName)
        {
            return ClassSchedule_Entity.BaseDataEnum_Entity.GetsameFartherData(FartherName);
        }

        /// <summary>
        /// 获取某个校区的有效教室
        /// </summary>
        /// <param name="basedae_id">校区编号</param>
        /// <returns></returns>
        public List<Classroom> GetEffectioveClassRoom(int basedae_id)
        {
            return Classroom_Entity.GetList().Where(c => c.IsDelete == false && c.BaseData_Id == basedae_id).ToList();
        }
        /// <summary>
        /// 根据名称获取上课时间段
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BaseDataEnum GetClassTime(string name)
        {
            List<BaseDataEnum> find_list_base = GetTimeCheckBox("上课时间类型");
            return find_list_base.Where(b => b.Name == name).FirstOrDefault();
        }
        /// <summary>
        /// 是否是最后一门课程
        /// </summary>
        /// <param name="name">课程名称</param>
        /// <returns></returns>
        public bool IsEndCurr(string name)
        {
            bool s = false;
            Curriculum fin_c = Curriculum_Entity.GetList().Where(c => c.CourseName == name).FirstOrDefault();
            if (fin_c != null)
            {
                Curriculum find_two_c = Curriculum_Entity.GetList().Where(c => c.Grand_Id == fin_c.Grand_Id).OrderBy(c => c.Sort).LastOrDefault();
                if (find_two_c.CurriculumID == fin_c.CurriculumID)
                {
                    s = true;
                }
            }
            return s;
        }

        public bool Inser_list(List<Reconcile> new_list)
        {
            bool s = false;
            try
            {
                foreach (Reconcile item in new_list)
                {                    
                        this.Insert(item);
                        s = true;                    
                }
            }
            catch (Exception ex)
            {

                s = false;
            }
            return s;
        }
       /// <summary>
       /// 是否有重复的数据 (false--没有重复，true--重复)
       /// </summary>
       /// <param name="r"></param>
       /// <returns></returns>
        public bool IsExcit(Reconcile r)
        {
            bool s = false;
            Reconcile find_r= this.GetList().Where(rs => rs.AnPaiDate == r.AnPaiDate && rs.ClassSchedule_Id == r.ClassSchedule_Id && rs.Curse_Id == r.Curse_Id).FirstOrDefault();
            if (find_r!=null)
            {
                s = true;
            }             
            return s;
        }
        
        /// <summary>
        /// 获取一周日期
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Mydate GetMydate(DateTime d)
        {
            Mydate mydate = new Mydate();
            DayOfWeek week = d.DayOfWeek;
            switch (week)
            {
                case DayOfWeek.Monday://星期一
                    mydate.StarTime = d;
                    mydate.EndTime = d.AddDays(5);
                    break;
                case DayOfWeek.Tuesday://星期二
                    mydate.StarTime = d.AddDays(-1);
                    mydate.EndTime = d.AddDays(4);
                    break;
                case DayOfWeek.Wednesday://星期三
                    mydate.StarTime = d.AddDays(-2);
                    mydate.EndTime = d.AddDays(3);
                    break;
                case DayOfWeek.Thursday://星期四
                    mydate.StarTime = d.AddDays(-3);
                    mydate.EndTime = d.AddDays(2);
                    break;
                case DayOfWeek.Friday://星期五
                    mydate.StarTime = d.AddDays(-4);
                    mydate.EndTime = d.AddDays(2);
                    break;
                case DayOfWeek.Saturday://星期六
                    mydate.StarTime = d.AddDays(-5);
                    mydate.EndTime = d;
                    break;
            }
            return mydate;
        }
        public Curriculum GetCurriculum(string Name)
        {
            return Curriculum_Entity.GetList().Where(w => w.CourseName == Name).FirstOrDefault();
        }
        /// <summary>
        /// 查看这个班级在这周有没有安排英语或班会课（true-已安排,false-未安排）
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="ClassName">班级</param>
        /// <param name="CurrName">英语课或班会课</param>
        /// <returns></returns>
        public bool Existence(Mydate time, int ClassName, string CurrName)
        {
            bool s = false;
            int count = this.GetList().Where(r => r.AnPaiDate >= time.StarTime && r.AnPaiDate <= time.EndTime && r.ClassSchedule_Id == ClassName && r.Curriculum_Id==CurrName).ToList().Count;
            if (count > 0)
            {
                s = true;
            }
            return s;
        }
      
        /// <summary>
        /// 查看这个教室是否已满
        /// </summary>
        /// <param name="c_id">教室编号</param>
        /// <param name="d">周期</param>
        /// <param name="time">上午1234，下午1234</param>
        /// <returns></returns>
        public string RoomTime(int c_id,DateTime d,int? base_id)
        {
           BaseDataEnum find_b= ClassSchedule_Entity.BaseDataEnum_Entity.GetEntity(base_id);
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            List<string> str = new List<string>();
            if (find_b.Name== "上午")
            {
                str.Add("下午12节");
                str.Add("下午34节");
            }
            else
            {
                str.Add("上午12节");
                str.Add("上午34节");
            }
            List<Reconcile> reconciles= this.GetList().Where(r => r.AnPaiDate == d && r.ClassRoom_Id == c_id).ToList();
            for (int i = 0; i < reconciles.Count; i++)
            {
                for (int j = 0; j < str.Count; j++)
                {
                    if (reconciles[i].Curse_Id==str[j])
                    {
                        str.Remove(str[j]);
                    }
                }
            }
            if (str.Count>0)
            {
                Random rs = new Random();
                int num = rs.Next(0,str.Count);
                sb.Append(str[num]);                    
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取空教室
        /// </summary>
        /// <param name="TimeName">'上午'--获取下午的空教室'下午'--获取上午的空教室</param>
        /// <param name="base_id">那个校区的</param>
        /// <param name="time">获取哪个日期里的空教室</param>
        /// <returns></returns>
        public List<Classroom> GetClassrooms(string TimeName,int base_id,DateTime time)
        {
            List<Classroom> c_list= Classroom_Entity.GetList().Where(c=>c.BaseData_Id==base_id).ToList();
            List<Reconcile> reconciles = this.GetList().Where(r=>r.Curse_Id==TimeName && r.AnPaiDate==time).ToList();
            for (int i = 0; i < reconciles.Count; i++)
            {
                for (int j = 0; j < c_list.Count; j++)
                {
                    if (reconciles[i].ClassRoom_Id==c_list[j].Id)
                    {
                        c_list.Remove(c_list[j]);
                    }
                }
            }
            return c_list;
        }
        /// <summary>
        /// 安排这个班级是什么阶段在上什么课程
        /// </summary>
        /// <param name="time">获取课表日期</param>
        /// <param name="basedae_id">校区名称</param>
        public void mmm(DateTime time, int basedae_id,int grand_id)
        {          
            //获取这周日期
            Mydate mydate = GetMydate(time);
            //获取上午班级跟下午班级
           // List<ClassSchedule> moringclass = ClassSchedule_Entity.GetList().Where(c => c.BaseDataEnum_Id == GetClassTime("上午").Id).ToList();
            List<ClassSchedule> afternoonclass = ClassSchedule_Entity.GetList().Where(c => c.BaseDataEnum_Id == GetClassTime("上午").Id && c.ClassStatus==false && c.IsDelete==false && c.grade_Id==grand_id).ToList();
            //获取教室 
            List<Classroom> moringroom = GetClassrooms("下午", basedae_id,time);
 
            //下午的班级上午的班会，英语
            foreach (Classroom room in moringroom)
            {
                foreach (ClassSchedule item in afternoonclass)
                {
                    //判断这个班级是否可以排英语，军事，职素课
                    bool s1 = Existence(mydate, item.id, "英语");//判断这周这个班安排英语课
                    if (s1 == false)
                    {
                        //看看这个教室排满了没有
                        string str = RoomTime(room.Id, time,item.BaseDataEnum_Id);
                        if (!string.IsNullOrEmpty(str))
                        {
                            Reconcile r = new Reconcile();
                            r.IsDelete = false;
                            r.AnPaiDate = time;
                            r.ClassRoom_Id = room.Id;
                            r.Curse_Id = str;
                            r.ClassSchedule_Id = item.id;
                            r.NewDate = DateTime.Now;
                            r.Curriculum_Id = "英语";
                            this.Insert(r);
                            break;
                        }                       
                    }
                     
                }
            }
        }
        
        /// <summary>
        /// 获取非专业老师
        /// </summary>
        /// <param name="PosionName">岗位名称</param>
        /// <returns></returns>
        public List<EmployeesInfo> GetEmployees(string PosionName)
        {
            BaseBusiness<EmployeesInfo> baseEmplo = new BaseBusiness<EmployeesInfo>();
            BaseBusiness<Position> basePosition = new BaseBusiness<Position>();            
            Position p = basePosition.GetList().Where(b=>b.IsDel==false && b.PositionName==PosionName).FirstOrDefault();
            if (p!=null)
            {
               return baseEmplo.GetList().Where(b => b.IsDel == false && b.PositionId==p.Pid).ToList();
            }
            else
            {
                return new List<EmployeesInfo>();
            }
        }
        //获取可以上职素的班主任
        public List<EmployeesInfo> GetMasTeacher()
        {
            List<EmployeesInfo> list = new List<EmployeesInfo>();
            BaseBusiness<Headmaster> basemaster = new BaseBusiness<Headmaster>();
            BaseBusiness<EmployeesInfo> baseEmplo = new BaseBusiness<EmployeesInfo>();
            List<EmployeesInfo> elist = baseEmplo.GetList();
            List<Headmaster> find_m= basemaster.GetList().Where(m => m.IsDelete == false && m.IsAttend == true).ToList();
            foreach (EmployeesInfo e1 in elist)
            {
                foreach (Headmaster m1 in find_m)
                {
                    if (m1.informatiees_Id==e1.EmployeeId)
                    {
                        list.Add(e1);
                    }
                }
            }
            return list;
        }
        //获取教官
        public List<EmployeesInfo> GetSir()
        {
            BaseBusiness<Position> position = new BaseBusiness<Position>();
            List<Position> p_list=  position.GetList().Where(p=>p.PositionName=="教官").ToList();
            List<EmployeesInfo> employees = new BaseBusiness<EmployeesInfo>().GetList();
            List<EmployeesInfo> em = new List<EmployeesInfo>();
            foreach (Position item1 in p_list)
            {
                foreach (EmployeesInfo item2 in employees)
                {
                    if (item2.PositionId==item1.Pid)
                    {
                        em.Add(item2);
                    }
                }
            }
            return em;
        }
        /// <summary>
        /// 获取排课数据(显示课表形式)
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="timename">上课时间段</param>
        /// <param name="classrooms">教室集合</param>
        /// <returns></returns>
        public List<AnPaiData> GetPaiDatas(DateTime time,string timename,List<Classroom> classrooms)
        {
            List<AnPaiData> a_list = new List<AnPaiData>();
            List<Reconcile> r_list= this.GetList().Where(r=>r.AnPaiDate==time).ToList();
            BaseBusiness<EmployeesInfo> entity = new BaseBusiness<EmployeesInfo>();
            if (timename=="上午12节"||timename=="上午34节")
            {
                foreach (Classroom c1 in classrooms)
                {
                    AnPaiData new_a = new AnPaiData();
                    foreach (Reconcile r1 in r_list)
                    {
                        if (r1.ClassRoom_Id == c1.Id && (r1.Curse_Id == timename || r1.Curse_Id=="上午"))
                        {
                            new_a.class_Id = r1.Id;
                            new_a.ClassName =ClassSchedule_Entity.GetEntity( r1.Id).ClassNumber;
                            new_a.Teacher = r1.EmployeesInfo_Id==null?"无":entity.GetEntity(r1.EmployeesInfo_Id).EmpName;
                            new_a.NeiRong = r1.Curriculum_Id;
                        }
                    }
                    a_list.Add(new_a);
                }
            }
            else if(timename == "下午12节" || timename == "下午34节")
            {
                foreach (Classroom c1 in classrooms)
                {
                    AnPaiData new_a = new AnPaiData();
                    foreach (Reconcile r1 in r_list)
                    {
                        if (r1.ClassRoom_Id == c1.Id && (r1.Curse_Id == timename || r1.Curse_Id == "下午"))
                        {
                            new_a.class_Id = r1.Id;
                            new_a.ClassName = ClassSchedule_Entity.GetEntity(r1.Id).ClassNumber;
                            new_a.Teacher = r1.EmployeesInfo_Id == null ? "无" : entity.GetEntity(r1.EmployeesInfo_Id).EmpName;
                            new_a.NeiRong = r1.Curriculum_Id;
                        }
                    }
                    a_list.Add(new_a);
                }
            }
            return a_list;
        }
         /// <summary>
         /// 判断这个老师在这个日期中的这个时间段是否有课(false--没有，true--有)
         /// </summary>
         /// <param name="Teacher_Id">老师编号</param>
         /// <param name="timename">时间段</param>
         /// <param name="date">日期</param>
         /// <returns></returns>
        public bool IsHaveClass(string Teacher_Id,string timename,DateTime date)
        {
            bool s = false;
            List<Reconcile> find_r= this.GetList().Where(r => r.EmployeesInfo_Id == Teacher_Id && r.AnPaiDate == date && r.Curse_Id==timename).ToList();
            if (find_r.Count>0)
            {
                s = true;
            }
            return s;
        }
        #region 提供修改排课数据的方法
        /// <summary>
        /// 获取XX班级在这XX天上XX课程的排课情况
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="class_id">班级编号</param>
        /// <param name="Time">上午或下午</param>
        /// <returns></returns>
        public List<Reconcile> GetReconcile(DateTime time, int class_id, string currName)
        {
           return this.GetList().Where(r => r.AnPaiDate == time && r.ClassSchedule_Id == class_id && r.Curse_Id == currName).ToList();
        }
        
        /// <summary>
        /// 转视图模型
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public ResconcileView ConvertToView(Reconcile r)
        {
            BaseBusiness<EmployeesInfo> entity = new BaseBusiness<EmployeesInfo>();
            ResconcileView new_r = new ResconcileView();
            new_r.AnPaiDate = r.AnPaiDate;
            new_r.ClassRoom_Id = Classroom_Entity.GetEntity(r.ClassRoom_Id);
            new_r.ClassSchedule_Id = ClassSchedule_Entity.GetEntity(r.Id);
            new_r.Curriculum_Id = r.Curriculum_Id;
            new_r.Curse_Id = r.Curse_Id;
            new_r.EmployeesInfo_Id = entity.GetEntity(r.EmployeesInfo_Id);
            new_r.Id = r.Id;
            new_r.IsDelete = r.IsDelete;
            new_r.NewDate = r.NewDate;
            new_r.Rmark = r.Rmark;
            return new_r;
        }
        /// <summary>
        /// 代课业务(fasle--操作不成功，ture--操作成功)
        /// </summary>
        /// <param name="dateTime">代课日期</param>
        /// <param name="emp_id">代课老师</param>
        /// <param name="timename">代课时间段（上午，下午）</param>
        /// <param name="class_id">代课班级Id</param>
        /// <returns></returns>
        public bool Daike(DateTime dateTime,string emp_id,string timename,int class_id)
        {
            bool s = false;
            try
            {
                Reconcile find_r = this.GetList().Where(r => r.AnPaiDate == dateTime && r.Curse_Id == timename && r.ClassSchedule_Id == class_id).FirstOrDefault();
                find_r.EmployeesInfo_Id = emp_id;
                this.Update(find_r);
                s = true;
            }
            catch (Exception)
            {

                s = false;
            }
            return s;
        }       
        #endregion
    }
}
