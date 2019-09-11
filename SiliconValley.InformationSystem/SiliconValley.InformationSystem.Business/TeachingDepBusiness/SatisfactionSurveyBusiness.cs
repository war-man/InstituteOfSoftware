using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;


    /// <summary>
    /// 满意度调查业务类
    /// </summary>
  public  class SatisfactionSurveyBusiness:BaseBusiness<SatisficingItem>
    {


        private readonly BaseBusiness<SatisficingType> db_saitemtype;
        public SatisfactionSurveyBusiness()
        {

            db_saitemtype = new BaseBusiness<SatisficingType>();

        }

        public List<SatisficingItem> GetAllSatisfactionItems()
        {

            return this.GetList().Where(d=>d.IsDel==false).ToList();

        }

        /// <summary>
        /// 对调查具体项进行筛选
        /// </summary>
        /// <param name="DepID">部门ID</param>
        /// <param name="satisfactionTypeID">调查类型ID 比如:学术能力、教学态度、教学能力</param>
        /// <returns></returns>
        public List<SatisficingItem> Screen(int DepID, int satisfactionTypeID)
        {

            List<SatisficingItem> resultlist = new List<SatisficingItem>();


            if (DepID == 0 && satisfactionTypeID == 0)
            {
                resultlist = this.GetAllSatisfactionItems();
            }
            else if (satisfactionTypeID == 0)
            {
                var list = this.GetAllSatisfactionItems();

                foreach (var item in list)
                {
                    //获取项的类型
                    var objtype = db_saitemtype.GetList().Where(d => d.ID == item.ItemType).FirstOrDefault();

                    if (objtype.DepartmentID == DepID)
                    {
                        resultlist.Add(item);
                    }


                }
            }
            else
            {
                var list = this.GetAllSatisfactionItems();

                foreach (var item in list)
                {
                    //获取项的类型
                    var objtype = db_saitemtype.GetList().Where(d => d.ID == satisfactionTypeID && d.DepartmentID ==DepID).FirstOrDefault();

                    if (item.ItemType == objtype.ID)
                    {
                       
                            resultlist.Add(item);
                       
                    }

                }

            }

           

          

            return resultlist;
            

        }


        /// <summary>
        /// 转为视图模型
        /// </summary>
        /// <param name="satisficingItem">数据实体</param>
        /// <returns></returns>
        public SatisfactionSurveyView ConvertModelView(SatisficingItem satisficingItem)
        {
            SatisfactionSurveyView surveyView = new SatisfactionSurveyView();

            surveyView.IsDel = satisficingItem.IsDel;
            surveyView.ItemContent = satisficingItem.ItemContent;
            surveyView.ItemID = satisficingItem.ItemID;

           var type = db_saitemtype.GetList().Where(d=>d.ID==satisficingItem.ItemType).FirstOrDefault();

            if (type != null)
            {
                surveyView.ItemType = type;
            }
            else
            {
                surveyView.ItemType = null;
            }

            surveyView.Remark = satisficingItem.Remark;
            
            return surveyView;
        }

        /// <summary>
        /// 获取所有调查类型
        /// </summary>
        /// <returns></returns>
        public List<SatisficingType> GetSatisficingTypes()
        {
           return db_saitemtype.GetList();
        }

        /// <summary>
        /// 筛选调查项类型
        /// </summary>
        /// <param name="typename">类型名称</param>
        /// <returns></returns>
        public List<SatisficingType> Screen(string  typename, int depid)
        {

            if (string.IsNullOrEmpty(typename))

            {
                return this.GetSatisficingTypes().Where(d => depid == d.DepartmentID).ToList();

            }
            else
            {
                return this.GetSatisficingTypes().Where(d => d.TypeName.Trim() == typename.Trim() && depid == d.DepartmentID).ToList();
            }

        }

        /// <summary>
        /// 添加调查项类型
        /// </summary>
        /// <param name="typename">类型名称</param>
        public void AddSurveyItemType(string typename, int depid)
        {
            SatisficingType satisficingType = new SatisficingType();

            satisficingType.DepartmentID = depid;
            satisficingType.TypeName = typename.Trim();

            db_saitemtype.Insert(satisficingType);

        }


    }
}
