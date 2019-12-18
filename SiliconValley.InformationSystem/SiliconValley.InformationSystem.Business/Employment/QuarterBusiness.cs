using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 就业毕业计划业务类
    /// </summary>
    public class QuarterBusiness : BaseBusiness<Quarter>
    {
        private EmploymentStaffBusiness dbemploymentStaff;
        private EmpQuarterClassBusiness dbempQuarterClass;
        private EmpClassBusiness dbempClass;
        /// <summary>
        /// 获取全部可用的就业计划
        /// </summary>
        /// <returns></returns>
        public List<Quarter> GetQuarters()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }


        /// <summary>
        /// 年度 eg：2019 /2020
        /// </summary>
        /// <returns></returns>
        public List<EmploymentYearView> yearplan(List<Quarter> data)
        {
          
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = data.Count - 1; j > i; j--)
                {
                    if (data[i].RegDate.Year == data[j].RegDate.Year)
                    {
                        data.Remove(data[j]);
                    }
                }
            }

            List<EmploymentYearView> result = new List<EmploymentYearView>();
            foreach (var item in data)
            {
                EmploymentYearView view = new EmploymentYearView();
                view.YearTitle = item.RegDate.Year + "年度";
                view.Year = item.RegDate.Year;
                result.Add(view);
            }
           
            return result;
        }

       
        /// <summary>
        /// 根据该员工返回这个员工就业计划设计到得班级
        /// </summary>
        /// <param name="empid">员工id</param>
        /// <returns></returns>
        public List<EmpQuarterClass> GetEmpQuartersByempid(int empid) {
            dbempQuarterClass = new EmpQuarterClassBusiness();
            dbempClass = new EmpClassBusiness();
            var queryempclss = dbempQuarterClass.GetEmpQuarters();
            var queryempclsslist = dbempClass.GetEmpsByEmpID(empid);
            try
            {
              

                for (int i = queryempclss.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < queryempclsslist.Count; j++)
                    {
                        if (queryempclss[i].Classid != queryempclsslist[j].ClassId)
                        {
                            if (j == queryempclsslist.Count - 1)
                            {
                                queryempclss.RemoveAt(i);


                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return queryempclss;
        }

        /// <summary>
        /// 根据员工id 返回它涉及到得计划个 根据班级来的。
        /// </summary>
        /// <returns></returns>
        public List<Quarter> GetQuartersByempid(int empid)
        {
            var querydata = this.GetEmpQuartersByempid(empid);
            List<Quarter> quarters = new List<Quarter>();
            foreach (var item in querydata)
            {
                quarters.Add(this.GetEntity(item.QuarterID));
            }
            for (int i = 0; i < quarters.Count; i++)
            {
                for (int j = quarters.Count - 1; j > i; j--)  //内循环是 外循环一次比较的次数
                {

                    if (quarters[i].ID == quarters[j].ID)
                    {
                        quarters.RemoveAt(j);
                    }
                }
            }
            return quarters;
        }

        /// <summary>
        /// 根据年度获取这次年度的记录
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<Quarter> GetQuartersByYear(int year) {
            return this.GetQuarters().Where(a => a.RegDate.Year == year).ToList();
        }
        /// <summary>
        /// 根据年度以及这个专员id获取这次年度的记录
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<Quarter> GetQuartersByYearandempid(int year, int empid)
        {
            var aa = this.GetQuartersByYear(year);
            var bb = this.GetEmpQuartersByempid(empid);
            for (int i = bb.Count-1; i >=0 ; i--)
            {
                for (int j = 0; j < aa.Count; j++)
                {
                    if (bb[i].QuarterID != aa[j].ID)
                    {
                        if (j==aa.Count-1)
                        {
                            bb.Remove(bb[i]);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return this.conversionempquarterclasstoquarter(bb);
        }

        /// <summary>
        /// 把季度带班记录转为季度对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Quarter> conversionempquarterclasstoquarter(List<EmpQuarterClass> data)  {
            List<Quarter> quarter = new List<Quarter>();
            foreach (var item in data)
            {
                quarter.Add(this.GetEntity(item.QuarterID));
            }
            return quarter;
        }




    }
}
