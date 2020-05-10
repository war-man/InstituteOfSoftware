using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
using SiliconValley.InformationSystem.Business.DepartmentBusiness;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
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
        public static readonly EvningSelfStudyManeger EvningSelfStudy_Entity = new EvningSelfStudyManeger();

        /// <summary>
        /// 从缓存中获取排课所有数据
        /// </summary>
        /// <returns></returns>
        public List<Reconcile> AllReconcile()
        {

            //List<Reconcile> get_reconciles_list = new List<Reconcile>();
            //get_reconciles_list = Reconcile_Com.redisCache.GetCache<List<Reconcile>>("ReconcileList");
            //if (get_reconciles_list == null || get_reconciles_list.Count == 0)
            //{
            //    get_reconciles_list = this.GetList();
            //    Reconcile_Com.redisCache.SetCache("ReconcileList", get_reconciles_list);
            //}
            return SQLGetReconcileDate();
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
                List<Reconcile> find_list = AllReconcile().Where(rs => rs.AnPaiDate == r.AnPaiDate && rs.ClassSchedule_Id == r.ClassSchedule_Id && rs.Curriculum_Id == r.Curriculum_Id).ToList();
                int count = find_list.Count;
                if (count <= 0)
                {
                    this.Insert(r);
                    s = true;
                   // Reconcile_Com.redisCache.RemoveCache("ReconcileList");
                }
                else
                {
                    s = true;
                }
            }
            catch (Exception ex)
            {
                string e = ex.Message;
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
                Reconcile find_r = AllReconcile().Where(f => f.Id == r.Id).FirstOrDefault();
                find_r.AnPaiDate = r.AnPaiDate;//日期                
                a.Success = true;
                //Reconcile_Com.redisCache.RemoveCache("ReconcileList");
            }
            catch (Exception ex)
            {
                a.Msg = ex.Message;
                a.Success = false;
            }

            return a;
        }
        /// <summary>
        /// 修改数据（true--成功，false--失败）
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public AjaxResult Updata_data(ReconView r)
        {
            AjaxResult a = new AjaxResult();
            Reconcile find_r = this.GetEntity(r.id);
            try
            {
                //除了专业课之外的改为自习课
                if (find_r.Curse_Id.Contains("12") || find_r.Curse_Id.Contains("34"))
                {
                    find_r.Curriculum_Id = "自习";
                    find_r.EmployeesInfo_Id = null;
                    find_r.Rmark = r.ramak;
                    this.Update(find_r);
                    //清空缓存
                    //Reconcile_Com.redisCache.RemoveCache("ReconcileList");
                    a.Success = true;
                    a.Msg = "编辑成功";
                }
                else
                {
                    if (r.curdname == "上午四节" || r.curdname == "下午四节")
                    {
                        //直接改为自习
                        find_r.Curriculum_Id = "自习";
                        find_r.EmployeesInfo_Id = null;
                        find_r.Rmark = r.ramak;
                        this.Update(find_r);
                        //清空缓存
                        //Reconcile_Com.redisCache.RemoveCache("ReconcileList");
                        a.Success = true;
                        a.Msg = "编辑成功";
                    }
                    else
                    {
                        //添加数据
                        Reconcile add = new Reconcile();
                        add.AnPaiDate = find_r.AnPaiDate;
                        add.ClassRoom_Id = find_r.ClassRoom_Id;
                        add.ClassSchedule_Id = find_r.ClassSchedule_Id;

                        if (r.curdname == "上午12节")
                        {
                            find_r.Curse_Id = "上午34节";
                        }
                        else if (r.curdname == "下午12节")
                        {
                            find_r.Curse_Id = "下午34节";
                        }
                        else if (r.curdname == "上午34节")
                        {
                            find_r.Curse_Id = "上午12节";
                        }
                        else if (r.curdname == "下午34节")
                        {
                            find_r.Curse_Id = "下午12节";
                        }
                        add.EmployeesInfo_Id = null;
                        add.IsDelete = false;
                        add.NewDate = DateTime.Now;
                        add.Curse_Id = r.curdname;
                        add.Curriculum_Id = "自习";
                        bool s = this.AddData(add);
                        if (s)
                        {
                            this.Update(find_r);

                            //清空缓存
                            //Reconcile_Com.redisCache.RemoveCache("ReconcileList");
                            a.Success = true;
                            a.Msg = "编辑成功";
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                a.Msg = ex.Message;
                a.Success = false;
            }
            return a;
        }

        public AjaxResult Update_data2(List<Reconcile> r, string timename)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                foreach (Reconcile item in r)
                {
                    if (item.Curse_Id == "下午" || item.Curse_Id == "上午")
                    {
                        item.Curse_Id = timename;
                    }
                    else
                    {
                        string tr = item.Curse_Id.Substring(2, 2);
                        item.Curse_Id = timename == "上午" ? "下午" + tr + "节" : "上午" + tr + "节";
                    }


                    this.Update(item);
                }
                //Reconcile_Com.redisCache.RemoveCache("ReconcileList");
                a.Success = true;
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = ex.Message;

            }

            return a;
        }

        public AjaxResult Update_date3(List<Reconcile> r, string emp)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                foreach (Reconcile item in r)
                {
                    item.EmployeesInfo_Id = emp;
                    this.Update(item);
                }
                a.Success = true;
                //Reconcile_Com.redisCache.RemoveCache("ReconcileList");
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = ex.Message;
            }
            return a;
        }

        public AjaxResult update_date4(List<Reconcile> r, DateTime time)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                foreach (Reconcile em in r)
                {
                    em.AnPaiDate = time;
                    this.Update(em);
                }
                a.Success = true;
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = ex.Message;
            }
            return a;

        }


        /// <summary>
        /// 判断日期是否可以排课（true--可以排课，false--不可以排课）
        /// </summary>
        /// <param name="xmlfile">xml路径</param>
        /// <param name="time">日期</param>
        /// <returns></returns>
        public bool IsShangKe(string xmlfile, DateTime time)
        {
            bool s = true;
            GetYear yy = MyGetYear(time.Year.ToString(), xmlfile);
            if (time.Month >= yy.StartmonthName && time.Month <= yy.EndmonthName)
            {
                //单休
                //判断time是否是星期天
                int tt = IsSaturday(time);
                if (tt == 2)
                {
                    s = false;
                }
            }
            else
            {
                //双休
                int tt = IsSaturday(time);
                if (tt == 2 || tt == 1)
                {
                    s = false;
                }
            }
            return s;
        }
        //获取可以上职素的班主任集合
        public List<EmployeesInfo> GetMasTeacher(DateTime time, string timename)
        {
            List<EmployeesInfo> list = new List<EmployeesInfo>();

            //s1，s2,s3的班主任
            BaseBusiness<Headmaster> basemaster = new BaseBusiness<Headmaster>();
            //List<EmployeesInfo> elist = Reconcile_Com.GetAllNoGoingEMP();
            List<Headmaster> find_m = basemaster.GetList().Where(m => m.IsDelete == false && m.IsAttend == true).ToList();//获取没有辞职的可以上职素课的班主任
            foreach (Headmaster e1 in find_m)
            {
               EmployeesInfo findata= Reconcile_Com.Employees_Entity.GetEntity(e1.informatiees_Id);
                if (findata!=null)
                {
                    //判断这个班主任是否有课
                    bool s = IsHaveClass(findata.EmployeeId, timename, time);
                    if (s == false)
                    {
                        list.Add(findata);
                    }
                }
                //foreach (Headmaster m1 in find_m)
                //{
                //    if (m1.informatiees_Id == e1.EmployeeId)
                //    {
                //        //判断这个班主任是否有课
                //        bool s = IsHaveClass(e1.EmployeeId, timename, time);
                //        if (s == false)
                //        {
                //            list.Add(e1);
                //        }
                //    }
                //}
            }

            //s3，s4的
            List<EmployeesInfo> list_saff = Reconcile_Com.GetObtainTeacher();//获取未辞职的就业部老师
                                                                              
           // Department find_d = Reconcile_Com.GetDempt("s3教质部");
           // Position find_p = Reconcile_Com.GetPsit(find_d, "班主任");
           // List<EmployeesInfo> e_list = Reconcile_Com.Employees_Entity.GetList().Where(e => e.IsDel == false && e.PositionId == find_p.Pid).ToList();
            //List<Headmaster> head_list = Reconcile_Com.Headmaster_Etity.GetList().Where(h => h.IsAttend == true).ToList();
            //foreach (Headmaster i1 in head_list)
            //{
            //    foreach (EmployeesInfo i2 in e_list)
            //    {
            //        if (i1.informatiees_Id == i2.EmployeeId)
            //        {
            //            list_saff.Add(i2);
            //        }
            //    }
            //}

            foreach (EmployeesInfo item in list_saff)
            {
                bool s = IsHaveClass(item.EmployeeId, timename, time);
                if (s == false)
                {
                    list.Add(item);//获取就业部空闲的老师
                }
            }



            return list;
        }
        //获取教官
        public List<EmployeesInfo> GetSir(DateTime time, string timename)
        {
            DepartmentManage department = new DepartmentManage();
            BaseBusiness<Position> position = new BaseBusiness<Position>();
            List<EmployeesInfo> employees = Reconcile_Com.Employees_Entity.GetList().Where(e => e.IsDel == false).ToList();
            List<EmployeesInfo> em = new List<EmployeesInfo>();
            List<EmployeesInfo> em2 = new List<EmployeesInfo>();

            //S1，S2教官
            Department find_d1 = department.GetList().Where(d => d.DeptName == "s1、s2教质部").FirstOrDefault();
            Position p1 = position.GetList().Where(p => p.PositionName == "教官" && p.DeptId == find_d1.DeptId).FirstOrDefault();
            em.AddRange(employees.Where(c => c.PositionId == p1.Pid).ToList());

            //S3,S4教官
            Department find_d2 = department.GetList().Where(d => d.DeptName == "s3教质部").FirstOrDefault();
            Position p2 = position.GetList().Where(p => p.PositionName == "教官" && p.DeptId == find_d2.DeptId).FirstOrDefault();
            em.AddRange(employees.Where(c => c.PositionId == p2.Pid).ToList());

            //判断教官是否在这个时间段有课
            foreach (EmployeesInfo ee in em)
            {
                bool s = IsHaveClass(ee.EmployeeId, timename, time);
                if (s == false)
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

            return list_Teacher.Where(c => c.IsDel == false).ToList();
        }
        /// <summary>
        /// 安排英语老师上课
        /// </summary>
        /// <param name="grand_id">阶段Id</param>
        /// <param name="curr_name">课程名称</param>
        /// <param name="timename">时间段</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public string AnpaiTeacher(int grand_id, string curr_name, string timename, DateTime date)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            List<Teacher> gettea = GetListTeacher(grand_id, curr_name);//获取英语老师
            foreach (Teacher tt in gettea)
            {
                bool s = IsHaveClass(tt.EmployeeId, timename, date);
                if (s == false)
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
        /// 获取所有阶段集合
        /// </summary>
        /// <returns></returns>
        public List<Grand> GetEffectiveData()
        {
            List<Grand> grands = Reconcile_Com.Grand_Entity.GetList().Where(g => g.IsDelete == false).ToList();
            return grands;
        }
        /// <summary>
        /// 获取属于某个阶段的有效班级集合
        /// </summary>
        /// <param name="grand_id"></param>
        /// <returns></returns>
        public List<ClassSchedule> GetGrandClass(int grand_id)
        {
            //获取有效的班级数据//获取属于某个阶段的班级
            List<ClassSchedule> c_list = Reconcile_Com.ClassSchedule_Entity.GetList().Where(c => c.ClassStatus == false && c.IsDelete == false && c.grade_Id == grand_id && c.ClassstatusID == null).ToList();
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
        public string GetMasterTeacher(int class_id, DateTime time, string timename)
        {
            //获取该班级的班主任
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            AjaxResult find_a = Reconcile_Com.GetZhisuTeacher(class_id);
            int count = 0;
            if (find_a.Success == true)
            {
                EmployeesInfo find_ff = find_a.Data as EmployeesInfo;
                AjaxResult find_class = Reconcile_Com.GetHadMasterClass(find_ff.EmployeeId);
                if (find_class.Success == true)
                {
                    List<HeadClass> headClasses = find_class.Data as List<HeadClass>;
                    foreach (HeadClass c in headClasses)
                    {
                        //判断是否有带的班级已在这时间段安排了班会课
                        count = count + AllReconcile().Where(r => r.AnPaiDate == time && r.Curse_Id == timename && r.EmployeesInfo_Id == find_a.Data.ToString() && r.ClassSchedule_Id == c.ClassID).ToList().Count;
                    }
                }
                if (count <= 0)
                {
                    sb.Append(find_ff.EmployeeId);
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
                s = Convert.ToBoolean(fin_c.IsEndCurr);
            }
            return s;
        }
        /// <summary>
        /// 大批量添加数据
        /// </summary>
        /// <param name="new_list">排课数据集合</param>
        /// <returns></returns>
        public AjaxResult Inser_list(List<Reconcile> new_list)
        {
            AjaxResult s = new AjaxResult();
            List<Reconcile> list = new List<Reconcile>();
            int orrindex = 0;
            try
            {
                foreach (Reconcile item in new_list)
                {
                    //判断是否有重复的值
                    bool ss = IsExcit(item, false);
                    if (ss)
                    {
                        orrindex++;
                    }
                    else
                    {
                        list.Add(item);
                        
                    }
                   
                }
                if (orrindex != 0)
                {
                    s.Msg = "已有重复数据" + orrindex + "条";
                }
                else
                {
                    s.Msg = "无重复数据，已安排成功";
                }
                this.Insert(list);
                s.Success = true;
                //Reconcile_Com.redisCache.RemoveCache("ReconcileList");
            }
            catch (Exception ex)
            {
                s.Success = false;
                s.Msg = ex.Message;
            }
            return s;
        }
        /// <summary>
        /// 是否有重复的数据--用于大批量添加数据 (false--没有重复，true--重复)
        /// </summary>
        /// <param name="r"></param>
        /// <param name="singledata">true--单条数据判断,false--大批量数据判断</param>
        /// <returns></returns>
        public bool IsExcit(Reconcile r, bool singledata)
        {
            bool s = false;
            if (singledata)
            {
                //单条数据判断
                Reconcile find_r = AllReconcile().Where(rs => rs.ClassSchedule_Id == r.ClassSchedule_Id && rs.Curriculum_Id == r.Curriculum_Id && rs.AnPaiDate == r.AnPaiDate && rs.Curse_Id == r.Curse_Id).FirstOrDefault();
                if (find_r != null)
                {
                    s = true;
                }
            }
            else
            {
                //集合数据判断
                Reconcile find_r = AllReconcile().Where(rs => rs.Curriculum_Id == r.Curriculum_Id && rs.AnPaiDate == r.AnPaiDate && rs.ClassSchedule_Id == r.ClassSchedule_Id).FirstOrDefault();
                if (find_r != null)
                {
                    s = true;
                }
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
        public bool ToHavaKe(int class_id, DateTime time, string currname)
        {
            bool s = false;
            int count = AllReconcile().Where(r => r.ClassSchedule_Id == class_id && r.AnPaiDate == time && r.Curriculum_Id == currname).ToList().Count;
            if (count > 0)
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
            int count = AllReconcile().Where(r => r.AnPaiDate >= time.StarTime && r.AnPaiDate <= time.EndTime && r.ClassSchedule_Id == ClassName && r.Curriculum_Id == CurrName).ToList().Count;
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
        public bool ExistenceToday(DateTime? time, int class_id, string currname)
        {
            bool s = false;
            int count = AllReconcile().Where(r => r.ClassSchedule_Id == class_id && r.AnPaiDate == time && r.Curriculum_Id == currname).ToList().Count;
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
        public string RoomTime(int c_id, DateTime d, int? base_id)
        {
            BaseDataEnum find_b = Reconcile_Com.ClassSchedule_Entity.BaseDataEnum_Entity.GetEntity(base_id);
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            List<string> str = new List<string>();
            if (find_b.Name == "上午")
            {
                str.Add("下午12节");
                str.Add("下午34节");
            }
            else
            {
                str.Add("上午12节");
                str.Add("上午34节");
            }
            List<Reconcile> reconciles = AllReconcile().Where(r => r.AnPaiDate == d && r.ClassRoom_Id == c_id).ToList();
            for (int i = 0; i < reconciles.Count; i++)
            {
                for (int j = 0; j < str.Count; j++)
                {
                    if (reconciles[i].Curse_Id == "上午")
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
            if (str.Count > 0)
            {
                Random rs = new Random();
                int num = rs.Next(0, str.Count);
                sb.Append(str[num]);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取空教室
        /// </summary>
        /// <param name="TimeName">'上午'--获取下午的空教室'下午'--获取上午的空教室</param>
        /// <param name="time">获取哪个日期里的空教室</param>
        /// <returns></returns>
        public List<Classroom> GetClassrooms(string TimeName, DateTime time)
        {
            List<Classroom> c_list = Reconcile_Com.Classroom_Entity.GetEffectiveClass();
            if (TimeName == "晚自习" || TimeName == "晚一" || TimeName == "晚二")
            {
                List<Reconcile> reconciles = AllReconcile().Where(r => (r.Curse_Id == "晚自习" || r.Curse_Id == "晚一" || r.Curse_Id == "晚二") && r.AnPaiDate == time).ToList();
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
                if (TimeName.Contains("上午"))
                {
                    reconciles = AllReconcile().Where(r => (r.Curse_Id == TimeName || r.Curse_Id == "上午") && r.AnPaiDate == time).ToList();
                }
                else if (TimeName.Contains("下午"))
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
        /// 判断这个班级在这个日期中的时间段是否有课(false--没有，true--有)
        /// </summary>
        /// <param name="class_id">班级编号</param>
        /// <param name="timename">时间段</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public bool IshavaOrther(int class_id, string timename, DateTime time)
        {
            bool s = false;
            int count = AllReconcile().Where(r => r.ClassSchedule_Id == class_id && r.AnPaiDate == time && r.Curse_Id == timename).ToList().Count;
            if (count > 0)
            {
                s = true;
            }
            return s;
        }
        
      
        /// <summary>
        ///  判断XX班级在这期间是否上过XX课程(false--没有，ture--有)
        /// </summary>
        /// <param name="startime">开始日期</param>
        /// <param name="endtime">结束日期</param>
        /// <param name="currName">课程名称</param>
        /// <param name="class_id">班级名称</param>
        /// <returns></returns>
        public bool IsHaveCurr(DateTime startime, DateTime endtime, string currName, int class_id)
        {
            bool s = false;
            int count = AllReconcile().Where(r => r.ClassSchedule_Id == class_id && r.Curriculum_Id == currName && r.AnPaiDate >= startime && r.AnPaiDate <= endtime).ToList().Count();
            if (count > 0)
            {
                s = true;
            }
            return s;
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
            Position p = basePosition.GetIQueryable().Where(b => b.IsDel == false && b.PositionName == PosionName).FirstOrDefault();
            if (p != null)
            {
                return baseEmplo.GetIQueryable().Where(b => b.IsDel == false && b.PositionId == p.Pid).ToList();//根据岗位匹配未辞职的老师
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
                Reconcile find_r = AllReconcile().Where(r => r.Id == r_id).FirstOrDefault();
                this.Delete(find_r);
                a.Success = true;
                a.Msg = "操作成功";
                //Reconcile_Com.redisCache.RemoveCache("ReconcileList");
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = ex.Message;
            }
            return a;
        }

        /// <summary>
        /// 获取单个语数英老师
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="currname">课程</param>
        /// <param name="timename">时间段</param>
        /// <returns></returns>
        public string GetNomaginTeacher(DateTime time, string currname, string timename, int grand_id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            switch (currname)
            {
                case "语文":
                    List<EmployeesInfo> list_e = GetEmployees("语文老师");
                    if (list_e.Count > 0)
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
                    sb.Append(AnpaiTeacher(grand_id, currname, timename, time));
                    break;
            }
            //查看这个老师这一天是否有课
            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                int count = AllReconcile().Where(r => r.EmployeesInfo_Id == sb.ToString() && r.AnPaiDate == time && r.Curse_Id == timename).ToList().Count;
                if (count > 0)
                {
                    sb.Append("");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取任课老师集合
        /// </summary>
        /// <param name="cur_id">课程Id</param>
        /// <param name="timename">时间段</param>
        /// <param name="time">日期</param>
        /// <returns></returns>
        public List<EmployeesInfo> GetMarjorTeacher(int cur_id, string timename, DateTime time)
        {
            //获取可以上这个课程的老师
            List<Teacher> g_list = Reconcile_Com.GoodSkill_Entity.GetTeachers(cur_id);
            //判断这个老师在XX日期XX时间段是否有课
            List<EmployeesInfo> e_list = Reconcile_Com.Employees_Entity.GetList();
            List<EmployeesInfo> emp = new List<EmployeesInfo>();
            foreach (EmployeesInfo item1 in e_list)
            {
                foreach (Teacher item2 in g_list)
                {
                    if (item1.EmployeeId == item2.EmployeeId)
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
        /// <summary>
        /// 全体大批量的调课
        /// </summary>
        /// <param name="date">开始日期</param>
        /// <param name="days">天数</param>
        /// <param name="s1ors3">教务（true--S1教务,false-S3教务）</param>
        /// <returns></returns>
        public bool AidAllData(DateTime date, int days)
        {
            bool s = false;
            int index = 0;
            try
            {
                List<Reconcile> list = new List<Reconcile>();
                List<Reconcile> reconciles = AllReconcile().Where(r => r.AnPaiDate >= date).ToList();

                foreach (Reconcile re in list)
                {
                    re.AnPaiDate = re.AnPaiDate.AddDays(days);
                    this.Update(re);
                    index++;
                }
                //清空缓存
                //Reconcile_Com.redisCache.RemoveCache("ReconcileList");
                if (index == list.Count)
                {
                    s = true;
                }
            }
            catch (Exception)
            {

                s = false;
            }

            return s;

        }
        /// <summary>
        /// 班级大批量的调课
        /// </summary>
        /// <param name="date">开始</param>
        /// <param name="days"></param>
        /// <param name="class_id"></param>
        /// <returns></returns>
        public bool AidClassData(DateTime date, int days, int class_id)
        {
            bool s = false;
            int index = 0;
            try
            {
                List<Reconcile> reconciles = AllReconcile().Where(r => r.AnPaiDate >= date && r.ClassSchedule_Id == class_id).ToList();
                foreach (Reconcile re in reconciles)
                {
                    re.AnPaiDate = re.AnPaiDate.AddDays(days);
                    this.Update(re);
                    index++;
                }
                //清空缓存
                //Reconcile_Com.redisCache.RemoveCache("ReconcileList");
                if (index == reconciles.Count)
                {
                    s = true;
                }
            }
            catch (Exception)
            {
                s = false;
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
        public AjaxResult Daike(DateTime dateTime, string emp_id, int class_id)
        {
            AjaxResult s = new AjaxResult();
            try
            {
                Reconcile find_r = AllReconcile().Where(r => r.AnPaiDate == dateTime && r.ClassSchedule_Id == class_id).FirstOrDefault();
                find_r.EmployeesInfo_Id = emp_id;
                this.Update(find_r);
                s.Msg = "代课成功";
                s.Success = true;
            }
            catch (Exception ex)
            {
                s.Msg = ex.Message;
                s.Success = false;
            }
            return s;
        }
        /// <summary>
        /// 加课
        /// </summary>
        /// <param name="dateTime">开始加课日期</param>
        /// <param name="emp_id">加课老师</param>
        /// <param name="timename">时间段(上午，下午，晚自习)</param>
        /// <param name="class_id">班级Id</param>
        /// <param name="curr_name">课程名称</param>
        /// <param name="addcount">加课次数</param>
        /// <param name="XmlFile">Server.MapPath("~/Xmlconfigure/Reconcile_XML.xml")</param>
        /// <returns></returns>
        public AjaxResult Addke(DateTime dateTime, string emp_id, string timename, int class_id, string curr_name, int addcount, string XmlFile)
        {
            AjaxResult data = new AjaxResult(); data.Success = false;
            int COU = 0;
            bool Schooladdress = Reconcile_Com.judgeClass(class_id);
            //判断是否是晚自习
            if (timename.Contains("晚自习"))
            {
                List<EvningSelfStudy> e_list = new List<EvningSelfStudy>();
                for (int i = 0; i < addcount; i++)
                {
                    if (i == 0)
                    {
                        dateTime = dateTime.AddDays(0);
                    }
                    else
                    {
                        dateTime = dateTime.AddDays(1);
                    }

                    //判断这个时间是否可以加课
                    GetYear getYear = MyGetYear(dateTime.Year.ToString(), XmlFile);
                    if (dateTime.Month >= getYear.StartmonthName && dateTime.Month <= getYear.EndmonthName)
                    {
                        //单休
                        if (IsSaturday(dateTime) == 2)
                        {
                            dateTime = dateTime.AddDays(1);
                        }
                    }
                    else
                    {
                        //双休
                        if (IsSaturday(dateTime) == 1)
                        {
                            dateTime = dateTime.AddDays(2);
                        }
                    }
                    //获取这个班级在哪里上晚自习
                    EvningSelfStudy find_eving = EvningSelfStudy_Entity.GetNving(dateTime, class_id);
                    if (find_eving == null)//如果还没有排晚自习
                    {
                        //获取空教室
                        ClassRoom_AddCourse find_emptyone = EvningSelfStudy_Entity.GetEmptyClassroom(dateTime, Schooladdress);
                        EvningSelfStudy new_eone = new EvningSelfStudy();
                        new_eone.Anpaidate = dateTime;
                        new_eone.Classroom_id = find_emptyone.ClassRoomId;
                        new_eone.ClassSchedule_id = class_id;
                        new_eone.curd_name = "晚一";
                        new_eone.emp_id = emp_id;
                        new_eone.IsDelete = false;
                        new_eone.Newdate = DateTime.Now;
                        bool e1 = EvningSelfStudy_Entity.Add_Data(new_eone).Success;
                        EvningSelfStudy new_eTwo = new EvningSelfStudy();
                        new_eTwo.Anpaidate = dateTime;
                        new_eTwo.Classroom_id = find_emptyone.ClassRoomId;
                        new_eTwo.ClassSchedule_id = class_id;
                        new_eTwo.curd_name = "晚二";
                        new_eTwo.emp_id = emp_id;
                        new_eTwo.IsDelete = false;
                        new_eTwo.Newdate = DateTime.Now;
                        bool e2 = EvningSelfStudy_Entity.Add_Data(new_eTwo).Success;
                        if (e1 == true && e2 == true)
                        {
                            COU++;
                            data.Success = true;
                            data.Msg = "加课成功";
                        }
                        else
                        {
                            data.Success = false;
                            data.Msg = "添加失败";
                        }
                    }
                    else
                    {
                        //修改晚自习排课信息
                        find_eving.emp_id = emp_id;
                        bool s1 = EvningSelfStudy_Entity.Update_DataTwo(find_eving).Success;
                        //获取这个教室的另外一个班级
                        EvningSelfStudy find_eving2 = EvningSelfStudy_Entity.GetOnCurrClass(dateTime, find_eving.Classroom_id).Where(e => e.ClassSchedule_id != class_id).FirstOrDefault();

                        //获取这一天晚自习的空教室
                        ClassRoom_AddCourse find_empty = EvningSelfStudy_Entity.GetEmptyClassroom(dateTime, Schooladdress);
                        find_eving2.Classroom_id = find_empty.ClassRoomId;
                        find_eving2.curd_name = find_empty.TimeName;
                        //修改另外一个表
                        bool s2 = EvningSelfStudy_Entity.Update_DataTwo(find_eving2).Success;
                        //添加一条数据
                        EvningSelfStudy new_e = new EvningSelfStudy();
                        new_e.Anpaidate = dateTime;
                        new_e.Classroom_id = find_eving.Classroom_id;
                        new_e.ClassSchedule_id = find_eving.ClassSchedule_id;
                        new_e.curd_name = find_eving.curd_name == "晚一" ? "晚二" : "晚一";
                        new_e.emp_id = emp_id;
                        new_e.IsDelete = false;
                        new_e.Newdate = DateTime.Now;
                        bool s3 = EvningSelfStudy_Entity.Add_Data(new_e).Success;

                        if (s1 == true && s2 == true && s3 == true)
                        {
                            COU++;
                        }

                    }


                }
                if (COU == addcount)
                {
                    data.Success = true;
                }
            }
            else
            {
                int yes = 0;
                try
                {
                    List<Reconcile> find_list = AllReconcile().Where(r => r.ClassSchedule_Id == class_id && r.AnPaiDate > dateTime).ToList();
                    foreach (Reconcile item in find_list)
                    {
                        item.AnPaiDate = item.AnPaiDate.AddDays(addcount);
                        this.Update(item);
                    }
                    //清空缓存
                    //Reconcile_Com.redisCache.RemoveCache("ReconcileList");
                    for (int i = 1; i <= addcount; i++)
                    {
                        Reconcile r = new Reconcile();
                        r.AnPaiDate = dateTime.AddDays(i);
                        r.ClassRoom_Id = find_list[0].ClassRoom_Id;
                        r.ClassSchedule_Id = class_id;
                        r.Curriculum_Id = curr_name;
                        r.Curse_Id = timename;
                        r.EmployeesInfo_Id = emp_id;
                        r.IsDelete = false;
                        r.NewDate = DateTime.Now;
                        bool s = this.AddData(r);
                        if (s == false)
                        {
                            break;
                        }
                        yes++;
                    }

                    if (yes == addcount)
                    {
                        data.Success = true;
                    }
                    else
                    {
                        data.Success = false;
                    }
                }
                catch (Exception ex)
                {

                    data.Success = false;
                    data.Msg = ex.Message;
                }


            }
            return data;
        }
        #endregion


        #region
        /// <summary>
        /// 获取这个日期的课表
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<Reconcile> GetReconciles(DateTime time)
        {
            return GetReconcileDate(time,false).Where(a => a.AnPaiDate == time).ToList();
        }

        /// <summary>
        /// 获取某校区指定日期的空教室
        /// </summary>
        /// <param name="timename">上课时间</param>
        /// <param name="time">日期</param>
        /// <param name="schooladdree">校区地址编号</param>
        /// <returns></returns>
        //public List<Classroom> GetSchoolEmptyRoomFunction(string timename, DateTime time, int schooladdree)
        //{
        //    List<Classroom> classrooms_data = Reconcile_Com.Classroom_Entity.GetAddreeClassRoom(schooladdree);//获取某校区所有教室

        //    List<Reconcile> find_reconcile_data = this.AllReconcile().Where(ll => ll.AnPaiDate == time && ll.Curse_Id.Contains(timename)).ToList();  //获取这个日期的排课数据

        //    for (int i = 0; i < find_reconcile_data.Count; i++) //获取空教室
        //    {
        //        Classroom find_over = classrooms_data.Where(c => c.Id == find_reconcile_data[i].ClassRoom_Id).FirstOrDefault();
        //        classrooms_data.Remove(find_over);
        //    }

        //    return classrooms_data;
        //}
        /// <summary>
        /// 高中生排课
        /// </summary>
        /// <param name="doublecease">true--双休，false--单休</param>
        /// <param name="curNo">课程编号</param>
        /// <param name="time">安排日期</param>
        /// <param name="timename">上课时间</param>
        /// <param name="classroomid">教室编号</param>
        /// <param name="empNo">老师员工编号</param>
        /// <param name="classNo">班级编号</param>
        /// <returns></returns>
        public List<Reconcile> HeghtStudentReconcileFunction(bool doublecease, int curNo, DateTime time, string timename, int classroomid, string empNo, int classNo,int grand,int marjion_id)
        {
            Curriculum find_curdata = Reconcile_Com.Curriculum_Entity.GetEntity(curNo);   //获取课程

            List<Curriculum> find_curr_list = this.GetCurr(grand, true, marjion_id).Where(c => c.Sort >= find_curdata.Sort).OrderBy(c=>c.Sort).ToList();

            List<Reconcile> r_list = new List<Reconcile>();

            foreach (Curriculum cur in find_curr_list)
            {
                int Sumcout = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cur.CourseCount / 4.0))); //获取总节数

                int index = 0;
                for (int i = 0; i < Sumcout; i++)
                {
                    index++;
                    if (doublecease == true) //双休
                    {
                        if (this.IsSaturday(time) == 1)
                        {
                            Sumcout = Sumcout + 2;
                            index = index + 2;
                            i = i + 2;
                            time = time.AddDays(2);
                        }

                    }
                    else //单休
                    {
                        if (this.IsSaturday(time) == 2)
                        {
                            time = time.AddDays(1);
                            Sumcout++;
                            index++;
                            i++;
                        }
                    }
                    Reconcile new_R = new Reconcile();
                    new_R.AnPaiDate = time;
                    new_R.Curse_Id = timename;
                    new_R.ClassRoom_Id = classroomid;
                    new_R.ClassSchedule_Id = classNo;
                    new_R.EmployeesInfo_Id = empNo;
                    new_R.NewDate = DateTime.Now;
                    new_R.IsDelete = false;
                    if (index == Sumcout)
                    {
                        if (this.IsEndCurr(cur.CourseName))
                        {
                            new_R.Curriculum_Id = "升学考试";
                            new_R.EmployeesInfo_Id = null;
                        }
                        else
                        {
                            new_R.Curriculum_Id = cur.CourseName + "考试";
                            new_R.EmployeesInfo_Id = null;
                        }

                    }
                    else
                    {
                        new_R.Curriculum_Id = cur.CourseName;
                    }
                    r_list.Add(new_R);
                    time = time.AddDays(1);

                    if (cur.IsEndCurr == true && index == Sumcout)
                    {
                        break;
                    }
                }
            }

            return r_list;
        }
        /// <summary>
        /// 初中生排课
        /// </summary>
        /// <param name="doublecease"></param>
        /// <param name="curNo"></param>
        /// <param name="time"></param>
        /// <param name="timename"></param>
        /// <param name="classroomid"></param>
        /// <param name="empNo"></param>
        /// <param name="classNo"></param>
        /// <param name="grandid"></param>
        /// <returns></returns>
        public List<Reconcile> MiddleStudentReconcileFunction(bool doublecease, int curNo, DateTime time, string timename, int classroomid, string empNo, int classNo, int grandid,string yuwen,string shuxue,string yingyu)
        {
            Curriculum find_curdata = Reconcile_Com.Curriculum_Entity.GetEntity(curNo);   //获取课程

            List<Curriculum> find_curr_list = Reconcile_Com.GetbehindCurri(find_curdata).OrderBy(c=>c.Sort).ToList();

            List<Reconcile> r_list = new List<Reconcile>();

            bool IsMargin = true;//true--安排专业课，false--安排语数英

            string[] strcurr = new string[] { "语文,数学", "数学,英语", "英语,语文" };

            string[] strtimename = new string[] { "上午12节,上午34节", "下午12节,下午34节" };

            foreach (Curriculum cur in find_curr_list)
            {
                int Sumcout = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cur.CourseCount / 4.0))); //获取总节数
                int nomargin = 0;
                for (int i = 0; i < Sumcout; i++)
                {

                    Reconcile r = new Reconcile();
                    //判断是否是单休
                    if (doublecease)
                    {
                        //双休
                        if (this.IsSaturday(time) == 1)
                        {
                            //如果是周六
                            // r.AnPaiDate = time.AddDays(i + 2);
                            time= time.AddDays(2);
                            i = i + 2;
                            Sumcout = Sumcout + 2;
                        }
                        //else
                        //{
                        //    // r.AnPaiDate = time.AddDays(i);
                        //    time = time.AddDays(1);
                        //}
                    }
                    else
                    {
                        //单休
                        if (this.IsSaturday(time) == 2)
                        {
                            //如果是周日
                            //r.AnPaiDate = time.AddDays(i + 1);
                            time= time.AddDays(1);
                            i++;
                            Sumcout++;
                        }
                        //else
                        //{
                        //    r.AnPaiDate = time.AddDays(i);
                        //}
                    }
                    r.AnPaiDate = time;
                    r.ClassRoom_Id = classroomid;
                    r.ClassSchedule_Id = classNo;

                    if (IsMargin == true)
                    {
                        r.EmployeesInfo_Id = empNo;
                        r.Curse_Id = timename;
                        r.Curriculum_Id = cur.CourseName;
                        r.Curse_Id = timename;
                        r.NewDate = DateTime.Now;
                        r.IsDelete = false;
                        r_list.Add(r);
                    }
                    else
                    {
                        Sumcout++;
                        string[] currname1 = strcurr[nomargin].Split(',');

                        //安排语数英
                        if (timename == "上午")
                        {
                            string[] timename1 = strtimename[0].Split(',');

                            for (int j = 0; j < timename1.Length; j++)
                            {
                                Reconcile cc1 = new Reconcile();
                                cc1.AnPaiDate = r.AnPaiDate;
                                cc1.ClassRoom_Id = classroomid;
                                cc1.ClassSchedule_Id = classNo;
                                cc1.Curriculum_Id = currname1[j];
                                cc1.Curse_Id = timename1[j];
                                cc1.NewDate = DateTime.Now;
                                cc1.IsDelete = false;
                                if (currname1[j] == "语文")
                                {
                                    cc1.EmployeesInfo_Id = yuwen == null ? null : yuwen;
                                }else if (currname1[j] == "数学")
                                {
                                    cc1.EmployeesInfo_Id = shuxue == null ? null : shuxue;
                                }
                                else if (currname1[j] == "英语")
                                {
                                    cc1.EmployeesInfo_Id = yingyu == null ? null : yingyu;
                                }                                                                 
                               r_list.Add(cc1);
                            }

                        }
                        else if (timename == "下午")
                        {
                            string[] timename1 = strtimename[1].Split(',');

                            for (int j = 0; j < timename1.Length; j++)
                            {
                                Reconcile cc1 = new Reconcile();
                                cc1.AnPaiDate = r.AnPaiDate;
                                cc1.ClassRoom_Id = classroomid;
                                cc1.ClassSchedule_Id = classNo;
                                cc1.Curriculum_Id = currname1[j];
                                cc1.Curse_Id = timename1[j];
                                cc1.NewDate = DateTime.Now;
                                cc1.IsDelete = false;
                                cc1.EmployeesInfo_Id = null;
                                if (currname1[j] == "语文")
                                {
                                    cc1.EmployeesInfo_Id = yuwen == null ? null : yuwen;
                                }
                                else if (currname1[j] == "数学")
                                {
                                    cc1.EmployeesInfo_Id = shuxue == null ? null : shuxue;
                                }
                                else if (currname1[j] == "英语")
                                {
                                    cc1.EmployeesInfo_Id = yingyu == null ? null : yingyu;
                                }
                                r_list.Add(cc1);
                            }
                        }

                        if (nomargin < 2)
                        {
                            nomargin++;//用于判断语数英
                        }
                        else
                        {
                            nomargin = 0;
                        }
                    }
                    IsMargin = IsMargin ? false : true;
                    time= time.AddDays(1);
                }

                if (cur.IsEndCurr == true)
                {
                    Reconcile mycc = new Reconcile();
                    mycc.AnPaiDate = time.AddDays(1);
                    mycc.ClassRoom_Id = classroomid;
                    mycc.ClassSchedule_Id = classNo;
                    mycc.Curriculum_Id = "升学考试";
                    mycc.Curse_Id = timename;
                    mycc.NewDate = DateTime.Now;
                    mycc.IsDelete = false;
                    r_list.Add(mycc);
                }
            }

            return r_list;
        }

        /// <summary>
        /// 获取阶段课程
        /// </summary>
        /// <param name="grand_id"></param>
        /// <param name="Marji_id"></param>
        /// <param name="whyMarjio">true---获取专业课程，false--获取非专业课程</param>
        /// <returns></returns>
        public List<Curriculum> GetCurr(int grand_id, bool whyMarjio, int marjoi_id)
        {
            int typeid = Reconcile_Com.CourseType_Entity.FindSingeData("专业课", false).Id;
            List<Curriculum> list = Reconcile_Com.Curriculum_Entity.GetIQueryable().Where(c => c.IsDelete == false).ToList(); //获取所有有效的课程           
            List<Curriculum> find_list = new List<Curriculum>();
            if (whyMarjio)//获取专业课程
            {
                list = list.Where(c =>c.CourseType_Id == typeid && c.Grand_Id == grand_id).ToList();
                //判断是否是Y1/S1
                GrandBusiness grand_entity = new GrandBusiness();
                int g_id1 = grand_entity.GetList().Where(g => g.GrandName.Equals("Y1", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().Id;
                int g_id2 = grand_entity.GetList().Where(g => g.GrandName.Equals("S1", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().Id;
                if (grand_id == g_id1 || grand_id == g_id2)
                {
                    find_list.AddRange(list);
                }
                else
                {
                    if (marjoi_id!=0)
                    {
                        find_list.AddRange(list.Where(c => c.MajorID == marjoi_id).ToList());//获取相关专业的课程
                    }
                     
                    find_list.AddRange(list.Where(c => c.MajorID == null).ToList());//获取公共课程
                }

            }
            else //获取非专业课程
            {
                list = list.Where(c => c.Grand_Id == grand_id || c.Grand_Id == null).ToList();
                List<Curriculum> find = list.Where(c => c.CourseType_Id != typeid && c.MajorID==null).OrderBy(c => c.CurriculumID).ToList();
                find_list.AddRange(find);

            }
            find_list = find_list.OrderBy(c => c.Sort).ToList();
            return find_list;
        }

        /// <summary>
        /// 获取可以上职素课的所有老师(包括就业部老师)
        /// </summary>
        /// <param name="MyIsAttend">true--获取可以上班会的老师，false--获取可以上职素课的老师</param>
        /// <returns></returns>
        public List<EmployeesInfo> GetMaster_All(bool MyIsAttend)
        {
            List<EmployeesInfo> list = new List<EmployeesInfo>();
            BaseBusiness<Headmaster> headmarster_Entity = new BaseBusiness<Headmaster>();
            List<EmployeesInfo> emplist = Reconcile_Com.Employees_Entity.GetIQueryable().ToList();//获取所有员工
            List<Headmaster> headmasterlist = new List<Headmaster>();
            if (MyIsAttend)//获取可以上班会课的老师
            {
                headmasterlist = headmarster_Entity.GetIQueryable().Where(h => h.IsDelete == false).ToList(); //获取所有在职的可以上职素课的班主任
            }
            else
            {
                headmasterlist = headmarster_Entity.GetIQueryable().Where(h => h.IsDelete == false && h.IsAttend == true).ToList(); //获取所有在职的可以上职素课的班主任
            }
             
            
            foreach (Headmaster h in headmasterlist)
            {
                list.Add(emplist.Where(e => e.EmployeeId == h.informatiees_Id).FirstOrDefault());
            }
            list.AddRange( Reconcile_Com.GetObtainTeacher());//获取就业部人员
            return list;
        }
        /// <summary>
        /// 获取所有的教官
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetInstructorAll()
        {
            List<Position> plist= Reconcile_Com.PositionBusiness.GetIQueryable().Where(p => p.PositionName.Contains("教官")).ToList();//获取教官岗位Id
            List<EmployeesInfo> emplist= Reconcile_Com.Employees_Entity.GetIQueryable().ToList();
            List<EmployeesInfo> list = new List<EmployeesInfo>();
            foreach (Position item in plist)
            {
               list.AddRange( emplist.Where(e => e.PositionId == item.Pid).ToList());
            }

            return list;
        }
        /// <summary>
        /// 获取所有的英语老师
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetEnglishTeacherAll()
        {
           int pid= Reconcile_Com.PositionBusiness.GetIQueryable().Where(p => p.PositionName.Contains("英语老师")).FirstOrDefault().Pid;
           return  Reconcile_Com.Employees_Entity.GetEmpByPid(pid).Where(e=>e.IsDel==false).ToList();
        }
       
        public int Days(DateTime time)
        {
            var day = time.DayOfWeek;
            switch (day)
            {
                case DayOfWeek.Friday://星期五
                    return 5;
                case DayOfWeek.Monday://星期一
                    return 1;
                case DayOfWeek.Thursday://星期四
                    return 4;
                case DayOfWeek.Tuesday:
                    return 2;//星期二
                case DayOfWeek.Wednesday:
                    return 3;//星期三
            }
            return 0;
        }

        public List<Reconcile> AnPaiNoMarginFuntion(DateTime time,int class_id,int classroom_id,string emp_id,string timename,int curtype_id,int grand_id,int days)
        {
            List<Reconcile> list = new List<Reconcile>();
            Curriculum find_cur = Reconcile_Com.Curriculum_Entity.GetIQueryable().Where(c => c.CourseType_Id == curtype_id && c.Grand_Id == grand_id).FirstOrDefault(); //根据课程类型/阶段获取课程编号
            if (find_cur!=null)
            {
                for (int i = 0; i < find_cur.CourseCount; i++)
                {
                    Reconcile new_r = new Reconcile();
                    new_r.ClassRoom_Id = classroom_id;
                    new_r.ClassSchedule_Id = class_id;
                    new_r.Curriculum_Id = find_cur.CourseName;
                    new_r.Curse_Id = timename;
                    new_r.EmployeesInfo_Id = emp_id;
                    new_r.IsDelete = false;
                    new_r.NewDate = DateTime.Now;
                    if (find_cur.CourseName.Contains("军事")) //两周一次
                    {
                        if (i == 0)
                        {
                            while (Days(time) != days)
                            {
                                time = time.AddDays(1);
                            }
                        }
                        else
                        {
                            time = time.AddDays(21);
                        }
                        new_r.AnPaiDate = time;
                    }
                    else if (find_cur.CourseName.Contains("班会") || find_cur.CourseName.Contains("英语"))//一周一次 
                    {
                        if (i == 0)
                        {
                            while (Days(time) != days)
                            {
                                time = time.AddDays(1);
                            }
                        }
                        else
                        {
                            time = time.AddDays(14);
                        }
                        new_r.AnPaiDate = time;
                    }
                    else if (find_cur.CourseName.Contains("职素"))//高中--两周一次，初中一周一次
                    {
                        Grand find_g = Reconcile_Com.Grand_Entity.GetEntity(grand_id);
                        if (find_g.GrandName.Equals("Y1", StringComparison.CurrentCultureIgnoreCase))
                        {
                            //初中
                            if (i == 0)
                            {
                                while (Days(time) != days)
                                {
                                    time = time.AddDays(1);
                                }
                            }
                            else
                            {
                                time = time.AddDays(14);
                            }
                            new_r.AnPaiDate = time;
                        }
                        else
                        {
                            //高中
                            if (i == 0)
                            {
                                while (Days(time) != days)
                                {
                                    time = time.AddDays(1);
                                }
                            }
                            else
                            {
                                time = time.AddDays(21);
                            }
                            new_r.AnPaiDate = time;
                        }
                    }
                    list.Add(new_r);
                }
            }
                      
            return list;
        }
       /// <summary>
       /// 获取所有在职的专业课老师
       /// </summary>
       /// <returns></returns>
        public List<EmployeesInfo> GetTeacherAll()
        {
            TeacherBusiness teacher_Entity = new TeacherBusiness();
            List<EmployeesInfo> list = new List<EmployeesInfo>();
             List<Teacher> tt_list= Reconcile_Com.Teacher_Entity.GetTeachers();
             List<EmployeesInfo> ee_list =  Reconcile_Com.Employees_Entity.GetIQueryable().ToList();
            foreach (Teacher t in tt_list)
            {
                list.Add( ee_list.Where(e => e.EmployeeId == t.EmployeeId).FirstOrDefault());
            }

            return list;
        }
       

        public AjaxResult UpdateSingleData(Reconcile reconcile)
        {
            StringBuilder ab = new StringBuilder();
            
            AjaxResult a = new AjaxResult();
            try
            {
                List<Reconcile> findlist= this.AllReconcile().Where(r => r.ClassRoom_Id == reconcile.ClassRoom_Id && r.Curse_Id == reconcile.Curse_Id && r.ClassSchedule_Id!=reconcile.ClassSchedule_Id).ToList();
                if (findlist.Count>0)
                {
                    for (int i=0;i<findlist.Count;i++)
                    {
                        ClassSchedule findclass= Reconcile_Com.ClassSchedule_Entity.GetEntity(findlist[i].ClassSchedule_Id);
                        if (i==(findlist.Count-1))
                        {
                            ab.Append(findclass.ClassNumber);
                        }
                        else
                        {
                            ab.Append(findclass.ClassNumber + "、");
                        }
                         
                    }
                    ab.Append("班级已在这个教室安排了课程,请注意查看!!!");
                    a.Msg = ab.ToString();
                }
                else
                {
                    a.Msg = "没有冲突数据,修改成功!!";
                }
                a.Success = true;
                this.Update(reconcile);
                //清空缓存
                //Reconcile_Com.redisCache.RemoveCache("ReconcileList");
            }
            catch (Exception)
            {

                a.Success = false;
                a.Msg = "修改数据有误！，请刷新重试！！";
            }

            return a;
        }

        /// <summary>
        /// 获取某个日期的排课数据
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="equsls">true--获取大于并等于该日期的数据，false--获取等于该日期的数据</param>
        /// <returns></returns>
        public List<Reconcile> GetReconcileDate(DateTime date,bool equsls)
        {
            string dd = date.Year + "-" + date.Month + "-" + date.Day;
            if (equsls)
            {
                //大于等于该日期的数据
                List<Reconcile> list = this.GetListBySql<Reconcile>("select * from Reconcile where AnPaiDate>='"+dd+"'");
                return list;
            }
            else
            {
                //等于该日期的数据
                List<Reconcile> list = this.GetListBySql<Reconcile>("select * from Reconcile where AnPaiDate='" + dd+"'");
                return list;
            }
            
        }
        
        /// <summary>
        /// 通过sql语句获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<Reconcile> SQLGetReconcileDate()
        {
            List<Reconcile> list = this.GetListBySql<Reconcile>("select * from Reconcile");

            return list;
        }
        
        #endregion
    }
}

