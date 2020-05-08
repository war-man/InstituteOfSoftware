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

namespace SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness
{
   public class StudentDataKeepAndRecordBusiness: BaseBusiness<StudentPutOnRecord>
    {
        StuStateManeger Statu_Entity;
        SchoolYearPlanBusiness Syb_Entity = new SchoolYearPlanBusiness();
        RedisCache redisCache;
        //创建一个用于查询数据的员工信息实体
        public EmployeesInfoManage Enplo_Entity; 
         //创建一个用于查询区域的实体
         RegionManeges region_Entity;

        /// <summary>
        /// 获取所有备案数据
        /// </summary>
        /// <returns></returns>
        public List<StudentPutOnRecord> GetAllStudentKeepData()
        {
            redisCache = new RedisCache();
            redisCache.RemoveCache("StudentKeepList");
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
                if (item.StatusTime!=null)
                {
                    if (Convert.ToDateTime(item.StatusTime).Year==year)
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
            Enplo_Entity = new EmployeesInfoManage();
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
            Enplo_Entity = new EmployeesInfoManage();
            if (IsIncumbency)
            {
                //获取所有在职员工
              return  Enplo_Entity.GetAll();
            }
            else
            {
                //获取所有员工
              return  Enplo_Entity.GetList();
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
                int count= GetAllStudentKeepData().Where(s => s.StuName == new_s.StuName && s.StuPhone == new_s.StuPhone).ToList().Count;
                if (count<=0)
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
            int id= Statu_Entity.GetList().Where(s => s.StatusName == "已报名").FirstOrDefault().Id;
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
    }
}
