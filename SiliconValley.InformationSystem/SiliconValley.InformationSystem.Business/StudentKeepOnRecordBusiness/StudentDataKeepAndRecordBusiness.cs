using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.StuSatae_Maneger;
using SiliconValley.InformationSystem.Business.Psychro;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.RegionManage;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System.Data.SqlClient;
using System.Data;
using SiliconValley.InformationSystem.Business.StuInfomationType_Maneger;
using SiliconValley.InformationSystem.Business.DepartmentBusiness;

namespace SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness
{
    public class StudentDataKeepAndRecordBusiness : BaseBusiness<StudentPutOnRecord>
    {
        StuStateManeger Statu_Entity;
        SchoolYearPlanBusiness Syb_Entity = new SchoolYearPlanBusiness();
        RedisCache redisCache;
        //创建一个用于查询数据的员工信息实体
        public EmployeesInfoManage Enplo_Entity = new EmployeesInfoManage();
        //创建一个用于查询区域的实体
        RegionManeges region_Entity;

        /// <summary>
        /// 获取所有备案数据
        /// </summary>
        /// <returns></returns>
        public List<StudentPutOnRecord> GetAllStudentKeepData()
        {
            redisCache = new RedisCache();
            //redisCache.RemoveCache("StudentKeepList");
            List<StudentPutOnRecord> get_studentkeep_list = new List<StudentPutOnRecord>();
            get_studentkeep_list = redisCache.GetCache<List<StudentPutOnRecord>>("StudentKeepList");
            if (get_studentkeep_list == null || get_studentkeep_list.Count == 0)
            {
                get_studentkeep_list = this.GetIQueryable().ToList();
                redisCache.SetCache("StudentKeepList", get_studentkeep_list);
            }
            return get_studentkeep_list;
        }
        /// <summary>
        /// 获取某一年的已报名备案数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public List<StudentPutOnRecord> GetEffectiveData(int year)
        {
            List<StudentPutOnRecord> list = new List<StudentPutOnRecord>();
            List<StudentPutOnRecord> All = GetAllStudentKeepData();
            foreach (StudentPutOnRecord item in All)
            {
                if (item.StatusTime != null)
                {
                    if (Convert.ToDateTime(item.StatusTime).Year == year)
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }
        //查询员工
        public string GetEmployeeValue(string id, bool isglu)
        {         
            if (isglu)
            {
                EmployeesInfo finde = Enplo_Entity.GetList().Where(s => s.EmployeeId == id && s.IsDel == false).FirstOrDefault();
                if (finde != null)
                {
                    return finde.EmpName;
                }
                else
                {
                    return "无";
                }
            }
            else
            {
                EmployeesInfo finde = Enplo_Entity.GetEntity(id);
                if (finde != null)
                {
                    return finde.EmpName;
                }
                else
                {
                    return "无";
                }
            }
        }
        /// <summary>
        /// 获取员工 
        /// </summary>
        /// <param name="IsIncumbency">(true--获取在职员工,false--获取所有员工)</param>
        /// <returns></returns>
        public List<EmployeesInfo> GetEffectiveEmpAll(bool IsIncumbency)
        {
            if (IsIncumbency)
            {
                //获取所有在职员工
                return Enplo_Entity.GetAll();
            }
            else
            {
                //获取所有员工
                return Enplo_Entity.GetList();
            }
        }

        /// <summary>
        /// 获取所有区域 
        /// </summary>
        /// <param name="IsIncumbercy">（true--获取有效区域,false--获取所有区域）</param>
        /// <returns></returns>
        public List<Region> GetEffectiveRegionAll(bool IsIncumbercy)
        {
            region_Entity = new RegionManeges();
            if (IsIncumbercy)
            {
                //获取有效的区域
                return region_Entity.GetList().Where(r => r.IsDel == false).ToList();
            }
            else
            {
                //获取所有区域
                return region_Entity.GetList();
            }
        }

        /// <summary>
        /// 通过员工名称获取员工编号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public EmployeesInfo GetNameSreachEmploId(string name)
        {
            return GetEffectiveEmpAll(false).Where(es => es.EmpName == name).FirstOrDefault();

        }      
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="new_s"></param>
        /// <returns></returns>
        public AjaxResult Add_data(StudentPutOnRecord new_s)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                //判断是否已备案
                int count = GetAllStudentKeepData().Where(s => s.StuName == new_s.StuName && s.StuPhone == new_s.StuPhone).ToList().Count;
                if (count <= 0)
                {
                    this.Insert(new_s);
                    redisCache.RemoveCache("StudentKeepList");
                    a.Success = true;
                    a.Msg = "备案成功";                  
                }
                else
                {
                    a.Success = false;
                }

            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = ex.Message;
            }

            return a;
        }
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="olds"></param>
        /// <returns></returns>
        public AjaxResult Update_data(StudentPutOnRecord olds)
        {
            StuStateManeger Stustate_Entity = new StuStateManeger();
            AjaxResult a = new AjaxResult();
            try
            {
                StudentPutOnRecord fins = this.GetAllStudentKeepData().Where(s => s.Id == olds.Id).FirstOrDefault();//找到要修改的实体
                fins.StuSex = olds.StuSex;
                fins.StuBirthy = olds.StuBirthy;
                fins.StuSchoolName = olds.StuSchoolName;
                fins.StuEducational = olds.StuEducational;
                fins.StuAddress = olds.StuAddress;
                fins.StuWeiXin = olds.StuWeiXin;
                fins.StuQQ = olds.StuQQ;
                fins.StuIsGoto = olds.StuIsGoto;
                fins.StuVisit = olds.StuVisit;
                fins.StuInfomationType_Id = olds.StuInfomationType_Id;
                fins.Party = olds.Party;
                if (olds.StuStatus_Id == -1 || olds.StuStatus_Id == null)
                {
                    AjaxResult a1 = Stustate_Entity.GetIdGiveName("未报名", false);
                    if (a1.Success == true)
                    {
                        StuStatus find_status = a1.Data as StuStatus;
                        fins.StuStatus_Id = find_status.Id;
                    }
                }
                else
                {
                    fins.StuStatus_Id = olds.StuStatus_Id;
                }
                fins.Region_id = olds.Region_id;
                this.Update(fins);
                redisCache.RemoveCache("StudentKeepList");
                a.Success = true;
                a.Msg = "修改成功！";
            }
            catch (Exception ex)
            {
                a.Msg = ex.Message;
                a.Success = false;
            }
            return a;
        }
        /// <summary>
        /// 将未上门学生改为已上门
        /// </summary>
        /// <param name="data"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public AjaxResult UpdateGotoShcool(List<StudentPutOnRecord> data,DateTime date)
        {
            AjaxResult a = new AjaxResult();
            redisCache = new RedisCache();
            List<StudentPutOnRecord> newdata = new List<StudentPutOnRecord>();
            foreach (StudentPutOnRecord item in data)
            {
                item.StuIsGoto = true;
                item.StuVisit = date;
                newdata.Add(item);
            }

            try
            {
                this.Update(newdata);
                a.Success = true;
                redisCache.RemoveCache("StudentKeepList");
            }
            catch (Exception)
            {

                a.Success = false;
            }

            return a;
        }
        /// <summary>
        /// 指派咨询师
        /// </summary>
        /// <param name="olds"></param>
        /// <returns></returns>
        public bool ZhipaiConsultTeacher(StudentPutOnRecord olds)
        {
            redisCache = new RedisCache();
            bool s = true;
            try
            {
                this.Update(olds);
                redisCache.RemoveCache("StudentKeepList");
            }
            catch (Exception)
            {
                s = false;
            }

            return s;
        }

        public List<EmployeesInfo> GetDepartmentPeople(int did)
        {             
           return Enplo_Entity.GetEmpsByDeptid(did);
        }

        #region 给市场用的方法
        /// <summary>
        /// 这是一个获取报名学生的方法
        /// </summary>
        /// <param name="EmpyId">员工编号</param>
        /// <returns></returns>
        public List<StudentPutOnRecord> GetrReport(string EmpyId)
        {    //获取报名id
            Statu_Entity = new StuStateManeger();
            int id = Statu_Entity.GetList().Where(s => s.StatusName == "已报名").FirstOrDefault().Id;
            //根据员工获取报名的数据
            return this.GetList().Where(s => s.StuStatus_Id == id && s.EmployeesInfo_Id == EmpyId).ToList();
        }
        /// <summary>
        /// 获取某一年的每个月的上门量
        /// </summary>
        /// <param name="YeanName"></param>
        /// <returns></returns>
        public int[] GetYearGotoCount(DateTime YeanName)
        {
            List<StudentPutOnRecord> student_data = this.GetList().Where(s => s.StuIsGoto == true && s.StuVisit <= YeanName).ToList();//获取匹配的数据
            int j = 1;                                                                                                               //拿到一月到12的人数            
            int[] ary = new int[12];
            for (int i = 0; i < 12; i++)
            {
                ary[i] = GetMonthCount(student_data, j);
                j++;
            }
            return ary;
        }
        /// <summary>
        /// 这个方法是就修改学生状态的方法
        /// </summary>
        /// <param name="id">备案id</param>
        /// <returns></returns>
        public bool ChangeStudentState(int id)
        {
            Statu_Entity = new StuStateManeger();
            redisCache = new RedisCache();
            StudentPutOnRecord find_s = this.GetEntity(id);
            if (find_s != null)
            {
                AjaxResult a = Statu_Entity.GetStu("已报名");
                if (a.Success == true)
                {
                    StuStatus find_statu = a.Data as StuStatus;
                    find_s.StuStatus_Id = find_statu.Id;
                    find_s.StatusTime = DateTime.Now;
                    this.Update(find_s);
                    redisCache.RemoveCache("StudentKeepList");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public int GetMonthCount(List<StudentPutOnRecord> student_list, int monthName)
        {
            var count = student_list.Select(s => Convert.ToDateTime(s.StuVisit).Month).Where(s => s == monthName).ToList().Count;
            return count;
        }
        /// <summary>
        /// 获取上门量
        /// </summary>
        /// <param name="PlanId"></param>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        public List<StudentPutOnRecord> GetGoSchoolByPlan(int? PlanId, string EmpId)
        {
            //当前查询的计划
            var nowplan = Syb_Entity.GetPlanByID(PlanId);
            //拿到下一个计划
            var nextplan = Syb_Entity.GetNextPlan(nowplan);

            List<StudentPutOnRecord> list_s = this.GetList().Where(s => s.EmployeesInfo_Id == EmpId && s.StuIsGoto == true).ToList();
            List<StudentPutOnRecord> resultlist = new List<StudentPutOnRecord>();
            foreach (var item in list_s)
            {
                if (item.StuVisit >= nowplan.PlanDate)
                {
                    if (nextplan.ID != 0)
                    {
                        if (item.StuVisit <= nextplan.PlanDate)
                        {
                            resultlist.Add(item);
                        }
                    }
                    else
                    {
                        resultlist.Add(item);
                    }
                }
            }
            return resultlist;
        }
        /// <summary>
        /// 获取备案量
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="PlanId"></param>
        /// <returns></returns>
        public List<StudentPutOnRecord> GetBeanCount(string EmpId, int? PlanId)
        {
            //当前查询的计划
            var nowplan = Syb_Entity.GetPlanByID(PlanId);
            //拿到下一个计划
            var nextplan = Syb_Entity.GetNextPlan(nowplan);

            List<StudentPutOnRecord> list_s = this.GetList().Where(s => s.EmployeesInfo_Id == EmpId).ToList();
            List<StudentPutOnRecord> resultlist = new List<StudentPutOnRecord>();
            foreach (var item in list_s)
            {
                if (item.StuDateTime >= nowplan.PlanDate)
                {
                    if (nextplan.ID != 0)
                    {
                        if (item.StuDateTime <= nextplan.PlanDate)
                        {
                            resultlist.Add(item);
                        }
                    }
                    else
                    {
                        resultlist.Add(item);
                    }
                }
            }
            return resultlist;
        }
        /// <summary>
        /// 获取报名量
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="PlanId"></param>
        /// <returns></returns>
        public List<StudentPutOnRecord> GetBaoMingCount(string EmpId, int? PlanId)
        {
            //当前查询的计划
            var nowplan = Syb_Entity.GetPlanByID(PlanId);
            //拿到下一个计划
            var nextplan = Syb_Entity.GetNextPlan(nowplan);
            //获取报名id
            Statu_Entity = new StuStateManeger();
            int id = Statu_Entity.GetList().Where(s => s.StatusName == "已报名").FirstOrDefault().Id;
            List<StudentPutOnRecord> list_s = this.GetList().Where(s => s.EmployeesInfo_Id == EmpId && s.StuStatus_Id == id).ToList();
            List<StudentPutOnRecord> resultlist = new List<StudentPutOnRecord>();
            foreach (var item in list_s)
            {
                if (item.StatusTime >= nowplan.PlanDate)
                {
                    if (nextplan.ID != 0)
                    {
                        if (item.StatusTime <= nextplan.PlanDate)
                        {
                            resultlist.Add(item);
                        }
                    }
                    else
                    {
                        resultlist.Add(item);
                    }
                }
            }
            return resultlist;
        }
        #endregion        
      

        #region 用于将远程数据库中的备案数据导入当前数据库中
        /// <summary>
        /// 获取远程备案数据
        /// </summary>
        /// <returns></returns>
        public List<Sch_Market> GetLongrageData()
        {
            List<Sch_Market> list = new List<Sch_Market>();
            string sql = "server=121.43.166.117;database=GUIGU;uid=sa;pwd=Guigu20202020;Max Pool Size = 512";
            SqlConnection con = new SqlConnection(sql);
            con.Open();
            SqlCommand comm = new SqlCommand("select * from Sch_Market where CreateDate>='2017-01-01' and CreateDate<='2019-12-30'", con);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(comm);
            da.Fill(ds);
            DataTable mm = ds.Tables[0];

            list = mm.ToList<Sch_Market>();
            con.Close();
            return list;
        }

        public void InServer()
        {
            Sch_MarketManeger marketEntity = new Sch_MarketManeger();
            List<Sch_Market> all = GetLongrageData();
            //List<Sch_Market> Onelist =all.Where(s => s.CreateDate >= Convert.ToDateTime("2017-01-01") && s.CreateDate <= Convert.ToDateTime("2017-12-31")).ToList();
            //bool result1=  marketEntity.AddData(Onelist);
            //if (result1)
            //{
                List<Sch_Market> Twolist= all.Where(s => s.CreateDate >= Convert.ToDateTime("2018-01-01") && s.CreateDate <= Convert.ToDateTime("2018-12-31")).ToList();
                bool result2 = marketEntity.AddData(Twolist);
                if (result2)
                {
                    List<Sch_Market> Threelist = all.Where(s => s.CreateDate >= Convert.ToDateTime("2019-01-01") && s.CreateDate <= Convert.ToDateTime("2019-12-31")).ToList();
                    bool result3 = marketEntity.AddData(Threelist);
                    
                }
            //}
        }

        /// <summary>
        /// 将远程数据转成本地视图数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<ExportStudentBeanData> LongrageDataToViewmodel(List<Sch_MarketView> list)
        {
            List<ExportStudentBeanData> newlist = list.Select(s2 => new ExportStudentBeanData
            {
                StuName = s2.StudentName,
                StuSex = s2.Sex,
                StuBirthy = null,
                IdCade = null,
                Stuphone = s2.Phone,
                StuSchoolName = s2.School,
                StuEducational = s2.Education,
                StuAddress = null,
                StuWeiXin = null,
                StuQQ = s2.QQ,
                stuinfomation = s2.source,
                StatusName = s2.MarketState,
                StuisGoto = false,
                StuVisit = null,
                empName = s2.SalePerson,
                Party = s2.RelatedPerson,
                BeanDate = s2.CreateDate,
                StuEntering = s2.CreateUserName,
                StatusTime = null,
                RegionName = s2.Area,
                Reak = null,
                ConsultTeacher = s2.Inquiry
            }).ToList();

            return newlist;
        }
        #endregion

        /// <summary>
        /// 获取两张表的所有数据
        /// </summary>
        /// <returns></returns>
        public List<ExportStudentBeanData> GETView()
        {
             List<ExportStudentBeanData> listall= this.GetListBySql<ExportStudentBeanData>("select * from StudentBeanView");

            List<Sch_MarketView> old = this.GetListBySql<Sch_MarketView>("select * from Sch_MarketView");

            listall.AddRange(this.LongrageDataToViewmodel(old));
            return listall;
        }

        /// <summary>
        /// 获取StudentPutOnRecord表的所有数据
        /// </summary>
        /// <returns></returns>
        public List<ExportStudentBeanData> GetSudentDataAll()
        {
            List<ExportStudentBeanData> listall = this.GetListBySql<ExportStudentBeanData>("select * from StudentBeanView");

            return listall;
        }

        /// <summary>
        /// 获取页面总条数
        /// </summary>
        /// <returns></returns>
        public int SumCount()
        {
           
            SqlConnection con = new SqlConnection("server=106.13.104.179;database=Coldairarrow.Fx.Net.Easyui.GitHub;uid=sa;pwd=tangdan2020@;Max Pool Size = 512;");
            con.Open();
            SqlCommand cmd = new SqlCommand("select count(*) as count from StudentBeanView",con);
            SqlCommand cmd2 = new SqlCommand("select count(*) as count from Sch_MarketView", con);
            int count=Convert.ToInt32(cmd.ExecuteScalar());
            count= count+ Convert.ToInt32(cmd2.ExecuteScalar());
            con.Close();
            return count;
        }

        /// <summary>
        /// 查看是否有重复的值 false--有，true--没有
        /// </summary>
        /// <param name="stuName"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool StudentOrride(string stuName,string phone)
        {
            stuName = stuName.Trim();
            List<ExportStudentBeanData> listall = this.GetListBySql<ExportStudentBeanData>("select * from StudentBeanView where StuName='"+stuName+"'and Stuphone='"+phone+"' ");
            List<Sch_MarketView> listal2 = this.GetListBySql<Sch_MarketView>("select * from Sch_MarketView where StudentName='"+stuName+"'and Phone='"+ phone + "'");
            return listall.Count + listal2.Count <= 0 ? true : false;
        }

        /// <summary>
        /// 获取有相同学生姓名备案数据
        /// </summary>
        /// <param name="stuName"></param>
        /// <returns></returns>
        public List<ExportStudentBeanData> StudentOrride(string stuName)
        {
            stuName = stuName.Trim();
            List<ExportStudentBeanData> listall = this.GetListBySql<ExportStudentBeanData>("select * from StudentBeanView where StuName='" + stuName + "'");

            List<Sch_MarketView> listal2 = this.GetListBySql<Sch_MarketView>("select * from Sch_MarketView where StudentName='" + stuName + "'");


            listall.AddRange(this.LongrageDataToViewmodel(listal2));

            return listall;
        }

        /// <summary>
        /// 获取已备案的疑似数据
        /// </summary>
        /// <param name="stuName"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public ExportStudentBeanData StudentOrrideData(string stuName, string phone)
        {
            stuName = stuName.Trim();
            List<ExportStudentBeanData> listall = this.GetListBySql<ExportStudentBeanData>("select * from StudentBeanView where StuName='" + stuName + "'and Stuphone='" + phone + "' ");
            List<Sch_MarketView> listal2 = this.GetListBySql<Sch_MarketView>("select * from Sch_MarketView where StudentName='" + stuName + "'and Phone='" + phone + "'");

            listall.AddRange(this.LongrageDataToViewmodel(listal2));

            return listall.Count > 0 ? listall[0] : null;
        }
    }
}
