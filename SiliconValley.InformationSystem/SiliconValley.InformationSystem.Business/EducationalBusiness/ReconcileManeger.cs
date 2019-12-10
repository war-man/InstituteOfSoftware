﻿using SiliconValley.InformationSystem.Business.ClassesBusiness;
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
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Reconcile GetSingleData(int id)
        {
           return AllReconcile().Where(r => r.Id == id).FirstOrDefault();
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
        public AjaxResult UpdateReconcile(Reconcile r)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                Reconcile find_r= AllReconcile().Where(f => f.Id == r.Id).FirstOrDefault();
                find_r.ClassRoom_Id = r.ClassRoom_Id;//教室
                find_r.AnPaiDate = r.AnPaiDate;//日期
                find_r.EmployeesInfo_Id = r.EmployeesInfo_Id;//教学老师
                find_r.Curriculum_Id = r.Curriculum_Id;//课程
                find_r.Curse_Id = r.Curse_Id;//时间段
                this.Update(find_r);
                a.Success = true;
                Reconcile_Com.redisCache.RemoveCache("ReconcileList");
            }
            catch (Exception ex)
            {
                a.Msg = ex.Message;
                a.Success = false;
            }
            
            return a;
        }
         /// <summary>
         /// 判断日期是否可以排课（true--可以排课，false--不可以排课）
         /// </summary>
         /// <param name="xmlfile">xml路径</param>
         /// <param name="time">日期</param>
         /// <returns></returns>
        public bool IsShangKe(string xmlfile,DateTime time)
        {
            bool s = true;
            GetYear yy = MyGetYear(time.Year.ToString(), xmlfile);
            if (time.Month >= yy.StartmonthName && time.Month <= yy.EndmonthName)
            {
                //单休
                //判断time是否是星期天
                int tt = IsSaturday(time);
                if (tt==2)
                {
                    s = false;
                }
            }
            else
            {
                //双休
                int tt = IsSaturday(time);
                if (tt == 2 || tt==1)
                {
                    s = false;
                }
            }
            return s;
        }
        //获取可以上职素的班主任集合
        public List<EmployeesInfo> GetMasTeacher(DateTime time,string timename)
        {
            List<EmployeesInfo> list = new List<EmployeesInfo>();
            BaseBusiness<Headmaster> basemaster = new BaseBusiness<Headmaster>();            
            List<EmployeesInfo> elist = Reconcile_Com.Employees_Entity.GetList();
            List<Headmaster> find_m = basemaster.GetList().Where(m => m.IsDelete == false && m.IsAttend == true).ToList();
            foreach (EmployeesInfo e1 in elist)
            {
                foreach (Headmaster m1 in find_m)
                {
                    if (m1.informatiees_Id == e1.EmployeeId)
                    {
                        //判断这个班主任是否有课
                        bool s = IsHaveClass(e1.EmployeeId, timename, time);
                        if (s==false)
                        {
                            list.Add(e1);
                        }                        
                    }
                }
            }
            return list;
        }        
        //获取教官
        public List<EmployeesInfo> GetSir(bool Is,DateTime time,string timename)
        {
            DepartmentManage department = new DepartmentManage();
            BaseBusiness<Position> position = new BaseBusiness<Position>();
            List<EmployeesInfo> employees = Reconcile_Com.Employees_Entity.GetList().Where(e => e.IsDel == false).ToList();
            List<EmployeesInfo> em = new List<EmployeesInfo>();
            List<EmployeesInfo> em2 = new List<EmployeesInfo>();
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
            //判断教官是否在这个时间段有课
            foreach (EmployeesInfo ee in em)
            {
                bool s = IsHaveClass(ee.EmployeeId,timename,time);
                if (s==false)
                {
                    em2.Add(ee);
                }
            }
            return em2;
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
        public EmployeesInfo GetRandMASteacher(bool teacher,bool s1ors2,DateTime time,string timename)
        {
            Random r = new Random();
            List<EmployeesInfo> list1 = GetMasTeacher(time,timename);
            List<EmployeesInfo> list2 = GetSir(s1ors2,time,timename);
                       
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
        /// <summary>
        /// 大批量添加数据
        /// </summary>
        /// <param name="new_list">排课数据集合</param>
        /// <returns></returns>
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
            catch (Exception)
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
        /// 判断某班级某天安排了某个课程(false--没有安排，true--已安排)
        /// </summary>
        /// <param name="class_id"></param>
        /// <param name="time"></param>
        /// <param name="currname"></param>
        /// <returns></returns>
        public bool ToHavaKe(int class_id,DateTime time,string currname)
        {
            bool s = false;
           int count= AllReconcile().Where(r => r.ClassSchedule_Id == class_id && r.AnPaiDate == time && r.Curriculum_Id == currname).ToList().Count;
            if (count>0)
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
        public Mydate GetMydate(DateTime d, int type)
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
            if (type == 2)
            {
                //输出两周的日期
                mydate.StarTime = mydate.StarTime.AddDays(-7);
            }
            return mydate;
        }
        /// <summary>
        /// 通过课程名称获取课程数据
        /// </summary>
        /// <param name="Name">课程名称</param>
        /// <returns></returns>
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
        public bool IsOkAnpiKecheng(int class_id)
        {
            bool s = true;
            //获取这个班级是哪个阶段
            ClassSchedule find_c= Reconcile_Com.ClassSchedule_Entity.GetEntity(class_id);
            Grand find_g= Reconcile_Com.Grand_Entity.GetEntity(find_c.grade_Id);
            //获取班级正在上的课程
            ClassTeacher find_ct= Reconcile_Com.TeacherClass_Entity.GetList().Where(c => c.ClassNumber == class_id && c.IsDel == false).FirstOrDefault();
            if (find_ct!=null)
            {
                int? curr_id = find_ct.Skill;
                //获取这个班级所在阶段
                switch (find_g.GrandName)
                {
                    case "Y1":

                        break;
                    case "S1":
                        //当上完C#考试之后就没有了
                        //获取C#课程Id
                        Curriculum find_curr = Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.CourseName.Equals("C#", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                        if (find_curr != null)
                        {
                            if (find_curr.CurriculumID == curr_id)
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
                        Specialty find_s1 = Reconcile_Com.Specialty_Entity.GetList().Where(p => p.SpecialtyName.Equals("Java") && p.IsDelete == false).FirstOrDefault();
                        Specialty find_s2 = Reconcile_Com.Specialty_Entity.GetList().Where(p => p.SpecialtyName.Equals("DotNet") && p.IsDelete == false).FirstOrDefault();
                        if (find_c.Major_Id == find_s1.Id)
                        {
                            //java
                            //获取javaADB的课程ID
                            Curriculum find_java = Reconcile_Com.Curriculum_Entity.GetList().Where(c => c.CourseName.Equals("Java-ADV", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                            if (find_java.CurriculumID == curr_id)
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
            }
            else
            {
                s = true;
            }             
            return s;
        }
        /// <summary>
        /// 判断这个班级在这个日期中的时间段是否有课(false--没有，true--有)
        /// </summary>
        /// <param name="class_id">班级编号</param>
        /// <param name="timename">时间段</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public bool IshavaOrther(int class_id,string timename,DateTime time)
        {
            bool s = false;
            int count= AllReconcile().Where(r => r.ClassSchedule_Id == class_id && r.AnPaiDate == time && r.Curse_Id == timename).ToList().Count;
            if (count>0)
            {
                s = true;
            }
            return s;
        }
        /// <summary>
        /// 判断这个班级在本周上自习的次数
        /// </summary>
        /// <param name="class_id">班级id</param>
        /// <param name="startime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public int count(int class_id,DateTime startime,DateTime endtime)
        {
           return AllReconcile().Where(r => r.ClassSchedule_Id == class_id && r.AnPaiDate >= startime && r.AnPaiDate <= endtime).ToList().Count;
        }
        public AjaxResult Thismm(bool IsOld, int base_id, string currname,DateTime starttime,DateTime endtime,string xmlfile)
         {
            AjaxResult a = new AjaxResult();
            try
            {
                Reconcile_Com.redisCache.RemoveCache("ReconcileList");
                //获取班级上课时间段
                int baid1 = Reconcile_Com.GetBase_Id("上课时间类型", "上午");
                int baid2 = Reconcile_Com.GetBase_Id("上课时间类型", "下午");
                //英语，班会，职素，军事课
                List<ClassSchedule> monring = Reconcile_Com.GetAppointClass(IsOld, Reconcile_Com.GetGrand_Id(IsOld), baid1);//获取上午班
                List<ClassSchedule> afternoon = Reconcile_Com.GetAppointClass(IsOld, Reconcile_Com.GetGrand_Id(IsOld), baid2);//获取下午班 
                 //获取时间集合                                                                                                             
                List<DateTime> times = new List<DateTime>();
                int i = 1;
                while (starttime <= endtime)
                {
                    times.Add(starttime);
                    starttime = starttime.AddDays(i);
                }
                if (IsOld)
                {
                    if (currname!="晚自习")
                    {
                        mm(IsOld, base_id, times, currname, monring, "下午", xmlfile);
                        mm(IsOld, base_id, times, currname, afternoon, "上午", xmlfile);
                    }
                    else
                    {
                        //晚自习安排
                        //获取班级
                        foreach (ClassSchedule item in afternoon)
                        {
                            monring.Add(item);
                        }
                        //获取教室
                        List<Classroom> classrooms = Reconcile_Com.Classroom_Entity.GetList().Where(c => c.BaseData_Id == base_id && c.ClassroomName != "报告厅" && c.ClassroomName != "操场").ToList();
                        //晚自习安排
                        NightAnpai(monring, times, classrooms, xmlfile);
                    }
                    
                }
                else
                {
                    if (currname != "晚自习")
                    {
                        mm2(IsOld, base_id, times, currname, monring, "下午", xmlfile);
                        mm2(IsOld, base_id, times, currname, afternoon, "上午", xmlfile);
                    }
                    else
                    {
                        //获取班级
                        foreach (ClassSchedule item in afternoon)
                        {
                            monring.Add(item);
                        }
                        //获取教室
                        List<Classroom> classrooms=  Reconcile_Com.Classroom_Entity.GetList().Where(c => c.BaseData_Id == base_id && c.ClassroomName!="报告厅" && c.ClassroomName!="操场").ToList();
                        //晚自习安排
                        NightAnpai(monring, times, classrooms,xmlfile);
                    }
                         
                }
                 
                a.Success = true;
            }
            catch (Exception ex)
            {
                a.Msg = ex.Message;
                a.Success = false;
            }
            return a;
        }
        public void mm2(bool IsOld, int base_id, List<DateTime> times, string currname, List<ClassSchedule> monring, string timename, string xmlfile)
        {
            foreach (DateTime time in times)
            {
                //判断这个时间是否是6至9月
                GetYear yy = MyGetYear(time.Year.ToString(), xmlfile);
                if (time.Month >= yy.StartmonthName && time.Month <= yy.EndmonthName)
                {
                    //单休
                    //判断time是否是星期天
                    int tt = IsSaturday(time);
                    if (tt == 2)
                    {
                        continue;
                    }
                }
                else
                {
                    //双休
                    //判断time是否是星期六或星期天
                    int tt = IsSaturday(time);
                    if (tt == 2 || tt == 1)
                    {
                        continue;
                    }
                }
                //获取这个日期下午的空教室
                List<Classroom> getroom2 = new List<Classroom>();
                if (currname=="军事")
                {
                    getroom2= this.GetClassrooms(timename, base_id, time).Where(c => c.ClassroomName == "操场").ToList();
                }
                else
                {
                    getroom2= this.GetClassrooms(timename, base_id, time).Where(c => c.ClassroomName != "报告厅" && c.ClassroomName != "操场").ToList();
                }
                foreach (ClassSchedule c1 in monring)
                {
                    foreach (Classroom c2 in getroom2)
                    {
                        //看看这个教室排满了没有
                        string str = RoomTime(c2.Id, time, c1.BaseDataEnum_Id);
                        if (!string.IsNullOrEmpty(str))
                        {
                            Reconcile r = new Reconcile();
                            //判断这个班级是否在这个期间安排了这个课程
                            bool Ishavecurr = IsHaveCurr(times[0], times[times.Count - 1], currname, c1.id);
                            if (Ishavecurr == false)
                            {
                                #region 获取非专老师
                                switch (currname)
                                {                                     
                                    case "军事":
                                        //判断该班级是否可以安排军事课
                                        bool isok3 = IsOkAnpiKecheng(c1.id);
                                        if (isok3)
                                        {
                                            //获取军事老师
                                            r.EmployeesInfo_Id = GetRandMASteacher(false, IsOld, time, r.Curse_Id).EmployeeId;
                                        }
                                        break;
                                    case "自习":
                                        r.EmployeesInfo_Id = null;
                                        break;
                                }
                                #endregion
                            }
                            r.Curse_Id = str;
                            r.AnPaiDate = time;
                            r.ClassRoom_Id = c2.Id;
                            r.ClassSchedule_Id = c1.id;
                            r.Curriculum_Id = currname;
                            r.IsDelete = false;
                            r.NewDate = DateTime.Now;

                            if (r.EmployeesInfo_Id != null || string.IsNullOrEmpty(r.EmployeesInfo_Id) == false)
                            {
                                //判断这个班在这个时间段是否有课
                                bool s2 = IshavaOrther(r.ClassSchedule_Id, r.Curse_Id, time);
                                if (s2 == false)
                                {
                                    //查看任课老师是否有冲突
                                    if (r.Curriculum_Id == "军事")
                                    {
                                        bool s1 = IsHaveClass(r.EmployeesInfo_Id, r.Curse_Id, time);
                                        if (s1 == false)
                                        {
                                            //安排
                                            this.AddData(r);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        //安排
                                        this.AddData(r);
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }

                            }
                            else
                            {
                                //安排自习课
                                //判断这个班在这个时间段是否有课
                                bool s2 = IshavaOrther(r.ClassSchedule_Id, r.Curse_Id, time);
                                if (s2 == false)
                                {
                                    //判断今天是否安排了自习课
                                    bool s5 = ToHavaKe(r.ClassSchedule_Id, Convert.ToDateTime(r.AnPaiDate), r.Curriculum_Id);
                                    if (s5 == false)
                                    {
                                        //判断安排自习课的次数
                                        int cout = count(r.ClassSchedule_Id, times[0], times[times.Count - 1]);
                                        if (cout <= 3)
                                        {
                                            //安排
                                            this.AddData(r);
                                            break;
                                        }
                                    }

                                }
                            }

                        }
                        else
                        {
                            continue;
                        }

                    }
                }
            }
        } 
        /// <summary>
        ///  判断XX班级在这期间是否上过XX课程(false--没有，ture--有)
        /// </summary>
        /// <param name="startime">开始日期</param>
        /// <param name="endtime">结束日期</param>
        /// <param name="currName">课程名称</param>
        /// <param name="class_id">班级名称</param>
        /// <returns></returns>
        public bool IsHaveCurr(DateTime startime,DateTime endtime,string currName,int class_id)
        {
            bool s = false;
            int count= AllReconcile().Where(r => r.ClassSchedule_Id == class_id && r.Curriculum_Id == currName && r.AnPaiDate >= startime && r.AnPaiDate <= endtime).ToList().Count();
            if (count>0)
            {
                s = true;
            }
            return s;
        }
        /// <summary>
        /// 安排英语，军事，职素，班会课程
        /// </summary>
        /// <param name="IsOld">是否是S1教务操作</param>
        /// <param name="base_id">S1或S3的教官Id</param>
        /// <param name="times">时间集合</param>
        /// <param name="currname">课程名称</param>
        /// <param name="monring">班级集合</param>
        /// <param name="timename">时间段</param>
        /// <param name="xmlfile">xml文件地址</param>
        public void mm(bool IsOld,int base_id,List<DateTime> times,string currname, List<ClassSchedule> monring,string timename,string xmlfile)
        {          
                foreach (DateTime time in times)
                {
                    bool isske=  IsShangKe(xmlfile, time);
                     if (isske==false)
                     {
                        continue;
                     }
                    //获取这个日期下午的空教室 
                    List<Classroom> getroom2 = new List<Classroom>();
                    if (currname == "军事")
                    {
                        getroom2 = this.GetClassrooms(timename, base_id, time).Where(c => c.ClassroomName == "操场").ToList();
                    }
                    else
                    {
                        getroom2 = this.GetClassrooms(timename, base_id, time).Where(c => c.ClassroomName != "报告厅" && c.ClassroomName != "操场").ToList();
                    }

                    foreach (ClassSchedule c1 in monring)
                    {
                        foreach (Classroom c2 in getroom2)
                        {
                            //看看这个教室排满了没有
                            string str = RoomTime(c2.Id, time, c1.BaseDataEnum_Id);
                            if (!string.IsNullOrEmpty(str))
                            {
                                Reconcile r = new Reconcile();
                                //判断这个班级是否在这个期间安排了这个课程
                                bool Ishavecurr = IsHaveCurr(times[0], times[times.Count - 1], currname, c1.id);
                                if (Ishavecurr == false)
                                {
                                    #region 获取非专老师
                                    switch (currname)
                                    {
                                        case "英语":
                                            //判断该班级是否可以安排英语课
                                            bool isok = IsOkAnpiKecheng(c1.id);
                                            if (isok)
                                            {
                                                //获取英语老师
                                                //如果是Y1阶段不需要安排
                                                bool is1 = Reconcile_Com.GetBrand(c1.grade_Id);
                                                if (is1 == false)
                                                {
                                                    r.EmployeesInfo_Id = AnpaiTeacher(c1.grade_Id, currname, str, time);
                                                }
                                            }
                                            break;
                                        case "班会":
                                            //获取班会老师
                                            r.EmployeesInfo_Id = GetMasterTeacher(c1.id, time, str);
                                            break;
                                        case "职素":
                                            //判断该班级是否可以安排职素课
                                            bool isok2 = IsOkAnpiKecheng(c1.id);
                                            if (isok2)
                                            {
                                                //获取职素老师
                                                r.EmployeesInfo_Id = GetRandMASteacher(true, IsOld,time,r.Curse_Id).EmployeeId;
                                            }
                                            break;
                                        case "军事":
                                            //判断该班级是否可以安排军事课
                                            bool isok3 = IsOkAnpiKecheng(c1.id);
                                            if (isok3)
                                            {
                                                //获取军事老师
                                                r.EmployeesInfo_Id = GetRandMASteacher(false, IsOld,time,r.Curse_Id).EmployeeId;
                                            }
                                            break;
                                        case "自习":
                                            r.EmployeesInfo_Id = null;
                                            break;
                                    }
                                    #endregion
                                }
                                r.Curse_Id = str;
                                r.AnPaiDate = time;
                                r.ClassRoom_Id = c2.Id;
                                r.ClassSchedule_Id = c1.id;
                                r.Curriculum_Id = currname;
                                r.IsDelete = false;
                                r.NewDate = DateTime.Now;

                                if (r.EmployeesInfo_Id != null || string.IsNullOrEmpty(r.EmployeesInfo_Id) == false)
                                {
                                    //判断这个班在这个时间段是否有课
                                    bool s2 = IshavaOrther(r.ClassSchedule_Id, r.Curse_Id, time);
                                    if (s2 == false)
                                    {
                                        //查看任课老师是否有冲突
                                        if (r.Curriculum_Id == "职素" || r.Curriculum_Id == "军事")
                                        {
                                            bool s1 = IsHaveClass(r.EmployeesInfo_Id, r.Curse_Id, time);
                                            if (s1 == false)
                                            {
                                                //安排
                                                this.AddData(r);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //安排
                                            this.AddData(r);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }

                                }
                                else
                                {
                                    //安排自习课
                                    //判断这个班在这个时间段是否有课
                                    bool s2 = IshavaOrther(r.ClassSchedule_Id, r.Curse_Id, time);
                                    if (s2 == false)
                                    {
                                        //判断今天是否安排了自习课
                                        bool s5 = ToHavaKe(r.ClassSchedule_Id, Convert.ToDateTime(r.AnPaiDate), r.Curriculum_Id);
                                        if (s5 == false)
                                        {
                                            //判断安排自习课的次数
                                            int cout = count(r.ClassSchedule_Id, times[0], times[times.Count - 1]);
                                            if (cout <= 3)
                                            {
                                                //安排
                                                this.AddData(r);
                                                break;
                                            }
                                        }

                                    }
                                }

                            }
                            else
                            {
                                continue;
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
        /// 删除数据(true--成功 ，true--失败)
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public AjaxResult DeleteReconcile(int r_id)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                Reconcile find_r= AllReconcile().Where(r => r.Id == r_id).FirstOrDefault();
                this.Delete(find_r);
                a.Success = true;
                a.Msg = "操作成功";
                Reconcile_Com.redisCache.RemoveCache("ReconcileList");
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = ex.Message;
            }
            return a;
        }
        /// <summary>
        /// 判断XX教室XX天排了晚一或晚二
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="classroom_id">教室编号</param>
        /// <returns></returns>
        public string Overflow(DateTime time,int classroom_id,string timename)
        {
           int count= AllReconcile().Where(r => r.AnPaiDate == time && r.ClassRoom_Id == classroom_id && r.Curse_Id == timename).ToList().Count();
            if (count<=0)
            {
                return timename;
            }
            else
            {
                if (timename == "晚一")
                {
                    int count2 = AllReconcile().Where(r => r.AnPaiDate == time && r.ClassRoom_Id == classroom_id && r.Curse_Id == "晚二").ToList().Count;
                    if (count2 <= 0)
                    {
                        return "晚二";
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    int count2 = AllReconcile().Where(r => r.AnPaiDate == time && r.ClassRoom_Id == classroom_id && r.Curse_Id == "晚一").ToList().Count;
                    if (count2 <= 0)
                    {
                        return "晚一";
                    }
                    else
                    {
                        return null;
                    }
                }
                
                 
            }
        }
        /// <summary>
        /// 晚自习安排
        /// </summary>
        /// <param name="classes">班级集合</param>
        /// <param name="times">日期</param>
        /// <param name="classrooms">教室集合</param>
        /// <param name="xmlfile">Xml文件</param>
        public void NightAnpai(List<ClassSchedule> classes,List<DateTime> times,List<Classroom> classrooms, string xmlfile)
        {
            Random rand = new Random();
            foreach (DateTime time in times)
            {
                //判断周六是否要上课
                if(IsShangKe(xmlfile, time)==false)
                {
                    continue;
                }
                Reconcile r = new Reconcile();
                foreach (ClassSchedule c1 in classes)
                {
                    foreach (Classroom c2 in classrooms)
                    {
                        int number = rand.Next(1,100);
                        string str1= number%3==0? Overflow(time, c2.Id,"晚一"): Overflow(time, c2.Id, "晚二");
                        if (!string.IsNullOrEmpty(str1))
                        {
                            r.Curse_Id = str1;
                            r.ClassSchedule_Id = c1.id;
                            r.AnPaiDate = time;
                            r.ClassRoom_Id = c2.Id;
                            r.Curriculum_Id = "晚自习";
                            r.EmployeesInfo_Id = null;
                            r.IsDelete = false;
                            r.NewDate = DateTime.Now;
                            //判断今天这个班级是否已经安排这个课程
                           bool s1= ToHavaKe(r.ClassSchedule_Id, Convert.ToDateTime(r.AnPaiDate), r.Curriculum_Id);
                            if (!s1)
                            {
                                //安排
                                this.AddData(r);
                                break;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取单个语数英老师
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="currname">课程</param>
        /// <param name="timename">时间段</param>
        /// <returns></returns>
        public string GetNomaginTeacher(DateTime time,string currname,string timename,int grand_id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            switch (currname)
            {
                case "语文":
                    List<EmployeesInfo> list_e= GetEmployees("语文老师");
                    if (list_e.Count>0)
                    {
                        sb.Append(list_e[0].EmployeeId);
                    }
                    break;
                case "数学":
                    List<EmployeesInfo> list_e1 = GetEmployees("数学老师");
                    if (list_e1.Count > 0)
                    {
                       sb.Append(list_e1[0].EmployeeId);
                    }
                    break;
                case "英语":                 
                      sb.Append( AnpaiTeacher(grand_id, currname, timename, time));
                    break;
            }
            //查看这个老师这一天是否有课
            if (!string.IsNullOrEmpty(sb.ToString()))
            {
               int count= AllReconcile().Where(r => r.EmployeesInfo_Id == sb.ToString() && r.AnPaiDate == time && r.Curse_Id == timename).ToList().Count;
                if (count>0)
                {
                    sb.Append("");
                }
            }
            return sb.ToString();
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
            } else if (timename=="晚一" || timename == "晚二")
            {
                foreach (Classroom c1 in classrooms)
                {
                    AnPaiData new_a = new AnPaiData();
                    foreach (Reconcile r1 in r_list)
                    {
                        if (r1.ClassRoom_Id == c1.Id && r1.Curse_Id == timename )
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
        /// <summary>
        /// 获取任课老师集合
        /// </summary>
        /// <param name="cur_id">课程Id</param>
        /// <param name="timename">时间段</param>
        /// <param name="time">日期</param>
        /// <returns></returns>
        public List<EmployeesInfo> GetMarjorTeacher(int cur_id,string timename,DateTime time)
        {
            //获取可以上这个课程的老师
            List<Teacher>g_list= Reconcile_Com.GoodSkill_Entity.GetTeachers(cur_id);
            //判断这个老师在XX日期XX时间段是否有课
            List<EmployeesInfo> e_list = Reconcile_Com.Employees_Entity.GetList();
            List<EmployeesInfo> emp = new List<EmployeesInfo>();            
            foreach (EmployeesInfo item1 in e_list)
            {
                foreach (Teacher item2 in g_list)
                {
                    if (item1.EmployeeId==item2.EmployeeId)
                    {
                        bool s = IsHaveClass(item2.EmployeeId, timename, time);//判断老师是否有课
                        if (s == false)
                        {
                            emp.Add(item1);
                        }
                    }                    
                }
            }
            return emp;
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
