using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
using SiliconValley.InformationSystem.Business.DepartmentBusiness;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
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

        /// <summary>
        /// 从缓存中获取排课所有数据
        /// </summary>
        /// <returns></returns>
        public List<Reconcile> AllReconcile()
        {
            List<Reconcile> get_reconciles_list = new List<Reconcile>();
            get_reconciles_list = Reconcile_Com.redisCache.GetCache<List<Reconcile>>("ReconcileList");
            if (get_reconciles_list == null || get_reconciles_list.Count == 0)
            {
                get_reconciles_list = this.GetList();
                Reconcile_Com.redisCache.SetCache("ReconcileList", get_reconciles_list);
            }
            return get_reconciles_list;
        }
        /// <summary>
        /// 单条数据添加（false--失败，true--成功）
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public bool AddData(Reconcile r)
        {
            bool s = false;
            try
            {
                this.Insert(r);
                s = true;
                Reconcile_Com.redisCache.RemoveCache("ReconcileList");
            }
            catch (Exception)
            {
                s = false;                
            }
            return s;
        }
        /// <summary>
        /// 修改数据 （false--失败,true--成功）
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public bool Update(Reconcile r)
        {
            bool s = false;
            Reconcile find_r= this.GetEntity(r.Id);
            return s;
        }
        //获取可以上职素的班主任
        public List<EmployeesInfo> GetMasTeacher()
        {
            List<EmployeesInfo> list = new List<EmployeesInfo>();
            BaseBusiness<Headmaster> basemaster = new BaseBusiness<Headmaster>();
            BaseBusiness<EmployeesInfo> baseEmplo = new BaseBusiness<EmployeesInfo>();
            List<EmployeesInfo> elist = baseEmplo.GetList();
            List<Headmaster> find_m = basemaster.GetList().Where(m => m.IsDelete == false && m.IsAttend == true).ToList();
            foreach (EmployeesInfo e1 in elist)
            {
                foreach (Headmaster m1 in find_m)
                {
                    if (m1.informatiees_Id == e1.EmployeeId)
                    {
                        list.Add(e1);
                    }
                }
            }
            return list;
        }        
        //获取教官
        public List<EmployeesInfo> GetSir(bool Is)
        {
            DepartmentManage department = new DepartmentManage();
            BaseBusiness<Position> position = new BaseBusiness<Position>();
            List<EmployeesInfo> employees = new BaseBusiness<EmployeesInfo>().GetList().Where(e => e.IsDel == false).ToList();
            List<EmployeesInfo> em = new List<EmployeesInfo>();
            if (Is)
            {
                //S1，S2教官
                Department find_d1 = department.GetList().Where(d => d.DeptName == "s1、s2教质部").FirstOrDefault();
                Position p1 = position.GetList().Where(p => p.PositionName == "教官" && p.DeptId == find_d1.DeptId).FirstOrDefault();
                em = employees.Where(c => c.PositionId == p1.Pid).ToList();
            }
            else
            {
                //S3,S4教官
                Department find_d2 = department.GetList().Where(d => d.DeptName == "s3教质部").FirstOrDefault();
                Position p1 = position.GetList().Where(p => p.PositionName == "教官" && p.DeptId == find_d2.DeptId).FirstOrDefault();
                em = employees.Where(c => c.PositionId == p1.Pid).ToList();
            }
            return em;
        }
        /// <summary>
        /// 判断这个老师在这个日期中的这个时间段是否有课(false--没有，true--有)
        /// </summary>
        /// <param name="Teacher_Id">老师编号</param>
        /// <param name="timename">时间段</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public bool IsHaveClass(string Teacher_Id, string timename, DateTime date)
        {
            bool s = false;
            List<Reconcile> find_r = AllReconcile().Where(r => r.EmployeesInfo_Id == Teacher_Id && r.AnPaiDate == date && r.Curse_Id == timename).ToList();
            if (find_r.Count > 0)
            {
                s = true;
            }
            return s;
        }
        /// <summary>
        /// 随机获取班主任或教官
        /// </summary>
        /// <param name="teacher">false--得到的教官,true--班主任</param>
        /// <param name="s1ors2">哪个部门的教官</param>
        /// <returns></returns>
        public EmployeesInfo GetRandMASteacher(bool teacher,bool s1ors2)
        {
            Random r = new Random();
            List<EmployeesInfo> list1 = GetMasTeacher();
            List<EmployeesInfo> list2 = GetSir(s1ors2);
                       
            if (teacher)
            {
                //获取职素老师
                int number = r.Next(0, list1.Count);
                return list1[number];
            }
            else
            {
                //获取军事教官
                int number = r.Next(0, list2.Count);
                return list2[number];
            }
             
        }

        /// <summary>
        /// 获取可以上哪个阶段课程的老师
        /// </summary>
        /// <param name="grand_id"></param>
        /// <returns></returns>
        public List<Teacher> GetListTeacher(int grand_id, string curr_name)
        {
            List<Teacher> list_Teacher = new List<Teacher>();
            Curriculum find_c = Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.IsDelete == false && c.CourseName.Equals(curr_name, StringComparison.CurrentCultureIgnoreCase) && c.Grand_Id == grand_id).FirstOrDefault();//获取课程
            if (find_c != null)
            {
                List<GoodSkill> all_goodskill = Reconcile_Com.GoodSkill_Entity.GetList().Where(g => g.Curriculum == find_c.CurriculumID).ToList();//获取可以上这个课程的老师
                foreach (GoodSkill goods in all_goodskill)
                {
                    if (list_Teacher.Count > 0)
                    {
                        for (int i = 0; i < list_Teacher.Count; i++)
                        {
                            if (list_Teacher[i].TeacherID == goods.TearchID)
                            {
                                list_Teacher.Remove(list_Teacher[i]);
                                list_Teacher.Add(list_Teacher[i]);
                            }
                            else
                            {
                                list_Teacher.Add(list_Teacher[i]);
                            }
                        }
                    }
                    else
                    {
                        list_Teacher.Add(Reconcile_Com.Teacher_Entity.GetEntity(goods.TearchID));
                    }
                }
            }

            return list_Teacher;
        }
        /// <summary>
        /// 安排英语老师上课
        /// </summary>
        /// <param name="grand_id">阶段Id</param>
        /// <param name="curr_name">课程名称</param>
        /// <param name="timename">时间段</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public string AnpaiTeacher(int grand_id, string curr_name,string timename,DateTime date)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            List<Teacher> gettea = GetListTeacher(grand_id, curr_name);//获取英语老师
            foreach (Teacher tt in gettea)
            {
               bool s= IsHaveClass(tt.EmployeeId, timename, date);
                if (s==false)
                {
                    sb.Append(tt.EmployeeId);
                    break;
                }
            }
            return sb.ToString();
        }
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
            List<Grand> grands= Reconcile_Com.Grand_Entity.GetList().Where(g=>g.IsDelete==false).ToList();
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
            List<ClassSchedule> c_list = Reconcile_Com.ClassSchedule_Entity.GetList().Where(c => c.ClassStatus == false && c.IsDelete == false && c.grade_Id == grand_id && c.ClassstatusID==null).ToList();
            return c_list;
        }
        /// <summary>
        /// 获取班级上课时间
        /// </summary>
        /// <param name="FartherName">父级名称</param>
        /// <returns></returns>
        public List<BaseDataEnum> GetTimeCheckBox(string FartherName)
        {
            return Reconcile_Com.ClassSchedule_Entity.BaseDataEnum_Entity.GetsameFartherData(FartherName);
        }

        /// <summary>
        /// 获取某个校区的有效教室
        /// </summary>
        /// <param name="basedae_id">校区编号</param>
        /// <returns></returns>
        public List<Classroom> GetEffectioveClassRoom(int basedae_id)
        {
            return Reconcile_Com.Classroom_Entity.GetList().Where(c => c.IsDelete == false && c.BaseData_Id == basedae_id).ToList();
        }
        /// <summary>
        /// 获取班主任员工编号（获取可以上班会课的老师编号）
        /// </summary>
        /// <param name="class_id"></param>
        /// <returns></returns>
        public string GetMasterTeacher(int class_id,DateTime time,string timename)
        {
            //获取该班级的班主任
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            AjaxResult find_a = Reconcile_Com.GetZhisuTeacher(class_id);          
            int count = 0;
            if (find_a.Success==true)
            {
                AjaxResult find_class = Reconcile_Com.GetHadMasterClass(find_a.Data.ToString());
                if (find_class.Success==true)
                {
                    List<HeadClass> headClasses = find_class.Data as List<HeadClass>;
                    foreach (HeadClass c in headClasses)
                    {
                        //判断是否有带的班级已在这时间段安排了班会课
                        count= count+ AllReconcile().Where(r => r.AnPaiDate == time && r.Curse_Id == timename && r.EmployeesInfo_Id == find_a.Data.ToString() && r.ClassSchedule_Id==c.ClassID).ToList().Count;
                    }
                }
                if (count <= 0)
                {
                    sb.Append(find_a.Data.ToString());
                }
            }            
            return sb.ToString();
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
            Curriculum fin_c = Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.CourseName == name).FirstOrDefault();
            if (fin_c != null)
            {
                Curriculum find_two_c = Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.Grand_Id == fin_c.Grand_Id).OrderBy(c => c.Sort).LastOrDefault();
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
                Reconcile_Com.redisCache.RemoveCache("ReconcileList");
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
            Reconcile find_r= AllReconcile().Where(rs => rs.AnPaiDate == r.AnPaiDate && rs.ClassSchedule_Id == r.ClassSchedule_Id && rs.Curse_Id == r.Curse_Id).FirstOrDefault();
            if (find_r!=null)
            {
                s = true;
            }             
            return s;
        }
        
        /// <summary>
        /// 获取一周或两周的日期
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Mydate GetMydate(DateTime d,int type)
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
            if (type==2)
            {
                //输出两周的日期
                mydate.StarTime = mydate.StarTime.AddDays(-7);               
            }
            return mydate;
        }
        public Curriculum GetCurriculum(string Name)
        {
            return Reconcile_Com.Curriculum_Entity.GetList().Where(w => w.CourseName == Name).FirstOrDefault();
        }
        /// <summary>
        /// 查看这个班级在这周有没有安排XXX课（true-已安排,false-未安排）
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="ClassName">班级</param>
        /// <param name="CurrName">英语课或班会课</param>
        /// <returns></returns>
        public bool Existence(Mydate time, int ClassName, string CurrName)
        {
            bool s = false;
            int count = AllReconcile().Where(r => r.AnPaiDate >= time.StarTime && r.AnPaiDate <= time.EndTime && r.ClassSchedule_Id == ClassName && r.Curriculum_Id==CurrName).ToList().Count;
            if (count > 0)
            {
                s = true;
            }
            return s;
        }
        /// <summary>
        /// 查看xx班级是否安排了XX课程(false=没有安排，true=已安排)
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="class_id">班级</param>
        /// <param name="currname">课程名称</param>
        /// <returns></returns>
        public bool ExistenceToday(DateTime? time,int class_id,string currname)
        {
            bool s = false;
           int count= AllReconcile().Where(r => r.ClassSchedule_Id == class_id && r.AnPaiDate == time && r.Curriculum_Id== currname).ToList().Count;
            if (count>0)
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
            BaseDataEnum find_b= Reconcile_Com.ClassSchedule_Entity.BaseDataEnum_Entity.GetEntity(base_id);
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
            List<Reconcile> reconciles= AllReconcile().Where(r => r.AnPaiDate == d && r.ClassRoom_Id == c_id).ToList();
            for (int i = 0; i < reconciles.Count; i++)
            {
                for (int j = 0; j < str.Count; j++)
                {
                    if (reconciles[i].Curse_Id=="上午")
                    {
                        str.Remove("上午12节");
                        str.Remove("上午34节");
                    }
                    else if (reconciles[i].Curse_Id == "下午")
                    {
                        str.Remove("下午12节");
                        str.Remove("下午34节");
                    }
                    else
                    {
                        if (reconciles[i].Curse_Id == str[j])
                        {
                            str.Remove(str[j]);
                        }
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
            List<Classroom> c_list = Reconcile_Com.Classroom_Entity.GetList().Where(c => c.BaseData_Id == base_id).ToList();
            if (TimeName=="晚自习" || TimeName=="晚一" || TimeName=="晚二")
            {                
                List<Reconcile> reconciles = AllReconcile().Where(r =>( r.Curse_Id == "晚自习" || r.Curse_Id == "晚一" || r.Curse_Id == "晚二") && r.AnPaiDate == time).ToList();
                for (int i = 0; i < reconciles.Count; i++)
                {
                    for (int j = 0; j < c_list.Count; j++)
                    {
                        if (reconciles[i].ClassRoom_Id == c_list[j].Id)
                        {
                            c_list.Remove(c_list[j]);
                        }
                    }
                }
                return c_list;
            }
            else
            {
                List<Reconcile> reconciles = new List<Reconcile>();
                if (TimeName=="上午")
                {
                    reconciles= AllReconcile().Where(r => (r.Curse_Id == TimeName || r.Curse_Id=="上午") && r.AnPaiDate == time).ToList();
                }
                else if(TimeName == "下午")
                {
                    reconciles = AllReconcile().Where(r => (r.Curse_Id == TimeName || r.Curse_Id == "下午") && r.AnPaiDate == time).ToList();
                }
                for (int i = 0; i < reconciles.Count; i++)
                {
                    for (int j = 0; j < c_list.Count; j++)
                    {
                        if (reconciles[i].ClassRoom_Id == c_list[j].Id)
                        {
                            c_list.Remove(c_list[j]);
                        }
                    }
                }
                return c_list;
            }
            
        }        
        /// <summary>
        /// 判断这个班级是否可以安排英语/职素/军事(true--可以安排，false--不可以安排)
        /// </summary>
        /// <param name="class_id"></param>
        /// <returns></returns>
        public bool IsOkAnpiKecheng(int class_id ,int classnumber)
        {
            bool s = true;
            //获取这个班级是哪个阶段
            ClassSchedule find_c= Reconcile_Com.ClassSchedule_Entity.GetEntity(class_id);
            Grand find_g= Reconcile_Com.Grand_Entity.GetEntity(find_c.grade_Id);
            //获取班级正在上的课程
            int? curr_id = Reconcile_Com.TeacherClass_Entity.GetList().Where(c=>c.ClassNumber== class_id && c.IsDel==false).FirstOrDefault().Skill;
           //获取这个班级所在阶段
            switch (find_g.GrandName)
            {
                case "Y1":
                    
                    break;
                case "S1":
                    //当上完C#考试之后就没有了
                    //获取C#课程Id
                    Curriculum find_curr= Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.CourseName.Equals("C#", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    if (find_curr!=null)
                    {
                        if (find_curr.CurriculumID==curr_id)
                        {
                            s = false;
                        }
                    }
                    else
                    {
                        s = false;
                    }
                    break;
                case "S2":
                    //判断班级是net(C#ADB)专业还是java(javaADB)专业
                    Specialty find_s1= Reconcile_Com.Specialty_Entity.GetList().Where(p => p.SpecialtyName.Equals("Java") && p.IsDelete == false).FirstOrDefault();
                    Specialty find_s2 = Reconcile_Com.Specialty_Entity.GetList().Where(p => p.SpecialtyName.Equals("DotNet") && p.IsDelete == false).FirstOrDefault();
                    if (find_c.Major_Id==find_s1.Id)
                    {
                        //java
                        //获取javaADB的课程ID
                        Curriculum find_java = Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.CourseName.Equals("Java-ADV", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                        if (find_java.CurriculumID== curr_id)
                        {
                            s = false;
                        }
                        else
                        {
                            s = true;
                        }
                    }
                    else
                    {
                        //c#
                        //获取NetADB的课程ID
                        Curriculum find_net = Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.CourseName.Equals("C#-ADV", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                        if (find_net.CurriculumID == curr_id)
                        {
                            s = false;
                        }
                        else
                        {
                            s = true;
                        }
                    }
                    break;
                case "S3":
                    break;
                case "S4":
                    break;                   
            }
            return s;
        }
        /// <summary>
        /// 安排这个班级是什么阶段在上什么课程
        /// </summary>
        /// <param name="CurrName">课程名称</param>
        /// <param name="time">获取课表日期</param>
        /// <param name="afternoonclass">班级集合</param>
        /// <param name="moringroom">教室集合</param>
        public void mmm(string CurrName, DateTime time,List<ClassSchedule> afternoonclass, List<Classroom> moringroom,bool s1ors3)
        {
            Reconcile_Com.redisCache.RemoveCache("ReconcileList");
            //获取这周日期
            Mydate mydate = GetMydate(time, 1);
            Mydate mydate2 = GetMydate(time, 2);

            foreach (Classroom room in moringroom)
            {                 
                foreach (ClassSchedule item in afternoonclass)
                {
                    //看看这个教室排满了没有
                    string str = RoomTime(room.Id, time, item.BaseDataEnum_Id);
                    Reconcile r = new Reconcile();
                    r.IsDelete = false;
                    r.AnPaiDate = time;
                    r.ClassSchedule_Id = item.id;
                    r.NewDate = DateTime.Now;
                    r.Curriculum_Id = CurrName;                    
                   //判断这个班级是否可以排英语,班会
                   bool s1 = Existence(mydate, item.id, CurrName);
                   if (s1 == false)
                        {                             
                            if (!string.IsNullOrEmpty(str))
                            {                                 
                                r.ClassRoom_Id = room.Id;
                                r.Curse_Id = str;                                                                                                
                                //安排任课老师
                                if (CurrName == "英语")
                                {
                                   // 获取英语老师
                                    r.EmployeesInfo_Id = AnpaiTeacher(item.grade_Id, CurrName, str, time);
                                    if (string.IsNullOrEmpty(r.EmployeesInfo_Id) || r.EmployeesInfo_Id == null)
                                    {
                                        r.EmployeesInfo_Id = null;
                                        r.Curriculum_Id = "自习";
                                    }
                                }
                                else
                                {
                                    //获取班会
                                    r.EmployeesInfo_Id = GetMasterTeacher(item.id, time, str);
                                    if (string.IsNullOrEmpty(r.EmployeesInfo_Id)|| r.EmployeesInfo_Id==null)
                                    {
                                        r.EmployeesInfo_Id = null;
                                        r.Curriculum_Id = "自习";
                                    }

                                }
                                   bool s = ExistenceToday(r.AnPaiDate, r.ClassSchedule_Id, r.Curriculum_Id);
                                   if (s == false)
                                   {
                                       this.AddData(r);
                                   }
                            }
                            else
                            {
                                break;
                            }
                        }
                   if (CurrName == "职素" || CurrName == "军事")
                    {
                        //判断这个班级是否可以排职素，军事
                        bool s2 = Existence(mydate2, item.id, CurrName);
                        if (s2==false)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                r.ClassRoom_Id = room.Id;
                                r.Curse_Id = str;
                                //安排任课老师
                                if (CurrName == "职素")
                                {
                                    // 获取职素老师
                                    r.EmployeesInfo_Id = GetRandMASteacher(true,s1ors3).EmpName;
                                    bool ishave= IsHaveClass(r.EmployeesInfo_Id, r.Curse_Id, time);
                                    if (ishave)
                                    {
                                        r.EmployeesInfo_Id = null;
                                        r.Curriculum_Id = "自习";
                                    }
                                }
                                else
                                {
                                    //获取军事
                                    // 获取职素老师
                                    r.EmployeesInfo_Id = GetRandMASteacher(false, s1ors3).EmpName;
                                    bool ishave = IsHaveClass(r.EmployeesInfo_Id, r.Curse_Id, time);
                                    if (ishave)
                                    {
                                        r.EmployeesInfo_Id = null;
                                        r.Curriculum_Id = "自习";
                                    }

                                    bool s = ExistenceToday(r.AnPaiDate, r.ClassSchedule_Id, r.Curriculum_Id);
                                    if (s == false)
                                    {
                                        this.AddData(r);
                                    }
                                }                                 
                            }
                            else
                            {
                                break;
                            }
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
            List<Reconcile> r_list= AllReconcile().Where(r=>r.AnPaiDate==time).ToList();
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
                            new_a.ClassName = Reconcile_Com.ClassSchedule_Entity.GetEntity( r1.ClassSchedule_Id).ClassNumber;
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
                            new_a.ClassName = Reconcile_Com.ClassSchedule_Entity.GetEntity(r1.ClassSchedule_Id).ClassNumber;
                            new_a.Teacher = r1.EmployeesInfo_Id == null ? "无" : entity.GetEntity(r1.EmployeesInfo_Id).EmpName;
                            new_a.NeiRong = r1.Curriculum_Id;
                        }
                    }
                    a_list.Add(new_a);
                }
            }
            return a_list;
        }
         
        
        //public EmployeesInfo GetHeadTeacher(int class_id)
        //{
        //    EmployeesInfo find_e= Headmaster_Etity.ClassHeadmaster(class_id);
        //    Headmaster find_h= Headmaster_Etity.GetList().Where(w => w.informatiees_Id == find_e.EmployeeId).FirstOrDefault();
        //    if (find_h.IsAttend==true)
        //    {
        //        return find_e;
        //    }
        //    else
        //    {

        //    }
        //}
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
           return AllReconcile().Where(r => r.AnPaiDate == time && r.ClassSchedule_Id == class_id && r.Curse_Id == currName).ToList();
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
            new_r.ClassRoom_Id = Reconcile_Com.Classroom_Entity.GetEntity(r.ClassRoom_Id);
            new_r.ClassSchedule_Id = Reconcile_Com.ClassSchedule_Entity.GetEntity(r.Id);
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
                Reconcile find_r = AllReconcile().Where(r => r.AnPaiDate == dateTime && r.Curse_Id == timename && r.ClassSchedule_Id == class_id).FirstOrDefault();
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
        /// <summary>
        /// 加课
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="emp_id">加课老师</param>
        /// <param name="timename">时间段(上午，下午，晚自习)</param>
        /// <param name="class_id">班级Id</param>
        /// <param name="curr_name">课程名称</param>
        /// <returns></returns>
        public AjaxResult Addke(DateTime dateTime,string emp_id,string timename,int class_id,string curr_name,int addcount)
        {
            AjaxResult data = new AjaxResult();
             
            //判断这个老师在这个时间段有没有安排课
            bool Is = IsHaveClass(emp_id, timename, dateTime);
            if (Is==false)
            {
                int base_id = 0;
                //加课
                //获取班级课程Id
                Curriculum find_c= Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.CourseName == curr_name).FirstOrDefault();
                ClassSchedule find_class= Reconcile_Com.ClassSchedule_Entity.GetEntity(class_id);
                //判断该班级是哪个阶段的
                Grand find_g= Reconcile_Com.Grand_Entity.GetEntity(find_class.grade_Id);
                if (find_g.GrandName=="S1" || find_g.GrandName=="S2" || find_g.GrandName=="Y1")
                {
                    base_id = Reconcile_Com.ClassSchedule_Entity.BaseDataEnum_Entity.GetSingData("继善高科校区", false).Id;
                }
                else
                {
                    base_id = Reconcile_Com.ClassSchedule_Entity.BaseDataEnum_Entity.GetSingData("达嘉维康校区", false).Id;
                }
                if (find_c.IsEndCurr==false)
                {
                    //如果不是最后一门课，获取这个课程后面的所有课程
                    List<Curriculum> find_c2 = Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.Sort >= (find_c.Sort + 1) && c.Grand_Id == find_c.Grand_Id).ToList();
                    foreach (Curriculum item in find_c2)
                    {

                    }
                }
                else
                {
                    //如果是最后一门课程，直接添加
                    for (int i = 0; i < addcount; i++)
                    {
                        Reconcile r = new Reconcile();
                        r.AnPaiDate = dateTime;
                        r.ClassRoom_Id = GetClassrooms(timename, base_id, dateTime).First().Id;
                        r.ClassSchedule_Id = class_id;
                        r.Curriculum_Id = curr_name;
                        r.Curse_Id = timename;
                        r.EmployeesInfo_Id = emp_id;
                        r.IsDelete = false;
                        r.NewDate = DateTime.Now;
                        try
                        {
                            this.Insert(r);
                        }
                        catch (Exception)
                        {
                            data.ErrorCode = 500;
                        }
                        
                    }
                }
                data.ErrorCode = 200;
            }
            else
            {
                data.ErrorCode = 500;
                data.Msg = "该时间段老师已被安排其他内容！！！";
            }
            return data;
        }
        
        #endregion
    }
}
