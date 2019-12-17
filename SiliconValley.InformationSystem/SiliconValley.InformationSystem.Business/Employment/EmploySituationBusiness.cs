using SiliconValley.InformationSystem.Business.Base_SysManage;
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
    public class EmploySituationBusiness : BaseBusiness<EmploySituation>
    {
        private QuarterBusiness dbquarter;
        private EmploymentJurisdictionBusiness dbemploymentJurisdiction;
        private EmploymentStaffBusiness dbemploymentStaff;

        /// <summary>
        /// 加载右侧的树
        /// </summary>
        /// <returns></returns>
        public resultdtree loadtree() {
            dbquarter = new QuarterBusiness();
            dbemploymentJurisdiction = new EmploymentJurisdictionBusiness();
            dbemploymentStaff = new EmploymentStaffBusiness();

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            var queryempstaff = dbemploymentStaff.GetEmploymentByEmpInfoID(user.EmpNumber);
            //第一层
            var querydata = new List<EmploymentYearView>();
            bool isJurisdiction = dbemploymentJurisdiction.isstaffJurisdiction(user);
            if (!isJurisdiction)
            {
                var data = dbquarter.GetQuartersByempid(queryempstaff.ID);
                querydata = dbquarter.yearplan(data);
            }
            else
            {
                var data = dbquarter.GetQuarters();
                querydata = dbquarter.yearplan(data);
            }



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


                    List<Quarter> Quarterslist = new List<Quarter>();
                    //这是第二层数据
                    if (!isJurisdiction)
                    {
                        Quarterslist = dbquarter.GetQuartersByYearandempid(querydata[i].Year, queryempstaff.ID);
                    }
                    else
                    {
                        Quarterslist = dbquarter.GetQuartersByYear(querydata[i].Year);
                    }



                    if (Quarterslist.Count > 0)
                    {

                        //第二层的tree数据
                        List<dtreeview> Quarterlist = new List<dtreeview>();
                        for (int j = 0; j < Quarterslist.Count; j++)
                        {


                            dtreeview Quarters = new dtreeview();
                            Quarters.nodeId = Quarterslist[j].ID.ToString();
                            Quarters.context = Quarterslist[j].QuaTitle;
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

        
        
    }
}
