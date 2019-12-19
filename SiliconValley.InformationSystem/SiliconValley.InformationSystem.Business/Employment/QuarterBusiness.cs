using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
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
        /// 全部树
        /// </summary>
        /// <returns></returns>
        public resultdtree loadtree() {
           
         
            //第一层
            var querydata = this.yearplan(this.GetQuarters());
            //返回的结果
            resultdtree result = new resultdtree();

            //状态
            dtreestatus dtreestatus = new dtreestatus();

            //最外层的儿子数据
            List<dtreeview> childrendtreedata = new List<dtreeview>();

            for (int i = 0; i < querydata.Count; i++)
            {

                //第一层
                dtreeview seconddtree = new dtreeview();
                try
                {
                    if (i == 0)
                    {
                        seconddtree.spread = true;
                    }
                    seconddtree.nodeId = querydata[i].Year.ToString();
                    seconddtree.context = querydata[i].YearTitle;
                    seconddtree.last = false;
                    seconddtree.parentId = "0";
                    seconddtree.level = 0;

                   var data= this.GetQuartersByYear(querydata[i].Year);
                    if (data.Count > 0)
                    {

                        //第二层的tree数据
                        List<dtreeview> Quarterlist = new List<dtreeview>();
                        for (int j = 0; j < data.Count; j++)
                        {
                            dtreeview Quarters = new dtreeview();
                            Quarters.nodeId = data[j].ID.ToString();
                            Quarters.context = data[j].QuaTitle;
                            Quarters.last = true;
                            Quarters.parentId = querydata[i].Year.ToString();
                            Quarters.level = 1;
                            Quarterlist.Add(Quarters);


                        }
                        seconddtree.children = Quarterlist;

                    }
                    else
                    {
                        seconddtree.last = true;
                    }
                    dtreestatus.code = "200";
                    dtreestatus.message = "操作成功";
                }
                catch (Exception ex)
                {
                    dtreestatus.code = "1";
                    dtreestatus.code = "操作失败";
                    throw;
                }
                childrendtreedata.Add(seconddtree);
            }

            result.status = dtreestatus;
            result.data = childrendtreedata;
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
